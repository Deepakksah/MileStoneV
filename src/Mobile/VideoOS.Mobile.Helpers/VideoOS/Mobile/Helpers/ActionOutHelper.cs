namespace VideoOS.Mobile.Helpers;

public static class ActionOutHelper
{
	public delegate void OutAction<T1>(out T1 a);

	public delegate void OutAction<T1, T2>(out T1 a, out T2 b);
}
