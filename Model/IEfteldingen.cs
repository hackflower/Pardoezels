interface IEfteldingen<T>
{
    void Add(T task);
    void Remove(T task);
    T? FindBy<K>(K key, Func<T, K, bool> comparer);
}