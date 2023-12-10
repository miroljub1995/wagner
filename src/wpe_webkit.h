#ifndef WG_WPE_WEBKIT_H
#define WG_WPE_WEBKIT_H

#include <EGL/egl.h>
#include <GLES2/gl2.h>

GLuint wg_create_wpe_view_texture(GLsizei width, GLsizei height);
void wg_create_shader_program(GLuint *program, GLint *u_texture);
void wg_initialize_wpe_webkit(EGLDisplay display);

#endif
