using System.Collections.Generic;

namespace VideoOS.Mobile.MP4Muxing.VideoParser;

public class NaluHeaders
{
	private readonly Dictionary<byte, NaluInfo> _naluInfos;

	public int TotalLength { get; private set; }

	public Dictionary<byte, NaluInfo> NaluInfos => _naluInfos;

	public NaluHeaders()
	{
		_naluInfos = new Dictionary<byte, NaluInfo>();
		TotalLength = 0;
	}

	public bool AreAdjacent(byte naluTypeFirst, byte naluTypeSecond)
	{
		if (!_naluInfos.TryGetValue(naluTypeFirst, out var value))
		{
			return false;
		}
		if (!_naluInfos.TryGetValue(naluTypeSecond, out var value2))
		{
			return false;
		}
		return AreAdjacent(value, value2);
	}

	public static bool AreAdjacent(NaluInfo first, NaluInfo second)
	{
		if (first == null || second == null)
		{
			return false;
		}
		return first.End - second.Start <= 1;
	}

	public void Add(NaluInfo info)
	{
		int num = 0;
		if (_naluInfos.ContainsKey(info.NaluType))
		{
			num = _naluInfos[info.NaluType].Length;
		}
		_naluInfos[info.NaluType] = info;
		TotalLength += info.Length - num;
	}

	public NaluInfo Get(byte naluCode)
	{
		if (!_naluInfos.TryGetValue(naluCode, out var value))
		{
			return null;
		}
		return value;
	}
}
