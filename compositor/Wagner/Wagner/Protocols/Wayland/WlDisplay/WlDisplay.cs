using Wagner.Abstractions;
using Wagner.Protocols.Wayland.WlDisplay.Requests;

namespace Wagner.Protocols.Wayland.WlDisplay;

public abstract class WlDisplay : IWlInterface
{
    public abstract IWlClient Client { get; }

    public abstract uint Id { get; }

    public void Receive(ushort opcode, ReadOnlyMemory<byte> args)
    {
        if (opcode == 0)
        {
            Receive(new SyncRequest(Client, args));
        }
        else if (opcode == 1)
        {
            Receive(new GetRegistryRequest(Client, args));
        }
    }

    protected abstract void Receive(SyncRequest request);

    protected abstract void Receive(GetRegistryRequest request);
}