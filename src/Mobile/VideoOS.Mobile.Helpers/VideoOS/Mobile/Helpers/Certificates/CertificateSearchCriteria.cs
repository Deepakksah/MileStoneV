using System;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Mobile.Helpers.Certificates;

public class CertificateSearchCriteria : IComparable<CertificateSearchCriteria>
{
	public X509FindType FindType { get; }

	public string FindValue { get; }

	public StoreLocation StoreLocation { get; }

	public StoreName StoreName { get; }

	private bool IsCaseSensitive
	{
		get
		{
			X509FindType findType = FindType;
			if ((uint)(findType - 1) <= 3u)
			{
				return false;
			}
			return true;
		}
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Compare(this, (CertificateSearchCriteria)obj) == 0;
	}

	public override int GetHashCode()
	{
		return (int)(((uint)((((int)FindType * 397) ^ ((FindValue != null) ? FindValue.ToUpperInvariant().GetHashCode() : 0)) * 397) ^ (uint)StoreLocation) * 397) ^ (int)StoreName;
	}

	public int CompareTo(CertificateSearchCriteria other)
	{
		return Compare(this, other);
	}

	private static int Compare(CertificateSearchCriteria a, CertificateSearchCriteria b)
	{
		if ((object)a == b)
		{
			return 0;
		}
		if ((object)b == null)
		{
			return 1;
		}
		if ((object)a == null)
		{
			return -1;
		}
		int num = a.FindType.CompareTo(b.FindType);
		if (num != 0)
		{
			return num;
		}
		int num2 = string.Compare(a.FindValue, b.FindValue, a.IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
		if (num2 != 0)
		{
			return num2;
		}
		int num3 = a.StoreLocation.CompareTo(b.StoreLocation);
		if (num3 != 0)
		{
			return num3;
		}
		return a.StoreName.CompareTo(b.StoreName);
	}

	public CertificateSelection ToCertificateSelection(bool enabled)
	{
		return new CertificateSelection
		{
			Enabled = enabled,
			FindType = FindType,
			FindValue = FindValue,
			StoreLocation = StoreLocation,
			StoreName = StoreName
		};
	}

	public static bool operator ==(CertificateSearchCriteria leftHandSideComparisonItem, CertificateSearchCriteria rightHandSideComparisonItem)
	{
		return Compare(leftHandSideComparisonItem, rightHandSideComparisonItem) == 0;
	}

	public static bool operator !=(CertificateSearchCriteria leftHandSideComparisonItem, CertificateSearchCriteria rightHandSideComparisonItem)
	{
		return !(leftHandSideComparisonItem == rightHandSideComparisonItem);
	}

	public static bool operator <(CertificateSearchCriteria leftHandSideComparisonItem, CertificateSearchCriteria rightHandSideComparisonItem)
	{
		return Compare(leftHandSideComparisonItem, rightHandSideComparisonItem) < 0;
	}

	public static bool operator <=(CertificateSearchCriteria leftHandSideComparisonItem, CertificateSearchCriteria rightHandSideComparisonItem)
	{
		return Compare(leftHandSideComparisonItem, rightHandSideComparisonItem) <= 0;
	}

	public static bool operator >(CertificateSearchCriteria leftHandSideComparisonItem, CertificateSearchCriteria rightHandSideComparisonItem)
	{
		return Compare(leftHandSideComparisonItem, rightHandSideComparisonItem) > 0;
	}

	public static bool operator >=(CertificateSearchCriteria leftHandSideComparisonItem, CertificateSearchCriteria rightHandSideComparisonItem)
	{
		return Compare(leftHandSideComparisonItem, rightHandSideComparisonItem) >= 0;
	}
}
