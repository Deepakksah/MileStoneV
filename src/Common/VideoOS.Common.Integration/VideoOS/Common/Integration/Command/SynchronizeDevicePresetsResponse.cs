using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class SynchronizeDevicePresetsResponse : CommonResponseBase
{
	[DataMember]
	public string PresetsXml { get; private set; }

	public SynchronizeDevicePresetsResponse(string presetsXml)
	{
		PresetsXml = presetsXml;
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
