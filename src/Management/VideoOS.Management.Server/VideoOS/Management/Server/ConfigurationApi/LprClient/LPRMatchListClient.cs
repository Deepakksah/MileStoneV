using System;
using System.Linq;
using System.Net;
using System.ServiceModel;
using VideoOS.LPR.Common.WCF.Client;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.LprClient;

public class LPRMatchListClient : ILPRMatchListClient
{
	private readonly IPluginCommandClientProvider _clientProvider;

	private readonly Func<EnvironmentManager> _environmentManager;

	public LPRMatchListClient(IPluginCommandClientProvider clientProvider, Func<EnvironmentManager> environmentManager)
	{
		clientProvider.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\LprClient\\LPRMatchListClient.cs", 20);
		environmentManager.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\LprClient\\LPRMatchListClient.cs", 21);
		_clientProvider = clientProvider;
		_environmentManager = environmentManager;
	}

	public void CheckInit(IGeminiRequestContext requestContext)
	{
		MIPFactory.CheckAndInitialize(requestContext);
	}

	public AddLicensePlateListResp AddLicensePlateList(Guid listId, string name)
	{
		return Get((PluginCommandClient client, string token) => client.AddLicensePlateList(token, new AddLicensePlateListParams
		{
			Id = listId,
			Name = name
		}));
	}

	public DeleteLicensePlateListResp DeleteLicensePlateList(Guid listId)
	{
		return Get((PluginCommandClient client, string token) => client.DeleteLicensePlateList(token, new DeleteLicensePlateListParams
		{
			Id = listId
		}));
	}

	public LicensePlateList[] GetLicensePlateLists()
	{
		return Get((PluginCommandClient client, string token) => client.GetLicensePlateLists(token, new GetLicensePlateListsParams()).LicensePlateLists);
	}

	public LicensePlateList GetLicensePlateList(Guid listId)
	{
		return GetLicensePlateLists().FirstOrDefault((LicensePlateList s) => s.Id == listId);
	}

	public LicensePlateInfo[] GetLicensePlates(Guid listId, string searchFilter)
	{
		searchFilter.NullCheck("", "GetLicensePlates", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\LprClient\\LPRMatchListClient.cs", 90);
		return Get((PluginCommandClient client, string token) => client.GetLicensePlates(token, new GetLicensePlatesParams
		{
			ListId = listId,
			StartIndex = 0,
			Count = int.MaxValue,
			SearchFilter = searchFilter
		}).LicensePlateInfos);
	}

	public ModifyLicensePlateListResp ModifyLicensePlateList(Guid listId, string name, FQID[] triggerEvents, LicensePlatePropertyDefinition[] customFields)
	{
		name.NullCheck("", "ModifyLicensePlateList", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\LprClient\\LPRMatchListClient.cs", 110);
		triggerEvents.NullCheck("", "ModifyLicensePlateList", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\LprClient\\LPRMatchListClient.cs", 111);
		customFields.NullCheck("", "ModifyLicensePlateList", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\LprClient\\LPRMatchListClient.cs", 112);
		return Get((PluginCommandClient client, string token) => client.ModifyLicensePlateList(token, new ModifyLicensePlateListParams
		{
			Id = listId,
			Name = name,
			TriggerEvents = triggerEvents,
			PropertyDefinitions = customFields
		}));
	}

	public AddLicensePlatesResp AddLicensePlates(Guid listId, LicensePlateInfo[] licensePlates)
	{
		licensePlates.NullCheck("", "AddLicensePlates", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\LprClient\\LPRMatchListClient.cs", 130);
		return Get((PluginCommandClient client, string token) => client.AddLicensePlates(token, new AddLicensePlatesParams
		{
			ListId = listId,
			LicensePlateInfos = licensePlates
		}));
	}

	public DeleteLicensePlatesResp DeleteLicensePlates(Guid listId, LicensePlate[] licensePlates)
	{
		licensePlates.NullCheck("", "DeleteLicensePlates", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\LprClient\\LPRMatchListClient.cs", 147);
		return Get((PluginCommandClient client, string token) => client.DeleteLicensePlates(token, new DeleteLicensePlatesParams
		{
			ListId = listId,
			LicensePlates = licensePlates
		}));
	}

	public ClearLicensePlatesResp DeleteAllLicensePlates(Guid listId)
	{
		return Get((PluginCommandClient client, string token) => client.ClearLicensePlates(token, new ClearLicensePlatesParams
		{
			ListId = listId
		}));
	}

	private TResult Get<TResult>(Func<PluginCommandClient, string, TResult> caller) where TResult : class
	{
		try
		{
			return _clientProvider.Client.CallWithRetry(caller, "Get");
		}
		catch (ServiceFaultException ex)
		{
			throw HandleError(ex);
		}
	}

	private Exception HandleError(Exception ex)
	{
		_environmentManager().Log(error: true, "LPRMatchListClient", ex.Message);
		_clientProvider.Close();
		return Rethrow(ex);
	}

	private static Exception Rethrow(Exception ex)
	{
		if (IsLprPluginNotFoundException(ex))
		{
			throw new NotSupportedException("You have to install the LPR plugin to support this feature");
		}
		throw ex;
	}

	private static bool IsLprPluginNotFoundException(Exception ex)
	{
		if (!(ex.InnerException is ServerTooBusyException ex2))
		{
			return false;
		}
		if (!(ex2.InnerException is WebException ex3))
		{
			return false;
		}
		if (ex3.Status == WebExceptionStatus.ProtocolError)
		{
			return ex3.Message.Contains("(503)");
		}
		return false;
	}
}
