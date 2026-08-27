using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class VideoWallPermissionSet : PermissionSetBase, IEquatable<VideoWallPermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Write => base.PermissionStorage[1];

	public bool Delete => base.PermissionStorage[2];

	public bool Operate => base.PermissionStorage[3];

	public bool AdministrateSecurity => base.PermissionStorage[4];

	public bool Playback => base.PermissionStorage[5];

	protected override int NumberOfElements => 6;

	public VideoWallPermissionSet()
	{
	}

	public VideoWallPermissionSet(VideoWallPermission permission)
	{
		SetBit(permission);
	}

	public VideoWallPermissionSet(VideoWallPermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public VideoWallPermissionSet Add(VideoWallPermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(VideoWallPermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(VideoWallPermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static VideoWallPermissionSet SupportedPermissions(ServerVersion version)
	{
		VideoWallPermissionSet videoWallPermissionSet = new VideoWallPermissionSet();
		videoWallPermissionSet.SetBit(VideoWallPermission.Read);
		videoWallPermissionSet.SetBit(VideoWallPermission.Write);
		videoWallPermissionSet.SetBit(VideoWallPermission.Delete);
		videoWallPermissionSet.SetBit(VideoWallPermission.Operate);
		videoWallPermissionSet.SetBit(VideoWallPermission.AdministrateSecurity);
		videoWallPermissionSet.SetBit(VideoWallPermission.Playback);
		return videoWallPermissionSet;
	}

	public static VideoWallPermissionSet Add(VideoWallPermissionSet permissionSetLeft, VideoWallPermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static VideoWallPermissionSet operator +(VideoWallPermissionSet permissionSetLeft, VideoWallPermission permissionRight)
	{
		VideoWallPermissionSet videoWallPermissionSet = new VideoWallPermissionSet(permissionSetLeft);
		videoWallPermissionSet.SetBit(permissionRight);
		return videoWallPermissionSet;
	}

	public static VideoWallPermissionSet Subtract(VideoWallPermissionSet permissionSetLeft, VideoWallPermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static VideoWallPermissionSet operator -(VideoWallPermissionSet permissionSetLeft, VideoWallPermission permissionRight)
	{
		VideoWallPermissionSet videoWallPermissionSet = new VideoWallPermissionSet(permissionSetLeft);
		videoWallPermissionSet.ResetBit(permissionRight);
		return videoWallPermissionSet;
	}

	public static bool Equals(VideoWallPermissionSet permissionSetLeft, VideoWallPermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(VideoWallPermissionSet permissionSetLeft, VideoWallPermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(VideoWallPermissionSet permissionSetLeft, VideoWallPermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(VideoWallPermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is VideoWallPermissionSet other))
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
			2 => "DELETE", 
			3 => "OPERATE", 
			4 => "ADMIN_SECURITY", 
			5 => "PLAYBACK", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static VideoWallPermission CreateFromActionId(string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return actionId switch
		{
			"GENERIC_READ" => new VideoWallPermission(VideoWallPermissionSetValue.Read), 
			"GENERIC_WRITE" => new VideoWallPermission(VideoWallPermissionSetValue.Edit), 
			"DELETE" => new VideoWallPermission(VideoWallPermissionSetValue.Delete), 
			"OPERATE" => new VideoWallPermission(VideoWallPermissionSetValue.Operate), 
			"ADMIN_SECURITY" => new VideoWallPermission(VideoWallPermissionSetValue.AdministrateSecurity), 
			"PLAYBACK" => new VideoWallPermission(VideoWallPermissionSetValue.Playback), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		VideoWallPermissionSet result = new VideoWallPermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			VideoWallPermission videoWallPermission = CreateFromActionId(actionIdArray[i]);
			result += videoWallPermission;
		}
		return result;
	}

	public static string PermissionEnumToActionId(VideoWallPermissionSetValue value)
	{
		return value switch
		{
			VideoWallPermissionSetValue.Read => "GENERIC_READ", 
			VideoWallPermissionSetValue.Edit => "GENERIC_WRITE", 
			VideoWallPermissionSetValue.Delete => "DELETE", 
			VideoWallPermissionSetValue.Operate => "OPERATE", 
			VideoWallPermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			VideoWallPermissionSetValue.Playback => "PLAYBACK", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}
}
