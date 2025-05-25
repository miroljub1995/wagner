using System.Runtime.InteropServices;

namespace Wagner.Drm.Native;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct NativeStructDrmModeRes
{
    public int count_fbs;
    public uint* fbs;

    public int count_crtcs;
    public uint* crtcs;

    public int count_connectors;
    public uint* connectors;

    public int count_encoders;
    public uint* encoders;

    public int min_width, max_width;
    public int min_height, max_height;
}
