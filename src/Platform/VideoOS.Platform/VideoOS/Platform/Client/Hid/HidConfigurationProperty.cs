using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Client.Hid;

[DataContract]
public abstract class HidConfigurationProperty
{
	[DataMember]
	public Guid Id { get; private set; }

	public string Name { get; protected set; }

	public string ToolTip { get; set; }

	public HidConfigurationProperty(Guid id, string name)
	{
		Id = id;
		Name = name;
	}
}
