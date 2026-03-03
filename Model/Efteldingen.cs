using System.Collections;

public class Efteldingen<T> : IEfteldingen<T>, IEnumerable<T>
{
    private T[] _data = null!;
    private int _count;

    public int Count => _count;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();

            return _data[index];
        }
        set
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();

            _data[index] = value;
        }
    }

    public Efteldingen()
    {
        _data = new T[4];
        _count = 0;
    }
    public void Add(T task)
    {
        if (_count == _data.Length)
        {
            Resize();
        }

        _data[_count] = task;
        _count++;
    }

    public void Remove(T task)
    {
        for (int i = 0; i < _count; i++)
        {
            if (Equals(_data[i], task))
            {
                for (int j = i; j < _count - 1; j++)
                {
                    _data[j] = _data[j + 1];
                }

                _data[_count - 1] = default!;
                _count--;
                return;
            }
        }
    }

    public T? Find<K>(K key, Func<T, K, bool> comparer)
    {
        for (int i = 0; i < _count; i++)
        {
            if (comparer(_data[i], key))
            {
                return _data[i];
            }
        }

        return default;
    }

    private void Resize()
    {
        T[] newArray = new T[_data.Length * 2];

        for (int i = 0; i < _data.Length; i++)
        {
            newArray[i] = _data[i];
        }

        _data = newArray;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return _data[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}