using System;

namespace VideoOS.Management.VmoClient;

public sealed class MetadataDevicePermission : IEquatable<MetadataDevicePermission>
{
	public MetadataDevicePermissionSetValue Value { get; private set; }

	public static MetadataDevicePermission Read => new MetadataDevicePermission(MetadataDevicePermissionSetValue.Read);

	public static MetadataDevicePermission Edit => new MetadataDevicePermission(MetadataDevicePermissionSetValue.Edit);

	public static MetadataDevicePermission Live => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ViewLive);

	public static MetadataDevicePermission ViewRestrictedLive => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ViewRestrictedLive);

	public static MetadataDevicePermission Playback => new MetadataDevicePermission(MetadataDevicePermissionSetValue.Playback);

	public static MetadataDevicePermission PlaybackRestrictedMedia => new MetadataDevicePermission(MetadataDevicePermissionSetValue.PlaybackRestrictedMedia);

	public static MetadataDevicePermission ReadRemoteRecordings => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ReadRemoteRecordings);

	public static MetadataDevicePermission ReadSequences => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ReadSequences);

	public static MetadataDevicePermission Export => new MetadataDevicePermission(MetadataDevicePermissionSetValue.Export);

	public static MetadataDevicePermission CreateEvidenceLock => new MetadataDevicePermission(MetadataDevicePermissionSetValue.CreateEvidenceLock);

	public static MetadataDevicePermission ReadEvidenceLock => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ReadEvidenceLock);

	public static MetadataDevicePermission DeleteEvidenceLock => new MetadataDevicePermission(MetadataDevicePermissionSetValue.DeleteEvidenceLock);

	public static MetadataDevicePermission CreateRestrictedMedia => new MetadataDevicePermission(MetadataDevicePermissionSetValue.CreateRestrictedMedia);

	public static MetadataDevicePermission ReadRestrictedMedia => new MetadataDevicePermission(MetadataDevicePermissionSetValue.ReadRestrictedMedia);

	public static MetadataDevicePermission DeleteRestrictedMedia => new MetadataDevicePermission(MetadataDevicePermissionSetValue.DeleteRestrictedMedia);

	public static MetadataDevicePermission StartManualRecording => new MetadataDevicePermission(MetadataDevicePermissionSetValue.StartManualRecording);

	public static MetadataDevicePermission StopManualRecording => new MetadataDevicePermission(MetadataDevicePermissionSetValue.StopManualRecording);

	public static MetadataDevicePermission DeleteRecordings => new MetadataDevicePermission(MetadataDevicePermissionSetValue.DeleteRecordings);

	public static MetadataDevicePermission AdministrateSecurity => new MetadataDevicePermission(MetadataDevicePermissionSetValue.AdministrateSecurity);

	public MetadataDevicePermission(MetadataDevicePermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static MetadataDevicePermissionSet Add(MetadataDevicePermission permissionLeft, MetadataDevicePermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static MetadataDevicePermissionSet operator +(MetadataDevicePermission permissionLeft, MetadataDevicePermission permissionRight)
	{
		MetadataDevicePermissionSet metadataDevicePermissionSet = new MetadataDevicePermissionSet(permissionLeft);
		metadataDevicePermissionSet.SetBit(permissionRight);
		return metadataDevicePermissionSet;
	}

	public static MetadataDevicePermissionSet BitwiseOr(MetadataDevicePermission permissionLeft, MetadataDevicePermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static MetadataDevicePermissionSet operator |(MetadataDevicePermission permissionLeft, MetadataDevicePermission permissionRight)
	{
		MetadataDevicePermissionSet metadataDevicePermissionSet = new MetadataDevicePermissionSet(permissionLeft);
		metadataDevicePermissionSet.SetBit(permissionRight);
		return metadataDevicePermissionSet;
	}

	public static bool Equals(MetadataDevicePermission permissionLeft, MetadataDevicePermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(MetadataDevicePermission permissionLeft, MetadataDevicePermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(MetadataDevicePermission permissionLeft, MetadataDevicePermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static MetadataDevicePermissionSet ToMetadataDevicePermissionSet(MetadataDevicePermission permission)
	{
		return new MetadataDevicePermissionSet(permission);
	}

	public static implicit operator MetadataDevicePermissionSet(MetadataDevicePermission permission)
	{
		return new MetadataDevicePermissionSet(permission);
	}

	public bool Equals(MetadataDevicePermission other)
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
		if (!(obj is MetadataDevicePermission other))
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
