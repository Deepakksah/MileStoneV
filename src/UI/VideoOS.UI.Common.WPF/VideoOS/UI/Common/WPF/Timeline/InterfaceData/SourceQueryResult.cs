using System.Collections.Generic;
using System.Text;
using C5;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public class SourceQueryResult : TimeInterval
{
	private ISorted<Sequence> _sequences = (ISorted<Sequence>)(object)new SortedArray<Sequence>((MemoryType)0);

	public ISequenceSource Source { get; set; }

	public ISortedEnumerable<Sequence> Sequences
	{
		get
		{
			return _sequences.ToSortedEnumerable<Sequence>();
		}
		set
		{
			((ICollection<Sequence>)(object)_sequences).Clear();
			_sequences.AddSorted((IEnumerable<Sequence>)value);
		}
	}

	public SourceQueryResult()
	{
	}

	public SourceQueryResult(TimeInterval interval)
		: base(interval)
	{
	}

	public SourceQueryResult(SourceQuery query)
		: base(query)
	{
		Source = query.Source;
	}

	public SourceQueryResult(SourceQueryResult other)
		: base(other)
	{
		Source = other.Source;
		_sequences.AddSorted((IEnumerable<Sequence>)other.Sequences);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(base.ToString());
		if (Source != null)
		{
			stringBuilder.AppendFormat("Source: {0} ", Source.Name);
		}
		if (Sequences != null)
		{
			stringBuilder.Append(" [");
			foreach (Sequence sequence in Sequences)
			{
				stringBuilder.Append(sequence);
				stringBuilder.Append(", ");
			}
			stringBuilder.Append("]");
		}
		if (stringBuilder.Length == 0)
		{
			stringBuilder.Append("[Empty]");
		}
		return stringBuilder.ToString();
	}
}
