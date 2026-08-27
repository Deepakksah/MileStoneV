using System.Threading;
using System.Windows.Forms;
using VideoOS.Platform.Admin;

namespace VideoOS.Platform.SDK.Platform;

public class AsyncLookupCategoryContainer
{
	private Thread _callerThread;

	private object _callerReference;

	private AsyncItemsHandler _handler;

	private Control _control;

	private Category _category;

	public Thread CallerThread => _callerThread;

	public object CallerReference => _callerReference;

	public AsyncItemsHandler AsyncItemHandler => _handler;

	public Control Control => _control;

	public Category Category => _category;

	public AsyncLookupCategoryContainer(AsyncItemsHandler handler, Control control, object callerReference, Category id)
	{
		_callerThread = Thread.CurrentThread;
		_handler = handler;
		_callerReference = callerReference;
		_control = control;
		_category = id;
	}
}
