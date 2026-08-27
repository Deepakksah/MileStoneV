using System;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class HardwareUser
{
	[DataMember]
	public string UserId { get; set; }

	[DataMember]
	public string UserName { get; set; }

	[DataMember]
	public string UserPassword { get; set; }

	[DataMember]
	public HardwareUserLevel UserLevel { get; set; }

	[DataMember]
	public string UserLevelString { get; set; }

	public string UserLevelLabel { get; set; }

	public string UserNameAndUserLevel => FormattableString.Invariant($"{UserName} ({UserLevelLabel}{UserLevelString})");

	public HardwareUser()
	{
	}

	public HardwareUser(HardwareUser user)
	{
		if (user == null)
		{
			throw new ArgumentNullException("user");
		}
		UserId = user.UserId;
		UserName = user.UserName;
		UserPassword = user.UserPassword;
		UserLevel = user.UserLevel;
		UserLevelString = user.UserLevelString;
	}
}
