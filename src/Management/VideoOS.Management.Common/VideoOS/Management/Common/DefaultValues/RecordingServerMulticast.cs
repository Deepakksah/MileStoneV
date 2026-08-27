using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using VideoOS.Management.Common.Properties;

namespace VideoOS.Management.Common.DefaultValues;

public static class RecordingServerMulticast
{
	public static readonly IPAddress IPv4MulticastAllowedRangeStart = IPAddress.Parse("224.0.0.0");

	public static readonly IPAddress IPv4MulticastAllowedRangeEnd = IPAddress.Parse("239.255.255.255");

	public static readonly IPAddress IPv6MulticastAllowedRangeStart = IPAddress.Parse("FF00::0");

	public static readonly IPAddress IPv6MulticastAllowedRangeEnd = IPAddress.Parse("FFFF::FFFF");

	public static readonly IPAddress IPv4MulticastDefaultRangeStart = IPAddress.Parse("232.0.1.0");

	public static readonly IPAddress IPv4MulticastDefaultRangeEnd = IPAddress.Parse("232.255.255.255");

	public static readonly IPAddress IPv6MulticastDefaultRangeStart = IPAddress.Parse("FF30::8000:0");

	public static readonly IPAddress IPv6MulticastDefaultRangeEnd = IPAddress.Parse("FF3F::FFFF:FFFF");

	public const int MinMulticastPort = 1;

	public const int MaxMulticastPort = 65535;

	public const int MinRecordingServerPort = 0;

	public const int MaxRecordingServerPort = 65535;

	public static readonly string DefaultXml = "<properties><settings><setting><name>MulticastEnabled</name><value>True</value></setting><setting><name>ClientIPRangeStart</name><value>232.0.1.0</value></setting><setting><name>ClientIPRangeEnd</name><value>232.0.1.0</value></setting><setting><name>ClientPortRangeStart</name><value>6000</value></setting><setting><name>ClientPortRangeEnd</name><value>7000</value></setting><setting><name>MTU</name><value>1500</value></setting><setting><name>TTL</name><value>32</value></setting></settings></properties>";

	public static bool ValidateMulticastSettingsSchema(string settings)
	{
		bool errors = false;
		try
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			using StringReader input = new StringReader(Resources.MulticastSchema);
			xmlSchemaSet.Add("", XmlReader.Create(input));
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(settings);
			xmlDocument.Schemas.Add(xmlSchemaSet);
			xmlDocument.Validate(delegate
			{
				errors = true;
			});
		}
		catch
		{
			errors = true;
		}
		return errors;
	}
}
