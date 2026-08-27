using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;
using VideoOS.Server.Admin.Properties;

namespace VideoOS.Server.Admin.Utilities;

public class IisActual : IIisInternal
{
	private const int IIS7_IP_LINE_IN_SSL_CERTIFICATE_BINDING = 1;

	private const int II7_CERTIFICATE_LINE_IN_SSL_CERTIFICATE_BINDING = 2;

	public ILogger LogComponent { get; }

	public IisActual(ILogger logger)
	{
		LogComponent = logger;
	}

	public void Iis7DeleteSslCertificateBindings(int httpsPort)
	{
		try
		{
			using Process process = new Process();
			ProcessStartInfo startInfo = IIS7DeleteSSLCertProcessParameters(httpsPort);
			process.StartInfo = startInfo;
			process.Start();
			process.WaitForExit();
		}
		catch (Win32Exception)
		{
		}
	}

	private static ProcessStartInfo IIS7DeleteSSLCertProcessParameters(int httpsPort)
	{
		string text = Path.Combine(Environment.SystemDirectory, "netsh.exe");
		if (!File.Exists(text))
		{
			throw new ServerAdminException(IISHelper_Resources.Error_netsh_not_found);
		}
		ProcessStartInfo obj = new ProcessStartInfo
		{
			FileName = text,
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			WindowStyle = ProcessWindowStyle.Hidden,
			CreateNoWindow = true
		};
		string arguments = string.Format(CultureInfo.InvariantCulture, "http delete sslcert ipport=0.0.0.0:{0}", httpsPort);
		obj.Arguments = arguments;
		return obj;
	}

	public void DeleteSecureBindings(string websiteId)
	{
		using ServerManager serverManager = new ServerManager();
		Site site = FindSiteByID(serverManager, websiteId);
		if (site == null)
		{
			return;
		}
		List<Binding> list = site.Bindings.Where((Binding b) => b.Protocol.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)).ToList();
		if (list.Count <= 0)
		{
			return;
		}
		foreach (Binding item in list)
		{
			site.Bindings.Remove(item);
		}
		try
		{
			serverManager.CommitChanges();
		}
		catch (FileNotFoundException ex)
		{
			LoggerExtensions.LogDebug(LogComponent, ex.ToString(), Array.Empty<object>());
		}
	}

	private bool HasSecureBindings(string websiteId, int httpsPort)
	{
		HttpsBindingInformation httpsBindingInformation = CheckSecureBindingsAndGetHttpsPort(websiteId);
		if (httpsBindingInformation.HasBinding && httpsBindingInformation.Port == httpsPort)
		{
			return true;
		}
		return false;
	}

	public string GetPhysicalPath(string websiteId, string path)
	{
		using (ServerManager mgr = new ServerManager())
		{
			Site site = FindSiteByID(mgr, websiteId);
			if (site != null)
			{
				Application application = site.Applications.FirstOrDefault((Application a) => string.Equals(a.Path, path, StringComparison.OrdinalIgnoreCase));
				if (application != null)
				{
					return application.VirtualDirectories.First().PhysicalPath;
				}
			}
		}
		throw new ServerAdminException(IISHelper_Resources.Could_not_find_IIS_directory);
	}

	public HttpBindingInformation GetHttpPort(string websiteId)
	{
		using ServerManager mgr = new ServerManager();
		try
		{
			Site site = FindSiteByID(mgr, websiteId);
			if (site == null)
			{
				return new HttpBindingInformation();
			}
			foreach (Binding binding in site.Bindings)
			{
				if (string.Equals(binding.Protocol, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
				{
					BindingInformation bindingInformation = ParseBindingToPort(binding.BindingInformation);
					if (bindingInformation.HasBinding)
					{
						return new HttpBindingInformation(bindingInformation);
					}
				}
			}
			return new HttpBindingInformation();
		}
		catch (COMException innerException)
		{
			throw new ServerAdminException(IISHelper_Resources.Error_Get_secure_bindings, innerException);
		}
	}

	public void SetSecureBindings(string websiteId, int httpsPort)
	{
		if (HasSecureBindings(websiteId, httpsPort))
		{
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_secure_bindings_ok, httpsPort), Array.Empty<object>());
			return;
		}
		using (ServerManager serverManager = new ServerManager())
		{
			Site site = FindSiteByID(serverManager, websiteId);
			if (site != null)
			{
				Binding binding = site.Bindings.CreateElement("binding");
				binding.Protocol = Uri.UriSchemeHttps;
				binding.BindingInformation = string.Format(CultureInfo.InvariantCulture, "{0}:{1}:", "*", httpsPort);
				site.Bindings.Add(binding);
				serverManager.CommitChanges();
			}
		}
		if (HasSecureBindings(websiteId, httpsPort))
		{
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_secure_bindings_ok, httpsPort), Array.Empty<object>());
			return;
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Set_secure_bindings), Array.Empty<object>());
		throw new ServerAdminException(IISHelper_Resources.Error_Set_secure_bindings);
	}

	public void IisControlledReset()
	{
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_stopping_services), Array.Empty<object>());
		using IisControlledResetHelper iisControlledResetHelper = new IisControlledResetHelper("WAS", LogComponent);
		iisControlledResetHelper.StopServiceWithDependencies(TimeSpan.FromMinutes(2.0));
		Thread.Sleep(2000);
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_starting_services), Array.Empty<object>());
		iisControlledResetHelper.ResumeServiceWithDependencies(TimeSpan.FromMinutes(1.0));
	}

	public static BindingInformation ParseBindingToPort(string binding)
	{
		BindingInformation bindingInformation = new BindingInformation();
		if (string.IsNullOrWhiteSpace(binding))
		{
			return bindingInformation;
		}
		Match match = Regex.Match(binding, "(:)(\\d+)(:)");
		if (!match.Success)
		{
			return bindingInformation;
		}
		if (!int.TryParse(match.Groups[2].Value, out var result))
		{
			return bindingInformation;
		}
		bindingInformation.HasBinding = true;
		bindingInformation.Port = result;
		return bindingInformation;
	}

	private static Site FindSiteByID(ServerManager mgr, string websiteId)
	{
		foreach (Site site in mgr.Sites)
		{
			if (site.Id.ToString(CultureInfo.InvariantCulture) == websiteId)
			{
				return site;
			}
		}
		return null;
	}

	private HttpsBindingInformation CheckSecureBindingsAndGetHttpsBindingInformation(string websiteId)
	{
		try
		{
			using (ServerManager mgr = new ServerManager())
			{
				Site site = FindSiteByID(mgr, websiteId);
				if (site == null)
				{
					return new HttpsBindingInformation();
				}
				foreach (Binding binding in site.Bindings)
				{
					if (string.Equals(binding.Protocol, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
					{
						BindingInformation bindingInformation = ParseBindingToPort(binding.BindingInformation);
						if (bindingInformation.HasBinding)
						{
							return new HttpsBindingInformation(bindingInformation);
						}
					}
				}
			}
			return new HttpsBindingInformation();
		}
		catch (COMException innerException)
		{
			throw new ServerAdminException(IISHelper_Resources.Error_Get_secure_bindings, innerException);
		}
	}

	public HttpsBindingInformation CheckSecureBindingsAndGetHttpsPort(string websiteId)
	{
		return CheckSecureBindingsAndGetHttpsBindingInformation(websiteId);
	}

	public SslCertificateBindingInformation GetCertificateBinding(int httpsPort)
	{
		SslCertificateBindingInformation sslCertificateBindingInformation = IIS7CheckHttpsPortInSSLCertificateBindings(httpsPort);
		if (sslCertificateBindingInformation.HasCertificateBinding)
		{
			GetCertificateInformation(sslCertificateBindingInformation.CertificateHash, sslCertificateBindingInformation);
		}
		return sslCertificateBindingInformation;
	}

	public AppPoolState RecycleAppPool(string appPoolName)
	{
		return Convert(AppPoolHelper.RecycleApplicationPool(appPoolName));
	}

	public AppPoolState GetAppPoolState(string appPoolName)
	{
		return Convert(AppPoolHelper.GetAppPoolState(appPoolName));
	}

	private static AppPoolState Convert(ObjectState objectState)
	{
		return objectState switch
		{
			ObjectState.Starting => AppPoolState.Starting, 
			ObjectState.Started => AppPoolState.Started, 
			ObjectState.Stopping => AppPoolState.Stopping, 
			ObjectState.Stopped => AppPoolState.Stopped, 
			_ => AppPoolState.Unknown, 
		};
	}

	public void Control(IisAction action)
	{
		try
		{
			using Process process = new Process();
			ProcessStartInfo startInfo = CreateControlProcessParameters(action);
			process.StartInfo = startInfo;
			process.Start();
			process.WaitForExit();
		}
		catch (Win32Exception innerException)
		{
			throw new ServerAdminException(Resources.IISResetNotFound, innerException);
		}
	}

	private ProcessStartInfo CreateControlProcessParameters(IisAction action)
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = Path.Combine(folderPath, "iisreset.exe");
		processStartInfo.UseShellExecute = false;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.RedirectStandardError = true;
		processStartInfo.CreateNoWindow = true;
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		switch (action)
		{
		case IisAction.Start:
			processStartInfo.Arguments = "/start";
			LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Starting_the_IIS_service, Array.Empty<object>());
			break;
		case IisAction.Stop:
			processStartInfo.Arguments = "/stop /TIMEOUT:120";
			LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Stopping_the_IIS_service, Array.Empty<object>());
			break;
		case IisAction.Restart:
			processStartInfo.Arguments = "/TIMEOUT:120";
			LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Restarting_the_IIS_service, Array.Empty<object>());
			break;
		}
		return processStartInfo;
	}

	private static string Iis7GetHashFromSSLCertificateBindings(List<string> processOutput)
	{
		string result = null;
		int num = 0;
		foreach (string item in processOutput)
		{
			if (string.IsNullOrEmpty(item))
			{
				continue;
			}
			string[] array = item.Split(':');
			if (array.Length >= 2 && !string.IsNullOrEmpty(array[0].Trim()) && !string.IsNullOrEmpty(array[1].Trim()))
			{
				num++;
				if (num == 2)
				{
					result = array[1].Trim();
					break;
				}
			}
		}
		return result;
	}

	private static void Iis7FindHttpsPortInShowSSLCert(List<string> processOutput, int httpsPort, SslCertificateBindingInformation info)
	{
		info.FoundHttpsPortString = string.Empty;
		int num = 0;
		string value = httpsPort.ToString(CultureInfo.InvariantCulture);
		foreach (string item in processOutput)
		{
			if (string.IsNullOrEmpty(item))
			{
				continue;
			}
			string[] array = item.Split(':');
			if (array.Length < 2 || string.IsNullOrEmpty(array[0].Trim()) || string.IsNullOrEmpty(array[1].Trim()))
			{
				continue;
			}
			num++;
			if (num == 1)
			{
				info.FoundHttpsPortString = item;
				if (item.Contains(value))
				{
					info.HasCertificateBinding = true;
					return;
				}
			}
		}
		info.HasCertificateBinding = false;
	}

	private static SslCertificateBindingInformation IIS7CheckHttpsPortInSSLCertificateBindings(int httpsPort)
	{
		try
		{
			SslCertificateBindingInformation sslCertificateBindingInformation = new SslCertificateBindingInformation();
			List<string> processOutput = new List<string>();
			using (Process process = new Process())
			{
				ProcessStartInfo startInfo = IIS7ShowSSLCertProcessParameters(httpsPort);
				process.StartInfo = startInfo;
				process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
				{
					processOutput.Add(e.Data);
				};
				process.Start();
				process.BeginOutputReadLine();
				process.WaitForExit();
				process.CancelOutputRead();
			}
			Iis7FindHttpsPortInShowSSLCert(processOutput, httpsPort, sslCertificateBindingInformation);
			sslCertificateBindingInformation.CertificateHash = Iis7GetHashFromSSLCertificateBindings(processOutput);
			return sslCertificateBindingInformation;
		}
		catch (Win32Exception innerException)
		{
			throw new ServerAdminException(IISHelper_Resources.Error_Show_SSL_cert, innerException);
		}
	}

	private static ProcessStartInfo IIS7ShowSSLCertProcessParameters(int httpsPort)
	{
		string text = Path.Combine(Environment.SystemDirectory, "netsh.exe");
		if (!File.Exists(text))
		{
			throw new ServerAdminException(IISHelper_Resources.Error_netsh_not_found);
		}
		ProcessStartInfo obj = new ProcessStartInfo
		{
			FileName = text,
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			WindowStyle = ProcessWindowStyle.Hidden,
			CreateNoWindow = true
		};
		string arguments = string.Format(CultureInfo.InvariantCulture, "http show sslcert ipport=0.0.0.0:{0}", httpsPort);
		obj.Arguments = arguments;
		return obj;
	}

	private static void GetCertificateInformation(string certHash, SslCertificateBindingInformation binding)
	{
		binding.CertificateExistsInStore = false;
		binding.CertificateName = null;
		if (string.IsNullOrEmpty(certHash))
		{
			return;
		}
		X509Store x509Store = null;
		try
		{
			x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			x509Store.Open(OpenFlags.ReadOnly);
			X509Certificate2Enumerator enumerator = x509Store.Certificates.GetEnumerator();
			while (enumerator.MoveNext())
			{
				X509Certificate2 current = enumerator.Current;
				if (string.Equals(current.GetCertHashString(), certHash, StringComparison.OrdinalIgnoreCase))
				{
					binding.CertificateName = current.SubjectName.Name;
					binding.CertificateExistsInStore = true;
					break;
				}
			}
		}
		catch (CryptographicException innerException)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Cannot_read_certificate_store), innerException);
		}
		catch (SecurityException innerException2)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Cannot_read_certificate_store), innerException2);
		}
		finally
		{
			x509Store?.Close();
		}
	}

	public void AddSslCertificate(int httpsPort, string certificateHash)
	{
		try
		{
			using Process process = new Process();
			ProcessStartInfo startInfo = IIS7AddSSLCertProcessParameters(httpsPort, certificateHash);
			process.StartInfo = startInfo;
			process.Start();
			process.WaitForExit();
		}
		catch (Win32Exception innerException)
		{
			throw new ServerAdminException(IISHelper_Resources.Error_Add_SSL_cert, innerException);
		}
	}

	private static ProcessStartInfo IIS7AddSSLCertProcessParameters(int httpsPort, string certificateHash)
	{
		string text = Path.Combine(Environment.SystemDirectory, "netsh.exe");
		if (!File.Exists(text))
		{
			throw new ServerAdminException(IISHelper_Resources.Error_netsh_not_found);
		}
		Guid guid = Guid.NewGuid();
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = text;
		processStartInfo.UseShellExecute = false;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.RedirectStandardError = true;
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		processStartInfo.CreateNoWindow = true;
		string arguments = string.Format(CultureInfo.InvariantCulture, "http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={2}{3}{4}", httpsPort, certificateHash, "{", guid, "}");
		processStartInfo.Arguments = arguments;
		return processStartInfo;
	}

	public string CreateSelfSignedCertificateIfNeeded()
	{
		return CreateSelfSignedCertificateIfNeeded(Dns.GetHostEntry("localhost").HostName);
	}

	public string CreateSelfSignedCertificateIfNeeded(string iisCertificateName)
	{
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Creating_certificate, iisCertificateName), Array.Empty<object>());
		string text = ReadCertificateHashFromStore(iisCertificateName);
		if (string.IsNullOrEmpty(text))
		{
			CreateCertificateAndImportToStore(iisCertificateName);
			text = ReadCertificateHashFromStore(iisCertificateName);
			if (text == null)
			{
				throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Cannot_create_certificate, iisCertificateName));
			}
		}
		return text;
	}

	private static void ImportCertificateProcess(string filename, string password)
	{
		try
		{
			using Process process = new Process();
			ProcessStartInfo startInfo = ImportCertificateProcessParameters(filename, password);
			process.StartInfo = startInfo;
			process.Start();
			process.WaitForExit();
		}
		catch (Win32Exception innerException)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Cannot_import_certificate, filename), innerException);
		}
	}

	private static ProcessStartInfo ImportCertificateProcessParameters(string filename, string password)
	{
		string text = Path.Combine(Environment.SystemDirectory, "certutil.exe");
		if (!File.Exists(text))
		{
			throw new ServerAdminException(IISHelper_Resources.Error_certutil_not_found);
		}
		ProcessStartInfo obj = new ProcessStartInfo
		{
			FileName = text,
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			WindowStyle = ProcessWindowStyle.Hidden,
			CreateNoWindow = true
		};
		string arguments = string.Format(CultureInfo.InvariantCulture, "-f -p \"{0}\" -importpfx \"{1}\" NoRoot", password, filename);
		obj.Arguments = arguments;
		return obj;
	}

	private static bool CreateCertificateAndImportToStore(string certificateName)
	{
		try
		{
			if (ReadCertificateFromStore(certificateName) != null)
			{
				return false;
			}
			string text = string.Format(CultureInfo.InvariantCulture, "{0}.pfx", certificateName);
			string text2 = Membership.GeneratePassword(128, 0);
			byte[] buffer = NativeMethods.CreateSelfSignCertificatePfx(string.Format(CultureInfo.InvariantCulture, "CN={0}", certificateName), DateTime.Now, DateTime.Now.AddYears(100), text2);
			using (FileStream output = File.Open(text, FileMode.Create))
			{
				new BinaryWriter(output).Write(buffer);
			}
			ImportCertificateProcess(text, text2);
			File.Delete(text);
		}
		catch (Win32Exception innerException)
		{
			throw new ServerAdminException(string.Format(CultureInfo.InvariantCulture, IISHelper_Resources.Error_Cannot_create_certificate, certificateName), innerException);
		}
		return true;
	}

	private static X509Certificate2 ReadCertificateFromStore(string certificateName)
	{
		X509Store x509Store = null;
		X509Certificate2 result = null;
		try
		{
			string b = string.Format(CultureInfo.InvariantCulture, "CN={0}", certificateName);
			x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			x509Store.Open(OpenFlags.ReadOnly);
			X509Certificate2Enumerator enumerator = x509Store.Certificates.GetEnumerator();
			while (enumerator.MoveNext())
			{
				X509Certificate2 current = enumerator.Current;
				if (string.Equals(current.SubjectName.Name, b, StringComparison.OrdinalIgnoreCase))
				{
					result = current;
					break;
				}
			}
		}
		catch (CryptographicException innerException)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Cannot_read_certificate_store), innerException);
		}
		catch (SecurityException innerException2)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Cannot_read_certificate_store), innerException2);
		}
		finally
		{
			x509Store?.Close();
		}
		return result;
	}

	private static string ReadCertificateHashFromStore(string certificateName)
	{
		X509Store x509Store = null;
		string result = null;
		try
		{
			string b = string.Format(CultureInfo.InvariantCulture, "CN={0}", certificateName);
			x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			x509Store.Open(OpenFlags.ReadOnly);
			X509Certificate2Enumerator enumerator = x509Store.Certificates.GetEnumerator();
			while (enumerator.MoveNext())
			{
				X509Certificate2 current = enumerator.Current;
				if (string.Equals(current.SubjectName.Name, b, StringComparison.OrdinalIgnoreCase))
				{
					result = current.GetCertHashString();
					break;
				}
			}
		}
		catch (CryptographicException innerException)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Cannot_read_certificate_store), innerException);
		}
		catch (SecurityException innerException2)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Cannot_read_certificate_store), innerException2);
		}
		finally
		{
			x509Store?.Close();
		}
		return result;
	}
}
