namespace VideoOS.Utilities.Math;

public class Normalization
{
	public static double NormalizeValue(double value, double normalizedValueMin, double normalizedValueMax, double valueMin, double valueMax)
	{
		return normalizedValueMin + (normalizedValueMax - normalizedValueMin) * (value - valueMin) / (valueMax - valueMin);
	}

	public static double DeNormalizeValue(double value, double normalizedValueMin, double normalizedValueMax, double valueMin, double valueMax)
	{
		return NormalizeValue(value, valueMin, valueMax, normalizedValueMin, normalizedValueMax);
	}
}
