using System;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem.Configuration;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

public class AccessControlIntegrationItem : Item
{
	private AccessControlIntegrationItem _parentItem;

	private AccessControlSystemConfiguration _accessControlSystemConfiguration;

	public AccessControlIntegrationItem(ServerId serverId, AccessControlIntegrationItem parentItem, Guid objectId, string name, FolderType folderType, Guid kind, AccessControlSystemConfiguration accessControlSystemConfiguration)
		: base(serverId, parentItem?.FQID.ObjectId ?? Guid.Empty, objectId, name, folderType, kind)
	{
		_parentItem = parentItem;
		_accessControlSystemConfiguration = accessControlSystemConfiguration;
	}

	public override Item GetParent()
	{
		return _parentItem;
	}

	public virtual bool ContainsAccessControlCategory(Category category)
	{
		if (_accessControlSystemConfiguration.GetAccessControlElement(FQID.ObjectId) is AccessControlOperationableInstance accessControlOperationableInstance)
		{
			return accessControlOperationableInstance.AccessControlType.Categories.Contains(category);
		}
		return false;
	}
}
