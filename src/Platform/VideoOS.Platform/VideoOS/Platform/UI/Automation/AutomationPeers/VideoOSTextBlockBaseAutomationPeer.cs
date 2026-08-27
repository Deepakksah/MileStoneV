using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Automation.AutomationPeers;

public class VideoOSTextBlockBaseAutomationPeer : TextBlockAutomationPeer
{
	public VideoOSTextBlockBaseAutomationPeer(TextBlock owner)
		: base(owner)
	{
	}

	protected override string GetNameCore()
	{
		string nameCore = base.GetNameCore();
		if (!string.IsNullOrWhiteSpace(nameCore))
		{
			return nameCore;
		}
		return GetClassNameCore();
	}

	protected override string GetClassNameCore()
	{
		return base.Owner.GetType().Name;
	}

	protected override bool IsControlElementCore()
	{
		return base.Owner.IsVisible;
	}
}
