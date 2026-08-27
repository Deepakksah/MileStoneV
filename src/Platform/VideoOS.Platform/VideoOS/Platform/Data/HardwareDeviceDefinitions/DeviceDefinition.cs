using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Data.HardwareDeviceDefinitions;

public class DeviceDefinition
{
	private MediaContent _codec;

	private readonly ISet<MediaContent> _codecSet = new HashSet<MediaContent>();

	private readonly IDictionary<MediaContent, string> _acceptedCodecs;

	public MediaContent Codec
	{
		get
		{
			return _codec;
		}
		set
		{
			if (!_acceptedCodecs.ContainsKey(value))
			{
				throw new ArgumentOutOfRangeException(value.ToString());
			}
			_codec = value;
		}
	}

	public ISet<MediaContent> CodecSet => _codecSet;

	public DeviceDefinition(IDictionary<MediaContent, string> acceptedCodecs)
	{
		_acceptedCodecs = acceptedCodecs;
	}

	public virtual void GenerateDeviceParameters(string prefix, IDictionary<string, string> parameters)
	{
		if (CodecSet.Any((MediaContent codec) => !_acceptedCodecs.ContainsKey(codec)))
		{
			throw new InvalidOperationException("Codec list contains at least one non-supported codec");
		}
		if (Codec != 0)
		{
			parameters.Add(prefix + "Codec", _acceptedCodecs[Codec]);
		}
		if (CodecSet.Any())
		{
			parameters.Add(prefix + "CodecList", CodecSet.Select((MediaContent codec) => _acceptedCodecs[codec]).Aggregate((string s1, string s2) => s1 + "," + s2));
		}
	}
}
