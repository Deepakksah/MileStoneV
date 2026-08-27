using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class GetAbsolutePtzPositionResponse : CommonResponseBase
{
	[DataMember]
	public double Pan { get; set; }

	[DataMember]
	public double Tilt { get; set; }

	[DataMember]
	public double Zoom { get; set; }

	[DataMember]
	public Guid DeviceId { get; set; }

	public static GetAbsolutePtzPositionResponse Create(Guid deviceId, double pan, double tilt, double zoom)
	{
		return new GetAbsolutePtzPositionResponse
		{
			Pan = pan,
			Tilt = tilt,
			Zoom = zoom,
			DeviceId = deviceId
		};
	}

	private GetAbsolutePtzPositionResponse()
	{
	}

	public override string ToString()
	{
		return GetType().Name;
	}

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
}
