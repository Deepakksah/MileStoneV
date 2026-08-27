namespace VideoOS.Mobile.Portable.Utilities;

public interface IFpsMeterMeasure
{
	void Reset();

	void OnFrame();
}
