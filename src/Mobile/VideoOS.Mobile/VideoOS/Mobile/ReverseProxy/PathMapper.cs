using System;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Utilities.Validation;

namespace VideoOS.Mobile.ReverseProxy;

public class PathMapper : IPathMapper
{
	public const char Separator = '/';

	private const char QueryStart = '?';

	private readonly string _oldRoot;

	private readonly string _newRoot;

	public PathMapper(string oldRoot, string newRoot)
	{
		ValidateRoot(oldRoot);
		ValidateRoot(newRoot);
		_oldRoot = oldRoot;
		_newRoot = newRoot;
	}

	public string Map(string path)
	{
		path.NullCheck("", "Map", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\ReverseProxy\\PathMappers.cs", 38);
		if (string.Equals(path, _oldRoot, StringComparison.CurrentCultureIgnoreCase) || path.StartsWith(_oldRoot.TrimEnd('/') + "/", StringComparison.InvariantCultureIgnoreCase) || path.StartsWith(_oldRoot.TrimEnd('/') + "?", StringComparison.InvariantCultureIgnoreCase))
		{
			return path.ReplaceFirst(_oldRoot, _newRoot, StringComparison.InvariantCultureIgnoreCase);
		}
		throw new ArgumentException("Invalid path '" + path + "'");
	}

	private static void ValidateRoot(string root)
	{
		root.NullCheck("", "ValidateRoot", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\ReverseProxy\\PathMappers.cs", 52);
		if (!root.StartsWith('/'.ToString()))
		{
			throw new ArgumentException($"Invalid mapping. '{root}' must start with a '{'/'}'");
		}
	}
}
