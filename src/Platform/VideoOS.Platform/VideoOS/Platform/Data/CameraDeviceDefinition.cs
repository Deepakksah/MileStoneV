using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.Data.HardwareDeviceDefinitions;

namespace VideoOS.Platform.Data;

public class CameraDeviceDefinition
{
	private static readonly IDictionary<MediaContent, string> AcceptedCodecs = new Dictionary<MediaContent, string>
	{
		{
			MediaContent.Jpeg,
			"Jpeg"
		},
		{
			MediaContent.Mpeg,
			"Mpeg"
		},
		{
			MediaContent.H264,
			"H264"
		},
		{
			MediaContent.H265,
			"H265"
		}
	};

	private readonly ISet<string> _resolutionSet = new HashSet<string>();

	private readonly DeviceDefinition _deviceDefinition;

	public MediaContent Codec
	{
		get
		{
			return _deviceDefinition.Codec;
		}
		set
		{
			_deviceDefinition.Codec = value;
		}
	}

	public ISet<MediaContent> CodecSet => _deviceDefinition.CodecSet;

	public string Resolution { get; set; }

	public ISet<string> ResolutionSet => _resolutionSet;

	public double Fps { get; set; }

	public Tuple<double, double> FpsRange { get; set; }

	public int Gop { get; set; }

	public Tuple<int, int> GopRange { get; set; }

	public int Quality { get; set; }

	public Tuple<int, int> QualityRange { get; set; }

	public CameraDeviceDefinition()
	{
		_deviceDefinition = new DeviceDefinition(AcceptedCodecs);
	}

	public void GenerateDeviceParameters(IDictionary<string, string> parameters)
	{
		_deviceDefinition.GenerateDeviceParameters(string.Empty, parameters);
		if (Resolution != null)
		{
			parameters.Add("Resolution", Resolution);
		}
		if (ResolutionSet.Any())
		{
			parameters.Add("ResolutionList", ResolutionSet.Aggregate((string s1, string s2) => s1 + "," + s2));
		}
		if (Fps != 0.0)
		{
			parameters.Add("Fps", Fps.ToString(CultureInfo.InvariantCulture));
		}
		if (FpsRange != null)
		{
			parameters.Add("FpsRange", string.Format(CultureInfo.InvariantCulture, "{0}-{1}", FpsRange.Item1, FpsRange.Item2));
		}
		if (Gop != 0)
		{
			parameters.Add("Gop", Gop.ToString(CultureInfo.InvariantCulture));
		}
		if (GopRange != null)
		{
			parameters.Add("GopRange", string.Format(CultureInfo.InvariantCulture, "{0}-{1}", GopRange.Item1, GopRange.Item2));
		}
		if (Quality != 0)
		{
			parameters.Add("Quality", Quality.ToString(CultureInfo.InvariantCulture));
		}
		if (QualityRange != null)
		{
			parameters.Add("QualityRange", string.Format(CultureInfo.InvariantCulture, "{0}-{1}", QualityRange.Item1, QualityRange.Item2));
		}
	}
}
