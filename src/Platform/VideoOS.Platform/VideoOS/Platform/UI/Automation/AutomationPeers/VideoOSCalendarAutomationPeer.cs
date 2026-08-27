using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Automation.AutomationPeers;

public class VideoOSCalendarAutomationPeer : VideoOSAutomationPeerBase
{
	private readonly CalendarAutomationPeer _calendarAutomationPeer;

	public VideoOSCalendarAutomationPeer(Calendar owner, List<FrameworkElement> additionalChildren)
		: base(owner)
	{
		_calendarAutomationPeer = new CalendarAutomationPeer(owner);
		_calendarAutomationPeer.GetChildren().AddRange(additionalChildren.Select(UIElementAutomationPeer.CreatePeerForElement));
	}
}
