public class BinaryFairytaleTree<T> : IMyCollection<T> where T : IComparable<T>
{
    private class Node
    {
        public T Data { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }

    private Node? root;
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

    public void Sort(Comparison<T> comparison)
    {
        var array = ToArray();

        Array.Sort(array, comparison);

        root = null;
        _count = 0;

        foreach (var item in array)
            Add(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        Stack<Node> stack = new();
        Node? current = root;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            current = stack.Pop();

            yield return current.Data;

            current = current.Right;
        }
    }

    public void Add(T item)
    {
        if (root == null) {
            root = new Node(item);
            _count++;
            return;
        }

        Node? current = root;
        Node parent = current;

        while (current != null) {
            parent = current;
            int compare = item.CompareTo(current.Data);

            if (compare > 0) current = current.Right;
            else current = current.Left;
        }

        if (item.CompareTo(parent.Data) > 0) parent.Right = new(item);
        else parent.Left = new(item);

        _count++;
    }

    public T[] ToArray()
    {
        T[] array = new T[_count];
        int index = 0;

        foreach (var item in this) array[index++] = item;
        return array;
    }

    public IMyCollection<T> FromArray(T[] array)
    {
        var newTree = new BinaryFairytaleTree<T>();
        foreach (var item in array) newTree.Add(item);
        return newTree;
    }

    public override string ToString()
    {
        return string.Join(", ", ToArray());
    }
}