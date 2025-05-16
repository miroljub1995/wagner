using System.Buffers;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Wagner.Abstractions;
using Wagner.ProtocolsImpl.Wayland;

namespace Wagner;

public class WlClient : IWlClient
{
    private record Event(
        uint ObjectId,
        IWlEvent WlEvent
    );

    private readonly WlGlobalDisplay _globalDisplay;
    private readonly Socket _socket;

    private readonly Dictionary<uint, IWlInterface> _objects = [];

    private readonly BlockingCollection<Event> _eventsQueue = new();

    private readonly Task _receiveLoop;
    private readonly Task _sendLoop;

    public WlClient(WlGlobalDisplay globalDisplay, Socket socket)
    {
        _globalDisplay = globalDisplay;
        _socket = socket;

        _objects.Add(1, new WlDisplayImpl(globalDisplay, this));

        _receiveLoop = Task.Run(async () =>
        {
            var header = new byte[8];

            try
            {
                while (true)
                {
                    var headerToReceive = header.AsMemory();
                    while (headerToReceive.Length > 0)
                    {
                        var received = await socket.ReceiveAsync(headerToReceive);
                        if (received == 0)
                        {
                            // TODO: handle client close
                        }

                        headerToReceive = headerToReceive[received..];
                    }

                    var id = BitConverter.ToUInt32(header);
                    var opcode = BitConverter.ToUInt16(header, 4);
                    var argsSize = BitConverter.ToUInt16(header, 6) - 8;

                    var argsMemoryOwner = MemoryPool<byte>.Shared.Rent(argsSize);

                    var argsToReceive = argsMemoryOwner.Memory[..argsSize];
                    while (argsToReceive.Length > 0)
                    {
                        var received = await socket.ReceiveAsync(argsToReceive);
                        argsToReceive = argsToReceive[received..];
                    }

                    _globalDisplay.Enqueue(() =>
                    {
                        using var _ = argsMemoryOwner;
                        _objects[id].Receive(opcode, argsMemoryOwner.Memory[..argsSize]);
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });

        _sendLoop = Task.Run(async () =>
        {
            try
            {
                byte[] header = new byte[8];

                while (true)
                {
                    var ev = _eventsQueue.Take();
                    BitConverter.TryWriteBytes(header, ev.ObjectId);

                    var opcode = ev.WlEvent.GetOpcode();
                    BitConverter.TryWriteBytes(header.AsSpan(4..), opcode);

                    var argsSize = ev.WlEvent.GetSize();
                    var size = Convert.ToUInt16(argsSize + 8);
                    BitConverter.TryWriteBytes(header.AsSpan(6..), size);

                    var headerToSend = header.AsMemory();
                    while (headerToSend.Length > 0)
                    {
                        var sent = await _socket.SendAsync(headerToSend);
                        headerToSend = headerToSend[sent..];
                    }

                    using var argsMemoryOwner = MemoryPool<byte>.Shared.Rent(argsSize);

                    var data = argsMemoryOwner.Memory[..argsSize];
                    ev.WlEvent.SerializeArgs(data);

                    var dataToSend = data;
                    while (dataToSend.Length > 0)
                    {
                        var sent = await _socket.SendAsync(dataToSend);
                        dataToSend = dataToSend[sent..];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        });
    }

    public void AddObject(IWlInterface obj) => _objects[obj.Id] = obj;

    public TInterface GetObject<TInterface>(uint id)
        where TInterface : IWlInterface
    {
        return (TInterface)_objects[id];
    }

    public void Send(uint id, IWlEvent ev) => _eventsQueue.Add(new Event(id, ev));

    public void RemoveObject(uint id) => _objects.Remove(id);
}