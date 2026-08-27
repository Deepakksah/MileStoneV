namespace VideoOS.Common.Integration.Drivers;

public class PtzArgsRange
{
	public string Name { get; private set; }

	public double Min { get; private set; }

	public double Max { get; private set; }

	public double MinSpeed { get; private set; }

	public double MaxSpeed { get; private set; }

	public bool HasSpeedSupport { get; private set; }

	public PtzArgsRange(string name, double min, double max)
	{
		Name = name;
		Min = min;
		Max = max;
		HasSpeedSupport = false;
	}

	public PtzArgsRange(string name, double min, double max, double minSpeed, double maxSpeed)
	{
		Name = name;
		Min = min;
		Max = max;
		MinSpeed = minSpeed;
		MaxSpeed = maxSpeed;
		HasSpeedSupport = true;
	}
}
