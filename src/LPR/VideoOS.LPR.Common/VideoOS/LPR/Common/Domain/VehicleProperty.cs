namespace VideoOS.LPR.Common.Domain;

public class VehicleProperty
{
	public string Value { get; set; }

	public string Confidence { get; set; }

	public VehicleProperty(string value, string confidence)
	{
		Value = value ?? string.Empty;
		Confidence = confidence ?? string.Empty;
	}
}
