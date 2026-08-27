using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.OEGroupItem;

public interface IOEGroupItem
{
	int GetMembersCount();

	List<IOEGroupItem> GetMembersList();
}
