using System.Collections.Generic;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.Encryption;

public static class DhPublicKeyHelper
{
	public static readonly Dictionary<PrimeLength, int> MaxPublicKeyLens = new Dictionary<PrimeLength, int>
	{
		{
			PrimeLength.Prime1024,
			200
		},
		{
			PrimeLength.Prime2048,
			400
		}
	};

	public static int GetMaxPublicKeyLength(PrimeLength primeLength)
	{
		return MaxPublicKeyLens[primeLength];
	}
}
