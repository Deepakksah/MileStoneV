namespace VideoOS.Platform.Util;

public static class MipTrace
{
	public static void MgsCom(string where, string text)
	{
		EnvironmentManager.Instance.Log(error: false, where, text);
	}
}
