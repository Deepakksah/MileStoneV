using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PostBulkEventsResponseItem
{
	private IDictionary<string, object> _additionalProperties;

	[JsonProperty("data", Required = Required.Always)]
	[Required]
	public Event Data { get; set; } = new Event();

	[JsonProperty("status", Required = Required.Always)]
	public int Status { get; set; }

	[JsonExtensionData]
	public IDictionary<string, object> AdditionalProperties
	{
		get
		{
			return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>());
		}
		set
		{
			_additionalProperties = value;
		}
	}
}
