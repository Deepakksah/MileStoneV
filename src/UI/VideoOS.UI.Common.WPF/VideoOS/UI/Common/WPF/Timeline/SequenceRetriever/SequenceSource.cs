using System;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

public class SequenceSource : ISequenceSource
{
	public Guid SourceId { get; set; }

	public string DisplayName { get; set; }

	public string Name { get; set; }

	public VisualAppearance VisualAppearance { get; set; }

	public ISequenceRetriever SequenceRetriever { get; set; }

	public Guid InformationId { get; set; }

	public object SourceGroup { get; set; }

	public Brush CustomDataBrush { get; set; }

	public override bool Equals(object obj)
	{
		if (obj is SequenceSource)
		{
			SequenceSource sequenceSource = obj as SequenceSource;
			if (SourceId.Equals(sequenceSource.SourceId))
			{
				return InformationId.Equals(sequenceSource.InformationId);
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return SourceId.GetHashCode() ^ InformationId.GetHashCode();
	}

	public override string ToString()
	{
		return $"Name: {Name}, SourceId: {SourceId}, InformationId: {InformationId}";
	}
}
