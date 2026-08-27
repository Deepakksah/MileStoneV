using System.Collections.Generic;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class GenericEntriesBox<T> : GenericEntryCountBox where T : class
{
	private readonly IGenericBoxEntryProcessor<T> _processor;

	private readonly List<T> _entries = new List<T>();

	public override uint DataSize => base.DataSize + EntryCount * _processor.DataSize;

	public IReadOnlyList<T> Entries => _entries;

	public GenericEntriesBox(string boxType, IGenericBoxEntryProcessor<T> processor)
		: base(boxType)
	{
		_processor = processor;
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint num = 0u;
		num += base.ParseData(buffer, startIndex, restInBoxSize);
		for (int i = 0; i < EntryCount; i++)
		{
			T item = _processor.ParseData(buffer, startIndex, restInBoxSize, ref num);
			_entries.Add(item);
		}
		return num;
	}

	protected override void WriteData(Stream stream)
	{
		base.WriteData(stream);
		for (int i = 0; i < EntryCount; i++)
		{
			_processor.WriteToStream(stream, _entries[i]);
		}
	}

	public void AddEntry(T entry)
	{
		_entries.Add(entry);
		EntryCount = (uint)_entries.Count;
	}
}
