using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class QueryDeviceEvidenceLockSizesResponse : CommonResponseBase
{
	[DataMember]
	public Collection<DeviceEvidenceLockSize> Sizes { get; } = new Collection<DeviceEvidenceLockSize>();

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}

	public override string ToString()
	{
		return GetType().Name;
	}
}
