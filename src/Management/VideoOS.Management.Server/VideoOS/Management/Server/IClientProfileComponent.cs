using System;

namespace VideoOS.Management.Server;

public interface IClientProfileComponent : IDisposable
{
	void CreateClientProfile(string id, string name, string description, string settings, ClientProfileType profileType);

	string[] QueryClientProfileIds(ClientProfileType profileType);

	ClientProfile QueryClientProfileById(string id);

	ClientProfile[] QueryClientProfilesByIds(string[] ids);

	void UpdateClientProfile(string id, string name, string description, string settings);

	void DeleteClientProfile(string id);

	void AttachClientProfileToRole(string id, string roleId, Guid sessionId);

	void MovePriorityDown(string id);

	void MovePriorityUp(string id);

	string QueryActiveProfileId(string userSid, ClientProfileType profileType);

	string QueryProfileIdByRoleId(string roleId, ClientProfileType profileType);
}
