#include <wpe/fdo.h>
#include <wpe/fdo-egl.h>
#include <wpe/webkit.h>

#include "wpe_webkit.h"

struct view {
    struct wpe_view_backend_exportable_fdo *exportable;
};

static struct view exportable_view = {
    .exportable = NULL
};

void export_fdo_egl_image(void* data, struct wpe_fdo_egl_exported_image* image) {
    g_print("export_fdo_egl_image\n");

    wpe_view_backend_exportable_fdo_dispatch_frame_complete(exportable_view.exportable);
    wpe_view_backend_exportable_fdo_egl_dispatch_release_exported_image(exportable_view.exportable, image);
}

void destry_view_backend(gpointer data) {
    g_print("destry_view_backend\n");
}

static struct wpe_view_backend_exportable_fdo_egl_client exportableClient = {
    // export_egl_image
    NULL,
    // export_fdo_egl_image
    export_fdo_egl_image,
    // padding
    NULL, NULL, NULL
};


GLuint wg_create_wpe_view_texture(GLsizei width, GLsizei height) {
	GLuint texture = 0;

	glGenTextures(1, &texture);
    glBindTexture(GL_TEXTURE_2D, texture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, NULL);
    glBindTexture(GL_TEXTURE_2D, 0);

	return texture;
}

void wg_initialize_wpe_webkit(EGLDisplay display) {
    g_print("WPE WebKit %u.%u.%u",
        webkit_get_major_version(),
        webkit_get_minor_version(),
        webkit_get_micro_version());
    g_print("\n");

    wpe_loader_init("libWPEBackend-fdo-1.0.so");

    wpe_fdo_initialize_for_egl_display(display);

    WebKitNetworkSession* networkSession = webkit_network_session_new(NULL, NULL);
   
    WebKitWebContext* webContext = WEBKIT_WEB_CONTEXT(g_object_new(WEBKIT_TYPE_WEB_CONTEXT, "time-zone-override", NULL, NULL));
   
    WebKitSettings* settings = webkit_settings_new_with_settings(
        "enable-developer-extras", TRUE,
        "enable-webgl", TRUE,
        "enable-media-stream", TRUE,
        "enable-encrypted-media", TRUE,
        NULL);

    struct wpe_view_backend_exportable_fdo* exportable = wpe_view_backend_exportable_fdo_egl_create(&exportableClient, &exportable_view, 500, 500);
    exportable_view.exportable = exportable;

    struct wpe_view_backend* view_backend = wpe_view_backend_exportable_fdo_get_view_backend(exportable);

    WebKitWebViewBackend* webkit_view_backend = webkit_web_view_backend_new(view_backend, destry_view_backend, NULL);

    WebKitWebView* webView = WEBKIT_WEB_VIEW(g_object_new(WEBKIT_TYPE_WEB_VIEW,
        "backend", webkit_view_backend,
        "web-context", webContext,
        "network-session", networkSession,
        "settings", settings,
        "user-content-manager", NULL,
        NULL));
    g_object_unref(settings);

    webkit_web_view_load_uri(webView, "https://wpewebkit.org");
    wpe_view_backend_add_activity_state(view_backend, wpe_view_activity_state_visible | wpe_view_activity_state_focused | wpe_view_activity_state_in_window);
}
