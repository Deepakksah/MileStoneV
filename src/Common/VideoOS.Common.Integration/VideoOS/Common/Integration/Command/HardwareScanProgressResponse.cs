using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.DeviceDiscovery;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class HardwareScanProgressResponse : CommonResponseBase
{
	[DataMember]
	public string ScanResultXml { get; set; }

	public static HardwareScanProgressResponse Create(string scanResultXml)
	{
		return new HardwareScanProgressResponse
		{
			ScanResultXml = scanResultXml
		};
	}

	private HardwareScanProgressResponse()
	{
	}

	public HardwareScanProgressResponse(ScanResult scanResult)
	{
		if (scanResult == null)
		{
			throw new ArgumentNullException("scanResult");
		}
		ScanResultXml = scanResult.ToXml();
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
