using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class DataParams
{
	protected uint _paramMask;

	public virtual Guid StreamId { get; set; }

	public virtual StreamParamsHelper.ByteOrder ByteOrder { get; set; }
}
