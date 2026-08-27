using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class ProgressTracker : IProgressTracker, IProgressSet
{
	private readonly object _locker = new object();

	public int Progress
	{
		get
		{
			lock (_locker)
			{
				int num = MinProgress;
				try
				{
					num += (int)((float)(MaxProgress - MinProgress) * ((float)CurrentIndex / (float)TotalCount));
				}
				catch (Exception)
				{
				}
				return num;
			}
		}
	}

	public uint CurrentIndex { get; private set; }

	public uint TotalCount { get; private set; }

	public int MinProgress { get; private set; }

	public int MaxProgress { get; private set; }

	public ProgressTracker()
	{
		CurrentIndex = 0u;
		TotalCount = 1u;
		MinProgress = 0;
		MaxProgress = 1;
	}

	public void SetProgressMinMax(int min, int max)
	{
		lock (_locker)
		{
			CurrentIndex = 0u;
			TotalCount = 1u;
			MaxProgress = System.Math.Max(min, max);
			MinProgress = System.Math.Min(min, max);
		}
	}

	public void SetProgressCurrentTotal(uint current, uint total)
	{
		lock (_locker)
		{
			CurrentIndex = current;
			TotalCount = System.Math.Max(current, total);
		}
	}
}
