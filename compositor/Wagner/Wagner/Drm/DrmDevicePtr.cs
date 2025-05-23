using System.Runtime.InteropServices;
using Wagner.Drm.Native;

namespace Wagner.Drm;

[StructLayout(LayoutKind.Sequential)]
public struct DrmDevicePtr : IDisposable
{
    private nint _handle;

    public readonly DrmAvailableNode AvailableNodes => (DrmAvailableNode)Marshal.ReadInt32(_handle, nint.Size);
    public readonly int BusType => Marshal.ReadInt32(_handle, nint.Size + sizeof(DrmAvailableNode));

    public unsafe string GetNode(DrmNode node)
    {
        nint nodesPtr = Marshal.ReadIntPtr(_handle);
        Span<nint> nodes = new Span<nint>((nint*)nodesPtr, 4);
        nint nodePtr = nodes[(int)node];
        return Marshal.PtrToStringAnsi(nodePtr)!;
    }

    public void Dispose()
    {
        DrmNative.drmFreeDevice(ref _handle);
    }
}
