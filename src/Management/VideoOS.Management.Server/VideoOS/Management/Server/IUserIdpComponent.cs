using System;
using System.Collections.Generic;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.Management.Server;

public interface IUserIdpComponent : IDisposable
{
	BasicUser CreateVmsUser(CreateUserModel model);

	BasicUser UpdateBasicUser(string id, UpdateUserModel model);

	BasicUser GetVmsUserById(string id);

	BasicUser[] GetAllVmsUsers();

	BasicUser GetVmsUserByName(string name);

	BasicUser[] GetVmsUserCollection(ICollection<string> ids);

	BasicUser[] GetVmsUsersModifiedAfter(DateTime afterDateTime);

	GetLoginModel GetLogin(string provider, string key);

	GetLoginModel CreateLogin(CreateLoginModel model);
}
