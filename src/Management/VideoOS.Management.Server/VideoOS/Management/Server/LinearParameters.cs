namespace VideoOS.Management.Server;

public class LinearParameters
{
	public double A { get; private set; }

	public double B { get; private set; }

	public LinearParameters(double a, double b)
	{
		A = a;
		B = b;
	}
}
