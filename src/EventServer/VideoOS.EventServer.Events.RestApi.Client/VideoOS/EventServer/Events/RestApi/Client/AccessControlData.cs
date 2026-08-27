using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AccessControlData
{
	[JsonProperty("system", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public Guid? System { get; set; }

	[JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Id { get; set; }

	[JsonProperty("eventType", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public Guid? EventType { get; set; }

	[JsonProperty("source", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public Guid? Source { get; set; }

	[JsonProperty("sourceType", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public Guid? SourceType { get; set; }

	[JsonProperty("credentialHolders", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<string> CredentialHolders { get; set; }

	[JsonProperty("elements", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<Guid> Elements { get; set; }

	[JsonProperty("categories", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<Guid> Categories { get; set; }

	[JsonProperty("surveillanceItems", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<string> SurveillanceItems { get; set; }

	[JsonProperty("reason", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Reason { get; set; }

	[JsonProperty("properties", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public IDictionary<string, string> Properties { get; set; }
}
