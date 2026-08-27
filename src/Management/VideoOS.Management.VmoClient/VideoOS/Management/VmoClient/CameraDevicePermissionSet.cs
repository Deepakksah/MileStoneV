using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class CameraDevicePermissionSet : PermissionSetBase, IEquatable<CameraDevicePermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Edit => base.PermissionStorage[1];

	public bool ViewLive => base.PermissionStorage[2];

	public bool ViewRestrictedLive => base.PermissionStorage[3];

	public bool Playback => base.PermissionStorage[4];

	public bool PlaybackRestrictedMedia => base.PermissionStorage[5];

	public bool ReadRemoteRecordings => base.PermissionStorage[6];

	public bool ReadSequences => base.PermissionStorage[7];

	public bool SmartSearch => base.PermissionStorage[8];

	public bool Export => base.PermissionStorage[9];

	public bool CreateBookmark => base.PermissionStorage[10];

	public bool ReadBookmark => base.PermissionStorage[11];

	public bool EditBookmark => base.PermissionStorage[12];

	public bool DeleteBookmark => base.PermissionStorage[13];

	public bool CreateEvidenceLock => base.PermissionStorage[14];

	public bool ReadEvidenceLock => base.PermissionStorage[15];

	public bool DeleteEvidenceLock => base.PermissionStorage[16];

	public bool CreateRestrictedMedia => base.PermissionStorage[17];

	public bool ReadRestrictedMedia => base.PermissionStorage[18];

	public bool DeleteRestrictedMedia => base.PermissionStorage[18];

	public bool AuxCommands => base.PermissionStorage[20];

	public bool PtzControl => base.PermissionStorage[21];

	public bool ActivatePtzPresets => base.PermissionStorage[22];

	public bool ManagePresetAndPatrolling => base.PermissionStorage[23];

	public bool ManageLockPreset => base.PermissionStorage[24];

	public bool ReservePtzSession => base.PermissionStorage[25];

	public bool ReleasePtzSession => base.PermissionStorage[26];

	public bool StartManualRecording => base.PermissionStorage[27];

	public bool StopManualRecording => base.PermissionStorage[28];

	public bool DeleteRecordings => base.PermissionStorage[29];

	public bool LiftPrivacyMasking => base.PermissionStorage[30];

	public bool AdministrateSecurity => base.PermissionStorage[31];

	protected override int NumberOfElements => 32;

	public CameraDevicePermissionSet()
	{
	}

	public CameraDevicePermissionSet(CameraDevicePermission permission)
	{
		SetBit(permission);
	}

	public CameraDevicePermissionSet(CameraDevicePermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public CameraDevicePermissionSet Add(CameraDevicePermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(CameraDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(CameraDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static CameraDevicePermissionSet SupportedPermissions(ServerVersion version)
	{
		CameraDevicePermissionSet cameraDevicePermissionSet = new CameraDevicePermissionSet();
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.Read);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.Edit);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ViewLive);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ViewRestrictedLive);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.Playback);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.PlaybackRestrictedMedia);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ReadRemoteRecordings);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ReadSequences);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.SmartSearch);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.Export);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.CreateBookmarks);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ReadBookmarks);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.EditBookmarks);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.DeleteBookmarks);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.CreateEvidenceLock);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ReadEvidenceLock);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.DeleteEvidenceLock);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.CreateRestrictedMedia);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ReadRestrictedMedia);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.DeleteRestrictedMedia);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.StartManualRecording);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.StopManualRecording);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.AuxCommands);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.PtzControl);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ActivatePtzPreset);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ManagePresetAndPatrolling);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ManageLockPreset);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ReservePtzSession);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.ReleasePtzSession);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.DeleteRecordings);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.LiftPrivacyMasking);
		cameraDevicePermissionSet.SetBit(CameraDevicePermission.AdministrateSecurity);
		return cameraDevicePermissionSet;
	}

	public static CameraDevicePermissionSet Add(CameraDevicePermissionSet permissionSetLeft, CameraDevicePermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static CameraDevicePermissionSet operator +(CameraDevicePermissionSet permissionSetLeft, CameraDevicePermission permissionRight)
	{
		CameraDevicePermissionSet cameraDevicePermissionSet = new CameraDevicePermissionSet(permissionSetLeft);
		cameraDevicePermissionSet.SetBit(permissionRight);
		return cameraDevicePermissionSet;
	}

	public static CameraDevicePermissionSet Subtract(CameraDevicePermissionSet permissionSetLeft, CameraDevicePermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static CameraDevicePermissionSet operator -(CameraDevicePermissionSet permissionSetLeft, CameraDevicePermission permissionRight)
	{
		CameraDevicePermissionSet cameraDevicePermissionSet = new CameraDevicePermissionSet(permissionSetLeft);
		cameraDevicePermissionSet.ResetBit(permissionRight);
		return cameraDevicePermissionSet;
	}

	public static bool Equals(CameraDevicePermissionSet permissionSetLeft, CameraDevicePermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(CameraDevicePermissionSet permissionSetLeft, CameraDevicePermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(CameraDevicePermissionSet permissionSetLeft, CameraDevicePermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(CameraDevicePermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is CameraDevicePermissionSet other))
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
			2 => "VIEW_LIVE", 
			3 => "VIEW_RESTRICTED_LIVE", 
			4 => "PLAYBACK", 
			5 => "PLAYBACK_RESTRICTED_RECORDINGS", 
			6 => "READ_REMOTE_RECORDINGS", 
			7 => "READ_SEQUENCES", 
			8 => "SMARTSEARCH", 
			9 => "EXPORT", 
			10 => "CREATE_BOOKMARKS", 
			11 => "READ_BOOKMARKS", 
			12 => "EDIT_BOOKMARKS", 
			13 => "DELETE_BOOKMARKS", 
			14 => "CREATE_EVIDENCE_LOCK", 
			15 => "READ_EVIDENCE_LOCK", 
			16 => "DELETE_EVIDENCE_LOCK", 
			17 => "CREATE_RESTRICTIONS", 
			18 => "READ_RESTRICTIONS", 
			19 => "DELETE_RESTRICTIONS", 
			20 => "AUX_COMMANDS", 
			21 => "PTZ_CONTROL", 
			22 => "ACTIVATE_PTZ_PRESET", 
			23 => "MANAGE_PTZ_PRESET", 
			24 => "MANAGE_LOCK_PTZ_PRESET", 
			25 => "RESERVE_PTZ_SESSION", 
			26 => "RELEASE_PTZ_SESSION", 
			27 => "START_MANUAL_RECORDING", 
			28 => "STOP_MANUAL_RECORDING", 
			29 => "DELETE_RECORDINGS", 
			30 => "LIFT_PRIVACY_MASKING", 
			31 => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static CameraDevicePermission CreateFromActionId(string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return actionId switch
		{
			"GENERIC_READ" => new CameraDevicePermission(CameraDevicePermissionSetValue.Read), 
			"GENERIC_WRITE" => new CameraDevicePermission(CameraDevicePermissionSetValue.Edit), 
			"VIEW_LIVE" => new CameraDevicePermission(CameraDevicePermissionSetValue.ViewLive), 
			"VIEW_RESTRICTED_LIVE" => new CameraDevicePermission(CameraDevicePermissionSetValue.ViewRestrictedLive), 
			"PLAYBACK" => new CameraDevicePermission(CameraDevicePermissionSetValue.Playback), 
			"PLAYBACK_RESTRICTED_RECORDINGS" => new CameraDevicePermission(CameraDevicePermissionSetValue.PlaybackRestrictedMedia), 
			"READ_REMOTE_RECORDINGS" => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadRemoteRecordings), 
			"READ_SEQUENCES" => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadSequences), 
			"SMARTSEARCH" => new CameraDevicePermission(CameraDevicePermissionSetValue.SmartSearch), 
			"EXPORT" => new CameraDevicePermission(CameraDevicePermissionSetValue.Export), 
			"CREATE_BOOKMARKS" => new CameraDevicePermission(CameraDevicePermissionSetValue.CreateBookmarks), 
			"READ_BOOKMARKS" => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadBookmarks), 
			"EDIT_BOOKMARKS" => new CameraDevicePermission(CameraDevicePermissionSetValue.EditBookmarks), 
			"DELETE_BOOKMARKS" => new CameraDevicePermission(CameraDevicePermissionSetValue.DeleteBookmarks), 
			"CREATE_EVIDENCE_LOCK" => new CameraDevicePermission(CameraDevicePermissionSetValue.CreateEvidenceLock), 
			"READ_EVIDENCE_LOCK" => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadEvidenceLock), 
			"DELETE_EVIDENCE_LOCK" => new CameraDevicePermission(CameraDevicePermissionSetValue.DeleteEvidenceLock), 
			"CREATE_RESTRICTIONS" => new CameraDevicePermission(CameraDevicePermissionSetValue.CreateRestrictedMedia), 
			"READ_RESTRICTIONS" => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadRestrictedMedia), 
			"DELETE_RESTRICTIONS" => new CameraDevicePermission(CameraDevicePermissionSetValue.DeleteRestrictedMedia), 
			"AUX_COMMANDS" => new CameraDevicePermission(CameraDevicePermissionSetValue.AuxCommands), 
			"PTZ_CONTROL" => new CameraDevicePermission(CameraDevicePermissionSetValue.PtzControl), 
			"ACTIVATE_PTZ_PRESET" => new CameraDevicePermission(CameraDevicePermissionSetValue.ActivatePtzPreset), 
			"MANAGE_PTZ_PRESET" => new CameraDevicePermission(CameraDevicePermissionSetValue.ManagePresetAndPatrolling), 
			"MANAGE_LOCK_PTZ_PRESET" => new CameraDevicePermission(CameraDevicePermissionSetValue.ManageLockPreset), 
			"RESERVE_PTZ_SESSION" => new CameraDevicePermission(CameraDevicePermissionSetValue.ReservePtzSession), 
			"RELEASE_PTZ_SESSION" => new CameraDevicePermission(CameraDevicePermissionSetValue.ReleasePtzSession), 
			"START_MANUAL_RECORDING" => new CameraDevicePermission(CameraDevicePermissionSetValue.StartManualRecording), 
			"STOP_MANUAL_RECORDING" => new CameraDevicePermission(CameraDevicePermissionSetValue.StopManualRecording), 
			"DELETE_RECORDINGS" => new CameraDevicePermission(CameraDevicePermissionSetValue.DeleteRecordings), 
			"ADMIN_SECURITY" => new CameraDevicePermission(CameraDevicePermissionSetValue.AdministrateSecurity), 
			"LIFT_PRIVACY_MASKING" => new CameraDevicePermission(CameraDevicePermissionSetValue.LiftPrivacyMasking), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		CameraDevicePermissionSet result = new CameraDevicePermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			CameraDevicePermission cameraDevicePermission = CreateFromActionId(actionIdArray[i]);
			result += cameraDevicePermission;
		}
		return result;
	}

	public static string PermissionEnumToActionId(CameraDevicePermissionSetValue value)
	{
		return value switch
		{
			CameraDevicePermissionSetValue.Read => "GENERIC_READ", 
			CameraDevicePermissionSetValue.Edit => "GENERIC_WRITE", 
			CameraDevicePermissionSetValue.ViewLive => "VIEW_LIVE", 
			CameraDevicePermissionSetValue.ViewRestrictedLive => "VIEW_RESTRICTED_LIVE", 
			CameraDevicePermissionSetValue.Playback => "PLAYBACK", 
			CameraDevicePermissionSetValue.PlaybackRestrictedMedia => "PLAYBACK_RESTRICTED_RECORDINGS", 
			CameraDevicePermissionSetValue.ReadRemoteRecordings => "READ_REMOTE_RECORDINGS", 
			CameraDevicePermissionSetValue.ReadSequences => "READ_SEQUENCES", 
			CameraDevicePermissionSetValue.SmartSearch => "SMARTSEARCH", 
			CameraDevicePermissionSetValue.Export => "EXPORT", 
			CameraDevicePermissionSetValue.CreateBookmarks => "CREATE_BOOKMARKS", 
			CameraDevicePermissionSetValue.ReadBookmarks => "READ_BOOKMARKS", 
			CameraDevicePermissionSetValue.EditBookmarks => "EDIT_BOOKMARKS", 
			CameraDevicePermissionSetValue.DeleteBookmarks => "DELETE_BOOKMARKS", 
			CameraDevicePermissionSetValue.CreateEvidenceLock => "CREATE_EVIDENCE_LOCK", 
			CameraDevicePermissionSetValue.ReadEvidenceLock => "READ_EVIDENCE_LOCK", 
			CameraDevicePermissionSetValue.DeleteEvidenceLock => "DELETE_EVIDENCE_LOCK", 
			CameraDevicePermissionSetValue.CreateRestrictedMedia => "CREATE_RESTRICTIONS", 
			CameraDevicePermissionSetValue.ReadRestrictedMedia => "READ_RESTRICTIONS", 
			CameraDevicePermissionSetValue.DeleteRestrictedMedia => "DELETE_RESTRICTIONS", 
			CameraDevicePermissionSetValue.AuxCommands => "AUX_COMMANDS", 
			CameraDevicePermissionSetValue.PtzControl => "PTZ_CONTROL", 
			CameraDevicePermissionSetValue.ActivatePtzPreset => "ACTIVATE_PTZ_PRESET", 
			CameraDevicePermissionSetValue.ManagePresetAndPatrolling => "MANAGE_PTZ_PRESET", 
			CameraDevicePermissionSetValue.ManageLockPreset => "MANAGE_LOCK_PTZ_PRESET", 
			CameraDevicePermissionSetValue.ReservePtzSession => "RESERVE_PTZ_SESSION", 
			CameraDevicePermissionSetValue.ReleasePtzSession => "RELEASE_PTZ_SESSION", 
			CameraDevicePermissionSetValue.StartManualRecording => "START_MANUAL_RECORDING", 
			CameraDevicePermissionSetValue.StopManualRecording => "STOP_MANUAL_RECORDING", 
			CameraDevicePermissionSetValue.DeleteRecordings => "DELETE_RECORDINGS", 
			CameraDevicePermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			CameraDevicePermissionSetValue.LiftPrivacyMasking => "LIFT_PRIVACY_MASKING", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}
}
