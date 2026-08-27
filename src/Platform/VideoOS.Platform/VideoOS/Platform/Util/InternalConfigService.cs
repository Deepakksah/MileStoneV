using System;
using System.Collections.Generic;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Util;

public class InternalConfigService
{
	private static InternalConfigService _instance = new InternalConfigService();

	public static InternalConfigService Instance
	{
		get
		{
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public virtual bool Initialized => false;

	public virtual bool IsPrivacyMaskLiftingApproved => false;

	public virtual StreamType[] GetStreamTypes(FQID deviceFQID)
	{
		return Array.Empty<StreamType>();
	}

	public virtual List<DataType> GetMetadataTypes(FQID deviceFqid)
	{
		return new List<DataType>();
	}
}
