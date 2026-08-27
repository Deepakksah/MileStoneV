namespace VideoOS.Common.Integration;

public class ImmerVisionLensDescription
{
	private string _rpl;

	private string _name;

	public string LensProfile => _rpl;

	public string Name => _name;

	public ImmerVisionLensDescription(string name, string regPanLens)
	{
		_name = name;
		_rpl = regPanLens;
	}
}
