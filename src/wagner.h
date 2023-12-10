#ifndef WG_WAGNER_H
#define WG_WAGNER_H

#include <wlr/backend.h>

struct wagner_state {
	struct wl_display *display;
	struct wl_listener new_output;
	struct wl_listener new_input;
	struct wlr_renderer *renderer;
	struct wlr_allocator *allocator;
	struct timespec last_frame;
	float color[4];
	int dec;
};

struct wagner_output {
	struct wagner_state *wagner;
	struct wlr_output *output;
	struct wl_listener frame;
	struct wl_listener destroy;
	unsigned int wpe_view_texture;
	unsigned int wpe_view_shader_program;
	int wpe_view_u_texture;
};
#endif
