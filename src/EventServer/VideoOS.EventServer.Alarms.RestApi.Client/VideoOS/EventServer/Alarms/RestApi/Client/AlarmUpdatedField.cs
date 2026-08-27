using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AlarmUpdatedField
{
	[JsonProperty("field", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Field { get; set; }

	[JsonProperty("value", Required = Required.Always)]
	[Required]
	public object Value { get; set; }
}
