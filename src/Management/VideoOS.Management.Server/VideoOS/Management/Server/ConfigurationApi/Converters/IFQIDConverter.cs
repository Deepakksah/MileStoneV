using System;
using System.Collections.Generic;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi.Converters;

public interface IFQIDConverter
{
	string ConvertFQIDsToPaths(IEnumerable<FQID> fqids);

	string ConvertFQIDToPath(FQID fqid);

	FQID[] ConvertPathsToFQIDs(IGeminiRequestContext requestContext, IEnumerable<ConfigurationItemPath> paths);

	FQID ConvertPathToFQID(IGeminiRequestContext requestContext, ConfigurationItemPath path, Guid defaultMipKind);
}
