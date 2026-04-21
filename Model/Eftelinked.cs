
public class Node<T>
{
    public T Data { get; set; }
    public Node<T>? Next { get; set; }

    public Node(T data)
    {
        Data = data;
        Next = null;
    }
}

public class Eftelinked<T> : IMyCollection<T>
{
    public Node<T>? head;
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

    public Optional<T> FindBy<K>(K key, Func<T, K, int> comparer)
    {
        var current = head;

        while (current != null)
        {
            if (comparer(current.Data, key) == 0)
            {
                return Optional<T>.Some(current.Data);
            }
            current = current.Next;
        }
        return Optional<T>.None();
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var newList = new Eftelinked<T>();
        var current = head;

        while (current != null)
        {
            if (predicate(current.Data))
            {
                newList.Add(current.Data);
            }
            current = current.Next;
        }
        return newList;
    }

    public void Sort(Comparison<T> comparison)
    {
        if (head == null || head.Next == null)
        return;

        bool swapped;

        do
        {
            swapped = false;
            var current = head;

            while (current.Next != null)
            {
                if (comparison(current.Data, current.Next.Data) > 0)
                {
                    var temp = current.Data;
                    current.Data = current.Next.Data;
                    current.Next.Data = temp;

                    swapped = true;
                }

                current = current.Next;
            }

        } while (swapped);
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = head;

        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    public void Add(T item)
    {
        var newNode = new Node<T>(item);

        if (head == null)
        {
            head = newNode;
        }

        else
        {
            var current = head;

            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
        }

        _count++;
    }

    public void Remove(T item)
    {
        if (head == null || head.Data == null) return;

        if (head.Data.Equals(item))
        {
            head = head.Next;
            _count--;
            return;
        }

        var current = head;
        
        while (!current.Next!.Equals(null))
        {
            if (current.Next.Equals(item))
            {
                current.Next = current.Next.Next;
                _count--;
                return;
            }
            current = current.Next;
        }
    }
    public IMyCollection<R> Select<R>(Func<T, R> selector)
    {
        var newList = new Eftelinked<R>();
        var current = head;

        while (current != null)
        {
            newList.Add(selector(current.Data));
            current = current.Next;
        }
        return newList;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        var current = head;

        while (current != null)
        {
            result = accumulator(result, current.Data);
            current = current.Next;
        }
        return result;
    }

    public T[] ToArray()
    {
        T[] array = new T[_count];
        var current = head;
        int index = 0;

        while (current != null)
        {
            array[index++] = current.Data;
            current = current.Next;
        }
        return array;
    }

    public IMyCollection<T> FromArray(T[] array)
    {
        var newList = new Eftelinked<T>();
        foreach (var item in array)
        {
            newList.Add(item);
        }
        return newList;
    }

    public T Max(Comparison<T> comparison)
    {
        if (head == null) throw new InvalidOperationException("Collection is empty.");

        T max = head.Data;
        var current = head.Next;

        while (current != null)
        {
            if (comparison(current.Data, max) > 0)
            {
                max = current.Data;
            }

            current = current.Next;
        }

        return max;
    }
}