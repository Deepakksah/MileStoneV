using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Exceptions;

namespace VideoOS.Management.VmoClient;

public class SiteIdpConnector : ISiteIdpConnector
{
	private readonly IIdpDiscoveryService _idpDiscoveryService;

	private const int IdpDiscoTimeout = 3;

	private const int LongIdpDiscoTimeout = 300;

	private readonly object _locker = new object();

	public SiteIdpConnector(IIdpDiscoveryService idpDiscoveryService)
	{
		_idpDiscoveryService = idpDiscoveryService;
	}

	public async Task<IdpInfo> GetIdpInfo(Uri address, bool useLongTimeout, CancellationToken cancellationToken)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		int timeout = (useLongTimeout ? 300 : 3);
		try
		{
			IdpInfo obj = await _idpDiscoveryService.ValidateIdpServer(address, timeout, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!obj.IsOAuthSupported)
			{
				throw new IdpOfflineException("IDP is offline!");
			}
			return obj;
		}
		catch (NotFoundException)
		{
			throw new IdpOfflineException("IDP is offline!");
		}
		catch (UnauthorizedException)
		{
			throw new IdpOfflineException("IDP is offline!");
		}
		catch (ForbiddenException)
		{
			throw new IdpOfflineException("IDP is offline!");
		}
		catch (InternalServerErrorException)
		{
			throw new IdpOfflineException("IDP is offline!");
		}
	}

	public async Task<IdpInfo> GetFastestIdpInfo(IEnumerable<Uri> addresses, bool useLongTimeout, bool allowUnsecure, CancellationToken cancellationToken)
	{
		return (await GetIdpInfoOrError(addresses, useLongTimeout, allowUnsecure, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).Select<(Uri, IdpInfo, Exception), IdpInfo>(((Uri Address, IdpInfo IdpInfo, Exception Error) x) => x.IdpInfo).FirstOrDefault((IdpInfo y) => y != null);
	}

	public async Task<IEnumerable<(Uri Address, IdpInfo IdpInfo, Exception Error)>> GetIdpInfoOrError(IEnumerable<Uri> addresses, bool useLongTimeout, bool allowUnsecure, CancellationToken cancellationToken)
	{
		if (addresses == null)
		{
			throw new ArgumentNullException("addresses");
		}
		if (addresses.Count() == 0)
		{
			throw new InvalidArgumentException("Collection addresses can not be empty!");
		}
		CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		try
		{
			List<(Uri, IdpInfo, Exception)> result = (await Task.WhenAll(addresses.Select((Uri x) => (Address: x, GetIdpInfoTask: Task.Run(() => GetIdpInfo(x, useLongTimeout, cts.Token), cts.Token))).Select(async delegate((Uri Address, Task<IdpInfo> GetIdpInfoTask) x)
			{
				(Uri Address, IdpInfo IdpInfo, Exception Error) timeoutResult = (Address: x.Address, IdpInfo: null, Error: new TimeoutException());
				try
				{
					IdpInfo idpInfo = await x.GetIdpInfoTask.ConfigureAwait(continueOnCapturedContext: false);
					bool flag = false;
					lock (_locker)
					{
						if (!cts.IsCancellationRequested && (idpInfo.Address.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) || allowUnsecure))
						{
							cts.Cancel();
							return ((Uri Address, IdpInfo IdpInfo, Exception Error))(Address: x.Address, IdpInfo: idpInfo, Error: null);
						}
						if (!cts.IsCancellationRequested)
						{
							flag = true;
						}
					}
					if (flag)
					{
						await Task.Delay(2000, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
						lock (_locker)
						{
							if (!cts.IsCancellationRequested)
							{
								cts.Cancel();
								return ((Uri Address, IdpInfo IdpInfo, Exception Error))(Address: x.Address, IdpInfo: idpInfo, Error: null);
							}
						}
					}
					return timeoutResult;
				}
				catch (OperationCanceledException) when (cts.IsCancellationRequested)
				{
					return timeoutResult;
				}
				catch (Exception item)
				{
					return ((Uri Address, IdpInfo IdpInfo, Exception Error))(Address: x.Address, IdpInfo: null, Error: item);
				}
			})).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			cancellationToken.ThrowIfCancellationRequested();
			return result;
		}
		finally
		{
			if (cts != null)
			{
				((IDisposable)cts).Dispose();
			}
		}
	}
}
