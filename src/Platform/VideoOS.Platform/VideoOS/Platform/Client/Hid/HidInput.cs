using System;

namespace VideoOS.Platform.Client.Hid;

public abstract class HidInput
{
	public static readonly Guid ActionNoAction = new Guid("76BCDB6B-8FD1-43C7-891C-1D5B6B71ECED");

	public Guid Id { get; }

	public string Name { get; }

	public Guid DefaultActionId { get; }

	public event EventHandler<HidValueChangedEventArgs> ValueChangedEvent;

	public HidInput(Guid id, string name, Guid defaultActionId)
	{
		Id = id;
		Name = name;
		DefaultActionId = defaultActionId;
	}

	public void FireValueChangedEvent(HidValueChangedEventArgs e)
	{
		this.ValueChangedEvent?.Invoke(this, e);
	}
}
