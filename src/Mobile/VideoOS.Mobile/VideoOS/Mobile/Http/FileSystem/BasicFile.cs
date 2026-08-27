using System;
using System.IO;

namespace VideoOS.Mobile.Http.FileSystem;

public abstract class BasicFile : IFile, IResource, IDisposable
{
	private bool _disposed;

	public Types Type => Types.File;

	public string Name { get; private set; }

	public abstract Stream Stream { get; }

	public abstract bool Download { get; }

	public abstract bool NoDownload { get; }

	public abstract bool AllowXframe { get; }

	public abstract DateTime LastModified { get; }

	protected BasicFile(string name)
	{
		Name = name;
	}

	~BasicFile()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			_disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
