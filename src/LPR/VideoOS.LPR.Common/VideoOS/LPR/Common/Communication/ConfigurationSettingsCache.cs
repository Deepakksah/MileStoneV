using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.LPR.Common.Communication.Info;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.Communication;

public sealed class ConfigurationSettingsCache
{
	private readonly Dictionary<Guid, ListModel> _licensePlateLists = new Dictionary<Guid, ListModel>();

	private readonly HashSet<string> _countryCodes = new HashSet<string>();

	private readonly Dictionary<Guid, VideoSourceInfo> _videoSources = new Dictionary<Guid, VideoSourceInfo>();

	private readonly List<CountryModuleWithLicensePlateStyles> _licensePlateStyles = new List<CountryModuleWithLicensePlateStyles>();

	private readonly object _lock = new object();

	private static ConfigurationSettingsCache _instance;

	public static ConfigurationSettingsCache Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ConfigurationSettingsCache();
			}
			return _instance;
		}
	}

	public ICollection<ListModel> LicensePlateLists
	{
		get
		{
			lock (_lock)
			{
				return _licensePlateLists.Values.ToArray();
			}
		}
	}

	public ICollection<string> CountryCodes
	{
		get
		{
			lock (_lock)
			{
				return _countryCodes.ToArray();
			}
		}
	}

	public ICollection<VideoSourceInfo> VideoSources
	{
		get
		{
			lock (_lock)
			{
				return _videoSources.Values.ToArray();
			}
		}
	}

	public IEnumerable<CountryModuleWithLicensePlateStyles> LicensePlateStyles
	{
		get
		{
			lock (_lock)
			{
				return _licensePlateStyles.ToArray();
			}
		}
	}

	public event EventHandler ConfigurationSettingsChangedEvent;

	public static void CloseInstance()
	{
		_instance = null;
	}

	public void UpdateLicensePlateLists(IEnumerable<LicensePlateList> lists, Guid siteId)
	{
		lock (_lock)
		{
			if (lists != null)
			{
				foreach (LicensePlateList list in lists)
				{
					if (!_licensePlateLists.ContainsKey(list.Id))
					{
						_licensePlateLists.Add(list.Id, new ListModel
						{
							ListId = list.Id,
							ListName = list.Name,
							SiteId = siteId,
							PropertyDefinition = list.PropertyDefinitions
						});
					}
				}
			}
		}
		this.ConfigurationSettingsChangedEvent?.Invoke(this, EventArgs.Empty);
	}

	public void UpdateLicensePlateStyles(IEnumerable<CountryModuleWithLicensePlateStyles> lists)
	{
		lock (_lock)
		{
			if (lists != null)
			{
				foreach (CountryModuleWithLicensePlateStyles list in lists)
				{
					_licensePlateStyles.Add(list);
				}
			}
		}
		this.ConfigurationSettingsChangedEvent?.Invoke(this, EventArgs.Empty);
	}

	public void UpdateCountryCodes(IEnumerable<string> countryCodes)
	{
		lock (_lock)
		{
			if (countryCodes != null)
			{
				_countryCodes.Clear();
				foreach (string countryCode in countryCodes)
				{
					_countryCodes.Add(countryCode);
				}
			}
		}
		this.ConfigurationSettingsChangedEvent?.Invoke(this, EventArgs.Empty);
	}

	public void UpdateVideoSources(IEnumerable<VideoSourceInfo> videoSources)
	{
		lock (_lock)
		{
			if (videoSources != null)
			{
				foreach (VideoSourceInfo videoSource in videoSources)
				{
					if (!_videoSources.ContainsKey(videoSource.Id))
					{
						_videoSources.Add(videoSource.Id, videoSource);
					}
				}
			}
		}
		this.ConfigurationSettingsChangedEvent?.Invoke(this, EventArgs.Empty);
	}
}
