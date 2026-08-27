using System;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Media;

public interface ISignature
{
	DateTime SignatureTime { get; }

	IReadOnlyBuffer SignatureData { get; }
}
