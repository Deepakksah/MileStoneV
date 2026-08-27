using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Threading;
using VideoOS.Management.Common;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;

public class IntercommunicationCommandAgent
{
	private const int INITIAL_BINDING_TIMEOUT = 10000;

	private const int MAX_TRY_COUNT = 3;

	private string _componentName;

	private bool _initialized;

	public IntercommunicationCommandAgent(string componentName)
	{
		_componentName = componentName;
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format(CultureInfo.InvariantCulture, "{0}.EventServer.Intercommunication.MilestoneCentralServerSide.public.cer", Assembly.GetExecutingAssembly().GetName().Name));
		int num = (int)stream.Length;
		byte[] array = new byte[num];
		using (BufferedStream bufferedStream = new BufferedStream(stream))
		{
			bufferedStream.Read(array, 0, num);
		}
		X509Store x509Store = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
		x509Store.Open(OpenFlags.ReadWrite);
		X509Certificate2 x509Certificate = new X509Certificate2();
		x509Certificate.Import(array);
		x509Store.Add(x509Certificate);
		_initialized = true;
	}

	private static InterCommunicationCommandClient CreateIntercommunicationCommandClient(string serverAddress, int timeout)
	{
		UriBuilder obj = new UriBuilder(serverAddress)
		{
			Path = "Central/InterCommunication"
		};
		InterCommunicationCommandClient interCommunicationCommandClient = new InterCommunicationCommandClient(remoteAddress: new EndpointAddress(identity: EndpointIdentity.CreateDnsIdentity("MilestoneCentralServerSide"), uri: obj.Uri, addressHeaders: Array.Empty<AddressHeader>()), binding: new WSHttpBinding((obj.Uri.Scheme == Uri.UriSchemeHttp) ? SecurityMode.Message : SecurityMode.TransportWithMessageCredential)
		{
			Security = 
			{
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Certificate
				}
			},
			CloseTimeout = TimeSpan.FromMilliseconds(timeout),
			OpenTimeout = TimeSpan.FromMilliseconds(timeout),
			ReceiveTimeout = TimeSpan.FromMilliseconds(timeout),
			SendTimeout = TimeSpan.FromMilliseconds(timeout)
		});
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format(CultureInfo.InvariantCulture, "{0}.EventServer.Intercommunication.MilestoneCentralClientSide.private.pfx", Assembly.GetExecutingAssembly().GetName().Name));
		int num = (int)stream.Length;
		byte[] array = new byte[num];
		new BufferedStream(stream).Read(array, 0, num);
		interCommunicationCommandClient.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(array, string.Empty, X509KeyStorageFlags.UserKeySet);
		interCommunicationCommandClient.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;
		interCommunicationCommandClient.ClientCredentials.ServiceCertificate.Authentication.TrustedStoreLocation = StoreLocation.CurrentUser;
		return interCommunicationCommandClient;
	}

	public bool SendReloadConfiguration(IList<ServerAddress> addresses, ReportComponentStatus reportStatus)
	{
		if (addresses == null)
		{
			throw new ArgumentNullException("addresses");
		}
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		if (!_initialized)
		{
			return false;
		}
		foreach (ServerAddress address in addresses)
		{
			int num = 0;
			int num2 = 10000;
			while (num < 3)
			{
				try
				{
					InterCommunicationCommandClient interCommunicationCommandClient = CreateIntercommunicationCommandClient(address.URL, num2);
					try
					{
						interCommunicationCommandClient.Open();
						interCommunicationCommandClient.ReloadConfiguration();
						int num3 = 0;
						while (true)
						{
							int num4 = interCommunicationCommandClient.ReloadConfigurationProgress();
							if (interCommunicationCommandClient.IsReloadConfigurationStarted() && num3 < num4)
							{
								num3 = num4;
								reportStatus(_componentName, num4);
							}
							if (interCommunicationCommandClient.IsReloadConfigurationDone())
							{
								break;
							}
							Thread.Sleep(100);
						}
						return true;
					}
					finally
					{
						try
						{
							if (interCommunicationCommandClient.State == CommunicationState.Opened || interCommunicationCommandClient.State == CommunicationState.Created)
							{
								interCommunicationCommandClient.Close();
							}
							else
							{
								interCommunicationCommandClient.Abort();
							}
						}
						catch (CommunicationObjectFaultedException)
						{
						}
						catch (TimeoutException)
						{
						}
					}
				}
				catch (EndpointNotFoundException)
				{
				}
				catch (TimeoutException)
				{
					num++;
					if (num == 3)
					{
						break;
					}
					num2 *= 2;
					continue;
				}
				catch (Exception)
				{
					num++;
					if (num == 3)
					{
						break;
					}
					continue;
				}
				break;
			}
		}
		return false;
	}

	public string GetServiceAccount(IList<ServerAddress> addresses)
	{
		if (addresses == null)
		{
			throw new ArgumentNullException("addresses");
		}
		if (!_initialized)
		{
			return null;
		}
		foreach (ServerAddress address in addresses)
		{
			int num = 0;
			int num2 = 10000;
			while (num < 3)
			{
				try
				{
					InterCommunicationCommandClient interCommunicationCommandClient = CreateIntercommunicationCommandClient(address.URL, num2);
					try
					{
						interCommunicationCommandClient.Open();
						InterCommunicationUserID serviceAccount = interCommunicationCommandClient.GetServiceAccount();
						if (serviceAccount != null && !string.IsNullOrWhiteSpace(serviceAccount.Name) && !string.IsNullOrWhiteSpace(serviceAccount.SID) && !string.IsNullOrWhiteSpace(serviceAccount.Location))
						{
							string text = string.Empty;
							string text2 = string.Empty;
							string[] array = serviceAccount.Location.Split('\\');
							if (array.Length >= 2)
							{
								for (int i = 0; i < array.Length - 1; i++)
								{
									if (text.Length > 0)
									{
										text += "\\";
									}
									text += array[i];
								}
								text2 = array[array.Length - 1];
							}
							else if (array.Length == 1)
							{
								text2 = array[0];
							}
							bool flag = !string.IsNullOrEmpty(text);
							if (new SecurityIdentifier(serviceAccount.SID).Equals(new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null)) && text2 != Environment.MachineName)
							{
								if (flag)
								{
									return string.IsNullOrWhiteSpace(text) ? (text2 + "$") : (text + "\\" + text2 + "$");
								}
								return "NETWORK SERVICE";
							}
							if (flag)
							{
								return serviceAccount.Name;
							}
							string[] array2 = serviceAccount.Name.Split('\\');
							if (array2.Length > 1)
							{
								return array2[array2.Length - 1];
							}
							return serviceAccount.Name;
						}
						return null;
					}
					finally
					{
						try
						{
							if (interCommunicationCommandClient.State == CommunicationState.Opened || interCommunicationCommandClient.State == CommunicationState.Created)
							{
								interCommunicationCommandClient.Close();
							}
							else
							{
								interCommunicationCommandClient.Abort();
							}
						}
						catch (CommunicationObjectFaultedException)
						{
						}
						catch (TimeoutException)
						{
						}
					}
				}
				catch (EndpointNotFoundException)
				{
				}
				catch (TimeoutException)
				{
					num++;
					if (num == 3)
					{
						break;
					}
					num2 *= 2;
					continue;
				}
				catch (Exception)
				{
					num++;
					if (num == 3)
					{
						break;
					}
					continue;
				}
				break;
			}
		}
		return null;
	}
}
