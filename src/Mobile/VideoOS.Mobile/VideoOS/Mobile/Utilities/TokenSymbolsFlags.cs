using System;

namespace VideoOS.Mobile.Utilities;

[Flags]
public enum TokenSymbolsFlags
{
	CapitalLetters = 1,
	SmallLetters = 2,
	Digits = 4,
	Symbols = 8
}
