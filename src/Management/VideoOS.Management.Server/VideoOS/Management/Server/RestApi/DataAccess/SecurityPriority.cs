using Newtonsoft.Json;

namespace VideoOS.Management.Server.RestApi.DataAccess;

public class SecurityPriority
{
	[JsonProperty(PropertyName = "relatedToMask")]
	public ulong RelatedToMask { get; set; }

	[JsonProperty(PropertyName = "priority")]
	public int Priority { get; set; }
}
