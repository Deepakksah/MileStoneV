namespace VideoOS.Common.Interfaces.Licensing;

public interface IReplacementLimit
{
	int Minimum { get; set; }

	int Maximum { get; set; }

	int Percentage { get; set; }

	int CalculateTotalReplacementLimit(int totalCameraCount);
}
