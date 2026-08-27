using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

[DataContract]
public class ServerStatus
{
	public const string UpSinceXmlName = "UpSince";

	public const string ServerVersionXmlName = "ServerVersion";

	public const string CpuUsageXmlName = "CpuUsage";

	public const string ThreadsCountXmlName = "ThreadsCount";

	public const string MobileUsersName = "MobileUsers";

	public const string SingleUserName = "User";

	public const string CamerasXmlName = "Cameras";

	public const string SingleCameraXmlName = "Camera";

	public const string ServerEndpointsXmlName = "ServerEndpoint";

	public const string LastCheckXmlName = "EndpointsLastCheck";

	public const string ServerIpXmlName = "ServerIp";

	public const string HwAcceleratedCountXmlName = "HwAcceleratedCount";

	public const string HwAcceleratedTypeXmlName = "HwAcceleratedType";

	[JsonProperty("ServerVersion")]
	[XmlElement(ElementName = "ServerVersion")]
	public ServerVersion ServerVersion;

	[JsonProperty("UpSince")]
	[XmlElement(ElementName = "UpSince")]
	[DataMember]
	public string UpSinceString { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	[DataMember]
	public DateTime UpSince
	{
		get
		{
			DateTime result;
			try
			{
				result = TimeConverter.FromString(UpSinceString);
			}
			catch (Exception ex)
			{
				result = DateTime.UtcNow;
				Logger.Instance.Log(LogContent.Error, "ServerStatus.UpSince.Get", ex.Message);
			}
			return result;
		}
		set
		{
			UpSinceString = value.ToMilisecString();
		}
	}

	[JsonProperty("CpuUsage")]
	[XmlElement(ElementName = "CpuUsage")]
	[DataMember]
	public long CpuUsagePercentage { get; set; }

	[JsonProperty("ThreadsCount")]
	[XmlElement(ElementName = "ThreadsCount")]
	[DataMember]
	public long ThreadsCount { get; set; }

	[JsonProperty("MobileUsers")]
	[XmlArray(ElementName = "MobileUsers")]
	[XmlArrayItem(ElementName = "User")]
	public List<MobileUser> MobileUsers { get; set; }

	[JsonProperty("Cameras")]
	[XmlArray(ElementName = "Cameras")]
	[XmlArrayItem(ElementName = "Camera")]
	public List<Camera> Cameras { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	[DataMember]
	public long UsersCount { get; set; }

	[JsonProperty("ServerEndpoint")]
	[XmlElement("ServerEndpoint")]
	public List<ServerEndpoint> ServerEndpoints { get; set; }

	[JsonProperty("EndpointsLastCheck")]
	[XmlElement("EndpointsLastCheck")]
	public DateTime EndpointsLastCheck { get; set; }

	[JsonProperty("ServerIp")]
	[XmlElement("ServerIp")]
	public string ExternalIp { get; set; }

	[JsonIgnore]
	[DataMember]
	public long HwAcceleratedCount { get; set; }

	[JsonIgnore]
	[DataMember]
	public string HwAcceleratedType { get; set; }

	public ServerStatus()
	{
		MobileUsers = new List<MobileUser>();
		Cameras = new List<Camera>();
	}
}
