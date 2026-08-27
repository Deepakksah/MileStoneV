using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Login;

public delegate Task<DialogLoginResult> AsyncLoginCallback<T>(T connectionInfo, CancellationToken cancellationToken) where T : IConnectionInfo;
