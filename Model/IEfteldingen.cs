interface IEfteldingen<T>
{
    void Add(T task);
    void Remove(T task);
    T? Find<K>(K key, Func<T, K, bool> comparer);
    Efteldingen<T> Filter(Func<T, bool> predicate);
    void Sort(Comparison<T> comparison);
    int Count { get; }
    IEnumerator<T> GetEnumerator();
}
