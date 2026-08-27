using System;
using System.Collections.Specialized;

namespace VideoOS.Common.Interfaces.Communication;

public interface IHttpPost
{
	string Post(Uri url, string dataToPost, int timeout, NameValueCollection additionalHeaders, string userAgent, string contentType);
}
