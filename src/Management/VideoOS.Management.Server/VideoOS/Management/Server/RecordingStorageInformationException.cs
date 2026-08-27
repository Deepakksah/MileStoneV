using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageInformationException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61103;

	public RecordingStorageInformationException()
		: base(VmoResources.VMO61103)
	{
	}
}
