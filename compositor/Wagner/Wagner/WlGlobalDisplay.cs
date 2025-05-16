using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Wagner;

public class WlGlobalDisplay
{
    private readonly Dictionary<uint, WlGlobal> _globals = [];
    private readonly List<WlClient> _clients = [];

    private readonly BlockingCollection<Action> _actionsQueue = new();

    private Task _loop;

    public IReadOnlyDictionary<uint, WlGlobal> Globals => _globals;

    public WlGlobalDisplay()
    {
        _loop = Task.Run(() =>
        {
            while (true)
            {
                var action = _actionsQueue.Take();
                action();
            }
        });
    }

    public WlGlobalDisplay AddToRegistry()
    {
        throw new NotImplementedException();
        return this;
    }

    public void Enqueue(Action action) => _actionsQueue.Add(action);

    public async Task RunAsync(CancellationToken token)
    {
        var socketPath = Path.Join(Path.GetTempPath(), "wayland-0");
        File.Delete(socketPath);

        var endPoint = new UnixDomainSocketEndPoint(socketPath);
        var listener = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

        listener.Bind(endPoint);
        listener.Listen();

        while (!token.IsCancellationRequested)
        {
            var clientSocket = await listener.AcceptAsync(token);
            Console.WriteLine("Client connected.");

            _clients.Add(new WlClient(this, clientSocket));
        }
    }
}