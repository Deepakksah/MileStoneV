using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MarkedData : IMarkedData
{
	private Guid idField;

	private Guid[] deviceIdsField;

	private DateTime createdField;

	private DateTime lastModifiedField;

	private string referenceField;

	private string headerField;

	private string descriptionField;

	private string userField;

	private DateTime startTimeField;

	private DateTime tagTimeField;

	private DateTime endTimeField;

	private bool useRetentionField;

	private DateTime retentionExpireField;

	private long retentionSizeField;

	private RetentionOption retentionOptionField;

	IRetentionOption IMarkedData.RetentionOption
	{
		get
		{
			return RetentionOption;
		}
		set
		{
			RetentionOption = (RetentionOption)value;
		}
	}

	public Guid Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	public Guid[] DeviceIds
	{
		get
		{
			return deviceIdsField;
		}
		set
		{
			deviceIdsField = value;
		}
	}

	public DateTime Created
	{
		get
		{
			return createdField;
		}
		set
		{
			createdField = value;
		}
	}

	public DateTime LastModified
	{
		get
		{
			return lastModifiedField;
		}
		set
		{
			lastModifiedField = value;
		}
	}

	public string Reference
	{
		get
		{
			return referenceField;
		}
		set
		{
			referenceField = value;
		}
	}

	public string Header
	{
		get
		{
			return headerField;
		}
		set
		{
			headerField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	public string User
	{
		get
		{
			return userField;
		}
		set
		{
			userField = value;
		}
	}

	public DateTime StartTime
	{
		get
		{
			return startTimeField;
		}
		set
		{
			startTimeField = value;
		}
	}

	public DateTime TagTime
	{
		get
		{
			return tagTimeField;
		}
		set
		{
			tagTimeField = value;
		}
	}

	public DateTime EndTime
	{
		get
		{
			return endTimeField;
		}
		set
		{
			endTimeField = value;
		}
	}

	public bool UseRetention
	{
		get
		{
			return useRetentionField;
		}
		set
		{
			useRetentionField = value;
		}
	}

	public DateTime RetentionExpire
	{
		get
		{
			return retentionExpireField;
		}
		set
		{
			retentionExpireField = value;
		}
	}

	public long RetentionSize
	{
		get
		{
			return retentionSizeField;
		}
		set
		{
			retentionSizeField = value;
		}
	}

	public RetentionOption RetentionOption
	{
		get
		{
			return retentionOptionField;
		}
		set
		{
			retentionOptionField = value;
		}
	}
}
