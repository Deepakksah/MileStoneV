using System;
using System.Globalization;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.ConfigurationAPI;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class LicenseConfigurationApiObjectBase : ConfigurationApiBase, IConfigurationApiSupport
{
	private class DefaultLicenseDataRepository : ILicenseDataRepository
	{
		public static ILicenseDataRepository Instance { get; } = new DefaultLicenseDataRepository();

		private DefaultLicenseDataRepository()
		{
		}

		public void HasPermission(IGeminiRequestContext requestContext, string actionId)
		{
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, actionId);
		}

		public string GetLicenseInformationXml()
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			string text = serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.GetLicenseInformation(), null, (Exception ex, Action ac) => false);
			if (text == null)
			{
				return null;
			}
			return text;
		}

		public ILicenseDocument QueryLicenseDocument(IGeminiRequestContext requestContext)
		{
			using ILicenseComponent licenseComponent = requestContext.CreateLicenseComponent();
			return licenseComponent.QueryLicenseDocument();
		}

		public string QueryMIPLicenseItem(IGeminiRequestContext requestContext)
		{
			using ILicenseComponent licenseComponent = requestContext.CreateLicenseComponent();
			return licenseComponent.QueryLicenseItemById(License.MipPluginsLicenseId)?.DataAsString;
		}

		public string GetServerId(IGeminiRequestContext requestContext)
		{
			return ServerInformation.GetServerInformation(requestContext).Id;
		}

		public string QuerySystemSetting(IGeminiRequestContext requestContext, string id)
		{
			using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
			string systemSetting = managementServerComponent.GetSystemSetting(id);
			return SystemSettingsManager.ConvertToInternalServerValue(requestContext, id, systemSetting);
		}

		public bool SetSystemSetting(IGeminiRequestContext requestContext, string id, string value)
		{
			string value2 = SystemSettingsManager.ConvertToDatabaseValue(requestContext, id, value);
			using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
			return managementServerComponent.SetSystemSetting(id, value2);
		}

		public Task CreateLicenseActivateTask(IGeminiRequestContext requestContext, string username, string password)
		{
			return Task.ActivateLicense(requestContext, username, password);
		}

		public void UpdateLicense(IGeminiRequestContext requestContext, byte[] license, string ownerXml, string siteInformation)
		{
			using (ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture)))
			{
				serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
				{
					client.UpdateLicense(license, ownerXml, siteInformation);
				}, null, (Exception ex, Action ac) => true);
			}
			GeminiApplicationCore.FeatureCacheManager.Update(requestContext);
		}

		public byte[] RequestLicense()
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			return serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.GetLicenseRequestFromConfiguration(), null, (Exception ex, Action ac) => false);
		}

		public Version GetVersion(IGeminiRequestContext requestContext)
		{
			return ServerInformation.GetServerInformation(requestContext).Version;
		}
	}

	protected ILicenseDataRepository Repository { get; }

	protected LicenseConfigurationApiObjectBase(ILicenseDataRepository repository)
	{
		Repository = repository ?? DefaultLicenseDataRepository.Instance;
	}

	public abstract ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId);
}
