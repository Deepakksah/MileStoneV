using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ServerService : GeminiWebServiceBase, IServerService
{
	public ServerService()
		: base(connect: false)
	{
	}

	public string CheckAuthentication()
	{
		CheckConnect();
		string empty = string.Empty;
		try
		{
			MethodInformation methodInformation = new MethodInformation("CheckAuthentication");
			EnterWebMethod(methodInformation);
			return base.RequestContext.UserDomainAccountName;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Version GetServerVersion()
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServerVersion");
			EnterWebMethod(methodInformation);
			return GeminiApplicationCore.ServerVersion();
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ServerInformationOld GetServerInformation()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServerInformation");
			EnterWebMethod(methodInformation);
			return ServerInformationOld.GetServerInformation(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ServerInformationOld UpdateServerInformation(string name, string description, ServerAddress[] serverAddresses)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateServerInformation");
			methodInformation.AddParameter("name", name);
			methodInformation.AddParameter("description", description);
			methodInformation.AddArrayParameter("serverAddresses", serverAddresses);
			EnterWebMethod(methodInformation);
			Validation.CheckName(name, "name");
			base.RequestContext.GetService<SecurityManagerService>().CheckApplicationWritePermission(base.RequestContext);
			ServerInformationOld serverInformationOld = new ServerInformationOld
			{
				Name = name,
				Description = description,
				Addresses = serverAddresses
			};
			return ServerInformationOld.UpdateServerInformation(base.RequestContext, name, description, serverAddresses);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}
}
