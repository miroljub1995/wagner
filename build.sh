WPE_WEBKIT_FLAGS=`pkg-config --cflags wpebackend-fdo-1.0 wpe-webkit-2.0`
WPE_WEBKIT_FLAGS+=" -Wl,-rpath,/usr/local/lib"

WPE_WEBKIT_LIBS=`pkg-config --libs wpebackend-fdo-1.0 wpe-webkit-2.0 glib-2.0`

gcc -Werror \
		-DWLR_USE_UNSTABLE \
        `pkg-config --cflags wlroots` \
        ${WPE_WEBKIT_FLAGS} \
        -I /usr/include/wpe-webkit-2.0  \
		-o wagner \
        src/main.c \
        src/main_loop.c \
        src/wpe_webkit.c \
		`pkg-config --libs wlroots wayland-client wayland-server xkbcommon egl glesv2` \
        ${WPE_WEBKIT_LIBS}
