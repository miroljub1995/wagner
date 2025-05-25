using System.Runtime.InteropServices;
using Wagner.Drm.Native;

namespace Wagner.Drm;

[StructLayout(LayoutKind.Sequential)]
public struct DrmDevicePtr : IDisposable
{
    private nint _handle;

    public readonly DrmAvailableNode AvailableNodes
    {
        get
        {
            unsafe
            {
                NativeStructDrmDevice* thisPtr = (NativeStructDrmDevice*)_handle;
                return (DrmAvailableNode)thisPtr->available_nodes;
            }
        }
    }

    public readonly int BusType
    {
        get
        {
            unsafe
            {
                NativeStructDrmDevice* thisPtr = (NativeStructDrmDevice*)_handle;
                return thisPtr->bustype;
            }
        }
    }

    public unsafe string GetNode(DrmNode node)
    {
        unsafe
        {
            NativeStructDrmDevice* thisPtr = (NativeStructDrmDevice*)_handle;
            Span<nint> nodes = new(thisPtr->nodes, 4);
            nint nodePtr = nodes[(int)node];
            return Marshal.PtrToStringAnsi(nodePtr)!;
        }
    }

    public void Dispose()
    {
        DrmNative.drmFreeDevice(ref _handle);
    }
}
