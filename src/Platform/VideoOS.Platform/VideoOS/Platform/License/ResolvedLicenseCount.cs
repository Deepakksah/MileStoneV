using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.License;

public class ResolvedLicenseCount
{
	public int? Available { get; set; }

	public int Used { get; set; }

	public int Activated { get; set; }

	public int TotalActivated { get; set; }

	public int Temporary { get; set; }

	public int Expired { get; set; }

	public int Missing { get; set; }

	public ResolvedLicenseCount(Guid pluginId, LicenseInformation licenseRequest)
	{
		if (licenseRequest == null)
		{
			throw new ArgumentNullException("licenseRequest");
		}
		ResolvedLicenseCountLoad((EnvironmentManager.Instance != null) ? (EnvironmentManager.Instance.LicenseManager.GetLicense(pluginId, licenseRequest.LicenseType) as LicenseResponse) : null, licenseRequest);
	}

	public ResolvedLicenseCount(LicenseResponse activatedLicense, LicenseInformation licenseRequest)
	{
		ResolvedLicenseCountLoad(activatedLicense, licenseRequest);
	}

	private void ResolvedLicenseCountLoad(LicenseResponse activatedLicense, LicenseInformation licenseRequest)
	{
		Activated = activatedLicense?.Activated ?? 0;
		TotalActivated = activatedLicense?.TotalActivated ?? 0;
		Collection<LicenseItem> collection = licenseRequest.ItemIdentifications ?? new Collection<LicenseItem>();
		bool flag = licenseRequest.CounterMethod == LicenseInformation.CounterMethodEnum.CountByCounter || (licenseRequest.CounterMethod == LicenseInformation.CounterMethodEnum.Auto && collection.Count == 0);
		bool flag2 = activatedLicense != null && activatedLicense.ItemIdentifications != null && activatedLicense.ItemIdentifications.Count == 0;
		Used = (flag ? licenseRequest.Counter : collection.Count);
		if (activatedLicense != null)
		{
			Available = activatedLicense.Counter;
		}
		else
		{
			Available = ((!flag) ? new int?(licenseRequest.Counter) : ((int?)null));
		}
		DateTime now = DateTime.UtcNow;
		if (flag)
		{
			Temporary = 0;
			Expired = 0;
			int num = Math.Max(TotalActivated - Activated, 0);
			Missing = (Available.HasValue ? Math.Max(0, Used - (Available.Value - num)) : 0);
			if (Available.HasValue)
			{
				int val = Math.Max(0, Used - Activated);
				int val2 = Math.Max(0, Available.Value - TotalActivated);
				Temporary = Math.Min(val2, val);
			}
			else if (licenseRequest.Expire < now.Date)
			{
				Expired = Used;
			}
			else
			{
				Temporary = Used;
			}
			return;
		}
		HashSet<string> expiredActivatedItemIds = new HashSet<string>();
		HashSet<string> temporaryActivatedItemIds = new HashSet<string>();
		HashSet<string> activatedItemIds;
		if (activatedLicense != null)
		{
			if (!flag2)
			{
				activatedItemIds = new HashSet<string>(from i in activatedLicense.ItemIdentifications
					where !i.ItemTrial
					select i.Id);
				expiredActivatedItemIds = new HashSet<string>(from i in activatedLicense.ItemIdentifications
					where i.ItemTrial && i.ItemTrialEnd < now
					select i.Id);
				temporaryActivatedItemIds = new HashSet<string>(from i in activatedLicense.ItemIdentifications
					where i.ItemTrial && i.ItemTrialEnd >= now
					select i.Id);
			}
			else
			{
				activatedItemIds = new HashSet<string>(from i in collection.Where((LicenseItem i) => !i.ItemTrial).Take(Activated)
					select i.Id);
			}
		}
		else
		{
			activatedItemIds = new HashSet<string>();
		}
		IEnumerable<LicenseItem> source = collection.Where((LicenseItem i) => !activatedItemIds.Contains(i.Id)).ToList();
		Missing = source.Count((LicenseItem i) => !i.ItemTrial);
		Temporary = source.Count((LicenseItem i) => i.ItemTrial && i.ItemTrialEnd >= now);
		Expired = source.Count((LicenseItem i) => i.ItemTrial && i.ItemTrialEnd < now);
		if (Temporary == 0 && Expired == 0 && (expiredActivatedItemIds.Any() || temporaryActivatedItemIds.Any()))
		{
			Expired += expiredActivatedItemIds.Count;
			Temporary += temporaryActivatedItemIds.Count;
			Missing -= source.Count((LicenseItem i) => !i.ItemTrial && (expiredActivatedItemIds.Contains(i.Id) || temporaryActivatedItemIds.Contains(i.Id)));
			Activated -= Expired + Temporary;
		}
		if (Temporary == 0 && Expired == 0 && Available.HasValue && Available.Value > TotalActivated)
		{
			int num2 = Math.Min(Available.Value - TotalActivated, Missing);
			Temporary += num2;
			Missing -= num2;
		}
	}
}
