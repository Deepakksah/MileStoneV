using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace VideoOS.Mobile.Utilities;

public class PhysicalAddressHelper
{
	public const int BytesCount = 6;

	public static PhysicalAddress FromGuid(Guid guid)
	{
		return new PhysicalAddress(guid.ToByteArray().Take(6).ToArray());
	}

	public static PhysicalAddress NewPhysicalAddress()
	{
		return FromGuid(Guid.NewGuid());
	}
}
