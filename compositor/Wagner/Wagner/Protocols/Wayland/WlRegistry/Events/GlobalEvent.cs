using Wagner.Abstractions;

namespace Wagner.Protocols.Wayland.WlRegistry.Events;

public class GlobalEvent : IWlEvent
{
    public required uint Name { get; init; }

    public required string Interface { get; init; }

    public required uint Version { get; init; }

    public ushort GetSize()
    {
        throw new NotImplementedException();
    }

    public ushort GetOpcode()
    {
        throw new NotImplementedException();
    }

    public void SerializeArgs(Memory<byte> buffer)
    {
        throw new NotImplementedException();
    }
}