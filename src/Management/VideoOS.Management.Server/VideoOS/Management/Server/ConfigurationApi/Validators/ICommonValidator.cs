using System.Collections.Generic;

namespace VideoOS.Management.Server.ConfigurationApi.Validators;

public interface ICommonValidator
{
	void ValidatePath(ConfigurationItemPath path);

	void ValidateItemListPath(string pathString, ISet<string> itemTypes, string propertyKey);

	void ValidateItemListPath(string pathString, ISet<string> itemTypes, string propertyKey, out ConfigurationItemPath path);
}
