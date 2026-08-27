using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class HardwareUserModel
{
	public string UserId { get; set; }

	public string UserName { get; set; }

	public string UserLevelString { get; set; }

	public string UserPassword { get; set; }

	public HardwareUserLevel UserLevel { get; set; }

	public static HardwareUserModel FromHardwareUser(HardwareUser hardwareUser)
	{
		if (hardwareUser == null)
		{
			throw new ArgumentNullException("hardwareUser");
		}
		return new HardwareUserModel
		{
			UserId = hardwareUser.UserId,
			UserName = hardwareUser.UserName,
			UserLevelString = hardwareUser.UserLevelString,
			UserPassword = hardwareUser.UserPassword,
			UserLevel = hardwareUser.UserLevel
		};
	}

	public static HardwareUser ToHardwareUser(HardwareUserModel hardwareUserModel)
	{
		if (hardwareUserModel == null)
		{
			throw new ArgumentNullException("hardwareUserModel");
		}
		return new HardwareUser
		{
			UserId = hardwareUserModel.UserId,
			UserName = hardwareUserModel.UserName,
			UserLevelString = hardwareUserModel.UserLevelString,
			UserPassword = hardwareUserModel.UserPassword,
			UserLevel = hardwareUserModel.UserLevel
		};
	}
}
