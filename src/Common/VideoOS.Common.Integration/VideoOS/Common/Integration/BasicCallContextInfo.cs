using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class BasicCallContextInfo : ICallContextInfo, ISerializable
{
	public string LocationIPAddress { get; private set; }

	public string UserDomainAccountName { get; private set; }

	public string UserSid { get; private set; }

	public BasicCallContextInfo(ICallContextInfo callContextInfo)
	{
		if (callContextInfo == null)
		{
			throw new ArgumentNullException("callContextInfo");
		}
		UserSid = callContextInfo.UserSid;
		UserDomainAccountName = callContextInfo.UserDomainAccountName;
		LocationIPAddress = callContextInfo.LocationIPAddress;
	}

	public BasicCallContextInfo(string userSid, string userDomainAccountName, string locationIPAddress)
	{
		UserSid = userSid;
		UserDomainAccountName = userDomainAccountName;
		LocationIPAddress = locationIPAddress;
	}

	protected BasicCallContextInfo(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		LocationIPAddress = info.GetString("LocationIPAddress");
		UserDomainAccountName = info.GetString("UserDomainAccountName");
		UserSid = info.GetString("UserSid");
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("LocationIPAddress", LocationIPAddress);
		info.AddValue("UserDomainAccountName", UserDomainAccountName);
		info.AddValue("UserSid", UserSid);
	}
}
