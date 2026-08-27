using System;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using VideoOS.Utilities.Network;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("AddressIntervals")]
public class AddressIntervals
{
	[XmlElement("AddressInterval")]
	public AddressIntervalCollection Items { get; set; }

	public AddressIntervals()
	{
		Items = new AddressIntervalCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static AddressIntervals FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<AddressIntervals>(xml);
	}

	public AddressIntervals Clone()
	{
		return FromXml(ToXml());
	}

	public void LoadMachineDefaults()
	{
		foreach (ManagementObject item in new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = TRUE").Get())
		{
			string[] array = (string[])item["IPAddress"];
			string[] array2 = (string[])item["IPSubnet"];
			if (array == null || array2 == null)
			{
				continue;
			}
			for (int i = 0; i < array.Length; i++)
			{
				string ipString = array[i];
				string ipString2 = array2[i];
				if (IPAddress.TryParse(ipString, out var address) && address.AddressFamily == AddressFamily.InterNetwork && IPAddress.TryParse(ipString2, out var address2) && address != IPAddress.Any && address2 != IPAddress.Any)
				{
					IPAddress iPAddress = new IPAddress((uint)((int)address.Address & (int)address2.Address));
					IPAddress firstAddress = IPHelper.IncrementIPAddress(iPAddress);
					IPAddress lastAddress = IPHelper.DecrementIPAddress(new IPAddress((uint)((int)iPAddress.Address | (int)(~address2.Address))));
					if (firstAddress.Address < lastAddress.Address && !Items.Any((AddressInterval a) => string.Equals(a.FirstIPAddress, firstAddress.ToString(), StringComparison.Ordinal) && string.Equals(a.LastIPAddress, lastAddress.ToString(), StringComparison.Ordinal)))
					{
						Items.Add(firstAddress.ToString(), lastAddress.ToString(), useSecurePort: false);
					}
				}
			}
		}
	}
}
