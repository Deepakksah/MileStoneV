using System;

namespace VideoOS.Management.VmoClient;

public sealed class CameraDevicePermission : IEquatable<CameraDevicePermission>
{
	public CameraDevicePermissionSetValue Value { get; private set; }

	public static CameraDevicePermission Read => new CameraDevicePermission(CameraDevicePermissionSetValue.Read);

	public static CameraDevicePermission Edit => new CameraDevicePermission(CameraDevicePermissionSetValue.Edit);

	public static CameraDevicePermission ViewLive => new CameraDevicePermission(CameraDevicePermissionSetValue.ViewLive);

	public static CameraDevicePermission ViewRestrictedLive => new CameraDevicePermission(CameraDevicePermissionSetValue.ViewRestrictedLive);

	public static CameraDevicePermission Playback => new CameraDevicePermission(CameraDevicePermissionSetValue.Playback);

	public static CameraDevicePermission PlaybackRestrictedMedia => new CameraDevicePermission(CameraDevicePermissionSetValue.PlaybackRestrictedMedia);

	public static CameraDevicePermission ReadRemoteRecordings => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadRemoteRecordings);

	public static CameraDevicePermission ReadSequences => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadSequences);

	public static CameraDevicePermission SmartSearch => new CameraDevicePermission(CameraDevicePermissionSetValue.SmartSearch);

	public static CameraDevicePermission Export => new CameraDevicePermission(CameraDevicePermissionSetValue.Export);

	public static CameraDevicePermission CreateBookmarks => new CameraDevicePermission(CameraDevicePermissionSetValue.CreateBookmarks);

	public static CameraDevicePermission ReadBookmarks => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadBookmarks);

	public static CameraDevicePermission EditBookmarks => new CameraDevicePermission(CameraDevicePermissionSetValue.EditBookmarks);

	public static CameraDevicePermission DeleteBookmarks => new CameraDevicePermission(CameraDevicePermissionSetValue.DeleteBookmarks);

	public static CameraDevicePermission CreateEvidenceLock => new CameraDevicePermission(CameraDevicePermissionSetValue.CreateEvidenceLock);

	public static CameraDevicePermission ReadEvidenceLock => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadEvidenceLock);

	public static CameraDevicePermission DeleteEvidenceLock => new CameraDevicePermission(CameraDevicePermissionSetValue.DeleteEvidenceLock);

	public static CameraDevicePermission CreateRestrictedMedia => new CameraDevicePermission(CameraDevicePermissionSetValue.CreateRestrictedMedia);

	public static CameraDevicePermission ReadRestrictedMedia => new CameraDevicePermission(CameraDevicePermissionSetValue.ReadRestrictedMedia);

	public static CameraDevicePermission DeleteRestrictedMedia => new CameraDevicePermission(CameraDevicePermissionSetValue.DeleteRestrictedMedia);

	public static CameraDevicePermission StartManualRecording => new CameraDevicePermission(CameraDevicePermissionSetValue.StartManualRecording);

	public static CameraDevicePermission StopManualRecording => new CameraDevicePermission(CameraDevicePermissionSetValue.StopManualRecording);

	public static CameraDevicePermission AuxCommands => new CameraDevicePermission(CameraDevicePermissionSetValue.AuxCommands);

	public static CameraDevicePermission PtzControl => new CameraDevicePermission(CameraDevicePermissionSetValue.PtzControl);

	public static CameraDevicePermission ActivatePtzPreset => new CameraDevicePermission(CameraDevicePermissionSetValue.ActivatePtzPreset);

	public static CameraDevicePermission ManagePresetAndPatrolling => new CameraDevicePermission(CameraDevicePermissionSetValue.ManagePresetAndPatrolling);

	public static CameraDevicePermission ManageLockPreset => new CameraDevicePermission(CameraDevicePermissionSetValue.ManageLockPreset);

	public static CameraDevicePermission ReservePtzSession => new CameraDevicePermission(CameraDevicePermissionSetValue.ReservePtzSession);

	public static CameraDevicePermission ReleasePtzSession => new CameraDevicePermission(CameraDevicePermissionSetValue.ReleasePtzSession);

	public static CameraDevicePermission DeleteRecordings => new CameraDevicePermission(CameraDevicePermissionSetValue.DeleteRecordings);

	public static CameraDevicePermission LiftPrivacyMasking => new CameraDevicePermission(CameraDevicePermissionSetValue.LiftPrivacyMasking);

	public static CameraDevicePermission AdministrateSecurity => new CameraDevicePermission(CameraDevicePermissionSetValue.AdministrateSecurity);

	public CameraDevicePermission(CameraDevicePermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static CameraDevicePermissionSet Add(CameraDevicePermission permissionLeft, CameraDevicePermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static CameraDevicePermissionSet operator +(CameraDevicePermission permissionLeft, CameraDevicePermission permissionRight)
	{
		CameraDevicePermissionSet cameraDevicePermissionSet = new CameraDevicePermissionSet(permissionLeft);
		cameraDevicePermissionSet.SetBit(permissionRight);
		return cameraDevicePermissionSet;
	}

	public static CameraDevicePermissionSet BitwiseOr(CameraDevicePermission permissionLeft, CameraDevicePermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static CameraDevicePermissionSet operator |(CameraDevicePermission permissionLeft, CameraDevicePermission permissionRight)
	{
		CameraDevicePermissionSet cameraDevicePermissionSet = new CameraDevicePermissionSet(permissionLeft);
		cameraDevicePermissionSet.SetBit(permissionRight);
		return cameraDevicePermissionSet;
	}

	public static bool Equals(CameraDevicePermission permissionLeft, CameraDevicePermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(CameraDevicePermission permissionLeft, CameraDevicePermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(CameraDevicePermission permissionLeft, CameraDevicePermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static CameraDevicePermissionSet ToCameraDevicePermissionSet(CameraDevicePermission permission)
	{
		return new CameraDevicePermissionSet(permission);
	}

	public static implicit operator CameraDevicePermissionSet(CameraDevicePermission permission)
	{
		return new CameraDevicePermissionSet(permission);
	}

	public bool Equals(CameraDevicePermission other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		return Value == other.Value;
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is CameraDevicePermission other))
		{
			return false;
		}
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}
}
