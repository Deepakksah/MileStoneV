using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Client;

public class CustomViewControlItem : ViewControlItem
{
	public Dictionary<string, string> Properties { get; set; }

	public CustomViewControlItem(Guid viewItemTypeId, int indexInViewLayout)
		: base(indexInViewLayout)
	{
		base.ViewItemTypeId = viewItemTypeId;
	}
}
