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

    public override string ToString()
    {
        return string.Join(", ", this.ToArray());
    }

    public void Add(T item)
    {
        throw new NotImplementedException();
    }

    public void Remove(T value)
    {
        Node parent = null;
        Node current = root;

        while (current != null && !current.Value.Equals(value))
        {
            parent = current;

            if (value.CompareTo(current.Value) < 0)
                current = current.Left;
            else
                current = current.Right;
        }

        if (current == null)
            return;

        if (current.Left != null && current.Right != null)
        {
            Node successorParent = current;
            Node successor = current.Right;

            while (successor.Left != null)
            {
                successorParent = successor;
                successor = successor.Left;
            }

            current.Value = successor.Value;

            current = successor;
            parent = successorParent;
        }

        Node child = (current.Left != null) ? current.Left : current.Right;

        if (parent == null)
        {
            root = child;
        }
        else if (parent.Left == current)
        {
            parent.Left = child;
        }
        else
        {
            parent.Right = child;
        }
        _count--;
     }

    public Optional<T> FindBy<K>(K key, Func<T, K, int> comparer)
    {
        Node current = root;

        while (current != null)
        {
            int comparison = comparer(current.Value, key);

            if (comparison == 0)
                return Optional<T>.Some(current.Value);

            if (comparison > 0)
                current = current.Left;
            else
                current = current.Right;
        }

        return Optional<T>.None();
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new BinarySearchTree<T>();
        var stack = new Stack<Node>();
        var current = root;
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

            if (predicate(current.Value))
                result.Add(current.Value);

            current = current.Right;
        }

        return result;
    }

    public T Max(Comparison<T> comparison)
    {
        if (root == null)
            throw new InvalidOperationException("Tree is empty");

        var stack = new Stack<Node>();
        stack.Push(root);

        T max = root.Value;

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (comparison(current.Value, max) > 0)
                max = current.Value;

            if (current.Left != null)
                stack.Push(current.Left);

            if (current.Right != null)
                stack.Push(current.Right);
        }

        return max;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;

        var stack = new Stack<Node>();
        var current = root;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            current = stack.Pop();

            result = accumulator(result, current.Value);

            current = current.Right;
        }

        return result;
    }

    public IMyCollection<R> Select<R>(Func<T, R> selector)
        where R : IComparable<R>
    {
        var result = new BinarySearchTree<R>();

        var stack = new Stack<Node>();
        var current = root;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            current = stack.Pop();

            R newValue = selector(current.Value);
            result.Insert(newValue);

            current = current.Right;
        }

        return result;
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