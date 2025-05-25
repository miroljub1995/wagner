using System.Runtime.InteropServices;
using Wagner.Drm.Native;

namespace Wagner.Drm;

[StructLayout(LayoutKind.Sequential)]
public struct DrmModeResPtr
{
    private nint _handle;

    public readonly bool IsNull => _handle == nint.Zero;

    public readonly Span<uint> Fbs
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeRes* thisPtr = (NativeStructDrmModeRes*)_handle;
                return new Span<uint>(thisPtr->fbs, thisPtr->count_fbs);
            }
        }
    }

    public readonly Span<uint> Crtcs
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeRes* thisPtr = (NativeStructDrmModeRes*)_handle;
                return new Span<uint>(thisPtr->crtcs, thisPtr->count_crtcs);
            }
        }
    }

    public readonly Span<uint> Connectors
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeRes* thisPtr = (NativeStructDrmModeRes*)_handle;
                return new Span<uint>(thisPtr->connectors, thisPtr->count_connectors);
            }
        }
    }

    public readonly Span<uint> Encoders
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeRes* thisPtr = (NativeStructDrmModeRes*)_handle;
                return new Span<uint>(thisPtr->encoders, thisPtr->count_encoders);
            }
        }
    }

    public readonly int MinWidth
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeRes* thisPtr = (NativeStructDrmModeRes*)_handle;
                return thisPtr->min_width;
            }
        }
    }

    public readonly int MaxWidth
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeRes* thisPtr = (NativeStructDrmModeRes*)_handle;
                return thisPtr->max_width;
            }
        }
    }

    public readonly int MinHeight
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeRes* thisPtr = (NativeStructDrmModeRes*)_handle;
                return thisPtr->min_height;
            }
        }
    }

    public readonly int MaxHeight
    {
        get
        {
            unsafe
            {
                NativeStructDrmModeRes* thisPtr = (NativeStructDrmModeRes*)_handle;
                return thisPtr->max_height;
            }
        }
    }
}
