using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Utilities.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

[DataContract]
public class ServerEndpoint
{
	[JsonIgnore]
	[XmlIgnore]
	[JsonIgnore]
	public ServerEndpointType Type { get; set; }

	[JsonProperty("Type")]
	[DataMember(Name = "Type")]
	[JsonElement("Type")]
	public string TypeString
	{
		get
		{
			return Type.ToString();
		}
		set
		{
			Type = (Enum.TryParse<ServerEndpointType>(value, ignoreCase: true, out var result) ? result : ServerEndpointType.AutomaticUrl);
		}
	}

	[DataMember(Name = "ConnectionString")]
	[JsonProperty("ConnectionString")]
	[JsonElement("ConnectionString")]
	[XmlAttribute("ConnectionString")]
	public string ConnectionString { get; set; }
}
