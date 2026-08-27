using System;
using System.Collections.Generic;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Config;

public class VideoSourceItemConfig : ItemConfigBase, IVideoSourceConfig
{
	private bool? _enabled;

	private FQID _cameraFQID;

	private string _description;

	private Guid[] _licensePlateLists;

	private string[] _countryModules;

	public FQID FQID { get; set; }

	public string Name { get; set; }

	public IRecognitionSettings RecognitionSettings { get; private set; }

	public IStoppingCriterion StoppingCriterion { get; private set; }

	public bool Enabled
	{
		get
		{
			if (!_enabled.HasValue)
			{
				_enabled = base.GetProperty("Enabled", defaultValue: false);
			}
			return _enabled.Value;
		}
		set
		{
			_enabled = value;
			base.SetProperty("Enabled", value);
		}
	}

	public FQID CameraFQID
	{
		get
		{
			if (_cameraFQID == null)
			{
				FQID defaultValue = new FQID(new ServerId("", "", 0, Guid.Empty), Guid.Empty, Guid.Empty, FolderType.No, Guid.Empty);
				_cameraFQID = base.GetProperty("CameraFQID", defaultValue);
			}
			return _cameraFQID;
		}
		set
		{
			_cameraFQID = value;
			base.SetProperty("CameraFQID", value);
		}
	}

	public string Description
	{
		get
		{
			if (_description == null)
			{
				_description = base.GetProperty("Description", string.Empty);
			}
			return _description;
		}
		set
		{
			_description = value;
			base.SetProperty("Description", value);
		}
	}

	public Guid[] LicensePlateLists
	{
		get
		{
			if (_licensePlateLists == null)
			{
				_licensePlateLists = base.GetProperty("LicensePlateLists", Array.Empty<Guid>());
			}
			return _licensePlateLists;
		}
		set
		{
			_licensePlateLists = value;
			base.SetProperty("LicensePlateLists", value);
		}
	}

	public string[] CountryModules
	{
		get
		{
			if (_countryModules == null)
			{
				_countryModules = base.GetProperty("CountryModules", Array.Empty<string>());
			}
			return _countryModules;
		}
		set
		{
			_countryModules = value;
			base.SetProperty("CountryModules", value);
		}
	}

	public VideoSourceItemConfig(FQID fqid, string name, Dictionary<string, string> properties)
		: base(properties, "")
	{
		FQID = fqid;
		Name = name;
		RecognitionSettings = new RecognitionSettingsItemConfig(base.Properties);
		StoppingCriterion = new StoppingCriterionItemConfig(base.Properties);
	}
}
