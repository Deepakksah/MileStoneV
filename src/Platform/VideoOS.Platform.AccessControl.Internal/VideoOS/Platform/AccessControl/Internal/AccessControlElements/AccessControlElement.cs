using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public abstract class AccessControlElement
{
	public Guid Id { get; private set; }

	public AccessControlSystemManager AccessControlSystemManager { get; private set; }

	public string Name => AccessControlSystemManager.AccessControlSystemConfiguration.GetName(this);

	public string ExternalId => AccessControlSystemManager.AccessControlSystemConfiguration.GetExternalId(this);

	public Guid IconKey => AccessControlSystemManager.AccessControlSystemConfiguration.GetIconKey(this);

	public Image Icon => AccessControlSystemManager.AccessControlSystemConfiguration.GetIconBitmap(this);

	public IEnumerable<Property> Properties => AccessControlSystemManager.AccessControlSystemConfiguration.GetProperties(this);

	public abstract Guid TypeId { get; }

	public AccessControlElement(Guid id, AccessControlSystemManager accessControlSystemManager)
	{
		Id = id;
		AccessControlSystemManager = accessControlSystemManager;
	}
}
