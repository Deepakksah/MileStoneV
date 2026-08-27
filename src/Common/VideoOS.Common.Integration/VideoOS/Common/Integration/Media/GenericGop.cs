using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Media;

public class GenericGop : IFrameGroup, IReferenceCounted, IDisposable
{
	private List<GenericFrame> _frameEntries = new List<GenericFrame>();

	private int _refCount;

	private bool disposed;

	public bool IsDisposed
	{
		[DebuggerStepThrough]
		get
		{
			return disposed;
		}
	}

	public DateTime BeginTimestamp
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			lock (_frameEntries)
			{
				return _frameEntries[0].Timestamp;
			}
		}
	}

	public DateTime EndTimestamp
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			lock (_frameEntries)
			{
				return _frameEntries[_frameEntries.Count - 1].Timestamp;
			}
		}
	}

	public int FrameCount
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			lock (_frameEntries)
			{
				return _frameEntries.Count;
			}
		}
	}

	public ulong DataSize
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			lock (_frameEntries)
			{
				ulong num = 0uL;
				for (int i = 0; i < _frameEntries.Count; i++)
				{
					num += _frameEntries[i].FrameDataView.Length;
				}
				return num;
			}
		}
	}

	public GenericGop()
	{
	}

	public GenericGop(GenericFrame[] frames)
	{
		if (frames == null)
		{
			throw new ArgumentNullException("frames");
		}
		if (frames.Length == 0)
		{
			throw new ArgumentException("No frames", "frames");
		}
		for (int i = 0; i < frames.Length; i++)
		{
			AddFrame(frames[i]);
		}
	}

	public void IncrementReferenceCount()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		Interlocked.Increment(ref _refCount);
	}

	public void DecrementReferenceCount()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		if (Interlocked.Decrement(ref _refCount) == 0)
		{
			Dispose();
		}
	}

	public void DecrementReferenceCount(int decrementBy)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		if (Interlocked.Add(ref _refCount, -decrementBy) <= 0)
		{
			Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				for (int i = 0; i < _frameEntries.Count; i++)
				{
					_frameEntries[i].Dispose();
				}
				_frameEntries.Clear();
			}
			_frameEntries = null;
		}
		disposed = true;
	}

	public IFrame GetFrame(int index)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		lock (_frameEntries)
		{
			return _frameEntries[index];
		}
	}

	public GenericFrame GetGenericFrame(int index)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		lock (_frameEntries)
		{
			return _frameEntries[index];
		}
	}

	public DateTime GetFrameTime(int frameIndex)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		lock (_frameEntries)
		{
			return _frameEntries[frameIndex].Timestamp;
		}
	}

	public ReadOnlySharedBuffer[] GetUnmanagedFrames()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		ReadOnlySharedBuffer[] array = new ReadOnlySharedBuffer[FrameCount];
		for (int i = 0; i < _frameEntries.Count; i++)
		{
			array[i] = _frameEntries[i].FrameBuffer;
		}
		return array;
	}

	public int FindFrameIndex(DateTime dt, GopMediaSearchCriteria searchCriterion, bool stayInsideGop)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		if (stayInsideGop)
		{
			if (searchCriterion == GopMediaSearchCriteria.Exact)
			{
				searchCriterion = GopMediaSearchCriteria.Nearest;
			}
			if (dt <= BeginTimestamp && (searchCriterion == GopMediaSearchCriteria.Nearest || searchCriterion == GopMediaSearchCriteria.Before || searchCriterion == GopMediaSearchCriteria.AtOrBefore))
			{
				return 0;
			}
			if (dt >= EndTimestamp && (searchCriterion == GopMediaSearchCriteria.Nearest || searchCriterion == GopMediaSearchCriteria.After || searchCriterion == GopMediaSearchCriteria.AtOrAfter))
			{
				return FrameCount - 1;
			}
		}
		GenericFrame genericFrame = null;
		switch (searchCriterion)
		{
		case GopMediaSearchCriteria.Nearest:
		{
			if (dt <= BeginTimestamp)
			{
				genericFrame = _frameEntries.First();
				break;
			}
			if (EndTimestamp <= dt)
			{
				genericFrame = _frameEntries.Last();
				break;
			}
			genericFrame = _frameEntries.Last((GenericFrame f) => f.Timestamp <= dt);
			GenericFrame genericFrame2 = _frameEntries.First((GenericFrame f) => dt <= f.Timestamp);
			if (TimeSpan.Compare(dt - genericFrame.Timestamp, genericFrame2.Timestamp - dt) == 1)
			{
				genericFrame = genericFrame2;
			}
			break;
		}
		case GopMediaSearchCriteria.Before:
			genericFrame = _frameEntries.LastOrDefault((GenericFrame f) => f.Timestamp < dt);
			break;
		case GopMediaSearchCriteria.AtOrBefore:
			genericFrame = _frameEntries.LastOrDefault((GenericFrame f) => f.Timestamp <= dt);
			break;
		case GopMediaSearchCriteria.After:
			genericFrame = _frameEntries.FirstOrDefault((GenericFrame f) => dt < f.Timestamp);
			break;
		case GopMediaSearchCriteria.AtOrAfter:
			genericFrame = _frameEntries.FirstOrDefault((GenericFrame f) => dt <= f.Timestamp);
			break;
		case GopMediaSearchCriteria.Exact:
			genericFrame = _frameEntries.FirstOrDefault((GenericFrame f) => dt == f.Timestamp);
			break;
		}
		return genericFrame?.FrameIndex ?? (-1);
	}

	public void AddFrame(GenericFrame frame)
	{
		if (frame == null)
		{
			throw new ArgumentNullException("frame");
		}
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		frame.ParentGop = this;
		lock (_frameEntries)
		{
			frame.FrameIndex = _frameEntries.Count;
			_frameEntries.Add(frame);
		}
	}
}
