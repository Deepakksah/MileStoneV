namespace VideoOS.Common.Integration.Threading.Internal;

public class CanceledWorkItemsGroup
{
	public static readonly CanceledWorkItemsGroup NotCanceledWorkItemsGroup = new CanceledWorkItemsGroup();

	public bool IsCanceled { get; set; }

	public CanceledWorkItemsGroup()
	{
		IsCanceled = false;
	}
}
