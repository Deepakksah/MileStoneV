using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Http.FileSystem;

public interface IFolder : IResource, IDisposable
{
	IResource this[string name] { get; }

	IEnumerable<IResource> Items { get; }

	IFile Index { get; }

	void AddItem(IResource item);

	void RemoveItem(IResource item);

	IEnumerable<IResource> GetItems(Types types);
}
