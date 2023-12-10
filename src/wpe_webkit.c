#include <wpe/fdo.h>
#include <wpe/fdo-egl.h>
#include <wpe/webkit.h>

#include "wpe_webkit.h"

void export_fdo_egl_image(void *data, struct wpe_fdo_egl_exported_image *image)
{
    g_print("export_fdo_egl_image\n");

    struct wagner_output *wagner_output = data;
    if (wagner_output->wpe_image)
    {
        wpe_view_backend_exportable_fdo_egl_dispatch_release_exported_image(wagner_output->wpe_view_backend_exportable, wagner_output->wpe_image);
    }

    wagner_output->wpe_image = image;
}

void destry_view_backend(gpointer data)
{
    g_print("destry_view_backend\n");
}

static struct wpe_view_backend_exportable_fdo_egl_client exportableClient = {
    // export_egl_image
    NULL,
    // export_fdo_egl_image
    export_fdo_egl_image,
    // padding
    NULL, NULL, NULL};

GLuint wg_create_wpe_view_texture(GLsizei width, GLsizei height)
{
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

void wg_create_shader_program(GLuint *program, GLint *u_texture)
{
    static const char *vertex_shader_source =
        "attribute vec2 pos;\n"
        "attribute vec2 texture;\n"
        "varying vec2 v_texture;\n"
        "void main() {\n"
        "  v_texture = texture;\n"
        "  gl_Position = vec4(pos, 0, 1);\n"
        "}\n";
    static const char *fragment_shader_source =
        "precision mediump float;\n"
        "uniform sampler2D u_texture;\n"
        "varying vec2 v_texture;\n"
        "void main() {\n"
        "  gl_FragColor = texture2D(u_texture, v_texture);\n"
        "}\n";

    GLuint vertexShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vertexShader, 1, &vertex_shader_source, NULL);
    glCompileShader(vertexShader);

    GLuint fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fragmentShader, 1, &fragment_shader_source, NULL);
    glCompileShader(fragmentShader);

    *program = glCreateProgram();
    glAttachShader(*program, vertexShader);
    glAttachShader(*program, fragmentShader);

    glBindAttribLocation(*program, 0, "pos");
    glBindAttribLocation(*program, 1, "texture");

    glLinkProgram(*program);
    *u_texture = glGetUniformLocation(*program, "u_texture");
}

void wg_wpe_dispatch_frame_complete(struct wagner_output *wagner_output)
{
    wpe_view_backend_exportable_fdo_dispatch_frame_complete(wagner_output->wpe_view_backend_exportable);
}

void wg_initialize_wpe_webkit(struct wagner_output *wagner_output, EGLDisplay display)
{
    g_print("WPE WebKit %u.%u.%u",
            webkit_get_major_version(),
            webkit_get_minor_version(),
            webkit_get_micro_version());
    g_print("\n");

    wpe_loader_init("libWPEBackend-fdo-1.0.so");

    wpe_fdo_initialize_for_egl_display(display);

    WebKitNetworkSession *networkSession = webkit_network_session_new(NULL, NULL);

    WebKitWebContext *webContext = WEBKIT_WEB_CONTEXT(g_object_new(WEBKIT_TYPE_WEB_CONTEXT, "time-zone-override", NULL, NULL));

    WebKitSettings *settings = webkit_settings_new_with_settings(
        "enable-developer-extras", TRUE,
        "enable-webgl", TRUE,
        "enable-media-stream", TRUE,
        "enable-encrypted-media", TRUE,
        NULL);

    wagner_output->wpe_view_backend_exportable = wpe_view_backend_exportable_fdo_egl_create(&exportableClient, wagner_output, 500, 500);
    wagner_output->wpe_view_backend = wpe_view_backend_exportable_fdo_get_view_backend(wagner_output->wpe_view_backend_exportable);
    WebKitWebViewBackend *webkit_view_backend = webkit_web_view_backend_new(wagner_output->wpe_view_backend, destry_view_backend, NULL);

    WebKitWebView *webView = WEBKIT_WEB_VIEW(g_object_new(WEBKIT_TYPE_WEB_VIEW,
                                                          "backend", webkit_view_backend,
                                                          "web-context", webContext,
                                                          "network-session", networkSession,
                                                          "settings", settings,
                                                          "user-content-manager", NULL,
                                                          NULL));
    g_object_unref(settings);

    webkit_web_view_load_uri(webView, "https://wpewebkit.org");
    wpe_view_backend_add_activity_state(wagner_output->wpe_view_backend, wpe_view_activity_state_visible | wpe_view_activity_state_focused | wpe_view_activity_state_in_window);
}
