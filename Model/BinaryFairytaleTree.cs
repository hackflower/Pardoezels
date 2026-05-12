 
public class BinarySearchTree<T> : IMyCollection<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value;
        public Node Left;
        public Node Right;

        public Node(T value)
        {
            Value = value;
        }
    }

    private Node root;
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
    }
}