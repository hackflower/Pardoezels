interface IEfteldingen<T>
{
    void Add(T task);
    void Remove(T task);
    T? Find<K>(K key, Func<T, K, bool> comparer);
}