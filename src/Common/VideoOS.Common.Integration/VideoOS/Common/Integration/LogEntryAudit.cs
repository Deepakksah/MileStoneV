using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class LogEntryAudit : LogEntry, ISerializable
{
	private string _userCredentials;

	private string _location;

	private string _resourceId;

	private string _resourceName = string.Empty;

	private string _resourceLocation = string.Empty;

	public AccessGranted AccessGranted { get; private set; }

	public string AccessGrantedValue { get; set; }

	public AuditType AuditType { get; private set; }

	public string AuditTypeValue { get; set; }

	public string UserCredentials
	{
		get
		{
			return _userCredentials;
		}
		set
		{
			_userCredentials = value;
		}
	}

	public string Location
	{
		get
		{
			return _location;
		}
		set
		{
			_location = value;
		}
	}

	public string ResourceID
	{
		get
		{
			return _resourceId;
		}
		set
		{
			_resourceId = value;
		}
	}

	public ResourceType ResourceType { get; private set; }

	public string ResourceTypeValue { get; set; }

	public string ResourceName
	{
		get
		{
			return _resourceName;
		}
		set
		{
			_resourceName = value;
		}
	}

	public string ResourceLocation
	{
		get
		{
			return _resourceLocation;
		}
		set
		{
			_resourceLocation = value;
		}
	}

	public LogEntryAudit(LogEntryId logEntryId, params string[] arguments)
		: base((int)logEntryId)
	{
	}

	public LogEntryAudit(LogEntryId logEntryId, string location, Guid sourceId, SourceType sourceType, string sourceName, string userCredentials, AuditType auditType, AccessGranted accessGranted, params string[] arguments)
		: base((int)logEntryId, sourceId, sourceType, sourceName, arguments)
	{
		_location = location;
		_userCredentials = ((userCredentials == null) ? string.Empty : userCredentials.ToUpperInvariant());
		AuditTypeValue = auditType.ToString();
		AuditType = auditType;
		AccessGrantedValue = accessGranted.ToString();
		AccessGranted = accessGranted;
	}

	public LogEntryAudit(LogEntryId logEntryId, string location, SourceType sourceType, string sourceName, string userCredentials, ResourceType resourceType, string resourceName, string resourceLocation, string resourceId, AuditType auditType, AccessGranted accessGranted, params string[] arguments)
		: base((int)logEntryId, Guid.Empty, sourceType, sourceName, arguments)
	{
		_location = location;
		_userCredentials = ((userCredentials == null) ? string.Empty : userCredentials.ToUpperInvariant());
		AuditTypeValue = auditType.ToString();
		AuditType = auditType;
		AccessGrantedValue = accessGranted.ToString();
		AccessGranted = accessGranted;
		_resourceLocation = resourceLocation;
		_resourceName = resourceName;
		ResourceTypeValue = resourceType.ToString();
		ResourceType = resourceType;
		_resourceId = resourceId;
	}

	public LogEntryAudit(string location, SourceType sourceType, string sourceName, string userCredentials, ResourceType resourceType, string resourceName, string resourceLocation, string resourceId, AuditType auditType, AccessGranted accessGranted, LogEntryDefinition definition)
		: base(Guid.Empty, sourceType, sourceName, definition)
	{
		_location = location;
		_userCredentials = ((userCredentials == null) ? string.Empty : userCredentials.ToUpperInvariant());
		AuditTypeValue = auditType.ToString();
		AuditType = auditType;
		AccessGrantedValue = accessGranted.ToString();
		AccessGranted = accessGranted;
		_resourceLocation = resourceLocation;
		_resourceName = resourceName;
		ResourceTypeValue = resourceType.ToString();
		ResourceType = resourceType;
		_resourceId = resourceId;
	}

	protected LogEntryAudit(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		AccessGrantedValue = info.GetString("AccessGrantedValue");
		AuditTypeValue = info.GetString("AuditTypeValue");
		UserCredentials = info.GetString("UserCredentials");
		Location = info.GetString("Location");
		ResourceID = info.GetString("ResourceID");
		ResourceTypeValue = info.GetString("ResourceTypeValue");
		ResourceName = info.GetString("ResourceName");
		ResourceLocation = info.GetString("ResourceLocation");
	}

	protected override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("AccessGrantedValue", AccessGrantedValue);
		info.AddValue("AuditTypeValue", AuditTypeValue);
		info.AddValue("UserCredentials", UserCredentials);
		info.AddValue("Location", Location);
		info.AddValue("ResourceID", ResourceID);
		info.AddValue("ResourceTypeValue", ResourceTypeValue);
		info.AddValue("ResourceName", ResourceName);
		info.AddValue("ResourceLocation", ResourceLocation);
	}

	public static AuditType GetAuditType(string auditTypeName)
	{
		if (Enum.TryParse<AuditType>(auditTypeName, ignoreCase: true, out var result))
		{
			return result;
		}
		return AuditType.Unknown;
	}

	public static AccessGranted GetAccessGranted(string accessGrantedName)
	{
		if (Enum.TryParse<AccessGranted>(accessGrantedName, ignoreCase: true, out var result))
		{
			return result;
		}
		return AccessGranted.Default;
	}

	public static ResourceType GetResourceType(string resourceTypeName)
	{
		if (Enum.TryParse<ResourceType>(resourceTypeName, ignoreCase: true, out var result))
		{
			return result;
		}
		return ResourceType.Unknown;
	}
}
