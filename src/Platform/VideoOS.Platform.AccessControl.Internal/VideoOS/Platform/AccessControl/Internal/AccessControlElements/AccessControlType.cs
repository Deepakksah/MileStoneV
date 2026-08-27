using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;
using VideoOS.Platform.AccessControl.Internal.Util;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public abstract class AccessControlType : AccessControlElement
{
	public IEnumerable<Category> DefaultCategories => base.AccessControlSystemManager.AccessControlCategoryMappingConfiguration.GetDefaultCategories(this);

	public IEnumerable<Category> CustomCategories
	{
		get
		{
			return base.AccessControlSystemManager.AccessControlCategoryMappingConfiguration.GetCustomCategories(this);
		}
		set
		{
			base.AccessControlSystemManager.AccessControlCategoryMappingConfiguration.SetCustomCategories(this, value);
		}
	}

	public IEnumerable<Category> Categories => base.AccessControlSystemManager.AccessControlCategoryMappingConfiguration.GetResolvedCategories(this);

	public IEnumerable<Category> PossibleCategories => CategoryHandler.GetCategories(base.AccessControlSystemManager.PluginId, GetType());

	public AccessControlType(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
