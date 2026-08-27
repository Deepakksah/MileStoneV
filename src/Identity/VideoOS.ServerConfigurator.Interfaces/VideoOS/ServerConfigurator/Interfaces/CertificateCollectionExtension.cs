using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.ServerConfigurator.Interfaces;

public static class CertificateCollectionExtension
{
	public static bool Contains(this IEnumerable<ICertificate> collection, string thumbprint)
	{
		return collection.Any((ICertificate x) => AreEqual(x.Thumbprint, thumbprint));
	}

	public static ICertificate FirstOrDefault(this IEnumerable<ICertificate> collection, string thumbprint)
	{
		return collection.FirstOrDefault((ICertificate x) => AreEqual(x.Thumbprint, thumbprint));
	}

	private static bool AreEqual(string thumbprintA, string thumbprintB)
	{
		return string.Compare(thumbprintA, thumbprintB, StringComparison.OrdinalIgnoreCase) == 0;
	}
}
