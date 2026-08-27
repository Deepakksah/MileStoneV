using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class MetadataDevicePermissionSet : PermissionSetBase, IEquatable<MetadataDevicePermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Edit => base.PermissionStorage[1];

	public bool Live => base.PermissionStorage[2];

	public bool ViewRestrictedLive => base.PermissionStorage[3];

	public bool Playback => base.PermissionStorage[4];

	public bool PlaybackRestrictedMedia => base.PermissionStorage[5];

	public bool ReadRemoteRecordings => base.PermissionStorage[6];

	public bool ReadSequences => base.PermissionStorage[7];

	public bool Export => base.PermissionStorage[8];

	public bool CreateEvidenceLock => base.PermissionStorage[9];

	public bool ReadEvidenceLock => base.PermissionStorage[10];

	public bool DeleteEvidenceLock => base.PermissionStorage[11];

	public bool CreateRestrictedMedia => base.PermissionStorage[12];

	public bool ReadRestrictedMedia => base.PermissionStorage[13];

	public bool DeleteRestrictedMedia => base.PermissionStorage[13];

	public bool StartManualRecording => base.PermissionStorage[15];

	public bool StopManualRecording => base.PermissionStorage[16];

	public bool DeleteRecordings => base.PermissionStorage[17];

	public bool AdministrateSecurity => base.PermissionStorage[18];

	protected override int NumberOfElements => 19;

	public MetadataDevicePermissionSet()
	{
	}

	public MetadataDevicePermissionSet(MetadataDevicePermission permission)
	{
		SetBit(permission);
	}

	public MetadataDevicePermissionSet(MetadataDevicePermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public MetadataDevicePermissionSet Add(MetadataDevicePermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(MetadataDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(MetadataDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static MetadataDevicePermissionSet SupportedPermissions(ServerVersion version)
	{
		MetadataDevicePermissionSet metadataDevicePermissionSet = new MetadataDevicePermissionSet();
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.Read);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.Edit);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.Live);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.ViewRestrictedLive);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.Playback);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.PlaybackRestrictedMedia);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.ReadRemoteRecordings);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.ReadSequences);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.Export);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.CreateEvidenceLock);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.ReadEvidenceLock);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.DeleteEvidenceLock);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.CreateRestrictedMedia);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.ReadRestrictedMedia);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.DeleteRestrictedMedia);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.StartManualRecording);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.StopManualRecording);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.DeleteRecordings);
		metadataDevicePermissionSet.SetBit(MetadataDevicePermission.AdministrateSecurity);
		return metadataDevicePermissionSet;
	}

	public static MetadataDevicePermissionSet Add(MetadataDevicePermissionSet permissionSetLeft, MetadataDevicePermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static MetadataDevicePermissionSet operator +(MetadataDevicePermissionSet permissionSetLeft, MetadataDevicePermission permissionRight)
	{
		MetadataDevicePermissionSet metadataDevicePermissionSet = new MetadataDevicePermissionSet(permissionSetLeft);
		metadataDevicePermissionSet.SetBit(permissionRight);
		return metadataDevicePermissionSet;
	}

	public static MetadataDevicePermissionSet Subtract(MetadataDevicePermissionSet permissionSetLeft, MetadataDevicePermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static MetadataDevicePermissionSet operator -(MetadataDevicePermissionSet permissionSetLeft, MetadataDevicePermission permissionRight)
	{
		MetadataDevicePermissionSet metadataDevicePermissionSet = new MetadataDevicePermissionSet(permissionSetLeft);
		metadataDevicePermissionSet.ResetBit(permissionRight);
		return metadataDevicePermissionSet;
	}

	public static bool Equals(MetadataDevicePermissionSet permissionSetLeft, MetadataDevicePermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(MetadataDevicePermissionSet permissionSetLeft, MetadataDevicePermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(MetadataDevicePermissionSet permissionSetLeft, MetadataDevicePermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(MetadataDevicePermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is MetadataDevicePermissionSet other))
		{
			return false;
		}
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	protected override string PermissionEnumToString(int value)
	{
		return value switch
		{
			0 => "GENERIC_READ", 
			1 => "GENERIC_WRITE", 
			2 => "LIVE", 
			3 => "VIEW_RESTRICTED_LIVE", 
			4 => "PLAYBACK", 
			5 => "PLAYBACK_RESTRICTED_RECORDINGS", 
			6 => "READ_REMOTE_RECORDINGS", 
			7 => "READ_SEQUENCES", 
			8 => "EXPORT", 
			9 => "CREATE_EVIDENCE_LOCK", 
			10 => "READ_EVIDENCE_LOCK", 
			11 => "DELETE_EVIDENCE_LOCK", 
			12 => "CREATE_RESTRICTIONS", 
			13 => "READ_RESTRICTIONS", 
			14 => "DELETE_RESTRICTIONS", 
			15 => "START_MANUAL_RECORDING", 
			16 => "STOP_MANUAL_RECORDING", 
			17 => "DELETE_RECORDINGS", 
			18 => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static MetadataDevicePermission CreateFromActionId(string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return actionId switch
		{
			"GENERIC_READ" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.Read), 
			"GENERIC_WRITE" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.Edit), 
			"LIVE" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ViewLive), 
			"VIEW_RESTRICTED_LIVE" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ViewRestrictedLive), 
			"PLAYBACK" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.Playback), 
			"PLAYBACK_RESTRICTED_RECORDINGS" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.PlaybackRestrictedMedia), 
			"READ_REMOTE_RECORDINGS" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ReadRemoteRecordings), 
			"READ_SEQUENCES" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ReadSequences), 
			"EXPORT" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.Export), 
			"CREATE_EVIDENCE_LOCK" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.CreateEvidenceLock), 
			"READ_EVIDENCE_LOCK" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ReadEvidenceLock), 
			"DELETE_EVIDENCE_LOCK" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.DeleteEvidenceLock), 
			"CREATE_RESTRICTIONS" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.CreateRestrictedMedia), 
			"READ_RESTRICTIONS" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ReadRestrictedMedia), 
			"DELETE_RESTRICTIONS" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.DeleteRestrictedMedia), 
			"START_MANUAL_RECORDING" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.StartManualRecording), 
			"STOP_MANUAL_RECORDING" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.StopManualRecording), 
			"DELETE_RECORDINGS" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.DeleteRecordings), 
			"ADMIN_SECURITY" => new MetadataDevicePermission(MetadataDevicePermissionSetValue.AdministrateSecurity), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		MetadataDevicePermissionSet result = new MetadataDevicePermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			MetadataDevicePermission metadataDevicePermission = CreateFromActionId(actionIdArray[i]);
			result += metadataDevicePermission;
		}
		return result;
	}

	public static string PermissionEnumToActionId(MetadataDevicePermissionSetValue value)
	{
		return value switch
		{
			MetadataDevicePermissionSetValue.Read => "GENERIC_READ", 
			MetadataDevicePermissionSetValue.Edit => "GENERIC_WRITE", 
			MetadataDevicePermissionSetValue.ViewLive => "LIVE", 
			MetadataDevicePermissionSetValue.ViewRestrictedLive => "VIEW_RESTRICTED_LIVE", 
			MetadataDevicePermissionSetValue.Playback => "PLAYBACK", 
			MetadataDevicePermissionSetValue.PlaybackRestrictedMedia => "PLAYBACK_RESTRICTED_RECORDINGS", 
			MetadataDevicePermissionSetValue.ReadRemoteRecordings => "READ_REMOTE_RECORDINGS", 
			MetadataDevicePermissionSetValue.ReadSequences => "READ_SEQUENCES", 
			MetadataDevicePermissionSetValue.Export => "EXPORT", 
			MetadataDevicePermissionSetValue.CreateEvidenceLock => "CREATE_EVIDENCE_LOCK", 
			MetadataDevicePermissionSetValue.ReadEvidenceLock => "READ_EVIDENCE_LOCK", 
			MetadataDevicePermissionSetValue.DeleteEvidenceLock => "DELETE_EVIDENCE_LOCK", 
			MetadataDevicePermissionSetValue.CreateRestrictedMedia => "CREATE_RESTRICTIONS", 
			MetadataDevicePermissionSetValue.ReadRestrictedMedia => "READ_RESTRICTIONS", 
			MetadataDevicePermissionSetValue.DeleteRestrictedMedia => "DELETE_RESTRICTIONS", 
			MetadataDevicePermissionSetValue.StartManualRecording => "START_MANUAL_RECORDING", 
			MetadataDevicePermissionSetValue.StopManualRecording => "STOP_MANUAL_RECORDING", 
			MetadataDevicePermissionSetValue.DeleteRecordings => "DELETE_RECORDINGS", 
			MetadataDevicePermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}
}
