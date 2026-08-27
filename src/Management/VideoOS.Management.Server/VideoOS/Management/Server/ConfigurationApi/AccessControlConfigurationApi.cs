using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.ConfigurationApi.AccessControl.Builders;
using VideoOS.Management.Server.ConfigurationApi.AccessControl.Methods;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.ConfigurationApi.Validators;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIAccessControlSystem")]
[EntityName("ConfigAPIAccessControlSystemFolder")]
[EntityName("ConfigAPIAccessControlUnit")]
[EntityName("ConfigAPIAccessControlUnitFolder")]
public class AccessControlConfigurationApi : SecurityConfigurationApi, IConfigurationApiSupport
{
	private readonly IConfigurationApiSupport _service;

	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => new Dictionary<string, List<string>>
	{
		{
			"AccessControlGroup",
			new List<string> { "ADMIN_SECURITY" }
		},
		{
			"AccessControlUnit",
			new List<string> { "ADMIN_SECURITY" }
		}
	};

	public AccessControlConfigurationApi()
	{
		MIPObjectProxy mipObjectProxy = new MIPObjectProxy();
		MethodProvider systemMethodProvider = new MethodProvider(new IMethod[1]
		{
			new GetInternalUnitPath(mipObjectProxy)
		});
		MethodProvider acUnitMethodProvider = new MethodProvider(new IMethod[2]
		{
			new AddGroupMember(mipObjectProxy),
			new RemoveGroupMember(mipObjectProxy)
		});
		_service = new VideoOS.Management.Server.ConfigurationApi.AccessControl.ConfigurationApiService(new ConfigurationItemBuilder(), systemMethodProvider, acUnitMethodProvider, new CommonValidator(), new MIPServerGroupProxy(), mipObjectProxy);
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return _service.GetChildItems(requestContext, path);
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return _service.GetItem(requestContext, path);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		GroupMemberUtility.HasManagementPermission(requestContext, path.Id);
		if (method == "ChangeSecurityPermissions")
		{
			return InvokeSecurityMethod(requestContext, item, path, method);
		}
		return _service.InvokeMethod(requestContext, item, path, parentPath, method);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		GroupMemberUtility.HasManagementPermission(requestContext, path.Id);
		return _service.SetItem(requestContext, item, path, parentPath);
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		GroupMemberUtility.HasManagementPermission(requestContext, path.Id);
		return _service.ValidateItem(requestContext, item, path, parentPath);
	}

	public override bool SetSecurityOnRoleOnly()
	{
		return true;
	}
}
