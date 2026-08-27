using System.Windows;
using System.Windows.Automation.Peers;

namespace VideoOS.UI.Common.WPF.Utils.Automation;

public class AutomationShapePeer : FrameworkElementAutomationPeer
{
	public AutomationShapePeer(AutomationShape owner)
		: base(owner)
	{
	}

	protected override string GetClassNameCore()
	{
		return "AutomationShape";
	}

	protected override AutomationControlType GetAutomationControlTypeCore()
	{
		return AutomationControlType.Image;
	}

	protected override bool IsContentElementCore()
	{
		return true;
	}

	protected override bool IsControlElementCore()
	{
		return true;
	}

	protected override Rect GetBoundingRectangleCore()
	{
		return ((AutomationShape)base.Owner).GetScreenBoundingBox();
	}
}
