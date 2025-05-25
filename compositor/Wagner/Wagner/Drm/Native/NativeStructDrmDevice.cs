using System.Runtime.InteropServices;

namespace Wagner.Drm.Native;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct NativeStructDrmDevice
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Businfo
    {
        [FieldOffset(0)]
        void* pci;

        [FieldOffset(0)]
        void* usb;

        [FieldOffset(0)]
        void* platform;

        [FieldOffset(0)]
        void* host1x;
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Deviceinfo
    {
        [FieldOffset(0)]
        void* pci;

        [FieldOffset(0)]
        void* usb;

        [FieldOffset(0)]
        void* platform;

        [FieldOffset(0)]
        void* host1x;
    }

    public byte** nodes;
    public int available_nodes;
    public int bustype;
    public Businfo* businfo;
    public Deviceinfo* deviceinfo;
}
