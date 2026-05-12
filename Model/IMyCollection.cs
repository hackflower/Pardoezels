public interface IMyCollection<T>
{
    void Add(T item);
    void Remove(T item);
    Optional<T> FindBy<K>(K key, Func<T, K, int> comparer);
    IMyCollection<T> Filter(Func<T, bool> predicate);
    void Sort(Comparison<T> comparison);
    int Count { get; }
    bool Dirty { get; set; }
    T Max(Comparison<T> comparison);
    R Reduce<R>(R initial, Func<R, T, R> accumulator);
    IEnumerator<T> GetEnumerator();
    IMyCollection<R> Select<R>(Func<T, R> selector)
    where R : IComparable<R>;
    T[] ToArray();
    IMyCollection<T> FromArray(T[] array);
}
