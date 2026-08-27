using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Mobile.Configuration.Section;

namespace VideoOS.Mobile.Configuration.Group;

[DataContract]
public class CommonChannelConfigGroup : IConfigGroup
{
	[DataMember]
	public AddressConfig Address { get; private set; }

	public List<IConfigSection> Items { get; private set; }

	public virtual string GroupName => "CommonChannel";

	public CommonChannelConfigGroup()
	{
		Address = new AddressConfig();
		Items = new List<IConfigSection>();
		Items.Add(Address);
	}
}
