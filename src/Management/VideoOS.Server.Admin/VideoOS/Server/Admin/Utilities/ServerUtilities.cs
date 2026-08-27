using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;
using VideoOS.Management.VmoClient.Proxy;
using VideoOS.Server.Admin.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Admin.Utilities;

public class ServerUtilities
{
	private IisHelper _iisHelper;

	public ILogger LogComponent { get; public set; }

	public ServerUtilities(ILogger logComponent)
	{
		LogComponent = logComponent;
		VerifyUser();
		_iisHelper = new IisHelper(logComponent);
	}

	private void VerifyUser()
	{
		bool flag = false;
		Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
		if (Thread.CurrentPrincipal is WindowsPrincipal windowsPrincipal && windowsPrincipal.Identity.IsAuthenticated && windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
		{
			flag = true;
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.User, windowsPrincipal.Identity.Name), Array.Empty<object>());
		}
		else
		{
			WindowsPrincipal windowsPrincipal2 = new WindowsPrincipal(WindowsIdentity.GetCurrent());
			if (windowsPrincipal2 != null && windowsPrincipal2.Identity.IsAuthenticated && windowsPrincipal2.IsInRole(WindowsBuiltInRole.Administrator))
			{
				flag = true;
			}
		}
		if (!flag)
		{
			throw new ServerAdminException(Resources.NotSufficientPrivileges);
		}
	}

	public void SetAdminRoleAccount(AccountInfo serviceAccount, IDatabaseConnectionInfo databaseConnectionInfo)
	{
		if (serviceAccount == null)
		{
			throw new ArgumentNullException(ServerUtils_Resources.ServiceAccountIsNull);
		}
		if (!serviceAccount.IsValid())
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidAccount, serviceAccount.AccountName));
		}
		if (databaseConnectionInfo == null)
		{
			throw new ArgumentNullException("databaseConnectionInfo");
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Adding_Service_Account_to_Administrators_Role, serviceAccount.AccountName), Array.Empty<object>());
		using (DatabaseUtils databaseUtils = new DatabaseUtils(new SqlConnectionManager(databaseConnectionInfo, LogComponent), LogComponent))
		{
			databaseUtils.SetAdminRoleAccount(serviceAccount);
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Service_Account_Has_Been_Added_to_Administrators_Role, serviceAccount.AccountName), Array.Empty<object>());
	}

	public void SetWebServiceAccount(AccountInfo serviceAccount, string serverAddress, int serverPort, string websiteId)
	{
		if (serviceAccount == null)
		{
			throw new ArgumentNullException(ServerUtils_Resources.ServiceAccountIsNull);
		}
		if (!serviceAccount.IsValid())
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidAccount, serviceAccount.AccountName));
		}
		_iisHelper.ValidateWebService(serviceAccount, serverAddress, serverPort, websiteId);
		LoggerExtensions.LogDebug(LogComponent, ServerUtils_Resources.Validated_the_web_server, Array.Empty<object>());
	}

	public void SetDatabaseAccount(AccountInfo serviceAccount, IDatabaseConnectionInfo databaseConnectionInfo)
	{
		if (serviceAccount == null)
		{
			throw new ArgumentNullException(ServerUtils_Resources.ServiceAccountIsNull);
		}
		if (!serviceAccount.IsValid())
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidAccount, serviceAccount.AccountName));
		}
		if (databaseConnectionInfo == null)
		{
			throw new ArgumentNullException("databaseConnectionInfo");
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Setting_database_account_on_server, databaseConnectionInfo.ServerName, databaseConnectionInfo.DatabaseName, serviceAccount.AccountName), Array.Empty<object>());
		using (DatabaseUtils databaseUtils = new DatabaseUtils(new SqlConnectionManager(databaseConnectionInfo, LogComponent), LogComponent))
		{
			databaseUtils.SetDatabaseAccount(serviceAccount);
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Database_account_has_been_set, serviceAccount.AccountName), Array.Empty<object>());
	}

	public void TakeDatabaseOwnership(IDatabaseConnectionInfo databaseConnectionInfo)
	{
		if (databaseConnectionInfo == null)
		{
			throw new ArgumentNullException("databaseConnectionInfo");
		}
		using DatabaseUtils databaseUtils = new DatabaseUtils(new SqlConnectionManager(databaseConnectionInfo, LogComponent), LogComponent);
		if (databaseUtils.DatabaseExists())
		{
			databaseUtils.TakeDatabaseOwnership();
		}
	}

	public void AddUser(string serverAddress, int serverPort, string roleName, string userName)
	{
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Adding_user_to_the_management_server, userName, roleName), Array.Empty<object>());
		using ServerConnection serverConnection = new ServerConnection(serverAddress);
		serverConnection.ServerPort = serverPort;
		using VmoClient vmoClient = new VmoClient(serverConnection);
		try
		{
			vmoClient.ServerConnection.Connect();
		}
		catch (Exception ex)
		{
			throw new ServerAdminException(string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Error_connecting_to_server, ex.Message));
		}
		VideoOS.Management.VmoClient.Role role = vmoClient.ManagementServer.SecuritySection.Roles.FirstOrDefault((VideoOS.Management.VmoClient.Role f) => f.Name == roleName);
		if (role == null)
		{
			throw new ServerAdminException(ServerUtils_Resources.The_role_does_not_exist);
		}
		try
		{
			role.AddMember(userName);
		}
		catch (Exception ex2)
		{
			throw new ServerAdminException(string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Error_adding_user, ex2.Message));
		}
	}

	public void AddRole(string serverAddress, int serverPort, string role)
	{
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Adding_role_to_the_management_server, role), Array.Empty<object>());
		using ServerConnection serverConnection = new ServerConnection(serverAddress);
		serverConnection.ServerPort = serverPort;
		using VmoClient vmoClient = new VmoClient(serverConnection);
		try
		{
			vmoClient.ServerConnection.Connect();
		}
		catch (Exception ex)
		{
			throw new ServerAdminException(string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Error_connecting_to_server, ex.Message));
		}
		try
		{
			VideoOS.Management.VmoClient.Role role2 = vmoClient.ManagementServer.SecuritySection.Roles.FirstOrDefault((VideoOS.Management.VmoClient.Role f) => f.Name == role);
			if (role2 == null)
			{
				role2 = new VideoOS.Management.VmoClient.Role(vmoClient.ManagementServer, role);
				role2.Create();
				serverConnection.GetClientProxy<SecurityProxy>().AddPermissions(entries: new List<AccessControlEntry>
				{
					new AccessControlEntry
					{
						Sid = role2.Id.ToString(),
						ActionId = "CONNECT",
						AccessDeny = false
					}
				}.ToArray(), namespaceId: SecurityNamespaceIdentifier.ManagementServer.ToString(), securityToken: "/");
			}
		}
		catch (Exception ex2)
		{
			throw new ServerAdminException(string.Format(CultureInfo.InvariantCulture, ServerUtils_Resources.Error_adding_role, ex2.Message));
		}
	}
}
