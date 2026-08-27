using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Xml;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util.Svc;

namespace VideoOS.Platform.Util.Security;

public class SecurityClientXPCO : IDisposable
{
	private const int DEFAULT_RETRY_COUNT = 1;

	private readonly LoginSettings _loginSettings;

	private readonly ClientPool<SecurityServiceClient> _clientPool;

	public virtual string ServiceName => "SecurityService.svc";

	public Uri ServiceAddress
	{
		get
		{
			if (LoginSettings.IsBasicUser || LoginSettings.IdentityTokenCache != null)
			{
				return GetServerAddress();
			}
			return new Uri("urn:videoos:management:server:securityservice");
		}
	}

	public LoginSettings LoginSettings
	{
		[DebuggerStepThrough]
		get
		{
			return _loginSettings;
		}
	}

	public SecurityClientXPCO(LoginSettings loginSettings)
	{
		_loginSettings = loginSettings;
		_clientPool = new ClientPool<SecurityServiceClient>(CreateClient, 1);
	}

	public virtual Binding GetBinding()
	{
		if (LoginSettings.IsBasicUser)
		{
			return new BasicHttpBinding
			{
				Security = 
				{
					Mode = BasicHttpSecurityMode.Transport,
					Transport = 
					{
						ClientCredentialType = HttpClientCredentialType.Basic
					}
				},
				MaxBufferPoolSize = 2147483647L,
				MaxReceivedMessageSize = 2147483647L,
				ReaderQuotas = XmlDictionaryReaderQuotas.Max,
				CloseTimeout = TimeSpan.FromSeconds(5.0)
			};
		}
		return new WSHttpBinding
		{
			Security = 
			{
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows
				}
			},
			MaxBufferPoolSize = 2147483647L,
			MaxReceivedMessageSize = 2147483647L,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max,
			CloseTimeout = TimeSpan.FromSeconds(5.0)
		};
	}

	public virtual SecurityServiceClient CreateClient()
	{
		SecurityServiceClient securityServiceClient = new SecurityServiceClient(GetBinding(), GetEndpointAddress());
		ConfigureEndpoint(securityServiceClient.Endpoint);
		ConfigureCredentials(securityServiceClient.ClientCredentials);
		return securityServiceClient;
	}

	public SecurityAction[] QueryObjectClassActions(string classId)
	{
		try
		{
			SecurityNamespaceInfo[] array = _clientPool.CallWithRetry((SecurityServiceClient client) => client.QuerySecurityNamespaces(classId));
			if (array == null || array.Length == 0)
			{
				return null;
			}
			List<SecurityAction> list = new List<SecurityAction>();
			ActionDefinition[] actions = array[0].Actions;
			foreach (ActionDefinition actionDefinition in actions)
			{
				list.Add(new SecurityAction
				{
					ActionId = actionDefinition.Name,
					DisplayName = actionDefinition.DisplayName
				});
			}
			return list.ToArray();
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void RegisterObject(string namespaceId, string objectId)
	{
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.RegisterObject(namespaceId, objectId, null);
			});
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void RegisterObject(string namespaceId, string objectId, string parentObjectId)
	{
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.RegisterObject(namespaceId, objectId, parentObjectId);
			});
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void TryRegisterObject(string classId, string objectId)
	{
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.RegisterObject(classId, objectId, null);
			});
		}
		catch (Exception)
		{
		}
	}

	public void TryRegisterObject(string objectId, string namespaceId, string parentClassId)
	{
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.RegisterObject(namespaceId, objectId, parentClassId);
			});
		}
		catch (Exception)
		{
		}
	}

	public void UnregisterObject(string namespaceId, string objectId)
	{
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.UnregisterObject(namespaceId, objectId);
			});
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public bool HasPermission(Guid kind, string objectId, string actionId)
	{
		try
		{
			try
			{
				return _clientPool.CallWithRetry((SecurityServiceClient client) => client.HasPermissionByActionListCurrentUser(kind.ToString(), new string[1] { actionId }, objectId)).First();
			}
			catch (Exception ex) when (ex is MessageSecurityException || ex is ActionNotSupportedException)
			{
				_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
				{
					client.CheckPermission(objectId, actionId);
				});
				return true;
			}
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public bool IsMember(string identitySid, string groupSid)
	{
		try
		{
			return _clientPool.CallWithRetry((SecurityServiceClient client) => client.IsMember(identitySid, groupSid));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void CreateObjectClass(string objectClassId, string[] classActions)
	{
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.CreateObjectClass(objectClassId, classActions);
			});
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void UpdateSecurityNamespace(string objectId, string[] actionIds)
	{
		List<ActionDefinition> list = new List<ActionDefinition>();
		foreach (string text in actionIds)
		{
			list.Add(new ActionDefinition
			{
				DisplayName = text,
				Name = text,
				NamespaceId = objectId,
				Visible = (text != "NOWDEFINED")
			});
		}
		SecurityNamespaceInfo nameSpaceInfo = new SecurityNamespaceInfo
		{
			Name = objectId,
			DisplayName = objectId,
			NamespaceId = objectId,
			Actions = list.ToArray(),
			Separator = '/',
			Structure = 1
		};
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.UpdateSecurityNamespace(nameSpaceInfo);
			});
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void AddAccessControlEntries(Guid kind, string objectId, AccessControlEntry[] entries)
	{
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.AddPermissions(kind.ToString(), objectId, entries);
			});
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void RemoveAccessControlEntries(Guid kind, string objectId, AccessControlEntry[] entries)
	{
		try
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.RemovePermissions(kind.ToString(), objectId, entries);
			});
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionIdList, string objectId)
	{
		try
		{
			return _clientPool.CallWithRetry((SecurityServiceClient client) => client.HasPermissionByActionList(namespaceId, identity, actionIdList, objectId));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectIdList)
	{
		try
		{
			return _clientPool.CallWithRetry((SecurityServiceClient client) => client.HasPermissionByObjectList(namespaceId, identity, actionId, objectIdList));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public bool HasPermission(string namespaceId, string actionId)
	{
		try
		{
			return _clientPool.CallWithRetry((SecurityServiceClient client) => client.HasPermissionByActionListCurrentUser(namespaceId, new string[1] { actionId }, "/")).First();
		}
		catch (Exception ex) when (ex is MessageSecurityException || ex is ActionNotSupportedException)
		{
			_clientPool.CallWithRetry(delegate(SecurityServiceClient client)
			{
				client.CheckPermission(namespaceId, actionId);
			});
			return true;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public Identity ReadIdentityViaName(string userName)
	{
		try
		{
			Identity[] array = _clientPool.CallWithRetry((SecurityServiceClient client) => client.ReadIdentities(SearchFactor.AccountName, new string[1] { userName }, QueryMembership.Expanded));
			if (array.Length != 0)
			{
				return array[0];
			}
			return null;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public Identity ReadIdentityViaSid(string sid)
	{
		try
		{
			Identity[] array = _clientPool.CallWithRetry((SecurityServiceClient client) => client.ReadIdentities(SearchFactor.Sid, new string[1] { sid }, QueryMembership.None));
			if (array.Length != 0)
			{
				return array[0];
			}
			return null;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public EndpointAddress GetEndpointAddress()
	{
		return new EndpointAddress(ServiceAddress, new SpnEndpointIdentity(SpnFactory.GetSpn(GetServerAddress())));
	}

	private Uri GetServerAddress()
	{
		return new UriBuilder(_loginSettings.UriCorporate)
		{
			Path = "/ManagementServer/" + ServiceName
		}.Uri;
	}

	public virtual void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint)
	{
		serviceEndpoint.EndpointBehaviors.Add(new ClientViaBehavior(GetServerAddress()));
	}

	public void ConfigureCredentials(ClientCredentials clientCredentials)
	{
		if (!_loginSettings.IsBasicUser)
		{
			clientCredentials.Windows.ClientCredential = _loginSettings.NetworkCredential;
		}
		else
		{
			clientCredentials.UserName.UserName = _loginSettings.NetworkCredential.UserName;
			clientCredentials.UserName.Password = _loginSettings.NetworkCredential.Password;
		}
		clientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
		{
			CertificateValidationMode = X509CertificateValidationMode.None
		};
	}

	private static Exception HandleException(Exception ex)
	{
		return new NotAuthorizedMIPException(ex.Message, ex);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			_clientPool.Dispose();
		}
	}

	public void Initialize()
	{
		_clientPool.Logoff();
	}

	public void Logoff()
	{
		_clientPool.Logoff();
	}
}
