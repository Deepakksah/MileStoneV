using System;

namespace VideoOS.Mobile.Http.FileSystem;

public interface IResource : IDisposable
{
	Types Type { get; }

	string Name { get; }
}
