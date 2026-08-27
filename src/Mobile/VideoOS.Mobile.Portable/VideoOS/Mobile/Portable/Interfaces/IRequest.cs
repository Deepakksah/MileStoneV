using System;

namespace VideoOS.Mobile.Portable.Interfaces;

public interface IRequest
{
	string Method { get; set; }

	void GetResponse(Action<IResponse> asyncCallback);
}
