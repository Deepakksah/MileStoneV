using System;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Mobile.Helpers.Certificates;

public class CertificateSelection
{
	private string _findValue = string.Empty;

	public bool Enabled { get; set; } = true;

	public X509FindType FindType { get; set; } = X509FindType.FindBySubjectDistinguishedName;

	public string FindValue
	{
		get
		{
			return _findValue;
		}
		set
		{
			_findValue = value ?? throw new ArgumentNullException("value");
		}
	}

	public StoreLocation StoreLocation { get; set; } = StoreLocation.LocalMachine;

	public StoreName StoreName { get; set; } = StoreName.My;

	public override int GetHashCode()
	{
		return 1;
	}

	public static bool operator ==(CertificateSelection left, CertificateSelection right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(CertificateSelection left, CertificateSelection right)
	{
		return !(left == right);
	}

	public bool Equals(CertificateSelection other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		if (Enabled == other.Enabled && FindType == other.FindType && string.Equals(FindValue, other.FindValue, StringComparison.Ordinal))
		{
			return StoreLocation == other.StoreLocation;
		}
		return false;
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is CertificateSelection other))
		{
			return false;
		}
		return Equals(other);
	}
}
