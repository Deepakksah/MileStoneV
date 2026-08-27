using System;
using Newtonsoft.Json;

namespace VideoOS.Management.Server.RestApi.DataAccess;

public class PermissionTimeprofile
{
	[JsonProperty(PropertyName = "relatedToMask")]
	public ulong RelatedToMask { get; set; }

	[JsonProperty(PropertyName = "timeProfilePath")]
	public PathDataModel TimeProfilePath { get; set; }

	[JsonProperty(PropertyName = "timeBackwards")]
	public int TimeBackwards { get; set; }

	public PermissionTimeprofile()
	{
	}

	public PermissionTimeprofile(ulong mask)
	{
		RelatedToMask = mask;
		TimeProfilePath = new PathDataModel
		{
			Id = Guid.Empty.ToString(),
			Type = "timeProfiles"
		};
		TimeBackwards = 0;
	}
}
