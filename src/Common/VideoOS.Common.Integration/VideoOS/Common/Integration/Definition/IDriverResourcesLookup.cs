using System.Globalization;

namespace VideoOS.Common.Integration.Definition;

public interface IDriverResourcesLookup
{
	string GetDisplayNameByGuid(string displayNameReferenceId, CultureInfo culture);
}
