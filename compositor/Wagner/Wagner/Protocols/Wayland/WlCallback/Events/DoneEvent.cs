using Wagner.Abstractions;

namespace Wagner.Protocols.Wayland.WlCallback.Events;

public class DoneEvent : IWlEvent
{
    public required uint CallbackData { get; init; }

    public ushort GetSize() => sizeof(uint);

    public ushort GetOpcode() => 0;

    public void SerializeArgs(Memory<byte> buffer)
    {
        BitConverter.TryWriteBytes(buffer.Span, CallbackData);
    }
}