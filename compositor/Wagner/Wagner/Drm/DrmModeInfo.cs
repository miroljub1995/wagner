using Microsoft.Win32.SafeHandles;

namespace Wagner.Drm;

public class DrmModeInfo : SafeHandleZeroOrMinusOneIsInvalid
{
    public DrmModeInfo() : this(true)
    {
    }

    public DrmModeInfo(bool ownsHandle) : base(ownsHandle)
    {
    }

    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}
