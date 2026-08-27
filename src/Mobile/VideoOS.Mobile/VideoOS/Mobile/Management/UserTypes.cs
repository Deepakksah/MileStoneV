using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Management;

public class UserTypes
{
	public const int MinIndex = 0;

	public const int MaxIndex = 3;

	public const int BitsPerByte = 8;

	public const uint ByteMask = 255u;

	public uint UserTypeLogOrder { get; set; }

	public UserType this[int index]
	{
		get
		{
			if (index < 0 || index > 3)
			{
				return UserType.Unknown;
			}
			return GetAtIndex(index);
		}
		set
		{
			if (index >= 0 && index <= 3)
			{
				SetAtIndex(index, value);
			}
		}
	}

	public uint Count
	{
		get
		{
			uint num = 0u;
			for (int i = 0; i <= 3 && IsSetAt(i); i++)
			{
				num++;
			}
			return num;
		}
	}

	public void Clear()
	{
		UserTypeLogOrder = 0u;
	}

	private UserType GetAtIndex(int index)
	{
		return (UserType)((UserTypeLogOrder >> index * 8) & 0xFF);
	}

	private void SetAtIndex(int index, UserType type)
	{
		UserTypeLogOrder |= (uint)((int)type << index * 8);
	}

	public bool IsSetAt(int index)
	{
		return GetAtIndex(index) != UserType.Unknown;
	}

	public int AddUserType(UserType type)
	{
		int num = (int)Count;
		if (num < 3)
		{
			SetAtIndex(num, type);
		}
		else
		{
			num = -1;
		}
		return num;
	}

	public UserTypes AddUserTypeE(UserType type)
	{
		int num = AddUserType(type);
		if (num < 0 || num > 3)
		{
			throw new IndexOutOfRangeException();
		}
		return this;
	}

	public bool IsSetUserType(UserType userType)
	{
		for (int i = 0; i <= 3; i++)
		{
			if (userType == GetAtIndex(i))
			{
				return true;
			}
		}
		return false;
	}
}
