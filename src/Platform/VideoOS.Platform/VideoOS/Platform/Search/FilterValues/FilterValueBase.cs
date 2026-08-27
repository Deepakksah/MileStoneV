using System;

namespace VideoOS.Platform.Search.FilterValues;

public abstract class FilterValueBase
{
	public SearchFilter SearchFilter { get; set; }

	public event EventHandler Changed;

	public abstract string FallbackDisplayValue();

	protected void FireChangedEvent()
	{
		this.Changed?.Invoke(this, EventArgs.Empty);
	}

	public abstract string Serialize();

	public abstract bool Deserialize(string data);
}
