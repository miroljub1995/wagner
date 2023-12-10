#include <assert.h>
#include <glib-unix.h>
#include <unistd.h>
#include <wayland-client.h>

#include "main_loop.h"

static gboolean wayland_message_callback(gint fd, GIOCondition condition, gpointer user_data)
{
    struct wl_event_loop *wl_loop = user_data;

    if (condition & G_IO_IN) {
        if (wl_event_loop_dispatch(wl_loop, -1) < 0) {
            return FALSE;
        }

        return TRUE;
    }

    return FALSE;
}

static GSource *create_wayland_source(struct wl_display *display) {
    struct wl_event_loop *wl_loop = wl_display_get_event_loop(display);

    assert(wl_event_loop_dispatch(wl_loop, 0) == 0);
    GSource* source = g_unix_fd_source_new(wl_event_loop_get_fd(wl_loop), G_IO_IN | G_IO_ERR | G_IO_HUP);
    g_source_set_callback(source, (GSourceFunc)wayland_message_callback, wl_loop, NULL);

    return source;
}

void wg_main_loop_run(struct wl_display *wl_display) {
    GMainLoop *loop = g_main_loop_new(NULL, FALSE);

    GSource *wl_source = create_wayland_source(wl_display);
    g_source_attach(wl_source, NULL);

    g_main_loop_run(loop);

    g_source_destroy(wl_source);
    g_main_loop_unref(loop);
}
