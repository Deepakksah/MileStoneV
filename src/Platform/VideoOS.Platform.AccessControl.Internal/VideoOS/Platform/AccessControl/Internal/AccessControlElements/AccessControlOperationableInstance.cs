using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public abstract class AccessControlOperationableInstance : AccessControlInstance
{
	public IEnumerable<AccessControlCommandType> SupportedAccessControlCommandTypes => base.AccessControlSystemManager.AccessControlSystemConfiguration.GetSupportedAccessControlCommandTypes(this);

	public IEnumerable<FQID> RelatedSurveillanceItems
	{
		get
		{
			return base.AccessControlSystemManager.AccessControlSurveillanceItemMappingConfiguration.GetMappedSurveillanceItems(this);
		}
		set
		{
			base.AccessControlSystemManager.AccessControlSurveillanceItemMappingConfiguration.SetMappedSurveillanceItems(this, value);
		}
	}

	public Item MipItem => base.AccessControlSystemManager.AccessControlSystemConfiguration.GetIntegrationItem(this);

	public bool Enabled
	{
		get
		{
			return base.AccessControlSystemManager.AccessControlOperationableInstanceConfiguration.GetOperationableInstanceEnabled(this);
		}
		set
		{
			base.AccessControlSystemManager.AccessControlOperationableInstanceConfiguration.SetOperationableInstanceEnabled(this, value);
		}
	}

	public bool Licensed
	{
		get
		{
			if (base.AccessControlSystemManager.AccessControlLicenseManager != null)
			{
				return base.AccessControlSystemManager.AccessControlLicenseManager.ItemLicensed(base.Id);
			}
			return false;
		}
	}

	public DateTime LicenseExpiration
	{
		get
		{
			if (base.AccessControlSystemManager.AccessControlLicenseManager == null)
			{
				return DateTime.MinValue;
			}
			return base.AccessControlSystemManager.AccessControlLicenseManager.ItemLicenseExpiration(base.Id);
		}
	}

	protected AccessControlOperationableInstance(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
