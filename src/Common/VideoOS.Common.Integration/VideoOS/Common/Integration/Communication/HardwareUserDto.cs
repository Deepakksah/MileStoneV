using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class HardwareUserDto
{
	public string UserId { get; private set; }

	public string UserName { get; private set; }

	public string UserRole { get; private set; }

	public string UserPassword { get; private set; }

	public HardwareUserLevel UserLevel { get; private set; }

	public static HardwareUserDto CreateDto(HardwareUser responseUser)
	{
		if (responseUser == null)
		{
			throw new ArgumentNullException("responseUser");
		}
		return new HardwareUserDto
		{
			UserId = responseUser.UserId,
			UserName = responseUser.UserName,
			UserRole = responseUser.UserLevelString,
			UserPassword = responseUser.UserPassword,
			UserLevel = responseUser.UserLevel
		};
	}

	public static HardwareUser CreateHardwareUser(HardwareUserDto dtoUser)
	{
		if (dtoUser == null)
		{
			throw new ArgumentNullException("dtoUser");
		}
		return new HardwareUser
		{
			UserId = dtoUser.UserId,
			UserName = dtoUser.UserName,
			UserLevelString = dtoUser.UserRole,
			UserPassword = dtoUser.UserPassword,
			UserLevel = dtoUser.UserLevel
		};
	}
}
