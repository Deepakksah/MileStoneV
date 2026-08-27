using System;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.Providers;
using VideoOS.Platform.AccessControl.Internal.Providers.Strategies;
using VideoOS.Platform.AccessControl.Internal.Util;

namespace VideoOS.Platform.AccessControl.Internal;

public class BuiltInCategory : Category
{
	public string BuiltInId { get; private set; }

	public static BuiltInCategory[] BuiltInCategories(string builtInId, Type accessControlType)
	{
		return GuidProvider.Instance.GuidProviderStrategies.Select((IGuidProviderStrategy gps) => new BuiltInCategory(builtInId, accessControlType, gps)).ToArray();
	}

	public BuiltInCategory(string builtInId, Type accessControlType)
		: base(CreateCategoryId(builtInId, new Sha1ProviderStrategy()), DisplayTextHelper.GetBuiltInDisplayName(builtInId), accessControlType)
	{
		BuiltInId = builtInId;
	}

	public BuiltInCategory(string builtInId, Type accessControlType, IGuidProviderStrategy guidProviderStrategy)
		: base(CreateCategoryId(builtInId, guidProviderStrategy), DisplayTextHelper.GetBuiltInDisplayName(builtInId), accessControlType)
	{
		BuiltInId = builtInId;
	}

	public static Guid CreateCategoryId(string builtInId, IGuidProviderStrategy guidProviderStrategy)
	{
		return guidProviderStrategy.Create(Guid.Empty, builtInId);
	}
}
