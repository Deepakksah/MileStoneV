using System;
using System.IO;

namespace VideoOS.Mobile.Http.FileSystem;

public interface IFile : IResource, IDisposable
{
	Stream Stream { get; }

	bool Download { get; }

	bool NoDownload { get; }

	bool AllowXframe { get; }

	DateTime LastModified { get; }
}
