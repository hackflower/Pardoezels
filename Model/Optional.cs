public readonly struct Optional<T>
{
    private readonly T? _value;
    public T Value
    {
        get
        {
            if (HasValue)
            {
                return _value!;
            }
            throw new InvalidOperationException("Not a value present");
        }
    }
    public bool HasValue { get; }

    private Optional(T? value, bool hasValue)
    {
        _value = value;
        HasValue = hasValue;
    }

    public static Optional<T> Some(T value)
    {
        return new Optional<T>(value, true);
    }

    public static Optional<T> None()
    {
        return new Optional<T>(default, false);
    }
}