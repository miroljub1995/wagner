using System;
using System.Runtime.InteropServices;
using Wagner.Drm.Native;

namespace Wagner.Drm;

public static partial class DrmMode
{
    public static DrmModeResPtr GetResources(int fd) => DrmNative.drmModeGetResources(fd);
}
