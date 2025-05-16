using Wagner.Abstractions;
using Wagner.Protocols.Wayland.WlCallback.Events;

namespace Wagner.Protocols.Wayland.WlCallback;

public abstract class WlCallback : IWlInterface
{
    public abstract IWlClient Client { get; }

    public abstract uint Id { get; }

    public void Receive(ushort opcode, ReadOnlyMemory<byte> args)
    {
        throw new NotImplementedException();
    }

    public void Send(DoneEvent ev) => Client.Send(Id, ev);
}