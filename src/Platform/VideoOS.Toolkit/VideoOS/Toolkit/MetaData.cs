using NmToolkit;

namespace VideoOS.Toolkit;

public class MetaData : Data, IMetaData
{
	public unsafe MetaData(ImMetaData* data)
		: base((ImData*)data)
	{
	}
}
