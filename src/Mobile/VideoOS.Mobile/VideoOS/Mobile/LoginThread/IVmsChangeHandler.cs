using System;
using VideoOS.Mobile.Utilities;

namespace VideoOS.Mobile.LoginThread;

public interface IVmsChangeHandler
{
	IItemManagerAddRemove<object, EventHandler> EventHandlers { get; }

	string MessageId { get; }

	void Initialize();

	void Denitialize();

	void Process();
}
