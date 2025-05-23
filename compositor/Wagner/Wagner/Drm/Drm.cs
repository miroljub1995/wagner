using Wagner.Drm.Native;

namespace Wagner.Drm;

public static class Drm
{
    public static Span<DrmDevicePtr> GetDevices2(uint flags)
    {
        DrmDevicePtr[] res = new DrmDevicePtr[256];

        int numOfDevices;

        unsafe
        {
            fixed (void* ptr = res)
            {
                numOfDevices = DrmNative.drmGetDevices2(flags, (nint)ptr, res.Length);
            }
        }

        return res.AsSpan(..numOfDevices);
    }
}
