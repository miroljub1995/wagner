namespace Wagner.Helpers;

public readonly ref struct SpanElementsDisposer<TElement>(Span<TElement> elements) : IDisposable
    where TElement : IDisposable
{
    private readonly Span<TElement> _elements = elements;

    public void Dispose()
    {
        foreach (var e in _elements)
        {
            e.Dispose();
        }
    }
}
