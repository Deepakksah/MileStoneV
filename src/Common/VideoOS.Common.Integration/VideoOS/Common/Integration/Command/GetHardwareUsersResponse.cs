using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class GetHardwareUsersResponse : CommonResponseBase
{
	[DataMember]
	public Guid HardwareId { get; set; }

	[DataMember]
	public Collection<HardwareUser> HardwareUsers { get; private set; }

	public GetHardwareUsersResponse()
	{
	}

	public GetHardwareUsersResponse(Guid hardwareId, IList<HardwareUser> hardwareUsers)
	{
		HardwareId = hardwareId;
		HardwareUsers = new Collection<HardwareUser>(hardwareUsers);
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
