using System;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.UI.Automation.AutomationPeers;

public class VideoOSSliderBaseAutomationPeer : VideoOSAutomationPeerBase, IRangeValueProvider
{
	private VideoOSSliderBase Slider => (VideoOSSliderBase)base.Owner;

	public double SmallChange => Slider.SmallChange;

	public double LargeChange => Slider.LargeChange;

	public new double Value => Slider.Value;

	public double Maximum => Slider.Maximum;

	public double Minimum => Slider.Minimum;

	public new bool IsReadOnly => !Slider.IsEnabled;

	public VideoOSSliderBaseAutomationPeer(VideoOSSliderBase owner)
		: base(owner)
	{
	}

	public override object GetPattern(PatternInterface patternInterface)
	{
		if (patternInterface == PatternInterface.RangeValue)
		{
			return this;
		}
		return base.GetPattern(patternInterface);
	}

	public void SetValue(double value)
	{
		if (value < Slider.Minimum || value > Slider.Maximum)
		{
			throw new ArgumentOutOfRangeException("value");
		}
		Slider.Value = value;
	}
}
