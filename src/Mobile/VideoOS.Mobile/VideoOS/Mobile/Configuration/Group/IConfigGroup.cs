using System.Collections.Generic;
using VideoOS.Mobile.Configuration.Section;

namespace VideoOS.Mobile.Configuration.Group;

public interface IConfigGroup
{
	List<IConfigSection> Items { get; }

	string GroupName { get; }
}
