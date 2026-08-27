using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class BaseValueInfo
{
	public List<string> Values;

	private string _sEntryName;

	public BaseValueInfo(string sEntryName)
	{
		Initialize(sEntryName);
	}

	public BaseValueInfo(string sEntryName, IEnumerable<ValueInfo> infos)
	{
		Initialize(sEntryName);
		FromValueInfo(infos);
	}

	private void Initialize(string sEntryName)
	{
		_sEntryName = sEntryName;
		Values = new List<string>();
	}

	public List<ValueInfo> ToValueInfo()
	{
		return Values.Select((string value) => new ValueInfo
		{
			Name = _sEntryName,
			Value = value
		}).ToList();
	}

	public void FromValueInfo(IEnumerable<ValueInfo> infos)
	{
		Values.Clear();
		foreach (ValueInfo info in infos)
		{
			if (info.Name == _sEntryName)
			{
				Values.Add(info.Value);
			}
		}
	}
}
