namespace VideoOS.Common.Integration;

public class LensProfile
{
	public string RegisteredPanomorphLensNumber { get; private set; }

	public string Name { get; private set; }

	public double FieldOfView { get; private set; }

	public LensProfile(string registeredPanomorphLensNumber, string name, double fieldOfView)
	{
		RegisteredPanomorphLensNumber = registeredPanomorphLensNumber;
		Name = name;
		FieldOfView = fieldOfView;
	}
}
