namespace VideoOS.DataCollector.Service;

public class NativeMethodsAccessor : INativeMethods
{
	public string? EnumRelatedProducts(string upgradeCode)
	{
		return NativeMethods.EnumRelatedProducts(upgradeCode);
	}
}
