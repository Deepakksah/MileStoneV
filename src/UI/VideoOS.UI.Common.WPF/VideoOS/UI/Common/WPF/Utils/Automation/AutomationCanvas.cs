using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.Utils.Automation;

public class AutomationCanvas : Canvas
{
	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new FrameworkElementAutomationPeer(this);
	}
}
