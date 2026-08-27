using System.Collections.Generic;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Timelines.SlicedTimeline;

public class SequenceSourcesInfo
{
	private bool _sequenceSourcesAvailable;

	private List<ISequenceSource> _fillTopLineLowZOrderSequenceSources = new List<ISequenceSource>();

	private List<ISequenceSource> _fillTopLineHighZOrderSequenceSources = new List<ISequenceSource>();

	private List<ISequenceSource> _fillBottomLineLowZOrderSequenceSources = new List<ISequenceSource>();

	private List<ISequenceSource> _fillBottomLineHighZOrderSequenceSources = new List<ISequenceSource>();

	private List<ISequenceSource> _fillCustomDataLineSequenceSources = new List<ISequenceSource>();

	private List<ISequenceSource> _markerSequenceSources = new List<ISequenceSource>();

	private List<ISequenceSource> _fillExtraZOrder1SequenceSources = new List<ISequenceSource>();

	private List<ISequenceSource> _fillExtraZOrder2SequenceSources = new List<ISequenceSource>();

	public bool SequenceSourcesAvailable => _sequenceSourcesAvailable;

	public IEnumerable<ISequenceSource> FillTopLineLowZOrderSequenceSources => _fillTopLineLowZOrderSequenceSources;

	public IEnumerable<ISequenceSource> FillTopLineHighZOrderSequenceSources => _fillTopLineHighZOrderSequenceSources;

	public IEnumerable<ISequenceSource> FillBottomLineLowZOrderSequenceSources => _fillBottomLineLowZOrderSequenceSources;

	public IEnumerable<ISequenceSource> FillBottomLineHighZOrderSequenceSources => _fillBottomLineHighZOrderSequenceSources;

	public IEnumerable<ISequenceSource> FillCustomDataLineSequenceSources => _fillCustomDataLineSequenceSources;

	public IEnumerable<ISequenceSource> MarkerSequenceSources => _markerSequenceSources;

	public IEnumerable<ISequenceSource> FillExtraZOrder1SequenceSources => _fillExtraZOrder1SequenceSources;

	public IEnumerable<ISequenceSource> FillExtraZOrder2SequenceSources => _fillExtraZOrder2SequenceSources;

	public SequenceSourcesInfo()
	{
		_sequenceSourcesAvailable = false;
	}

	public SequenceSourcesInfo(IEnumerable<ISequenceSource> sequenceSources)
	{
		foreach (ISequenceSource sequenceSource in sequenceSources)
		{
			switch (sequenceSource.VisualAppearance)
			{
			case VisualAppearance.FillTopLineLowZOrder:
				_fillTopLineLowZOrderSequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			case VisualAppearance.FillTopLineHighZOrder:
				_fillTopLineHighZOrderSequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			case VisualAppearance.FillBottomLineLowZOrder:
				_fillBottomLineLowZOrderSequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			case VisualAppearance.FillBottomLineHighZOrder:
				_fillBottomLineHighZOrderSequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			case VisualAppearance.FillCustomDataLine:
				_fillCustomDataLineSequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			case VisualAppearance.Marker:
				_markerSequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			case VisualAppearance.CustomDataMarker:
				_markerSequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			case VisualAppearance.FillExtraZOrder1:
				_fillExtraZOrder1SequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			case VisualAppearance.FillExtraZOrder2:
				_fillExtraZOrder2SequenceSources.Add(sequenceSource);
				_sequenceSourcesAvailable = true;
				break;
			}
		}
	}
}
