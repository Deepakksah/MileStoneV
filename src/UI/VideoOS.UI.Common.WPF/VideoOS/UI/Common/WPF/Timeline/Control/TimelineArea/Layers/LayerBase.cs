using System.Windows.Controls;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers;

public class LayerBase : Grid
{
	private ITimelineSequenceCache _timelineCache;

	private TimeAndSpanController _timeAndSpanController = new TimeAndSpanController();

	private AppearanceAndBehaviourController _appearanceAndBehaviourController = new AppearanceAndBehaviourController();

	public TimeAndSpanController TimeAndSpanController => _timeAndSpanController;

	public AppearanceAndBehaviourController AppearanceAndBehaviourController => _appearanceAndBehaviourController;

	public ITimelineSequenceCache TimelineCache => _timelineCache;

	public virtual void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		_timeAndSpanController = timeAndSpanController;
		_appearanceAndBehaviourController = appearanceAndBehaviourController;
		_timelineCache = timelineCache;
	}

	public virtual void Close()
	{
	}
}
