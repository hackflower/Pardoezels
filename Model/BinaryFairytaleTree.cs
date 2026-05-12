<<<<<<< HEAD
using System;
using System.Collections;
using System.Collections.Generic;

public class BinaryFairytaleTree<T> : IMyCollection<T>, IEnumerable<T>
    where T : IComparable<T>
=======
public class BinaryFairytaleTree<T> : IMyCollection<T> where T : IComparable<T>
>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
{
    private class Node
    {
        public T Data { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(T data)
        {
            Data = data;
<<<<<<< HEAD
=======
            Left = null;
            Right = null;
>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
        }
    }

    private Node? root;
    private int _count;
    private bool _dirty;

    public bool Dirty
    {
<<<<<<< HEAD
        get => _dirty;
        set => _dirty = value;
    }

    public int Count => _count;

    public void Add(T item)
    {
        if (root == null)
        {
            root = new Node(item);
            _count++;
            return;
        }

        Node current = root;
        Node parent;

        while (true)
        {
            parent = current;
            int compare = item.CompareTo(current.Data);

            if (compare > 0)
            {
                if (current.Right == null)
                {
                    current.Right = new Node(item);
                    break;
                }
                current = current.Right;
            }
            else
            {
                if (current.Left == null)
                {
                    current.Left = new Node(item);
                    break;
                }
                current = current.Left;
            }
        }

        _count++;
=======
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
>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
    }

    public void Remove(T value)
    {
<<<<<<< HEAD
        Node? parent = null;
        Node? current = root;

        while (current != null && !current.Data.Equals(value))
        {
            parent = current;

            if (value.CompareTo(current.Data) < 0)
=======
        Node parent = null;
        Node current = root;

        while (current != null && !current.Value.Equals(value))
        {
            parent = current;

            if (value.CompareTo(current.Value) < 0)
>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
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

<<<<<<< HEAD
            current.Data = successor.Data;
=======
            current.Value = successor.Value;

>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
            current = successor;
            parent = successorParent;
        }

<<<<<<< HEAD
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
        Node? current = root;

        while (current != null)
        {
            int comparison = comparer(current.Data, key);

            if (comparison == 0)
                return Optional<T>.Some(current.Data);

            current = comparison > 0 ? current.Left : current.Right;
=======
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
>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
        }

        return Optional<T>.None();
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
<<<<<<< HEAD
        var result = new BinaryFairytaleTree<T>();

        foreach (var item in this)
            if (predicate(item))
                result.Add(item);

        return result;
    }

    public IMyCollection<R> Select<R>(Func<T, R> selector)
        where R : IComparable<R>
    {
        var result = new BinaryFairytaleTree<R>();

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
=======
        var result = new BinarySearchTree<T>();
        var stack = new Stack<Node>();
        var current = root;
    public void Sort(Comparison<T> comparison)
    {
        var array = ToArray();

>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
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
<<<<<<< HEAD
            yield return current.Data;
=======

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

>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
            current = current.Right;
        }
    }

<<<<<<< HEAD
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
=======
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
>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97

    public T[] ToArray()
    {
        T[] array = new T[_count];
        int index = 0;

<<<<<<< HEAD
        foreach (var item in this)
            array[index++] = item;

=======
        foreach (var item in this) array[index++] = item;
>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
        return array;
    }

    public IMyCollection<T> FromArray(T[] array)
    {
        var newTree = new BinaryFairytaleTree<T>();
<<<<<<< HEAD

        foreach (var item in array)
            newTree.Add(item);

=======
        foreach (var item in array) newTree.Add(item);
>>>>>>> 06eb63d8a2457d83ee9e40f49064a49aaae6ce97
        return newTree;
    }

    public override string ToString()
    {
        return string.Join(", ", ToArray());
    }
}