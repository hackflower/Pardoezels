using System.Collections;
using System.Text.Json;

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

    public Optional<T> Find<K>(K key, Func<T, K, bool> comparer)
    {
        for (int i = 0; i < _count; i++)
        {
            if (comparer(_data[i], key))
            {
                return Optional<T>.Some(_data[i]);
            }
        }

        return Optional<T>.None();
    }

    public void Sort(Comparison<T> comparison)
    {
        for (int i = 0; i < _count - 1; i++)
        {
            for (int j = 0; j < _count - i - 1; j++)
            {
                if (comparison(_data[j], _data[j + 1]) > 0)
                {
                    T temp = _data[j];
                    _data[j] = _data[j + 1];
                    _data[j + 1] = temp;
                }
            }
        }
    }

    public Efteldingen<T> Filter(Func<T, bool> predicate)
    {
        Efteldingen<T> result = new Efteldingen<T>();

        for (int i = 0; i < _count; i++)
        {
            if (predicate(_data[i]))
            {
                result.Add(_data[i]);
            }
        }

        return result;
    }

    public T? BinarySearch<K>(K key, Func<T, K, int> comparer)
    {
        int low = 0;
        int high = _count;

        while (high >= low)
        {
            int middle = (low + high) / 2;

            if (comparer(_data[middle], key) == 0)
            {
                return _data[middle];
            } else if (comparer(_data[middle], key) > 0)
            {
                low = middle + 1;
            } else
            {
                high = middle - 1;
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