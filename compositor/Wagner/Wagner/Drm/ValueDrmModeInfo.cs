using System.Runtime.InteropServices;
using Wagner.Drm.Native;

namespace Wagner.Drm;

[StructLayout(LayoutKind.Sequential)]
public struct ValueDrmModeInfo
{
    private NativeStructDrmModeInfo _value;

    public readonly uint Clock => _value.clock;

    public readonly ushort HDisplay => _value.hdisplay;
    public readonly ushort HSyncStart => _value.hsync_start;
    public readonly ushort HSyncEnd => _value.hsync_end;
    public readonly ushort HTotal => _value.htotal;
    public readonly ushort HSkew => _value.hskew;

    public readonly ushort VDisplay => _value.vdisplay;
    public readonly ushort VSyncStart => _value.vsync_start;
    public readonly ushort VSyncEnd => _value.vsync_end;
    public readonly ushort VTotal => _value.vtotal;
    public readonly ushort VScan => _value.vscan;

    public readonly uint VRefresh => _value.vrefresh;

    public readonly uint Flags => _value.flags;
    public readonly uint Type => _value.type;

    public readonly string Name
    {
        get
        {
            unsafe
            {
                fixed (byte* name = _value.name)
                {
                    return Marshal.PtrToStringAnsi((nint)name)!;
                }
            }
        }
    }
}
