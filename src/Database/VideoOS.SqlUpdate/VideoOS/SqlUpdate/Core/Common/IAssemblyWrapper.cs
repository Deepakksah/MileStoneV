using System.IO;

namespace VideoOS.SqlUpdate.Core.Common;

public interface IAssemblyWrapper
{
	Stream GetManifestResourceStream(string resourceName);

	string[] GetManifestResourceNames();
}
