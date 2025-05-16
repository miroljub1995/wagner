using Wagner.Abstractions;
using Wagner.Protocols.Wayland.WlRegistry.Events;

namespace Wagner.Protocols.Wayland.WlRegistry;

public abstract class WlRegistry : IWlInterface
{
    public abstract IWlClient Client { get; }

    public abstract uint Id { get; }

    public void Receive(ushort opcode, ReadOnlyMemory<byte> data)
    {
        throw new NotImplementedException();
    }

    public void Send(GlobalEvent ev) => Client.Send(Id, ev);
}