using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class MicrophoneDevicePermissionSet : PermissionSetBase, IEquatable<MicrophoneDevicePermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Edit => base.PermissionStorage[1];

	public bool Listen => base.PermissionStorage[2];

	public bool ListenRestricted => base.PermissionStorage[3];

	public bool Playback => base.PermissionStorage[4];

	public bool PlaybackRestrictedMedia => base.PermissionStorage[5];

	public bool ReadRemoteRecordings => base.PermissionStorage[6];

	public bool ReadSequences => base.PermissionStorage[7];

	public bool Export => base.PermissionStorage[8];

	public bool CreateBookmark => base.PermissionStorage[9];

	public bool ReadBookmark => base.PermissionStorage[10];

	public bool EditBookmark => base.PermissionStorage[11];

	public bool DeleteBookmark => base.PermissionStorage[12];

	public bool CreateEvidenceLock => base.PermissionStorage[13];

	public bool ReadEvidenceLock => base.PermissionStorage[14];

	public bool DeleteEvidenceLock => base.PermissionStorage[15];

	public bool CreateRestrictedMedia => base.PermissionStorage[16];

	public bool ReadRestrictedMedia => base.PermissionStorage[17];

	public bool DeleteRestrictedMedia => base.PermissionStorage[17];

	public bool StartManualRecording => base.PermissionStorage[19];

	public bool StopManualRecording => base.PermissionStorage[20];

	public bool DeleteRecordings => base.PermissionStorage[21];

	public bool AdministrateSecurity => base.PermissionStorage[22];

	protected override int NumberOfElements => 23;

	public MicrophoneDevicePermissionSet()
	{
	}

	public MicrophoneDevicePermissionSet(MicrophoneDevicePermission permission)
	{
		SetBit(permission);
	}

	public MicrophoneDevicePermissionSet(MicrophoneDevicePermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public MicrophoneDevicePermissionSet Add(MicrophoneDevicePermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(MicrophoneDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(MicrophoneDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static MicrophoneDevicePermissionSet SupportedPermissions(ServerVersion version)
	{
		MicrophoneDevicePermissionSet microphoneDevicePermissionSet = new MicrophoneDevicePermissionSet();
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.Read);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.Edit);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.Listen);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.ListenRestricted);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.Playback);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.PlaybackRestrictedMedia);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.ReadRemoteRecordings);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.ReadSequences);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.Export);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.CreateBookmarks);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.ReadBookmarks);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.EditBookmarks);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.DeleteBookmarks);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.CreateEvidenceLock);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.ReadEvidenceLock);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.DeleteEvidenceLock);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.CreateRestrictedMedia);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.ReadRestrictedMedia);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.DeleteRestrictedMedia);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.StartManualRecording);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.StopManualRecording);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.DeleteRecordings);
		microphoneDevicePermissionSet.SetBit(MicrophoneDevicePermission.AdministrateSecurity);
		return microphoneDevicePermissionSet;
	}

	public static MicrophoneDevicePermissionSet Add(MicrophoneDevicePermissionSet permissionSetLeft, MicrophoneDevicePermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static MicrophoneDevicePermissionSet operator +(MicrophoneDevicePermissionSet permissionSetLeft, MicrophoneDevicePermission permissionRight)
	{
		MicrophoneDevicePermissionSet microphoneDevicePermissionSet = new MicrophoneDevicePermissionSet(permissionSetLeft);
		microphoneDevicePermissionSet.SetBit(permissionRight);
		return microphoneDevicePermissionSet;
	}

	public static MicrophoneDevicePermissionSet Subtract(MicrophoneDevicePermissionSet permissionSetLeft, MicrophoneDevicePermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static MicrophoneDevicePermissionSet operator -(MicrophoneDevicePermissionSet permissionSetLeft, MicrophoneDevicePermission permissionRight)
	{
		MicrophoneDevicePermissionSet microphoneDevicePermissionSet = new MicrophoneDevicePermissionSet(permissionSetLeft);
		microphoneDevicePermissionSet.ResetBit(permissionRight);
		return microphoneDevicePermissionSet;
	}

	public static bool Equals(MicrophoneDevicePermissionSet permissionSetLeft, MicrophoneDevicePermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(MicrophoneDevicePermissionSet permissionSetLeft, MicrophoneDevicePermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(MicrophoneDevicePermissionSet permissionSetLeft, MicrophoneDevicePermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(MicrophoneDevicePermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is MicrophoneDevicePermissionSet other))
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
			2 => "LISTEN", 
			3 => "LISTEN_RESTRICTED_LIVE", 
			4 => "PLAYBACK", 
			5 => "PLAYBACK_RESTRICTED_RECORDINGS", 
			6 => "READ_REMOTE_RECORDINGS", 
			7 => "READ_SEQUENCES", 
			8 => "EXPORT", 
			9 => "CREATE_BOOKMARKS", 
			10 => "READ_BOOKMARKS", 
			11 => "EDIT_BOOKMARKS", 
			12 => "DELETE_BOOKMARKS", 
			13 => "CREATE_EVIDENCE_LOCK", 
			14 => "READ_EVIDENCE_LOCK", 
			15 => "DELETE_EVIDENCE_LOCK", 
			16 => "CREATE_RESTRICTIONS", 
			17 => "READ_RESTRICTIONS", 
			18 => "DELETE_RESTRICTIONS", 
			19 => "START_MANUAL_RECORDING", 
			20 => "STOP_MANUAL_RECORDING", 
			21 => "DELETE_RECORDINGS", 
			22 => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static MicrophoneDevicePermission CreateFromActionId(string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return actionId switch
		{
			"GENERIC_READ" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Read), 
			"GENERIC_WRITE" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Edit), 
			"LISTEN" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Listen), 
			"LISTEN_RESTRICTED_LIVE" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ListenRestricted), 
			"PLAYBACK" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Playback), 
			"PLAYBACK_RESTRICTED_RECORDINGS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.PlaybackRestrictedMedia), 
			"READ_REMOTE_RECORDINGS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadRemoteRecordings), 
			"READ_SEQUENCES" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadSequences), 
			"EXPORT" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Export), 
			"CREATE_BOOKMARKS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.CreateBookmarks), 
			"READ_BOOKMARKS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadBookmarks), 
			"EDIT_BOOKMARKS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.EditBookmarks), 
			"DELETE_BOOKMARKS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.DeleteBookmarks), 
			"CREATE_EVIDENCE_LOCK" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.CreateEvidenceLock), 
			"READ_EVIDENCE_LOCK" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadEvidenceLock), 
			"DELETE_EVIDENCE_LOCK" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.DeleteEvidenceLock), 
			"CREATE_RESTRICTIONS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.CreateRestrictedMedia), 
			"READ_RESTRICTIONS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadRestrictedMedia), 
			"DELETE_RESTRICTIONS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.DeleteRestrictedMedia), 
			"START_MANUAL_RECORDING" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.StartManualRecording), 
			"STOP_MANUAL_RECORDING" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.StopManualRecording), 
			"DELETE_RECORDINGS" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.DeleteRecordings), 
			"ADMIN_SECURITY" => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.AdministrateSecurity), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		MicrophoneDevicePermissionSet result = new MicrophoneDevicePermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			MicrophoneDevicePermission microphoneDevicePermission = CreateFromActionId(actionIdArray[i]);
			result += microphoneDevicePermission;
		}
		return result;
	}

	public static string PermissionEnumToActionId(MicrophoneDevicePermissionSetValue value)
	{
		return value switch
		{
			MicrophoneDevicePermissionSetValue.Read => "GENERIC_READ", 
			MicrophoneDevicePermissionSetValue.Edit => "GENERIC_WRITE", 
			MicrophoneDevicePermissionSetValue.Listen => "LISTEN", 
			MicrophoneDevicePermissionSetValue.ListenRestricted => "LISTEN_RESTRICTED_LIVE", 
			MicrophoneDevicePermissionSetValue.Playback => "PLAYBACK", 
			MicrophoneDevicePermissionSetValue.PlaybackRestrictedMedia => "PLAYBACK_RESTRICTED_RECORDINGS", 
			MicrophoneDevicePermissionSetValue.ReadRemoteRecordings => "READ_REMOTE_RECORDINGS", 
			MicrophoneDevicePermissionSetValue.ReadSequences => "READ_SEQUENCES", 
			MicrophoneDevicePermissionSetValue.Export => "EXPORT", 
			MicrophoneDevicePermissionSetValue.CreateBookmarks => "CREATE_BOOKMARKS", 
			MicrophoneDevicePermissionSetValue.ReadBookmarks => "READ_BOOKMARKS", 
			MicrophoneDevicePermissionSetValue.EditBookmarks => "EDIT_BOOKMARKS", 
			MicrophoneDevicePermissionSetValue.DeleteBookmarks => "DELETE_BOOKMARKS", 
			MicrophoneDevicePermissionSetValue.CreateEvidenceLock => "CREATE_EVIDENCE_LOCK", 
			MicrophoneDevicePermissionSetValue.ReadEvidenceLock => "READ_EVIDENCE_LOCK", 
			MicrophoneDevicePermissionSetValue.DeleteEvidenceLock => "DELETE_EVIDENCE_LOCK", 
			MicrophoneDevicePermissionSetValue.CreateRestrictedMedia => "CREATE_RESTRICTIONS", 
			MicrophoneDevicePermissionSetValue.ReadRestrictedMedia => "READ_RESTRICTIONS", 
			MicrophoneDevicePermissionSetValue.DeleteRestrictedMedia => "DELETE_RESTRICTIONS", 
			MicrophoneDevicePermissionSetValue.StartManualRecording => "START_MANUAL_RECORDING", 
			MicrophoneDevicePermissionSetValue.StopManualRecording => "STOP_MANUAL_RECORDING", 
			MicrophoneDevicePermissionSetValue.DeleteRecordings => "DELETE_RECORDINGS", 
			MicrophoneDevicePermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}
}
