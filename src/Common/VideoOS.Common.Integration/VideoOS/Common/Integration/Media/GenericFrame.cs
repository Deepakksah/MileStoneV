using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Media;

public class GenericFrame : IFrame, IFrameSimple, IReferenceCounted, IDisposable
{
	private GenericGop _gop;

	private ReadOnlySharedBuffer _data;

	private int _frameIndex;

	private DateTime _timestamp;

	private ushort _sequenceNumber;

	private Dictionary<Guid, IFrameMetadata> _metaData = new Dictionary<Guid, IFrameMetadata>();

	private bool disposed;

	public GenericGop ParentGop
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (_gop == null)
			{
				throw new InvalidOperationException("Gop has not been set");
			}
			return _gop;
		}
		public set
		{
			_gop = value;
		}
	}

	public IReadOnlyBuffer FrameDataView
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			return _data;
		}
	}

	public ReadOnlySharedBuffer FrameBuffer
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			return _data;
		}
	}

	public ulong FrameDataLength
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			return _data.Length;
		}
	}

	public bool IsKeyframe
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			return _frameIndex == 0;
		}
	}

	public ushort SequenceNumber
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			return _sequenceNumber;
		}
	}

	public DateTime SyncTimestamp
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (_gop == null)
			{
				throw new InvalidOperationException("Gop has not been set");
			}
			return _gop.BeginTimestamp;
		}
	}

	public DateTime Timestamp
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			return _timestamp;
		}
	}

	public int FrameIndex
	{
		get
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			return _frameIndex;
		}
		public set
		{
			_frameIndex = value;
		}
	}

	public bool IsNullKeyframe => this is NullKeyframe;

	public GenericFrame(ReadOnlySharedBuffer data, DateTime timestamp, ushort sequenceNumber)
	{
		_gop = null;
		_frameIndex = 0;
		_data = data;
		_timestamp = timestamp;
		_sequenceNumber = sequenceNumber;
	}

	public GenericFrame(ReadOnlySharedBuffer data, DateTime timestamp)
		: this(data, timestamp, 0)
	{
	}

	public void IncrementReferenceCount()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		_gop?.IncrementReferenceCount();
	}

	public void DecrementReferenceCount()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		_gop?.DecrementReferenceCount();
	}

	public IFrameMetadata GetMetadata(Guid metadataId)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		IFrameMetadata value = null;
		if (_metaData.TryGetValue(metadataId, out value))
		{
			return value;
		}
		return null;
	}

	public void SetMetadata(IFrameMetadata metadata)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		if (metadata == null)
		{
			throw new ArgumentNullException("metadata");
		}
		_metaData.Add(metadata.Id, metadata);
	}

	public IFrameMetadata[] GetMetadataCollection()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		return _metaData.Values.ToArray();
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
				_data?.Dispose();
				foreach (IFrameMetadata value in _metaData.Values)
				{
					value.Dispose();
				}
				_metaData.Clear();
			}
			_data = null;
			_gop = null;
			_metaData = null;
		}
		disposed = true;
	}
}
