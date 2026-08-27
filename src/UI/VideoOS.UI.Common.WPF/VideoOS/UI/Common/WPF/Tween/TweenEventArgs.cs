namespace VideoOS.UI.Common.WPF.Tween;

public class TweenEventArgs
{
	public double[] NewValues { get; set; }

	public double[] OldValues { get; set; }

	public TweenEventArgs(double[] newValues, double[] oldValues)
	{
		NewValues = newValues;
		OldValues = oldValues;
	}
}
