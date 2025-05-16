using Wagner.Abstractions;

namespace Wagner.Protocols.Wayland.WlDisplay.Requests;

public class GetRegistryRequest
{
    public uint Registry { get; }

    public GetRegistryRequest(IWlClient client, ReadOnlyMemory<byte> args)
    {
        Registry = BitConverter.ToUInt32(args.Span);
    }
}