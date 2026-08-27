using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Http.FileSystem;

public class BasicFolder : IFolder, IResource, IDisposable
{
	private readonly Dictionary<string, IResource> _subItems = new Dictionary<string, IResource>();

	private bool _disposed;

	public Types Type => Types.Folder;

	public string Name { get; private set; }

	public IResource this[string name] => _subItems[name];

	public IEnumerable<IResource> Items => _subItems.Select(delegate(KeyValuePair<string, IResource> kvp)
	{
		KeyValuePair<string, IResource> keyValuePair = kvp;
		return keyValuePair.Value;
	});

	public IFile Index { get; protected set; }

	public BasicFolder(string name)
	{
		Name = name;
	}

	~BasicFolder()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			foreach (IResource item in new List<IResource>(Items))
			{
				RemoveItem(item);
				item.Dispose();
			}
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public void AddItem(IResource item)
	{
		_subItems.Add(item.Name, item);
	}

	public void RemoveItem(IResource item)
	{
		_subItems.Remove(item.Name);
	}

	public IEnumerable<IResource> GetItems(Types types)
	{
		return _subItems.Where(delegate(KeyValuePair<string, IResource> kvp)
		{
			KeyValuePair<string, IResource> keyValuePair = kvp;
			return keyValuePair.Value.Type == types;
		}).Select(delegate(KeyValuePair<string, IResource> kvp)
		{
			KeyValuePair<string, IResource> keyValuePair = kvp;
			return keyValuePair.Value;
		});
	}
}
