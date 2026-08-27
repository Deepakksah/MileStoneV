namespace VideoOS.Management.VmoClient;

public class Location
{
	private Monitor _monitor;

	public double X
	{
		get
		{
			return _monitor.GetPropertyValue<double>("LocationX");
		}
		set
		{
			_monitor.SetPropertyValue("LocationX", value);
		}
	}

	public double Y
	{
		get
		{
			return _monitor.GetPropertyValue<double>("LocationY");
		}
		set
		{
			_monitor.SetPropertyValue("LocationY", value);
		}
	}

	public Location(Monitor monitor)
	{
		_monitor = monitor;
	}
}
