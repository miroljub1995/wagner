using Wagner.Protocols.Wayland.WlCallback.Events;
using Wagner.Protocols.Wayland.WlDisplay;
using Wagner.Protocols.Wayland.WlDisplay.Requests;
using Wagner.Protocols.Wayland.WlRegistry.Events;

namespace Wagner.ProtocolsImpl.Wayland;

public class WlDisplayImpl(WlGlobalDisplay globalDisplay, WlClient client) : WlDisplay
{
    public override WlClient Client => client;

    public override uint Id => 1;

    protected override void Receive(SyncRequest request)
    {
        var cb = new WlCallbackImpl(client, request.Callback);
        client.AddObject(cb);

        cb.Send(new DoneEvent
        {
            CallbackData = 1, // TODO: get display serial
        });

        client.RemoveObject(cb.Id);
    }

    protected override void Receive(GetRegistryRequest request)
    {
        var registry = new WlRegistryImpl(globalDisplay, client, request.Registry);
        client.AddObject(registry);

        foreach (var globalPair in globalDisplay.Globals)
        {
            registry.Send(new GlobalEvent
            {
                Interface = globalPair.Value.Interface,
                Name = globalPair.Key,
                Version = globalPair.Value.Version,
            });
        }
    }
}