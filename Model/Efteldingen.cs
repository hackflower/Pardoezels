using System.Collections;

public class Efteldingen<T> : IMyCollection<T>, IEnumerable<T>
{
    private T[] _data = null!;
    private int _count;
    private bool _dirty;

    public bool Dirty
    {
        get
        {
            return _dirty;
        }
        set
        {
            _dirty = value;
        }
    }
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

    public Optional<T> FindBy<K>(K key, Func<T, K, int> comparer)
    {
        for (int i = 0; i < _count; i++)
        {
            if (comparer(_data[i], key) == 0)
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

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        Efteldingen<T> result = [];

        for (int i = 0; i < _count; i++)
        {
            if (predicate(_data[i]))
            {
                result.Add(_data[i]);
            }
        }

        return result;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        for (int i = 0; i < _count; i++)
        {
            result = accumulator(result, _data[i]);
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

    public IMyCollection<R> Select<R>(Func<T, R> selector)
    {
        var newList = new Efteldingen<R>();

        for (int i = 0; i < _count; i++)
        {
            newList.Add(selector(_data[i]));
        }
        return newList;
    }

    public Efteldingen<R> Map<R>(Func<T, R> selector)
    {
        if (_count <= 0) throw new InvalidOperationException("Collection is empty.");

        var result = new Efteldingen<R>();

        foreach (var item in _data)
        {
            result.Add(selector(item));
        }

        return result;
    }

    public T Max(Comparison<T> comparison)
    {
        if (_count <= 0) throw new InvalidOperationException("Collection is empty.");

        T max = _data[0];
        for (int i = 1; i < _count; i++)
            if (comparison(_data[i], max) > 0)
                max = _data[i];

        return max;
    }

    private void Resize()
    {
        T[] newArray = new T[_count * 2];

        for (int i = 0; i < _count; i++)
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

    public T[] ToArray()
    {
        T[] array = new T[_count];
        for (int i = 0; i < _count; i++)
        {
            array[i] = _data[i];
        }
        return array;
    }

    public IMyCollection<T> FromArray(T[] array)
    {
        Efteldingen<T> efteldingen = [];
        for (int i = 0; i < array.Length; i++)
        {
            efteldingen.Add(array[i]);
        }
        return efteldingen;
    }

    public override string ToString()
    {
        return string.Join(", ", this.ToArray());
    }
}