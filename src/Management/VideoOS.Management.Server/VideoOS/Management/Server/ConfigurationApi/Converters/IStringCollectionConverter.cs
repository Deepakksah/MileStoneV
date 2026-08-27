using System.Collections.Generic;

namespace VideoOS.Management.Server.ConfigurationApi.Converters;

public interface IStringCollectionConverter
{
	string EnumerableToString(IEnumerable<IEnumerable<string>> enumerable);

	string EnumerableToString(IEnumerable<string> enumerable);

	string[][] StringTo2DimensionalArray(string input);

	string[] StringToArray(string input);
}
