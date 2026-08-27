using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public abstract class DeviceSecurity<TI, TC> : IDeviceSecurity where TI : IDeviceSecurityInfo where TC : TI, new()
{
	protected TI _deviceSecurityInfo;

	public bool Live => _deviceSecurityInfo.Live;

	public bool Browse => _deviceSecurityInfo.Browse;

	public bool GetSequences => _deviceSecurityInfo.GetSequences;

	public bool ExportDatabase => _deviceSecurityInfo.ExportDatabase;

	public bool BookmarkAdd => _deviceSecurityInfo.BookmarkAdd;

	public bool BookmarkDelete => _deviceSecurityInfo.BookmarkDelete;

	public bool BookmarkEdit => _deviceSecurityInfo.BookmarkEdit;

	public bool BookmarkView => _deviceSecurityInfo.BookmarkView;

	public bool RetentionCreate => _deviceSecurityInfo.RetentionCreate;

	public bool RetentionRemove => _deviceSecurityInfo.RetentionRemove;

	public bool RetentionView => _deviceSecurityInfo.RetentionView;

	public bool StartRecording => _deviceSecurityInfo.StartRecording;

	public bool RetrieveEdgeRecordings => _deviceSecurityInfo.RetrieveEdgeRecordings;

	public DeviceSecurity(TI deviceSecurityInfo)
	{
		_deviceSecurityInfo = deviceSecurityInfo;
	}
}
