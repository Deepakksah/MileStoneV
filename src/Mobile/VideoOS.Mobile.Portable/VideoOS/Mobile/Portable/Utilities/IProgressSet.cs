namespace VideoOS.Mobile.Portable.Utilities;

public interface IProgressSet
{
	uint CurrentIndex { get; }

	uint TotalCount { get; }

	void SetProgressCurrentTotal(uint current, uint total);
}
