using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class ArchiveStorageInformationException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61105;

	public ArchiveStorageInformationException()
		: base(VmoResources.VMO61105)
	{
	}
}
