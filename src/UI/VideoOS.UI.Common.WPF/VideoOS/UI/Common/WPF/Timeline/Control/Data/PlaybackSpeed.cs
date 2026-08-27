namespace VideoOS.UI.Common.WPF.Timeline.Control.Data;

public class PlaybackSpeed
{
	public double Speed { get; set; }

	public string Text { get; set; }

	public PlaybackSpeed(double speed, string text)
	{
		Speed = speed;
		Text = text;
	}

	public override bool Equals(object obj)
	{
		return Speed.Equals(((PlaybackSpeed)obj).Speed);
	}

	public override int GetHashCode()
	{
		return Speed.GetHashCode();
	}
}
