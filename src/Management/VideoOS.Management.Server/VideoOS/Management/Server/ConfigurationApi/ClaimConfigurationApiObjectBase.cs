using System;
using System.Linq;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class ClaimConfigurationApiObjectBase : ConfigurationApiBase, IConfigurationApiSupport
{
	private class DefaultRepository : IClaimRepository
	{
		public static IClaimRepository Instance { get; } = new DefaultRepository();

		private DefaultRepository()
		{
		}

		public Claim[] QueryRoleClaims(IGeminiRequestContext requestContext, string roleId)
		{
			return Role.QueryClaimsForRole(requestContext, roleId);
		}

		public Claim AddClaimToRole(IGeminiRequestContext requestContext, string roleId, string claimProvider, string claimName, string claimValue)
		{
			Role.AddClaimToRole(requestContext, roleId, claimProvider, claimName, claimValue);
			return new Claim(new Guid(claimProvider), claimName, claimValue);
		}

		public void RemoveClaimFromRole(IGeminiRequestContext requestContext, string roleId, string claimProvider, string claimName, string claimValue)
		{
			Role.RemoveClaimFromRole(requestContext, roleId, claimProvider, claimName, claimValue);
		}
	}

	protected IClaimRepository Repository { get; }

	protected ClaimConfigurationApiObjectBase(IClaimRepository repository)
	{
		Repository = repository ?? DefaultRepository.Instance;
	}

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

	protected static ConfigurationItem ConvertToConfigurationItem(Claim claim, ConfigurationItemPath parentPath)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "Claim",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.ChildItem,
			DisplayName = claim.Value,
			ParentPath = parentPath.Text
		};
		configurationItem.Properties = new Property[3]
		{
			ClaimPropertyDefinition.LoginProviderIdProperty(claim.LoginProviderId.ToString(), isSettable: false),
			ClaimPropertyDefinition.ClaimNameProperty(claim.Name, isSettable: false),
			ClaimPropertyDefinition.ClaimValueProperty(claim.Value, isSettable: false)
		};
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, "0").Text;
		configurationItem.Children = Array.Empty<ConfigurationItem>();
		configurationItem.ChildrenFilled = true;
		configurationItem.MethodIds = Array.Empty<string>();
		return configurationItem;
	}

	protected abstract void ThrowIfPathInvalid(ConfigurationItemPath path);

	protected abstract void ThrowIfParentPathInvalid(ConfigurationItemPath path);
}
