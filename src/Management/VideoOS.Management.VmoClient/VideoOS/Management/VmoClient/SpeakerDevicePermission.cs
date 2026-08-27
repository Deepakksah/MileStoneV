using System;

namespace VideoOS.Management.VmoClient;

public sealed class SpeakerDevicePermission : IEquatable<SpeakerDevicePermission>
{
	public SpeakerDevicePermissionSetValue Value { get; private set; }

	public static SpeakerDevicePermission Read => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Read);

	public static SpeakerDevicePermission Edit => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Edit);

	public static SpeakerDevicePermission Listen => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Listen);

	public static SpeakerDevicePermission ListenRestricted => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ListenRestricted);

	public static SpeakerDevicePermission Speak => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Speak);

	public static SpeakerDevicePermission Playback => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Playback);

	public static SpeakerDevicePermission PlaybackRestrictedMedia => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.PlaybackRestrictedMedia);

	public static SpeakerDevicePermission ReadRemoteRecordings => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadRemoteRecordings);

	public static SpeakerDevicePermission ReadSequences => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadSequences);

	public static SpeakerDevicePermission Export => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.Export);

	public static SpeakerDevicePermission CreateBookmarks => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.CreateBookmarks);

	public static SpeakerDevicePermission ReadBookmarks => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadBookmarks);

	public static SpeakerDevicePermission EditBookmarks => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.EditBookmarks);

	public static SpeakerDevicePermission DeleteBookmarks => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.DeleteBookmarks);

	public static SpeakerDevicePermission CreateEvidenceLock => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.CreateEvidenceLock);

	public static SpeakerDevicePermission ReadEvidenceLock => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadEvidenceLock);

	public static SpeakerDevicePermission DeleteEvidenceLock => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.DeleteEvidenceLock);

	public static SpeakerDevicePermission CreateRestrictedMedia => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.CreateRestrictedMedia);

	public static SpeakerDevicePermission ReadRestrictedMedia => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.ReadRestrictedMedia);

	public static SpeakerDevicePermission DeleteRestrictedMedia => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.DeleteRestrictedMedia);

	public static SpeakerDevicePermission StartManualRecording => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.StartManualRecording);

	public static SpeakerDevicePermission StopManualRecording => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.StopManualRecording);

	public static SpeakerDevicePermission DeleteRecordings => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.DeleteRecordings);

	public static SpeakerDevicePermission AdministrateSecurity => new SpeakerDevicePermission(SpeakerDevicePermissionSetValue.AdministrateSecurity);

	public SpeakerDevicePermission(SpeakerDevicePermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static SpeakerDevicePermissionSet Add(SpeakerDevicePermission permissionLeft, SpeakerDevicePermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static SpeakerDevicePermissionSet operator +(SpeakerDevicePermission permissionLeft, SpeakerDevicePermission permissionRight)
	{
		SpeakerDevicePermissionSet speakerDevicePermissionSet = new SpeakerDevicePermissionSet(permissionLeft);
		speakerDevicePermissionSet.SetBit(permissionRight);
		return speakerDevicePermissionSet;
	}

	public static SpeakerDevicePermissionSet BitwiseOr(SpeakerDevicePermission permissionLeft, SpeakerDevicePermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static SpeakerDevicePermissionSet operator |(SpeakerDevicePermission permissionLeft, SpeakerDevicePermission permissionRight)
	{
		SpeakerDevicePermissionSet speakerDevicePermissionSet = new SpeakerDevicePermissionSet(permissionLeft);
		speakerDevicePermissionSet.SetBit(permissionRight);
		return speakerDevicePermissionSet;
	}

	public static bool Equals(SpeakerDevicePermission permissionLeft, SpeakerDevicePermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(SpeakerDevicePermission permissionLeft, SpeakerDevicePermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(SpeakerDevicePermission permissionLeft, SpeakerDevicePermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static SpeakerDevicePermissionSet ToSpeakerDevicePermissionSet(SpeakerDevicePermission permission)
	{
		return new SpeakerDevicePermissionSet(permission);
	}

	public static implicit operator SpeakerDevicePermissionSet(SpeakerDevicePermission permission)
	{
		return new SpeakerDevicePermissionSet(permission);
	}

	public bool Equals(SpeakerDevicePermission other)
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
		if (!(obj is SpeakerDevicePermission other))
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
