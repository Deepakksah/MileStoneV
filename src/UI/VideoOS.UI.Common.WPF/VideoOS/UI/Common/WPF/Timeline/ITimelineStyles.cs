namespace VideoOS.UI.Common.WPF.Timeline;

public interface ITimelineStyles
{
	object FindResource(object resourceKey);

	object TryFindResource(object resourceKey);
}
