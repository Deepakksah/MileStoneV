using System;
using VideoOS.Management.VmoClient;

namespace VideoOS.Installer.RecorderTaskPlugin;

public interface IVmoWrapper
{
	bool RecorderExists(Guid id);

	void CreateRecorder(Guid id, string name, string hostName, string webServiceUri, RecordingServerCreateOptions options);

	Role GetAdministratorRole();

	void AddUserToAdminRole(Role theRole, string accountNameToUse);

	void Disconnect();
}
