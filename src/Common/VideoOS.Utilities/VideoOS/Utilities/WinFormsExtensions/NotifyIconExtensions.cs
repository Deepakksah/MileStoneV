using System;
using System.Reflection;
using System.Windows.Forms;

namespace VideoOS.Utilities.WinFormsExtensions;

public static class NotifyIconExtensions
{
	public static void SetText(this NotifyIcon notifyIcon, string text)
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		if (text.Length >= 128)
		{
			text = text.Substring(0, 124) + "...";
		}
		Type typeFromHandle = typeof(NotifyIcon);
		FieldInfo field = typeFromHandle.GetField("text", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field != null)
		{
			field.SetValue(notifyIcon, text);
		}
		FieldInfo field2 = typeFromHandle.GetField("added", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field2 != null && (bool)field2.GetValue(notifyIcon))
		{
			typeFromHandle.GetMethod("UpdateIcon", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(notifyIcon, new object[1] { true });
		}
	}
}
