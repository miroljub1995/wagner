using Wagner.Abstractions;

namespace Wagner.Protocols.Wayland.WlDisplay.Requests;

public class SyncRequest
{
    public uint Callback { get; }

    public SyncRequest(IWlClient client, ReadOnlyMemory<byte> args)
    {
        Callback = BitConverter.ToUInt32(args.Span);
    }
}