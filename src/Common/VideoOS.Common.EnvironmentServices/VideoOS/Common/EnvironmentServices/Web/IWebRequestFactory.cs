using System;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IWebRequestFactory
{
	IWebRequest Create(Uri uri);
}
