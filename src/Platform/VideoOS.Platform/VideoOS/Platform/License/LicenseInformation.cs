using System;
using System.Collections.ObjectModel;

namespace VideoOS.Platform.License;

public class LicenseInformation
{
	public enum CounterMethodEnum
	{
		Auto,
		CountByItemIdentifications,
		CountByCounter
	}

	public Guid PluginId { get; set; }

	public string Name { get; set; }

	public string LicenseType { get; set; }

	public int Counter { get; set; }

	public CounterMethodEnum CounterMethod { get; set; }

	public bool TrialMode { get; set; }

	public DateTime Expire { get; set; }

	public string CustomData { get; set; }

	public bool IsSiteLicense { get; set; }

	public Collection<LicenseItem> ItemIdentifications { get; set; }

	public DateTime PuAc { get; set; } = DateTime.MaxValue;

	public LicenseInformation()
	{
		ItemIdentifications = new Collection<LicenseItem>();
		CounterMethod = CounterMethodEnum.Auto;
	}

	public LicenseItem GetConfiguredLicenseItem(string itemId)
	{
		foreach (LicenseItem itemIdentification in ItemIdentifications)
		{
			if (itemIdentification.Id == itemId)
			{
				return itemIdentification;
			}
		}
		return null;
	}

	public void RegisterConfiguredLicenseItem(LicenseItem licenseItem)
	{
		if (EnvironmentManager.Instance.Mode != Mode.ServerSetup)
		{
			throw new ImplementationMIPException("Only supported in ServerSetup mode");
		}
		bool flag = false;
		foreach (LicenseItem itemIdentification in ItemIdentifications)
		{
			if (itemIdentification.Id == licenseItem.Id)
			{
				itemIdentification.ItemTrial = licenseItem.ItemTrial;
				itemIdentification.ItemTrialEnd = licenseItem.ItemTrialEnd;
				itemIdentification.Name = licenseItem.Name;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			ItemIdentifications.Add(licenseItem);
		}
		EnvironmentManager.Instance.LicenseManager.SaveLicenseInformation(this);
	}

	public void UnRegisterConfiguredLicenseItem(LicenseItem licenseItem)
	{
		foreach (LicenseItem itemIdentification in ItemIdentifications)
		{
			if (itemIdentification.Id == licenseItem.Id)
			{
				ItemIdentifications.Remove(itemIdentification);
				break;
			}
		}
		EnvironmentManager.Instance.LicenseManager.SaveLicenseInformation(this);
	}

	public void ActivateTrialsConfiguredLicenseItems(int maxCount)
	{
		try
		{
			int num = 0;
			foreach (LicenseItem itemIdentification in ItemIdentifications)
			{
				if (!itemIdentification.ItemTrial)
				{
					num++;
				}
			}
			if (maxCount > num)
			{
				bool flag = true;
				while (flag && maxCount > num)
				{
					flag = false;
					DateTime dateTime = DateTime.MaxValue;
					LicenseItem licenseItem = null;
					foreach (LicenseItem itemIdentification2 in ItemIdentifications)
					{
						if (itemIdentification2.ItemTrial && itemIdentification2.ItemTrialEnd < dateTime)
						{
							dateTime = itemIdentification2.ItemTrialEnd;
							licenseItem = itemIdentification2;
						}
					}
					if (licenseItem != null)
					{
						licenseItem.ItemTrial = false;
						licenseItem.ItemTrialEnd = DateTime.MaxValue;
						RegisterConfiguredLicenseItem(licenseItem);
						flag = true;
						num++;
					}
				}
			}
			else
			{
				if (maxCount >= num)
				{
					return;
				}
				while (maxCount < num)
				{
					LicenseItem licenseItem2 = null;
					foreach (LicenseItem itemIdentification3 in ItemIdentifications)
					{
						if (!itemIdentification3.ItemTrial)
						{
							licenseItem2 = itemIdentification3;
						}
					}
					if (licenseItem2 != null)
					{
						licenseItem2.ItemTrial = true;
						licenseItem2.ItemTrialEnd = DateTime.UtcNow + TimeSpan.FromDays(30.0);
						RegisterConfiguredLicenseItem(licenseItem2);
					}
					num--;
				}
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.ExceptionDialog("ActivateTrialsConfiguredLicense", ex);
			throw new InternalMIPException("ActivateTrialsConfiguredLicense", ex);
		}
	}

	public virtual LicenseInformation Clone()
	{
		Collection<LicenseItem> collection = new Collection<LicenseItem>();
		if (ItemIdentifications != null)
		{
			foreach (LicenseItem itemIdentification in ItemIdentifications)
			{
				collection.Add(new LicenseItem
				{
					Id = itemIdentification.Id,
					ItemTrial = itemIdentification.ItemTrial,
					ItemTrialEnd = itemIdentification.ItemTrialEnd,
					Name = itemIdentification.Name
				});
			}
		}
		return new LicenseInformation
		{
			PluginId = PluginId,
			Counter = Counter,
			LicenseType = LicenseType,
			Name = Name,
			Expire = Expire,
			TrialMode = TrialMode,
			CustomData = CustomData,
			CounterMethod = CounterMethod,
			ItemIdentifications = collection,
			IsSiteLicense = IsSiteLicense,
			PuAc = PuAc
		};
	}
}
