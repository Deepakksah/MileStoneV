using System;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

public interface ISequenceSource
{
	string DisplayName { get; set; }

	string Name { get; set; }

	Guid SourceId { get; set; }

	Guid InformationId { get; set; }

	VisualAppearance VisualAppearance { get; set; }

	ISequenceRetriever SequenceRetriever { get; set; }

	object SourceGroup { get; set; }

	Brush CustomDataBrush { get; set; }
}
