using System.Runtime.InteropServices;

namespace Wagner.Drm.Native;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct NativeStructDrmModeInfo
{
    public uint clock;
    public ushort hdisplay, hsync_start, hsync_end, htotal, hskew;
	public ushort vdisplay, vsync_start, vsync_end, vtotal, vscan;

	public uint vrefresh;

	public uint flags;
	public uint type;
	public fixed byte name[32];
}
