#include <assert.h>
#include <inttypes.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include <wayland-server-core.h>
#include <wlr/backend.h>
#include <wlr/backend/session.h>
#include <wlr/render/allocator.h>
#include <wlr/render/egl.h>
#include <wlr/render/gles2.h>
#include <wlr/render/wlr_renderer.h>
#include <wlr/types/wlr_output.h>
#include <wlr/types/wlr_input_device.h>
#include <wlr/types/wlr_keyboard.h>
#include <wlr/util/log.h>
#include <xkbcommon/xkbcommon.h>

#include "main_loop.h"
#include "wagner.h"
#include "wpe_webkit.h"

struct sample_keyboard {
	struct wagner_state *wagner;
	struct wlr_input_device *device;
	struct wl_listener key;
	struct wl_listener destroy;
};

static void output_frame_notify(struct wl_listener *listener, void *data) {
	struct wagner_output *wagner_output =
		wl_container_of(listener, wagner_output, frame);
	struct wagner_state *wagner = wagner_output->wagner;
	struct wlr_output *wlr_output = wagner_output->output;

	struct timespec now;
	clock_gettime(CLOCK_MONOTONIC, &now);

	long ms = (now.tv_sec - wagner->last_frame.tv_sec) * 1000 +
		(now.tv_nsec - wagner->last_frame.tv_nsec) / 1000000;
	int inc = (wagner->dec + 1) % 3;

	wagner->color[inc] += ms / 2000.0f;
	wagner->color[wagner->dec] -= ms / 2000.0f;

	if (wagner->color[wagner->dec] < 0.0f) {
		wagner->color[inc] = 1.0f;
		wagner->color[wagner->dec] = 0.0f;
		wagner->dec = inc;
	}

	wlr_output_attach_render(wlr_output, NULL);

	struct wlr_renderer *renderer = wagner->renderer;
	// printf("output_frame w: %d h: %d\n", wlr_output->width, wlr_output->height);
	struct wlr_egl *egl = wlr_gles2_renderer_get_egl(wlr_output->renderer);
	wlr_egl_make_current(egl);

	wlr_renderer_begin(renderer, wlr_output->width, wlr_output->height);

	glClearColor(wagner->color[0], wagner->color[1], wagner->color[2], wagner->color[3]);
	glClear(GL_COLOR_BUFFER_BIT);

	wlr_renderer_end(renderer);

	wlr_output_commit(wlr_output);
	wagner->last_frame = now;
}

static void output_remove_notify(struct wl_listener *listener, void *data) {
	struct wagner_output *wagner_output =
		wl_container_of(listener, wagner_output, destroy);
	wlr_log(WLR_DEBUG, "Output removed");
	wl_list_remove(&wagner_output->frame.link);
	wl_list_remove(&wagner_output->destroy.link);
	free(wagner_output);
}

static void new_output_notify(struct wl_listener *listener, void *data) {
	struct wlr_output *output = data;
	struct wagner_state *wagner =
		wl_container_of(listener, wagner, new_output);

	wlr_output_init_render(output, wagner->allocator, wagner->renderer);

	struct wlr_egl *egl_renderer = wlr_gles2_renderer_get_egl(wagner->renderer);
	assert(egl_renderer > 0);
	wg_initialize_wpe_webkit(egl_renderer->display);

	struct wagner_output *wagner_output =
		calloc(1, sizeof(struct wagner_output));
	wagner_output->output = output;
	wagner_output->wagner = wagner;
	wl_signal_add(&output->events.frame, &wagner_output->frame);
	wagner_output->frame.notify = output_frame_notify;
	wl_signal_add(&output->events.destroy, &wagner_output->destroy);
	wagner_output->destroy.notify = output_remove_notify;

	struct wlr_output_mode *mode = wlr_output_preferred_mode(output);
	if (mode != NULL) {
		wlr_output_set_mode(output, mode);
	}

	wlr_output_commit(wagner_output->output);
}

static void keyboard_key_notify(struct wl_listener *listener, void *data) {
	struct sample_keyboard *keyboard = wl_container_of(listener, keyboard, key);
	struct wagner_state *wagner = keyboard->wagner;
	struct wlr_event_keyboard_key *event = data;
	uint32_t keycode = event->keycode + 8;
	const xkb_keysym_t *syms;
	int nsyms = xkb_state_key_get_syms(keyboard->device->keyboard->xkb_state,
			keycode, &syms);
	for (int i = 0; i < nsyms; i++) {
		xkb_keysym_t sym = syms[i];
		if (sym == XKB_KEY_Escape) {
			wl_display_terminate(wagner->display);
		}
	}
}

static void keyboard_destroy_notify(struct wl_listener *listener, void *data) {
	struct sample_keyboard *keyboard =
		wl_container_of(listener, keyboard, destroy);
	wl_list_remove(&keyboard->destroy.link);
	wl_list_remove(&keyboard->key.link);
	free(keyboard);
}

static void new_input_notify(struct wl_listener *listener, void *data) {
	struct wlr_input_device *device = data;
	struct wagner_state *wagner = wl_container_of(listener, wagner, new_input);
	switch (device->type) {
	case WLR_INPUT_DEVICE_KEYBOARD:;
		struct sample_keyboard *keyboard =
			calloc(1, sizeof(struct sample_keyboard));
		keyboard->device = device;
		keyboard->wagner = wagner;
		wl_signal_add(&device->events.destroy, &keyboard->destroy);
		keyboard->destroy.notify = keyboard_destroy_notify;
		wl_signal_add(&device->keyboard->events.key, &keyboard->key);
		keyboard->key.notify = keyboard_key_notify;
		struct xkb_context *context = xkb_context_new(XKB_CONTEXT_NO_FLAGS);
		if (!context) {
			wlr_log(WLR_ERROR, "Failed to create XKB context");
			exit(1);
		}
		struct xkb_keymap *keymap = xkb_keymap_new_from_names(context, NULL,
			XKB_KEYMAP_COMPILE_NO_FLAGS);
		if (!keymap) {
			wlr_log(WLR_ERROR, "Failed to create XKB keymap");
			exit(1);
		}
		wlr_keyboard_set_keymap(device->keyboard, keymap);
		xkb_keymap_unref(keymap);
		xkb_context_unref(context);
		break;
	default:
		break;
	}
}

int main(void) {
	wlr_log_init(WLR_DEBUG, NULL);
	struct wl_display *display = wl_display_create();

	struct wagner_state state = {
		.color = { 1.0, 0.0, 0.0, 1.0 },
		.dec = 0,
		.last_frame = { 0 },
		.display = display
	};
	struct wlr_backend *backend = wlr_backend_autocreate(display);
	if (!backend) {
		exit(1);
	}

	int drm_fd = wlr_backend_get_drm_fd(backend);
	assert(drm_fd > 0);
	state.renderer = wlr_gles2_renderer_create_with_drm_fd(drm_fd);

	struct wlr_egl *egl_renderer = wlr_gles2_renderer_get_egl(state.renderer);
	assert(egl_renderer > 0);

	state.allocator = wlr_allocator_autocreate(backend, state.renderer);

	wl_signal_add(&backend->events.new_output, &state.new_output);
	state.new_output.notify = new_output_notify;
	wl_signal_add(&backend->events.new_input, &state.new_input);
	state.new_input.notify = new_input_notify;
	clock_gettime(CLOCK_MONOTONIC, &state.last_frame);

	if (!wlr_backend_start(backend)) {
		wlr_log(WLR_ERROR, "Failed to start backend");
		wlr_backend_destroy(backend);
		exit(1);
	}

    const char *socket = wl_display_add_socket_auto(display);
    if (!socket) {
        fprintf(stderr, "Unable to add socket to Wayland display.\n");
        return 1;
    }
    fprintf(stderr, "Running Wayland display on %s\n", socket);

	wg_main_loop_run(display);
	
	wl_display_destroy(display);
}
