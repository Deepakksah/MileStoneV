using System;

namespace VideoOS.Management.VmoClient;

public sealed class MicrophoneDevicePermission : IEquatable<MicrophoneDevicePermission>
{
	public MicrophoneDevicePermissionSetValue Value { get; private set; }

	public static MicrophoneDevicePermission Read => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Read);

	public static MicrophoneDevicePermission Edit => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Edit);

	public static MicrophoneDevicePermission Listen => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Listen);

	public static MicrophoneDevicePermission ListenRestricted => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ListenRestricted);

	public static MicrophoneDevicePermission Playback => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Playback);

	public static MicrophoneDevicePermission PlaybackRestrictedMedia => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.PlaybackRestrictedMedia);

	public static MicrophoneDevicePermission ReadRemoteRecordings => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadRemoteRecordings);

	public static MicrophoneDevicePermission ReadSequences => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadSequences);

	public static MicrophoneDevicePermission Export => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.Export);

	public static MicrophoneDevicePermission CreateBookmarks => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.CreateBookmarks);

	public static MicrophoneDevicePermission ReadBookmarks => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadBookmarks);

	public static MicrophoneDevicePermission EditBookmarks => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.EditBookmarks);

	public static MicrophoneDevicePermission DeleteBookmarks => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.DeleteBookmarks);

	public static MicrophoneDevicePermission CreateEvidenceLock => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.CreateEvidenceLock);

	public static MicrophoneDevicePermission ReadEvidenceLock => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadEvidenceLock);

	public static MicrophoneDevicePermission DeleteEvidenceLock => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.DeleteEvidenceLock);

	public static MicrophoneDevicePermission CreateRestrictedMedia => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.CreateRestrictedMedia);

	public static MicrophoneDevicePermission ReadRestrictedMedia => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.ReadRestrictedMedia);

	public static MicrophoneDevicePermission DeleteRestrictedMedia => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.DeleteRestrictedMedia);

	public static MicrophoneDevicePermission StartManualRecording => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.StartManualRecording);

	public static MicrophoneDevicePermission StopManualRecording => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.StopManualRecording);

	public static MicrophoneDevicePermission DeleteRecordings => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.DeleteRecordings);

	public static MicrophoneDevicePermission AdministrateSecurity => new MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue.AdministrateSecurity);

	public MicrophoneDevicePermission(MicrophoneDevicePermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static MicrophoneDevicePermissionSet Add(MicrophoneDevicePermission permissionLeft, MicrophoneDevicePermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static MicrophoneDevicePermissionSet operator +(MicrophoneDevicePermission permissionLeft, MicrophoneDevicePermission permissionRight)
	{
		MicrophoneDevicePermissionSet microphoneDevicePermissionSet = new MicrophoneDevicePermissionSet(permissionLeft);
		microphoneDevicePermissionSet.SetBit(permissionRight);
		return microphoneDevicePermissionSet;
	}

	public static MicrophoneDevicePermissionSet BitwiseOr(MicrophoneDevicePermission permissionLeft, MicrophoneDevicePermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static MicrophoneDevicePermissionSet operator |(MicrophoneDevicePermission permissionLeft, MicrophoneDevicePermission permissionRight)
	{
		MicrophoneDevicePermissionSet microphoneDevicePermissionSet = new MicrophoneDevicePermissionSet(permissionLeft);
		microphoneDevicePermissionSet.SetBit(permissionRight);
		return microphoneDevicePermissionSet;
	}

	public static bool Equals(MicrophoneDevicePermission permissionLeft, MicrophoneDevicePermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(MicrophoneDevicePermission permissionLeft, MicrophoneDevicePermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(MicrophoneDevicePermission permissionLeft, MicrophoneDevicePermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static MicrophoneDevicePermissionSet ToMicrophoneDevicePermissionSet(MicrophoneDevicePermission permission)
	{
		return new MicrophoneDevicePermissionSet(permission);
	}

	public static implicit operator MicrophoneDevicePermissionSet(MicrophoneDevicePermission permission)
	{
		return new MicrophoneDevicePermissionSet(permission);
	}

	public bool Equals(MicrophoneDevicePermission other)
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
		if (!(obj is MicrophoneDevicePermission other))
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
