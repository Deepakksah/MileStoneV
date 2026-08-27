using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;

namespace VideoOS.DataCollector.Service.Common.ExtensionMethods;

public static class WebHostExtensions
{
	public static IWebHostBuilder ConfigureKestrelEndpoint(this IWebHostBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder, "builder");
		builder.ConfigureKestrel(delegate(WebHostBuilderContext context, KestrelServerOptions options)
		{
			options.ConfigureEndpointDefaults(delegate(ListenOptions d)
			{
				d.Protocols = HttpProtocols.Http1AndHttp2;
			});
			IConfigurationSection kestrelSection = context.Configuration.GetSection("Kestrel");
			options.Configure(kestrelSection).Endpoint("HTTPS", delegate(EndpointConfiguration listenOptions)
			{
				string text = kestrelSection["Endpoints:Https:CertificateThumbprint"];
				if (string.IsNullOrEmpty(text))
				{
					throw new InvalidOperationException("Thumbprint must be specified when running HTTPS");
				}
				using X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
				x509Store.Open(OpenFlags.OpenExistingOnly);
				X509Certificate2 x509Certificate = x509Store.Certificates.Find(X509FindType.FindByThumbprint, text, validOnly: false).FirstOrDefault();
				if (x509Certificate == null)
				{
					throw new InvalidOperationException("Certificate with thumbprint '" + text + "' not found");
				}
				listenOptions.ListenOptions.UseHttps(x509Certificate);
			});
		});
		return builder;
	}
}
