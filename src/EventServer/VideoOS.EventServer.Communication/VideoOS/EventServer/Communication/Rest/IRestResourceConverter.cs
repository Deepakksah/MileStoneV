using System;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Rest;

public interface IRestResourceConverter
{
	RestResource GetRestResource(FQID deviceFqid);

	FQID GetFqid(RestResource restResource);

	Guid GetKindFromRestResource(string restResource, out bool isFolder);
}
