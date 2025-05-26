using System.Runtime.InteropServices;

namespace Wagner.Drm.Native;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct NativeStructDrmModeConnector
{
    public uint connector_id;
    public uint encoder_id;
    public uint connector_type;
    public uint connector_type_id;
    public DrmModeConnection connection;
    public uint mmWidth, mmHeight;
    public DrmModeSubPixel subpixel;

    public int count_modes;
    public NativeStructDrmModeInfo* modes;

    public int count_props;
    public uint* props;
    public ulong* prop_values;

    public int count_encoders;
    public uint* encoders;
}
