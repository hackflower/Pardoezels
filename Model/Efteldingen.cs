public class Efteldingen<T> : IEfteldingen<T>
{
    private T[] _data = null!;
    private int _count;

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

        _data.Append(task);
        _count++;
    }

    public void Remove(T Task)
    {
        
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
}