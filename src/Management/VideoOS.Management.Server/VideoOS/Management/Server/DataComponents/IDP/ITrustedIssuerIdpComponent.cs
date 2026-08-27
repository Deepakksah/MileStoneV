using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server.DataComponents.IDP;

public interface ITrustedIssuerIdpComponent : IDisposable
{
	TrustedIssuerData CreateTrustedIssuer(string issuer, Uri address);

	TrustedIssuerData[] QueryTrustedIssuers();

	TrustedIssuerData QueryTrustedIssuerById(int id);

	TrustedIssuerData[] QueryTrustedIssuerByIds(IEnumerable<int> ids);

	TrustedIssuerData UpdateTrustedIssuer(int id, string issuer, Uri address);

	void DeleteTrustedIssuer(int id);
}
