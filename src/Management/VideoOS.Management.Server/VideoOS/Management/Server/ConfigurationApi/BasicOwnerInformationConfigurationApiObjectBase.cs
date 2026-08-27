using System;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class BasicOwnerInformationConfigurationApiObjectBase : ConfigurationApiBase, IConfigurationApiSupport
{
	private class DefaultBasicOwnerInformationDataRepository : IBasicOwnerInformationDataRepository
	{
		public static IBasicOwnerInformationDataRepository Instance { get; } = new DefaultBasicOwnerInformationDataRepository();

		private DefaultBasicOwnerInformationDataRepository()
		{
		}

		public string GetServerId(IGeminiRequestContext requestContext)
		{
			return ServerInformation.GetServerInformation(requestContext).Id;
		}

		public BasicOwnerInfo[] QueryBasicUserInfo(IGeminiRequestContext requestContext)
		{
			using IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent();
			return basicOwnerInfoComponent.QueryBasicOwnerInfo();
		}

		public void AddBasicUserInfo(IGeminiRequestContext requestContext, string id, string name, string property, string order)
		{
			using IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent();
			basicOwnerInfoComponent.CreateBasicOwnerInfo(id, name, property, order);
		}

		public void RemoveBasicUserInfo(IGeminiRequestContext requestContext, string id)
		{
			using IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent();
			Guid sessionId = Guid.NewGuid();
			basicOwnerInfoComponent.DeleteBasicOwnerInfo(id, sessionId);
		}

		public void UpdateBasicOwnerInfo(IGeminiRequestContext requestContext, string id, string name, string value, string order, Guid sessionId)
		{
			using IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent();
			basicOwnerInfoComponent.UpdateBasicOwnerInfo(id, name, value, order, sessionId);
		}
	}

	protected IBasicOwnerInformationDataRepository Repository { get; }

	protected BasicOwnerInformationConfigurationApiObjectBase(IBasicOwnerInformationDataRepository repository)
	{
		Repository = repository ?? DefaultBasicOwnerInformationDataRepository.Instance;
	}

	public abstract ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId);
}
