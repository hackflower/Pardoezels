public class BinaryFairytaleTree<T> : IMyCollection<T>
{
    private class Node
    {
        public T Data { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(T data)
        {
            Data = data;
        }
    }

    private IComparer<T> _comparer;

    private Node? root;
    private int _count;
    private bool _dirty;

    public bool Dirty
    {
        get => _dirty;
        set => _dirty = value;
    }

    public int Count => _count;

    public BinaryFairytaleTree(IComparer<T> comparer)
    {
        _comparer = comparer;
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
            int compare = _comparer.Compare(item, current.Data);
            
            if (compare > 0) current = current.Right;
            else current = current.Left;
        }
            
        if (_comparer.Compare(item, parent.Data) > 0) parent.Right = new(item);
        else parent.Left = new(item);
        
        _count++;
    }

    public void Remove(T value)
    {
        Node? parent = null;
        Node? current = root;

        while (current != null)
        {
            int compare = _comparer.Compare(value, current.Data);

            if (compare == 0)
                break;

            parent = current;

            if (compare < 0)
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

            current.Data = successor.Data;

            current = successor;
            parent = successorParent;
        }

        Node? child = current.Left ?? current.Right;

        if (parent == null)
            root = child;
        else if (parent.Left == current)
            parent.Left = child;
        else
            parent.Right = child;

        _count--;
    }

    public Optional<T> FindBy<K>(K key, Func<T, K, int> comparer)
    {
        foreach (var item in this)
            if (comparer(item, key) == 0)
                return Optional<T>.Some(item);

        return Optional<T>.None();
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new BinaryFairytaleTree<T>(_comparer);

        foreach (var item in this)
            if (predicate(item))
                result.Add(item);

        return result;
    }

    public IMyCollection<R> Select<R>(Func<T, R> selector) where R : IComparable
    {
        var result = new BinaryFairytaleTree<R>(Comparer<R>.Default);

        foreach (var item in this)
            result.Add(selector(item));

        return result;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;

        foreach (var item in this)
            result = accumulator(result, item);

        return result;
    }

    public T Max(Comparison<T> comparison)
    {
        if (root == null)
            throw new InvalidOperationException();

        T max = root.Data;

        foreach (var item in this)
            if (comparison(item, max) > 0)
                max = item;

        return max;
    }

    public void Sort(Comparison<T> comparison)
    {
        var array = ToArray();
        
        _comparer = Comparer<T>.Create(comparison);

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
    

    public T[] ToArray()
    {
        T[] array = new T[_count];
        int index = 0;

        foreach (var item in this)
            array[index++] = item;

        return array;
    }

    public IMyCollection<T> FromArray(T[] array)
    {
        var newTree = new BinaryFairytaleTree<T>(_comparer);

        foreach (var item in array)
            newTree.Add(item);

        return newTree;
    }

    public override string ToString()
    {
        return "BinaryFairytaleTree";
    }
}