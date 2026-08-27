using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class RecordingServerInformation
{
	public string ComputerName { get; set; }

	public string OSName { get; set; }

	public string OSBuild { get; set; }

	public string OSServicePack { get; set; }

	public string Language { get; set; }

	public string Processor { get; set; }

	public int Cores { get; set; }

	public long PhysicalMemory { get; set; }

	public string DiskUsageXml { get; set; }

	public static RecordingServerInformation GetRecordingServerInformation(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId);
		RecordingServerInformation recordingServerInformation = new RecordingServerInformation();
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			recordingServerComponent.QueryRecordingServerHostName(recordingServerId);
			recordingServerInformation.DiskUsageXml = recordingServerComponent.QueryRecordingServerDiskUsage(recordingServerId);
		}
		try
		{
			RecordingServerDiskUsages.GetFromXml(recordingServerInformation.DiskUsageXml);
			return recordingServerInformation;
		}
		catch (ArgumentException)
		{
			throw new RecordingServerInformationException();
		}
	}
}
