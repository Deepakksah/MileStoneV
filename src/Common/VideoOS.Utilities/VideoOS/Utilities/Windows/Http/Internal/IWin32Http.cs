using System;
using System.Collections.Generic;

namespace VideoOS.Utilities.Windows.Http.Internal;

public interface IWin32Http
{
	IDisposable InitializeConfig();

	IEnumerable<UrlAclInfo> QueryUrlAclInfos();

	UrlAclInfo QueryUrlAclInfo(string urlPrefix);

	void DeleteUrlAclInfo(string urlPrefix);

	void SetUrlAclInfo(UrlAclInfo aclInfo);

	IEnumerable<SslConfiguration> QuerySslConfigurations();

	void SetSslConfiguration(SslConfiguration configuration);

	SslConfiguration QuerySslConfiguration(SslConfigurationKey configurationKey);

	void DeleteSslConfiguration(SslConfigurationKey configurationKey);
}
