using System.Reflection;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public static class DisconnectedItem
{
	private static object _instance;

	public static object Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = typeof(BindingExpressionBase).GetField("DisconnectedItem", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			}
			return _instance;
		}
	}
}
