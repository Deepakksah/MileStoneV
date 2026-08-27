using System;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils;

public class IsSelectedChangedEventManager : WeakEventManager
{
	private static IsSelectedChangedEventManager CurrentManager
	{
		get
		{
			Type typeFromHandle = typeof(IsSelectedChangedEventManager);
			IsSelectedChangedEventManager isSelectedChangedEventManager = (IsSelectedChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
			if (isSelectedChangedEventManager == null)
			{
				isSelectedChangedEventManager = new IsSelectedChangedEventManager();
				WeakEventManager.SetCurrentManager(typeFromHandle, isSelectedChangedEventManager);
			}
			return isSelectedChangedEventManager;
		}
	}

	public static void AddHandler(ISelectable source, EventHandler<EventArgs> handler)
	{
		CurrentManager.ProtectedAddHandler(source, handler);
	}

	public static void RemoveHandler(ISelectable source, EventHandler<EventArgs> handler)
	{
		CurrentManager.ProtectedRemoveHandler(source, handler);
	}

	protected override ListenerList NewListenerList()
	{
		return new ListenerList<EventArgs>();
	}

	protected override void StartListening(object source)
	{
		(source as ISelectable).IsSelectedChanged += selectable_IsSelectedChanged;
	}

	protected override void StopListening(object source)
	{
		(source as ISelectable).IsSelectedChanged -= selectable_IsSelectedChanged;
	}

	private void selectable_IsSelectedChanged(object sender, EventArgs e)
	{
		DeliverEvent(sender, e);
	}
}
