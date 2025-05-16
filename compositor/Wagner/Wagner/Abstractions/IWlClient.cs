namespace Wagner.Abstractions;

public interface IWlClient
{
    void AddObject(IWlInterface obj);

    TInterface GetObject<TInterface>(uint id)
        where TInterface : IWlInterface;

    void Send(uint id, IWlEvent ev);
    void RemoveObject(uint id);
}