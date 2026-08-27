namespace VideoOS.Platform.SDK.Interface;

public static class ServerOptionUtility
{
	public static bool IsValid(this IServerOption @this)
	{
		if (@this != null && @this.Key != null)
		{
			return @this.Value != null;
		}
		return false;
	}
}
