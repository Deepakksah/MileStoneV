using System;
using System.Net.Http;
using System.ServiceModel;
using System.Web;
using VideoOS.Management.Server.RestApi;

namespace VideoOS.Management.Server;

public class GeminiApplicationHost : ApplicationHost<GeminiRequestContext>, IDisposable
{
	private ServiceProvider _serviceProvider;

	public override Guid InstanceId => new Guid("3337EF88-7284-49E2-BBB9-A400BED62DD2");

	public GeminiApplicationHost(GeminiServiceHostProperties properties, GeminiApplicationHost parentHost)
	{
		_serviceProvider = new ServiceProvider();
	}

	public override void Start(GeminiRequestContext requestContext)
	{
	}

	public override void Stop(GeminiRequestContext requestContext)
	{
		throw new NotImplementedException();
	}

	public override GeminiRequestContext CreateRequest(RequestContextType requestType, HostRequestType type, params object[] additionalParameters)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		switch (type)
		{
		case HostRequestType.Default:
			return new GeminiRequestContext(this, requestType);
		case HostRequestType.AspNet:
			if (additionalParameters == null)
			{
				throw new ArgumentNullException("additionalParameters");
			}
			return new AspNetRequestContext(this, (HttpContext)additionalParameters[0]);
		case HostRequestType.Wcf:
			if (additionalParameters == null)
			{
				throw new ArgumentNullException("additionalParameters");
			}
			return new WcfRequestContext(this, (OperationContext)additionalParameters[0]);
		case HostRequestType.RestApi:
			if (additionalParameters == null)
			{
				throw new ArgumentNullException("additionalParameters");
			}
			return new RestApiRequestContext(this, (HttpRequestMessage)additionalParameters[0]);
		default:
			throw new ArgumentOutOfRangeException("type");
		}
	}

	public T GetService<T>(GeminiRequestContext requestContext) where T : class, IGeminiServerService
	{
		CheckIfDisposed();
		return _serviceProvider.GetService<T>(requestContext);
	}

	private void CheckIfDisposed()
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException("GeminiApplicationHost");
		}
	}
}
