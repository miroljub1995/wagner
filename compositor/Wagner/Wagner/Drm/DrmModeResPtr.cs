using System.Runtime.InteropServices;

namespace Wagner.Drm;

[StructLayout(LayoutKind.Sequential)]
public struct DrmModeResPtr
{
    private nint _handle;

    public readonly bool IsNull => _handle == nint.Zero;

    public readonly Span<int> Fbs
    {
        get
        {
            int count = Marshal.ReadInt32(_handle);
            nint ptr = Marshal.ReadIntPtr(_handle, sizeof(int));
            unsafe
            {
                return new Span<int>(ptr.ToPointer(), count);
            }
        }
    }

    public readonly Span<int> Crtcs
    {
        get
        {
            int count = Marshal.ReadInt32(_handle, sizeof(int) + nint.Size);
            nint ptr = Marshal.ReadIntPtr(_handle, sizeof(int) + nint.Size + sizeof(int));
            unsafe
            {
                return new Span<int>(ptr.ToPointer(), count);
            }
        }
    }

    public readonly Span<int> Connectors
    {
        get
        {
            int count = Marshal.ReadInt32(_handle, sizeof(int) + nint.Size + sizeof(int) + nint.Size);
            nint ptr = Marshal.ReadIntPtr(_handle, sizeof(int) + nint.Size + sizeof(int) + nint.Size + sizeof(int));
            unsafe
            {
                return new Span<int>(ptr.ToPointer(), count);
            }
        }
    }
}
