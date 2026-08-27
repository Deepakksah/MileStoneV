using System;
using System.IO;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IWebResponse : IDisposable
{
	Stream GetResponseStream();
}
