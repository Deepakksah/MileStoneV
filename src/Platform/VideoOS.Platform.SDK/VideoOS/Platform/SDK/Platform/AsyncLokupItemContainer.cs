using System.Threading;
using System.Windows.Forms;

namespace VideoOS.Platform.SDK.Platform;

public class AsyncLokupItemContainer
{
	private Thread _callerThread;

	private object _callerReference;

	private AsyncItemHandler _handler;

	private Control _control;

	private FQID _fqid;

	public Thread CallerThread => _callerThread;

	public object CallerReference => _callerReference;

	public AsyncItemHandler AsyncItemHandler => _handler;

	public Control Control => _control;

	public FQID FQID => _fqid;

	public AsyncLokupItemContainer(AsyncItemHandler handler, Control control, object callerReference, FQID fqid)
	{
		_callerThread = Thread.CurrentThread;
		_handler = handler;
		_callerReference = callerReference;
		_control = control;
		_fqid = fqid;
	}
}
