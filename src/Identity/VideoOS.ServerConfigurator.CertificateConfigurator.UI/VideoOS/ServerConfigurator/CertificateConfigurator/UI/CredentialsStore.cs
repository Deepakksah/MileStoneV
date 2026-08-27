using System;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using VideoOS.ServerConfigurator.CertificateConfigurator.UI.Properties;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class CredentialsStore : ICredentialsStore
{
	private static class NativeMethods
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct CREDUI_INFO
		{
			public int cbSize;

			public IntPtr hwndParent;

			public string pszMessageText;

			public string pszCaptionText;

			public IntPtr hbmBanner;
		}

		[DllImport("ole32.dll")]
		public static extern void CoTaskMemFree(IntPtr ptr);

		[DllImport("credui.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, uint cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainame, StringBuilder pszPassword, ref int pcchMaxPassword);

		[DllImport("credui.dll", CharSet = CharSet.Auto)]
		public static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO pUiInfo, int authError, ref uint authPackage, IntPtr InAuthBuffer, uint InAuthBufferSize, out IntPtr refOutAuthBuffer, out uint refOutAuthBufferSize, [MarshalAs(UnmanagedType.Bool)] ref bool fSave, int flags);
	}

	private readonly IApplicationResources _applicationResources;

	public NetworkCredential Credentials { get; private set; }

	public CredentialsStore(IApplicationResources applicationResources)
	{
		_applicationResources = applicationResources ?? throw new ArgumentNullException("applicationResources");
	}

	public NetworkCredential PromptForCredentials()
	{
		NativeMethods.CREDUI_INFO pUiInfo = default(NativeMethods.CREDUI_INFO);
		pUiInfo.pszCaptionText = Resources.CredentialDialogCaption;
		pUiInfo.pszMessageText = string.Format(CultureInfo.CurrentCulture, Resources.CredentialDialogMessage, _applicationResources.ProductNameWithoutCompany);
		pUiInfo.hwndParent = new WindowInteropHelper(Application.Current.MainWindow).Handle;
		pUiInfo.cbSize = Marshal.SizeOf(pUiInfo);
		uint authPackage = 0u;
		IntPtr refOutAuthBuffer = default(IntPtr);
		bool fSave = false;
		uint refOutAuthBufferSize;
		int num = NativeMethods.CredUIPromptForWindowsCredentials(ref pUiInfo, 0, ref authPackage, IntPtr.Zero, 0u, out refOutAuthBuffer, out refOutAuthBufferSize, ref fSave, 1);
		StringBuilder stringBuilder = new StringBuilder(100);
		StringBuilder stringBuilder2 = new StringBuilder(100);
		StringBuilder stringBuilder3 = new StringBuilder(100);
		int pcchMaxUserName = 100;
		int pcchMaxDomainame = 100;
		int pcchMaxPassword = 100;
		if (num == 0 && NativeMethods.CredUnPackAuthenticationBuffer(0, refOutAuthBuffer, refOutAuthBufferSize, stringBuilder, ref pcchMaxUserName, stringBuilder3, ref pcchMaxDomainame, stringBuilder2, ref pcchMaxPassword))
		{
			NativeMethods.CoTaskMemFree(refOutAuthBuffer);
			NetworkCredential networkCredential = new NetworkCredential
			{
				UserName = stringBuilder.ToString(),
				Password = stringBuilder2.ToString(),
				Domain = stringBuilder3.ToString()
			};
			if (string.IsNullOrEmpty(networkCredential.Domain))
			{
				int num2 = networkCredential.UserName.IndexOf('\\');
				if (num2 > -1)
				{
					networkCredential.Domain = networkCredential.UserName.Substring(0, num2);
					networkCredential.UserName = networkCredential.UserName.Substring(num2 + 1);
				}
			}
			Credentials = networkCredential;
			return networkCredential;
		}
		return null;
	}
}
