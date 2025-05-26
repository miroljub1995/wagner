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

    public uint EncoderId
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return thisPtr->encoder_id;
            }
        }
    }

    public uint ConnectorType
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return thisPtr->connector_type;
            }
        }
    }

    public uint ConnectorTypeId
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return thisPtr->connector_type_id;
            }
        }
    }

    public DrmModeConnection Connection
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return thisPtr->connection;
            }
        }
    }

    public uint MmWidth
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return thisPtr->mmWidth;
            }
        }
    }

    public uint MmHeight
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return thisPtr->mmHeight;
            }
        }
    }

    public DrmModeSubPixel SubPixel
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return thisPtr->subpixel;
            }
        }
    }

    public Span<ValueDrmModeInfo> Modes
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeConnector* thisPtr = (NativeStructDrmModeConnector*)handle;
                return new(thisPtr->modes, thisPtr->count_modes);
            }
        }
    }
}
