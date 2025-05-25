using Microsoft.Win32.SafeHandles;
using Wagner.Drm.Native;

namespace Wagner.Drm;

public class DrmModeConnector : SafeHandleZeroOrMinusOneIsInvalid
{
    public DrmModeConnector() : this(true)
    {
    }

    public DrmModeConnector(bool ownsHandle) : base(ownsHandle)
    {
    }

    protected override bool ReleaseHandle()
    {
        DrmNative.drmModeFreeConnector(this);
        return true;
    }

    public static DrmModeConnector Get(int fd, uint id)
    {
        return DrmNative.drmModeGetConnector(fd, id);
    }

    public uint ConnectorId
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return thisPtr->connector_id;
            }
        }
    }
}
