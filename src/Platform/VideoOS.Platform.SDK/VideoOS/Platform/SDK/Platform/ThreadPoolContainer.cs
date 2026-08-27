using System.Threading;
using System.Windows.Forms;

namespace VideoOS.Platform.SDK.Platform;

public class ThreadPoolContainer
{
	private Thread _callerThread;

	private object _callerReference;

	private AsyncItemsHandler _handler;

	private Control _control;

	public Thread CallerThread => _callerThread;

	public object CallerReference => _callerReference;

	public AsyncItemsHandler AsyncItemsHandler => _handler;

	public Control Control => _control;

	public ThreadPoolContainer(AsyncItemsHandler handler, Control control, object callerReference)
	{
		_callerThread = Thread.CurrentThread;
		_handler = handler;
		_callerReference = callerReference;
		_control = control;
	}
}
