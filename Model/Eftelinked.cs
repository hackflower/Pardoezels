
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
    private Node<T>? head;
    private int count;
    public int Count => count;

    public Optional<T> Find<K>(K key, Func<T, K, bool> comparer)
    {
        var current = head;

        while (current != null)
        {
            if (comparer(current.Data, key))
            {
                return Optional<T>.Some(current.Data);
            }
            current = current.Next;
        }
        return Optional<T>.None();
    }

    public Eftelinked<T> Filter(Func<T, bool> predicate)
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
    }

    public void Remove(T item)
    {
        if (head == null) return;

        if (head.Data.Equals(item))
        {
            head = head.Next;
            count--;
            return;
        }

        var current = head;
        
        while (!current.Next.Equals(null))
        {
            if (current.Next.Equals(item))
            {
                current.Next = current.Next.Next;
                count--;
                return;
            }
            current = current.Next;
        }
    }
}