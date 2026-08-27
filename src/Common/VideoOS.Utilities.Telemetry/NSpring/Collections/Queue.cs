namespace NSpring.Collections;

public abstract class Queue
{
	private static object[] EMPTY_OBJECT_ARRAY = new object[0];

	protected bool isOpen = true;

	protected int count;

	protected bool isNullAllowed;

	protected object monitor = new object();

	public bool IsOpen
	{
		get
		{
			lock (monitor)
			{
				return isOpen;
			}
		}
	}

	public int Count
	{
		get
		{
			lock (monitor)
			{
				return count;
			}
		}
	}

	public bool IsNullAllowed
	{
		get
		{
			lock (monitor)
			{
				return isNullAllowed;
			}
		}
		set
		{
			lock (monitor)
			{
				isNullAllowed = value;
			}
		}
	}

	public abstract void Add(object _object);

	public abstract object Peek();

	public abstract object Peek(int _millisecondsTimeout);

	public abstract object Remove();

	public abstract object Remove(int _millisecondsTimeout);

	public virtual object[] RemoveAll()
	{
		lock (monitor)
		{
			if (count == 0)
			{
				return EMPTY_OBJECT_ARRAY;
			}
			object[] array = new object[count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Remove();
			}
			return array;
		}
	}

	public void ReleaseWaitingThreads()
	{
		lock (monitor)
		{
			bool flag = isOpen;
			Close();
			isOpen = flag;
		}
	}

	public virtual void Open()
	{
		lock (monitor)
		{
			isOpen = true;
		}
	}

	public abstract void Close();

	public abstract void Clear();
}
