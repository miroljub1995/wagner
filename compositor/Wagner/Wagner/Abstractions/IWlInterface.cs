namespace Wagner.Abstractions;

public interface IWlInterface
{
    IWlClient Client { get; }

    uint Id { get; }

    void Receive(ushort opcode, ReadOnlyMemory<byte> args);
}