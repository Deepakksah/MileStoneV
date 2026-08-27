namespace VideoOS.Platform.Client;

public class CameraViewControlItem : ViewControlItem
{
	public FQID CameraId { get; }

	public CameraViewControlItem(int indexInViewLayout, FQID cameraId)
		: base(indexInViewLayout)
	{
		base.ViewItemTypeId = ViewAndLayoutItem.CameraBuiltinId;
		CameraId = cameraId;
	}
}
