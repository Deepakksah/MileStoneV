using System.Collections.Generic;
using System.IO;

namespace VideoOS.Common.Integration;

public interface IServerFileAccess
{
	bool Exists(string path);

	void Delete(string path);

	void Move(string source, string destination);

	Stream Open(string path, FileMode mode);

	void CreateDirectory(string path);

	IEnumerable<string> GetFiles(string path);
}
