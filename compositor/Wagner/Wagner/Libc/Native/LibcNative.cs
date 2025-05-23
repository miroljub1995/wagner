using System;
using System.Runtime.InteropServices;

namespace Wagner.Libc.Native;

public partial class LibcNative
{
    private const string LIBRARY_NAME = "libc";

    [LibraryImport(LIBRARY_NAME, StringMarshalling = StringMarshalling.Utf8)]
    public static partial int open(string pathname, int flags);
}
