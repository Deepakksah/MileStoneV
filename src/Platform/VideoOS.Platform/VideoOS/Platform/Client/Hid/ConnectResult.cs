using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Client.Hid;

public sealed class ConnectResult
{
	public bool ConnectionSuccessful { get; }

	public string ErrorMessage { get; }

	public IEnumerable<HidInput> HidInputs { get; }

	public ConnectResult(IEnumerable<HidInput> hidInputs)
	{
		ConnectionSuccessful = true;
		HidInputs = hidInputs ?? Enumerable.Empty<HidInput>();
	}

	public ConnectResult(string errorMessage)
	{
		ConnectionSuccessful = false;
		ErrorMessage = errorMessage;
	}
}
