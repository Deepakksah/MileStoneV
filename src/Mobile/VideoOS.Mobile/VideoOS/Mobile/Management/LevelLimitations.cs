using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Mobile.Utilities;
using VideoOS.Platform;

namespace VideoOS.Mobile.Management;

public class LevelLimitations : IComparable<LevelLimitations>
{
	private bool _bActivated;

	private string _name;

	private int _cpuThreshold;

	private int? _bandwidthThreshold;

	private int _limitFPS;

	private int _levelNumber;

	private VideoType _imageSizeLimit;

	public Size ImageSize { get; private set; }

	public int CPUThreshold
	{
		get
		{
			return _cpuThreshold;
		}
		set
		{
			_cpuThreshold = value;
		}
	}

	public int? Bandwidth
	{
		get
		{
			return _bandwidthThreshold;
		}
		set
		{
			_bandwidthThreshold = value;
		}
	}

	public int FPS
	{
		get
		{
			return _limitFPS;
		}
		set
		{
			_limitFPS = value;
		}
	}

	public string Name => _name;

	public bool Enabled
	{
		get
		{
			return _bActivated;
		}
		set
		{
			_bActivated = value;
		}
	}

	public VideoType VideoTypeLimit
	{
		get
		{
			return _imageSizeLimit;
		}
		set
		{
			_imageSizeLimit = value;
			if (VideoDefinitions.VideoSize.ContainsKey(value))
			{
				ImageSize = VideoDefinitions.VideoSize[value];
			}
		}
	}

	public int LevelNumber => _levelNumber;

	public LevelLimitations()
	{
	}

	public LevelLimitations(int levelNumber)
		: this()
	{
		_levelNumber = levelNumber;
	}

	public LevelLimitations(int cpuThreshold, int? bandwidthThreshold, int limitFPS, VideoType type, int levelNumber = 0, string name = "", bool bActivated = true)
	{
		_cpuThreshold = cpuThreshold;
		_bandwidthThreshold = bandwidthThreshold;
		_limitFPS = limitFPS;
		VideoTypeLimit = type;
		_name = name;
		_levelNumber = levelNumber;
		_bActivated = bActivated;
	}

	public void InitFromConfig(Dictionary<string, string> item, int nIndex)
	{
		DictionaryHelper.InitFromSettings(ref _cpuThreshold, item, nIndex, "LimitationsLevelCPU", int.Parse);
		DictionaryHelper.InitFromSettings(ref _name, item, nIndex, "LimitationsLevelName", (string x) => x);
		DictionaryHelper.InitFromSettings(ref _bandwidthThreshold, item, nIndex, "LimitationsLevelBandwidth", new NullableConverter<int>(int.TryParse).TryParse);
		DictionaryHelper.InitFromSettings(ref _limitFPS, item, nIndex, "LimitationsLevelFPS", int.Parse);
		DictionaryHelper.InitFromSettings(ref _imageSizeLimit, item, nIndex, "LimitationsLevelSize", (string x) => (VideoType)Enum.Parse(typeof(VideoType), x));
		DictionaryHelper.InitFromSettings(ref _bActivated, item, nIndex, "LimitationsLevelActive", bool.Parse);
		VideoTypeLimit = _imageSizeLimit;
	}

	public void WriteToConfig(Item item, int nIndex)
	{
		item.Properties.SetProperty("LimitationsLevelCPU", nIndex, _cpuThreshold);
		item.Properties.SetProperty("LimitationsLevelBandwidth", nIndex, _bandwidthThreshold);
		item.Properties.SetProperty("LimitationsLevelFPS", nIndex, _limitFPS);
		item.Properties.SetProperty("LimitationsLevelActive", nIndex, _bActivated);
		item.Properties.SetProperty("LimitationsLevelSize", nIndex, _imageSizeLimit);
		item.Properties.SetProperty("LimitationsLevelName", nIndex, _name);
	}

	public int CompareTo(LevelLimitations other)
	{
		int num = _cpuThreshold.CompareTo(other.CPUThreshold);
		if (num == 0)
		{
			num = ((_bandwidthThreshold.HasValue || other.Bandwidth.HasValue) ? ((!_bandwidthThreshold.HasValue || !other.Bandwidth.HasValue) ? (-1) : _bandwidthThreshold.Value.CompareTo(other.Bandwidth.Value)) : 0);
		}
		if (num == 0)
		{
			num = _limitFPS.CompareTo(other.FPS);
		}
		if (num == 0)
		{
			num = _imageSizeLimit.CompareTo(other.VideoTypeLimit);
		}
		if (num == 0)
		{
			num = _bActivated.CompareTo(other.Enabled);
		}
		return num;
	}
}
