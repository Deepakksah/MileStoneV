using System;

namespace VideoOS.Common.Interfaces.Communication;

public interface IUriHandler
{
	Uri CreateUri(string url);

	Uri CheckAndModifyUri(Uri uri);

	Uri CheckAndModifyUri(Uri uri, bool allowInvalidCertificate);

	Uri ModifyUri(Uri uri, bool isSecureAvailable, bool isUnsecureAvailable);

	Uri ForceModifyUri(Uri uri, bool isSecure);

	bool IsSiteWorking(Uri uri, out bool isSecureAvailable);

	bool IsSiteWorking(Uri uri, out bool isSecureAvailable, bool allowInvalidCertificate);

	void Launch(Uri uri);

	void Launch(Uri uri, bool allowInvalidCertificate);

	void ForceLaunch(Uri uri);
}
