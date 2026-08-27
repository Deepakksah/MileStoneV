using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using VideoOS.ServerConfigurator.CertificateConfigurator.UI.Properties;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class CertificateViewModel : IEquatable<CertificateViewModel>
{
	public string FriendlyName { get; private set; }

	public string Thumbprint { get; private set; }

	public ICertificate UnderlyingSource { get; private set; }

	public string DisplayInfo { get; private set; }

	public string Issuer { get; private set; }

	public DateTime ExpiryDate { get; set; }

	public CertificateViewModel(ICertificate source)
	{
		if (source == null)
		{
			FriendlyName = Resources.CertificateView_SelectCertificate;
			Thumbprint = string.Empty;
			DisplayInfo = Resources.CertificateView_NoCertificateSelected;
			return;
		}
		UnderlyingSource = source;
		Thumbprint = source.Thumbprint;
		Issuer = GetCommonName(source.IssuerName);
		ExpiryDate = source.ValidUntil;
		FriendlyName = (string.IsNullOrWhiteSpace(source.FriendlyName) ? GetCommonName(source.SubjectName) : source.FriendlyName);
		DisplayInfo = string.Format(CultureInfo.CurrentCulture, Resources.CertificateView_DisplayInfo, Issuer, ExpiryDate.ToShortDateString());
	}

	public void ShowCertificateDetails(Window parent)
	{
		IntPtr parentHandle = ((parent != null) ? new WindowInteropHelper(parent).Handle : ((IntPtr)0));
		ShowCertificateDetails(parentHandle);
	}

	public void ShowCertificateDetails(Form parent)
	{
		IntPtr parentHandle = parent?.Handle ?? ((IntPtr)0);
		ShowCertificateDetails(parentHandle);
	}

	public void ShowCertificateDetails()
	{
		ShowCertificateDetails((IntPtr)0);
	}

	private void ShowCertificateDetails(IntPtr parentHandle)
	{
		if (UnderlyingSource?.X509Certificate == null)
		{
			return;
		}
		try
		{
			if (parentHandle != (IntPtr)0)
			{
				X509Certificate2UI.DisplayCertificate(UnderlyingSource.X509Certificate, parentHandle);
			}
			else
			{
				X509Certificate2UI.DisplayCertificate(UnderlyingSource.X509Certificate);
			}
		}
		catch (CryptographicException)
		{
		}
	}

	private static string GetCommonName(string stringThatMayContainAttributes)
	{
		return (from attr in stringThatMayContainAttributes.Split(',')
			select attr.Split('=') into attr
			where attr.Length > 1 && attr[0].Trim() == "CN"
			select attr[1].Trim()).FirstOrDefault() ?? stringThatMayContainAttributes;
	}

	public bool Equals(CertificateViewModel other)
	{
		if (other == null)
		{
			return false;
		}
		return string.Compare(Thumbprint, other.Thumbprint, StringComparison.OrdinalIgnoreCase) == 0;
	}

	public override bool Equals(object obj)
	{
		CertificateViewModel other = obj as CertificateViewModel;
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return Thumbprint.GetHashCode();
	}

	public static bool operator ==(CertificateViewModel certificateA, CertificateViewModel certificateB)
	{
		return certificateA?.Equals(certificateB) ?? ((object)certificateB == null);
	}

	public static bool operator !=(CertificateViewModel certificateA, CertificateViewModel certificateB)
	{
		return !(certificateA == certificateB);
	}
}
