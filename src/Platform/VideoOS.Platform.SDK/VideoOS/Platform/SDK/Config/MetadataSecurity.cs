using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class MetadataSecurity : IDeviceSecurity
{
	private IMetadataDeviceSecurityInfo _metadataDeviceSecurityInfo;

	public bool Live
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.Live;
			}
			return true;
		}
	}

	public bool Browse
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.Browse;
			}
			return true;
		}
	}

	public bool GetSequences
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.GetSequences;
			}
			return true;
		}
	}

	public bool ExportDatabase
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.ExportDatabase;
			}
			return true;
		}
	}

	public bool BookmarkAdd => false;

	public bool BookmarkDelete => false;

	public bool BookmarkEdit => false;

	public bool BookmarkView => false;

	public bool RetentionCreate
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.RetentionCreate;
			}
			return false;
		}
	}

	public bool RetentionRemove
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.RetentionRemove;
			}
			return false;
		}
	}

	public bool RetentionView
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.RetentionView;
			}
			return false;
		}
	}

	public bool StartRecording
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.StartRecording;
			}
			return false;
		}
	}

	public bool RetrieveEdgeRecordings
	{
		get
		{
			if (_metadataDeviceSecurityInfo != null)
			{
				return _metadataDeviceSecurityInfo.RetrieveEdgeRecordings;
			}
			return false;
		}
	}

	public MetadataSecurity()
	{
	}

	public MetadataSecurity(IMetadataDeviceSecurityInfo metadataDeviceSecurityInfo)
	{
		_metadataDeviceSecurityInfo = metadataDeviceSecurityInfo;
	}
}
