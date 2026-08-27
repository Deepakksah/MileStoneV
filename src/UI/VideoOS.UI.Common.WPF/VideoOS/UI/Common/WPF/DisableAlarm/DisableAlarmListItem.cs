using System;
using System.Collections.ObjectModel;

namespace VideoOS.UI.Common.WPF.DisableAlarm;

public class DisableAlarmListItem
{
	private Collection<string> _preselectedEventTypes;

	private Collection<string> _availableEventTypes;

	public string Comment { get; set; }

	public Guid DisableAlarmItemId { get; set; }

	public Collection<string> PreselectedEventTypes => _preselectedEventTypes;

	public Collection<string> AvailableEventTypes => _availableEventTypes;

	public Guid SourceId { get; set; }

	public string SourceName { get; set; }

	public DateTime Time { get; set; }

	public object ServerId { get; set; }

	public void SetEventTypes(Collection<string> preselectedEventTypes, Collection<string> availableEventTypes)
	{
		_preselectedEventTypes = preselectedEventTypes;
		_availableEventTypes = availableEventTypes;
	}
}
