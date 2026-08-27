using System;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Media;

public class FrameMetadata : IFrameMetadata, IDisposable
{
	private bool disposed;

	public Guid Id { get; private set; }

	public bool IsInSequence { get; private set; }

	public ReadOnlySharedBuffer UnmanagedData { get; private set; }

	private FrameMetadata(Guid id)
	{
		Id = id;
		IsInSequence = false;
		UnmanagedData = null;
	}

	public FrameMetadata(Guid id, bool isInSequence)
		: this(id)
	{
		IsInSequence = isInSequence;
	}

	public FrameMetadata(Guid id, ReadOnlySharedBuffer unmanagedData)
		: this(id)
	{
		UnmanagedData = unmanagedData;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed && disposing && UnmanagedData != null)
		{
			UnmanagedData.Dispose();
			UnmanagedData = null;
		}
		disposed = true;
	}
}
