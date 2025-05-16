using Wagner.Abstractions;
using Wagner.Protocols.Wayland.WlCallback;

namespace Wagner.ProtocolsImpl.Wayland;

public class WlCallbackImpl(IWlClient client, uint id) : WlCallback
{
    public override IWlClient Client => client;
    public override uint Id => id;
}