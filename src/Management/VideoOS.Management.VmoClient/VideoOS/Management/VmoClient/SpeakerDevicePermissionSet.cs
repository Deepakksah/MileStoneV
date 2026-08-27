using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class SpeakerDevicePermissionSet : PermissionSetBase, IEquatable<SpeakerDevicePermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Edit => base.PermissionStorage[1];

	public bool Listen => base.PermissionStorage[2];

	public bool ListenRestricted => base.PermissionStorage[3];

	public bool Speak => base.PermissionStorage[4];

	public bool Playback => base.PermissionStorage[5];

	public bool PlaybackRestrictedMedia => base.PermissionStorage[6];

	public bool ReadRemoteRecordings => base.PermissionStorage[7];

	public bool ReadSequences => base.PermissionStorage[8];

	public bool Export => base.PermissionStorage[9];

	public bool CreateBookmark => base.PermissionStorage[10];

	public bool ReadBookmark => base.PermissionStorage[11];

	public bool EditBookmark => base.PermissionStorage[12];

	public bool DeleteBookmark => base.PermissionStorage[13];

	public bool CreateEvidenceLock => base.PermissionStorage[14];

	public bool ReadEvidenceLock => base.PermissionStorage[15];

	public bool DeleteEvidenceLock => base.PermissionStorage[19];

	public bool CreateRestrictedMedia => base.PermissionStorage[16];

	public bool ReadRestrictedMedia => base.PermissionStorage[17];

	public bool DeleteRestrictedMedia => base.PermissionStorage[17];

	public bool StartManualRecording => base.PermissionStorage[20];

	public bool StopManualRecording => base.PermissionStorage[21];

	public bool DeleteRecordings => base.PermissionStorage[22];

	public bool AdministrateSecurity => base.PermissionStorage[23];

	protected override int NumberOfElements => 24;

	public SpeakerDevicePermissionSet()
	{
	}

	public SpeakerDevicePermissionSet(SpeakerDevicePermission permission)
	{
		SetBit(permission);
	}

	public SpeakerDevicePermissionSet(SpeakerDevicePermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public SpeakerDevicePermissionSet Add(SpeakerDevicePermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(SpeakerDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(SpeakerDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static SpeakerDevicePermissionSet SupportedPermissions(ServerVersion version)
	{
		SpeakerDevicePermissionSet speakerDevicePermissionSet = new SpeakerDevicePermissionSet();
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.Read);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.Edit);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.Listen);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.ListenRestricted);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.Speak);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.Playback);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.PlaybackRestrictedMedia);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.ReadRemoteRecordings);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.ReadSequences);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.Export);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.CreateBookmarks);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.ReadBookmarks);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.EditBookmarks);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.DeleteBookmarks);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.CreateEvidenceLock);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.ReadEvidenceLock);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.DeleteEvidenceLock);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.CreateRestrictedMedia);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.ReadRestrictedMedia);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.DeleteRestrictedMedia);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.StartManualRecording);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.StopManualRecording);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.DeleteRecordings);
		speakerDevicePermissionSet.SetBit(SpeakerDevicePermission.AdministrateSecurity);
		return speakerDevicePermissionSet;
	}

	public static SpeakerDevicePermissionSet Add(SpeakerDevicePermissionSet permissionSetLeft, SpeakerDevicePermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static SpeakerDevicePermissionSet operator +(SpeakerDevicePermissionSet permissionSetLeft, SpeakerDevicePermission permissionRight)
	{
		SpeakerDevicePermissionSet speakerDevicePermissionSet = new SpeakerDevicePermissionSet(permissionSetLeft);
		speakerDevicePermissionSet.SetBit(permissionRight);
		return speakerDevicePermissionSet;
	}

	public static SpeakerDevicePermissionSet Subtract(SpeakerDevicePermissionSet permissionSetLeft, SpeakerDevicePermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static SpeakerDevicePermissionSet operator -(SpeakerDevicePermissionSet permissionSetLeft, SpeakerDevicePermission permissionRight)
	{
		SpeakerDevicePermissionSet speakerDevicePermissionSet = new SpeakerDevicePermissionSet(permissionSetLeft);
		speakerDevicePermissionSet.ResetBit(permissionRight);
		return speakerDevicePermissionSet;
	}

	public static bool Equals(SpeakerDevicePermissionSet permissionSetLeft, SpeakerDevicePermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(SpeakerDevicePermissionSet permissionSetLeft, SpeakerDevicePermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(SpeakerDevicePermissionSet permissionSetLeft, SpeakerDevicePermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(SpeakerDevicePermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is SpeakerDevicePermissionSet other))
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
			4 => "SPEAK", 
			5 => "PLAYBACK", 
			6 => "PLAYBACK_RESTRICTED_RECORDINGS", 
			7 => "READ_REMOTE_RECORDINGS", 
			8 => "READ_SEQUENCES", 
			9 => "EXPORT", 
			10 => "CREATE_BOOKMARKS", 
			11 => "READ_BOOKMARKS", 
			12 => "EDIT_BOOKMARKS", 
			13 => "DELETE_BOOKMARKS", 
			14 => "CREATE_EVIDENCE_LOCK", 
			15 => "READ_EVIDENCE_LOCK", 
			19 => "DELETE_EVIDENCE_LOCK", 
			16 => "CREATE_RESTRICTIONS", 
			17 => "READ_RESTRICTIONS", 
			18 => "DELETE_RESTRICTIONS", 
			20 => "START_MANUAL_RECORDING", 
			21 => "STOP_MANUAL_RECORDING", 
			22 => "DELETE_RECORDINGS", 
			23 => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static SpeakerDevicePermission CreateFromActionId(string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return actionId switch
		{
			"GENERIC_READ" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Read), 
			"GENERIC_WRITE" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Edit), 
			"LISTEN" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Listen), 
			"LISTEN_RESTRICTED_LIVE" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ListenRestricted), 
			"SPEAK" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Speak), 
			"PLAYBACK" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Playback), 
			"PLAYBACK_RESTRICTED_RECORDINGS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.PlaybackRestrictedMedia), 
			"READ_REMOTE_RECORDINGS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadRemoteRecordings), 
			"READ_SEQUENCES" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadSequences), 
			"EXPORT" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Export), 
			"CREATE_BOOKMARKS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.CreateBookmarks), 
			"READ_BOOKMARKS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadBookmarks), 
			"EDIT_BOOKMARKS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.EditBookmarks), 
			"DELETE_BOOKMARKS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.DeleteBookmarks), 
			"CREATE_EVIDENCE_LOCK" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.CreateEvidenceLock), 
			"READ_EVIDENCE_LOCK" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadEvidenceLock), 
			"DELETE_EVIDENCE_LOCK" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.DeleteEvidenceLock), 
			"CREATE_RESTRICTIONS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.CreateRestrictedMedia), 
			"READ_RESTRICTIONS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadRestrictedMedia), 
			"DELETE_RESTRICTIONS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.DeleteRestrictedMedia), 
			"START_MANUAL_RECORDING" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.StartManualRecording), 
			"STOP_MANUAL_RECORDING" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.StopManualRecording), 
			"DELETE_RECORDINGS" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.DeleteRecordings), 
			"ADMIN_SECURITY" => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.AdministrateSecurity), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		SpeakerDevicePermissionSet result = new SpeakerDevicePermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			SpeakerDevicePermission speakerDevicePermission = CreateFromActionId(actionIdArray[i]);
			result += speakerDevicePermission;
		}
		return result;
	}

	public static string PermissionEnumToActionId(SpeakerDevicePermissionSetValue value)
	{
		return value switch
		{
			SpeakerDevicePermissionSetValue.Read => "GENERIC_READ", 
			SpeakerDevicePermissionSetValue.Edit => "GENERIC_WRITE", 
			SpeakerDevicePermissionSetValue.Listen => "LISTEN", 
			SpeakerDevicePermissionSetValue.ListenRestricted => "LISTEN_RESTRICTED_LIVE", 
			SpeakerDevicePermissionSetValue.Speak => "SPEAK", 
			SpeakerDevicePermissionSetValue.Playback => "PLAYBACK", 
			SpeakerDevicePermissionSetValue.PlaybackRestrictedMedia => "PLAYBACK_RESTRICTED_RECORDINGS", 
			SpeakerDevicePermissionSetValue.ReadRemoteRecordings => "READ_REMOTE_RECORDINGS", 
			SpeakerDevicePermissionSetValue.ReadSequences => "READ_SEQUENCES", 
			SpeakerDevicePermissionSetValue.Export => "EXPORT", 
			SpeakerDevicePermissionSetValue.CreateBookmarks => "CREATE_BOOKMARKS", 
			SpeakerDevicePermissionSetValue.ReadBookmarks => "READ_BOOKMARKS", 
			SpeakerDevicePermissionSetValue.EditBookmarks => "EDIT_BOOKMARKS", 
			SpeakerDevicePermissionSetValue.DeleteBookmarks => "DELETE_BOOKMARKS", 
			SpeakerDevicePermissionSetValue.CreateEvidenceLock => "CREATE_EVIDENCE_LOCK", 
			SpeakerDevicePermissionSetValue.ReadEvidenceLock => "READ_EVIDENCE_LOCK", 
			SpeakerDevicePermissionSetValue.DeleteEvidenceLock => "DELETE_EVIDENCE_LOCK", 
			SpeakerDevicePermissionSetValue.CreateRestrictedMedia => "CREATE_RESTRICTIONS", 
			SpeakerDevicePermissionSetValue.ReadRestrictedMedia => "READ_RESTRICTIONS", 
			SpeakerDevicePermissionSetValue.DeleteRestrictedMedia => "DELETE_RESTRICTIONS", 
			SpeakerDevicePermissionSetValue.StartManualRecording => "START_MANUAL_RECORDING", 
			SpeakerDevicePermissionSetValue.StopManualRecording => "STOP_MANUAL_RECORDING", 
			SpeakerDevicePermissionSetValue.DeleteRecordings => "DELETE_RECORDINGS", 
			SpeakerDevicePermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}
}
