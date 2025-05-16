namespace Wagner.Abstractions;

public interface IWlEvent
{
    ushort GetSize();
    ushort GetOpcode();
    void SerializeArgs(Memory<byte> buffer);
}