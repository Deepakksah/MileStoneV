using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[DataContract(Name = "ServerInformation", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class ServerInformationOld
{
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public string InstanceId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string Platform { get; set; }

	[DataMember]
	public int Processors { get; set; }

	[DataMember]
	public int PhysicalMemory { get; set; }

	[DataMember]
	public string ComputerName { get; set; }

	[DataMember]
	public string ServiceAccount { get; set; }

	[DataMember]
	public Version Version { get; set; }

	[DataMember]
	public ServerAddress[] Addresses { get; set; }

	[DataMember]
	public string MasterSite { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	[DataMember]
	public string DomainName { get; set; }

	[DataMember]
	public DateTime LastStatusHandshake { get; set; }

	[DataMember]
	public int SynchronizationStatus { get; set; }

	[DataMember]
	public string TimeZone { get; set; }

	public static ServerInformationOld GetServerInformation(GeminiRequestContext requestContext)
	{
		return ConvertToServerInformationOld(ServerInformation.GetServerInformation(requestContext));
	}

	public static ServerInformationOld UpdateServerInformation(IGeminiRequestContext requestContext, string name, string description, ServerAddress[] serverAddresses)
	{
		return ConvertToServerInformationOld(ServerInformation.UpdateServerInformation(requestContext, name, description, serverAddresses, null));
	}

	private static ServerInformationOld ConvertToServerInformationOld(ServerInformation serverInformation)
	{
		return new ServerInformationOld
		{
			Addresses = serverInformation.Addresses,
			ComputerName = serverInformation.ComputerName,
			Description = serverInformation.Description,
			DomainName = serverInformation.DomainName,
			Id = "909749AE-88C4-4780-A381-383D691E98FA",
			InstanceId = serverInformation.Id,
			LastModified = serverInformation.LastModified,
			LastStatusHandshake = serverInformation.LastStatusHandshake,
			MasterSite = serverInformation.MasterSite,
			Name = serverInformation.Name,
			PhysicalMemory = serverInformation.PhysicalMemory,
			Platform = serverInformation.Platform,
			Processors = serverInformation.Processors,
			ServiceAccount = serverInformation.ServiceAccount,
			SynchronizationStatus = serverInformation.SynchronizationStatus,
			TimeZone = serverInformation.TimeZone,
			Version = serverInformation.Version
		};
	}
}
