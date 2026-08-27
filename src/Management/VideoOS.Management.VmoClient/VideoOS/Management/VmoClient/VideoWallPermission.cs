using System;
using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

public sealed class VideoWallPermission : IEquatable<VideoWallPermission>
{
	public VideoWallPermissionSetValue Value { get; private set; }

	public static VideoWallPermission Read
	{
		[DebuggerStepThrough]
		get
		{
			return new VideoWallPermission(VideoWallPermissionSetValue.Read);
		}
	}

	public static VideoWallPermission Write
	{
		[DebuggerStepThrough]
		get
		{
			return new VideoWallPermission(VideoWallPermissionSetValue.Edit);
		}
	}

	public static VideoWallPermission Delete
	{
		[DebuggerStepThrough]
		get
		{
			return new VideoWallPermission(VideoWallPermissionSetValue.Delete);
		}
	}

	public static VideoWallPermission Operate
	{
		[DebuggerStepThrough]
		get
		{
			return new VideoWallPermission(VideoWallPermissionSetValue.Operate);
		}
	}

	public static VideoWallPermission AdministrateSecurity
	{
		[DebuggerStepThrough]
		get
		{
			return new VideoWallPermission(VideoWallPermissionSetValue.AdministrateSecurity);
		}
	}

	public static VideoWallPermission Playback
	{
		[DebuggerStepThrough]
		get
		{
			return new VideoWallPermission(VideoWallPermissionSetValue.Playback);
		}
	}

	public VideoWallPermission(VideoWallPermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static VideoWallPermissionSet Add(VideoWallPermission permissionLeft, VideoWallPermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static VideoWallPermissionSet operator +(VideoWallPermission permissionLeft, VideoWallPermission permissionRight)
	{
		VideoWallPermissionSet videoWallPermissionSet = new VideoWallPermissionSet(permissionLeft);
		videoWallPermissionSet.SetBit(permissionRight);
		return videoWallPermissionSet;
	}

	public static VideoWallPermissionSet BitwiseOr(VideoWallPermission permissionLeft, VideoWallPermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static VideoWallPermissionSet operator |(VideoWallPermission permissionLeft, VideoWallPermission permissionRight)
	{
		VideoWallPermissionSet videoWallPermissionSet = new VideoWallPermissionSet(permissionLeft);
		videoWallPermissionSet.SetBit(permissionRight);
		return videoWallPermissionSet;
	}

	public static bool Equals(VideoWallPermission permissionLeft, VideoWallPermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(VideoWallPermission permissionLeft, VideoWallPermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(VideoWallPermission permissionLeft, VideoWallPermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static VideoWallPermissionSet ToVideoWallPermissionSet(VideoWallPermission permission)
	{
		return new VideoWallPermissionSet(permission);
	}

	public static implicit operator VideoWallPermissionSet(VideoWallPermission permission)
	{
		return new VideoWallPermissionSet(permission);
	}

	public bool Equals(VideoWallPermission other)
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
		if (!(obj is VideoWallPermission other))
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
