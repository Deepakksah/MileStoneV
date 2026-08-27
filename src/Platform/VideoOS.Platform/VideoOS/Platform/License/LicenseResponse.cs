using System.Collections.ObjectModel;

namespace VideoOS.Platform.License;

public class LicenseResponse : LicenseInformation
{
	public string Slc { get; set; }

	public int Activated { get; set; }

	public int TotalActivated { get; set; }

	public override LicenseInformation Clone()
	{
		Collection<LicenseItem> collection = new Collection<LicenseItem>();
		if (base.ItemIdentifications != null)
		{
			foreach (LicenseItem itemIdentification in base.ItemIdentifications)
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
		return new LicenseResponse
		{
			PluginId = base.PluginId,
			Counter = base.Counter,
			LicenseType = base.LicenseType,
			Name = base.Name,
			Expire = base.Expire,
			TrialMode = base.TrialMode,
			CustomData = base.CustomData,
			CounterMethod = base.CounterMethod,
			ItemIdentifications = collection,
			Activated = Activated,
			Slc = Slc,
			TotalActivated = TotalActivated,
			PuAc = base.PuAc
		};
	}
}
