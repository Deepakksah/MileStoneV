using System;
using System.Linq;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.VmoClient;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class VmoWrapper : IVmoWrapper
{
	private VmoClient _client;

	public VmoWrapper(VmoClient client)
	{
		_client = client;
	}

	public void CreateRecorder(Guid id, string name, string hostName, string webServiceUri, RecordingServerCreateOptions options)
	{
		new RecordingServer(_client.ManagementServer, id, name, hostName, webServiceUri, options).Create();
	}

	public bool RecorderExists(Guid id)
	{
		return _client.Repositories.GetObjectById<RecordingServer>(id, refreshCachedObjects: true) != null;
	}

	public Role GetAdministratorRole()
	{
		return _client.ManagementServer.SecuritySection.Roles.FirstOrDefault((Role f) => f.Id == SecurityManager.AdministratorRoleId);
	}

	public void AddUserToAdminRole(Role theRole, string accountNameToUse)
	{
		if (theRole == null)
		{
			throw new ArgumentNullException("theRole");
		}
		theRole.AddMember(accountNameToUse);
	}

	public void Disconnect()
	{
		if (_client != null)
		{
			_client.ServerConnection.Disconnect();
		}
	}
}
