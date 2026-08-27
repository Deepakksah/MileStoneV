using System;
using System.Linq;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class ConfigurationApiEntityBase : ConfigurationApiBase, IConfigurationApiSupport
{
	public abstract ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		ThrowIfPathInvalid(path);
		ThrowIfParentPathInvalid(parentPath);
		if (!item.MethodIds.Contains(methodId))
		{
			throw new ArgumentException(FormattableString.Invariant($"Invalid MethodId {methodId} on ItemType: {item.ItemType}"), "methodId");
		}
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return GetMethodInputParameters(requestContext, item, path, methodId);
		}
		return ExecuteMethod(requestContext, item, path, methodId);
	}

	protected abstract ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId);

	protected abstract ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId);

	protected abstract void ThrowIfPathInvalid(ConfigurationItemPath path);

	protected abstract void ThrowIfParentPathInvalid(ConfigurationItemPath path);
}
