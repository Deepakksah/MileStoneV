using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace VideoOS.Common.Help.UI;

public class HelpDisplayer : IHelpDisplayer
{
	public static readonly Lazy<IHelpDisplayer> Instance = new Lazy<IHelpDisplayer>(() => new HelpDisplayer());

	private HelpDisplayer()
	{
	}

	[DllImport("Shell32.dll")]
	private static extern int ShellExecuteA(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

	[DllImport("Shell32.dll")]
	private static extern int FindExecutable(string forFile, string directory, StringBuilder result);

	private static string GetBrowserLocation(string htmlPath)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		if (FindExecutable(htmlPath, null, stringBuilder) > 32)
		{
			return stringBuilder.ToString();
		}
		return "";
	}

	private static string FindDefaultBrowser(Uri resourceUri)
	{
		string text = null;
		string text2 = string.Empty;
		string scheme = resourceUri.Scheme;
		using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\" + scheme + "\\UserChoice"))
		{
			object value;
			text2 = ((registryKey == null || (value = registryKey.GetValue("Progid")) == null) ? scheme : (value as string));
		}
		using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + text2 + "\\shell\\open\\command"))
		{
			object value;
			if (registryKey2 != null && (value = registryKey2.GetValue(null)) != null)
			{
				text = value as string;
			}
		}
		if (text == null)
		{
			using RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\" + text2 + "\\shell\\open\\command");
			object value;
			if (registryKey3 != null && (value = registryKey3.GetValue(null)) != null)
			{
				text = value as string;
			}
		}
		if (text == null)
		{
			using RegistryKey registryKey4 = Registry.ClassesRoot.OpenSubKey(text2 + "\\shell\\open\\command");
			object value;
			if (registryKey4 != null && (value = registryKey4.GetValue(null)) != null)
			{
				text = value as string;
			}
		}
		if (text == null)
		{
			using RegistryKey registryKey5 = Registry.ClassesRoot.OpenSubKey(scheme + "\\shell\\open\\command");
			object value;
			if (registryKey5 != null && (value = registryKey5.GetValue(null)) != null)
			{
				text = value as string;
			}
		}
		if (text == null)
		{
			text = GetBrowserLocation(resourceUri.LocalPath);
		}
		if (!string.IsNullOrEmpty(text) && !text.EndsWith("exe", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring(0, text.LastIndexOf(".exe", StringComparison.OrdinalIgnoreCase) + 4).Trim('"');
		}
		return text;
	}

	private static bool IsEdgeBrowser(string browserLocation)
	{
		return browserLocation.Contains("LaunchWinApp");
	}

	private static void OpenUrlInEdgeBrowser(string url)
	{
		ShellExecuteA(IntPtr.Zero, "open", "shell:Appsfolder\\Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge", url, null, 10);
	}

	public void DisplayHtmlHelp(bool isOnline, Uri uri)
	{
		string text = FindDefaultBrowser(uri);
		if (IsEdgeBrowser(text))
		{
			OpenUrlInEdgeBrowser(uri.AbsoluteUri);
			return;
		}
		Process.Start(new ProcessStartInfo
		{
			FileName = text,
			Arguments = uri.AbsoluteUri,
			UseShellExecute = isOnline
		});
	}
}
