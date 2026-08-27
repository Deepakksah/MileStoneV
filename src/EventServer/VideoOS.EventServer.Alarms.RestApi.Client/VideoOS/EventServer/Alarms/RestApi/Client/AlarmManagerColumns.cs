using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AlarmManagerColumns
{
	[JsonProperty("enabledColumns", Required = Required.Always, ItemConverterType = typeof(StringEnumConverter))]
	[Required]
	public ICollection<AlarmListColumn> EnabledColumns { get; set; } = new Collection<AlarmListColumn>();
}
