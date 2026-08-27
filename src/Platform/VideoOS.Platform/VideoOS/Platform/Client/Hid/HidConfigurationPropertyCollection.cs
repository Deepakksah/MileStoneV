using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Client.Hid;

[DataContract]
public class HidConfigurationPropertyCollection
{
	[DataMember]
	public Guid PluginId { get; private set; }

	[DataMember]
	public Guid InstanceId { get; private set; }

	[DataMember]
	public HidConfigurationProperty[] Properties { get; private set; }

	public HidConfigurationPropertyCollection(Guid pluginId, Guid instanceId, params HidConfigurationProperty[] properties)
	{
		PluginId = pluginId;
		InstanceId = instanceId;
		Properties = properties;
	}
}
