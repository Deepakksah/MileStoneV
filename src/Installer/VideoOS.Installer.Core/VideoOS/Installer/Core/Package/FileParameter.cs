using System.Collections.Generic;

namespace VideoOS.Installer.Core.Package;

public class FileParameter : Parameter
{
	private List<string> _filterStrings = new List<string>();

	private List<string> _filterDisplayNames = new List<string>();

	public string[] FilterStrings => _filterStrings.ToArray();

	public string[] FilterDisplayNames => _filterDisplayNames.ToArray();

	public FileParameter(string displayName, string description, string name, List<string> filterStrings, List<string> filterDisplayNames, bool visible, bool readOnly, bool emptyAllowed, bool omitIfEmpty)
		: base(displayName, description, name, visible, readOnly, emptyAllowed, omitIfEmpty)
	{
		_filterStrings = filterStrings;
		_filterDisplayNames = filterDisplayNames;
	}
}
