using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.IM.Server.Service.WindowsRegistries;
using videoos.net._2.XProtectCSServiceRegistration;

namespace VideoOS.IM.Server.WebApi.Utils;

public static class LogServerAddressResolver
{
	private static readonly Guid LogServiceType = new Guid("3d6f1153-92ad-43f1-b467-9482ffd291b2");

	private static readonly IWinRegistryService WinRegistryService = new WinRegistryService();

	public static async Task<Uri> GetLogServerAddressAsync(string accessToken)
	{
		string text = WinRegistryService.ReadRegistryValue("ManagementServerAddress", string.Empty);
		if (string.IsNullOrEmpty(text))
		{
			throw new InvalidOperationException("Management Server address not found in the registry");
		}
		ServiceRegistrationServiceClient client = ServiceRegistrationServiceHelper.CreateClient(new Uri(text), accessToken);
		try
		{
			string obj = (await client.GetServicesAsync().ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault((ServiceInfo x) => x.type == LogServiceType)?.uri.FirstOrDefault();
			if (string.IsNullOrEmpty(obj))
			{
				throw new InvalidOperationException("Log Server address not found");
			}
			return new Uri(obj);
		}
		finally
		{
			if (client.State == CommunicationState.Opened || client.State == CommunicationState.Created)
			{
				client.Close();
			}
			else
			{
				client.Abort();
			}
		}
	}
}
