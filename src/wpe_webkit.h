#ifndef WG_WPE_WEBKIT_H
#define WG_WPE_WEBKIT_H

#include <EGL/egl.h>
#include <GL/gl.h>

GLuint wg_create_wpe_view_texture(GLsizei width, GLsizei height);
void wg_initialize_wpe_webkit(EGLDisplay display);

#endif
