using System.Runtime.InteropServices;

namespace Wagner.Drm;

[StructLayout(LayoutKind.Sequential)]
public struct DrmModeResPtr
{
    private nint _handle;

    [StructLayout(LayoutKind.Sequential)]
    private unsafe struct NativeStruct
    {
        public int count_fbs;
        public int* fbs;

        public int count_crtcs;
        public int* crtcs;

        public int count_connectors;
        public int* connectors;

        public int count_encoders;
        public int* encoders;

        public int min_width, max_width;
        public int min_height, max_height;
    }

    public readonly bool IsNull => _handle == nint.Zero;

    public readonly Span<int> Fbs
    {
        get
        {
            unsafe
            {
                NativeStruct* thisPtr = (NativeStruct*)_handle;
                return new Span<int>(thisPtr->fbs, thisPtr->count_fbs);
            }
        }
    }

    public readonly Span<int> Crtcs
    {
        get
        {
            unsafe
            {
                NativeStruct* thisPtr = (NativeStruct*)_handle;
                return new Span<int>(thisPtr->crtcs, thisPtr->count_crtcs);
            }
        }
    }

    public readonly Span<int> Connectors
    {
        get
        {
            unsafe
            {
                NativeStruct* thisPtr = (NativeStruct*)_handle;
                return new Span<int>(thisPtr->connectors, thisPtr->count_connectors);
            }
        }
    }

    public readonly Span<int> Encoders
    {
        get
        {
            unsafe
            {
                NativeStruct* thisPtr = (NativeStruct*)_handle;
                return new Span<int>(thisPtr->encoders, thisPtr->count_encoders);
            }
        }
    }

    public readonly int MinWidth
    {
        get
        {
            unsafe
            {
                NativeStruct* thisPtr = (NativeStruct*)_handle;
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
                NativeStruct* thisPtr = (NativeStruct*)_handle;
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
                NativeStruct* thisPtr = (NativeStruct*)_handle;
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
                NativeStruct* thisPtr = (NativeStruct*)_handle;
                return thisPtr->max_height;
            }
        }
    }
}
