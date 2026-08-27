using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class RecordingServerInformationGeneric
{
	public static GenericObject2 GetRecordingServerInformation(IGeminiRequestContext requestContext, string recordingServerId)
	{
		return ConvertToGenericObject2(RecordingServerInformation.GetRecordingServerInformation(requestContext, recordingServerId), recordingServerId);
	}

	private static GenericObject2 ConvertToGenericObject2(RecordingServerInformation recordingServerInformation, string recordingServerId)
	{
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("ComputerName", recordingServerInformation.ComputerName));
		list.Add(new ObjectProperty2("OSName", recordingServerInformation.OSName));
		list.Add(new ObjectProperty2("OSBuild", recordingServerInformation.OSBuild));
		list.Add(new ObjectProperty2("OSServicePack", recordingServerInformation.OSServicePack));
		list.Add(new ObjectProperty2("Language", recordingServerInformation.Language));
		list.Add(new ObjectProperty2("Processor", recordingServerInformation.Processor));
		list.Add(new ObjectProperty2("Cores", recordingServerInformation.Cores));
		list.Add(new ObjectProperty2("PhysicalMemory", recordingServerInformation.PhysicalMemory));
		list.Add(new ObjectProperty2("DiskUsageXml", recordingServerInformation.DiskUsageXml));
		return new GenericObject2(recordingServerId, "RecordingServerInformation", list.ToArray(), new RelationProperty[1]
		{
			new RelationProperty("RecordingServer", recordingServerId)
		});
	}
}
