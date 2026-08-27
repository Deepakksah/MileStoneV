using System;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Automation.AutomationPeers;

public class VideoOSScrollViewerAutomationPeer : ScrollViewerAutomationPeer, IValueProvider
{
	public string Value => VideoOSAutomationPeerBase.GetValue(base.Owner);

	public bool IsReadOnly => true;

	public VideoOSScrollViewerAutomationPeer(ScrollViewer owner)
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

	public void SetValue(string value)
	{
		throw new NotImplementedException();
	}

	public override object GetPattern(PatternInterface patternInterface)
	{
		if (patternInterface == PatternInterface.Value)
		{
			return this;
		}
		return base.GetPattern(patternInterface);
	}
}
