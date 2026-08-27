using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class GetAlarmHistoryResponse : PagedResponse
{
	[JsonProperty("array", Required = Required.Always)]
	[Required]
	public ICollection<AlarmUpdates> Array { get; set; } = new Collection<AlarmUpdates>();
}
