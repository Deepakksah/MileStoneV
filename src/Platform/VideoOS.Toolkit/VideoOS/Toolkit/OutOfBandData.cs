using NmToolkit;

namespace VideoOS.Toolkit;

public class OutOfBandData : Data, IOutOfBandData
{
	public unsafe OutOfBandData(ImOutOfBandData* data)
		: base((ImData*)data)
	{
	}
}
