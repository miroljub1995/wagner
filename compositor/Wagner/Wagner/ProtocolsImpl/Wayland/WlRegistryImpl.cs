using Wagner.Protocols.Wayland.WlRegistry;

namespace Wagner.ProtocolsImpl.Wayland;

public class WlRegistryImpl(WlGlobalDisplay globalDisplay, WlClient client, uint id) : WlRegistry
{
    public override WlClient Client => client;

    public override uint Id => id;
}