using System;

namespace VideoOS.Platform.Client;

public abstract class ViewControlItem
{
	public Guid ViewItemTypeId { get; protected set; }

	public int IndexInViewLayout { get; }

	public Guid UniqueIdentifier { get; } = Guid.NewGuid();

	public object Tag { get; set; }

	protected ViewControlItem(int indexInViewLayout)
	{
		IndexInViewLayout = indexInViewLayout;
	}
}
