using System;
using System.Runtime.InteropServices;

namespace Wagner.Drm.Native;

public partial class DrmNative
{
    private const string LIBRARY_NAME = "libdrm";

    [LibraryImport(LIBRARY_NAME)]
    public static partial int drmGetDevices2(uint flags, nint devices, int maxDevices);

    [LibraryImport(LIBRARY_NAME)]
    public static partial void drmFreeDevice(ref nint device);

    [LibraryImport(LIBRARY_NAME)]
    public static partial DrmModeResPtr drmModeGetResources(int fd);
}
