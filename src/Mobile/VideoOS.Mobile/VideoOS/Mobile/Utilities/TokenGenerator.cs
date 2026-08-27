using System;
using System.Collections.Generic;
using System.Text;
using VideoOS.Mobile.Portable.Encryption;

namespace VideoOS.Mobile.Utilities;

public class TokenGenerator
{
	private static readonly string Digits = "0123456789";

	private static readonly string CapitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private static readonly string SmallLetters = "abcdefghijklmnopqrstuvwxyz";

	private static readonly string SpecialSymbols = "!#$%()*+,-./;=?@[\\]^_`{|}~";

	private readonly Dictionary<TokenSymbolsFlags, string> _tokenSymbolsRepo = new Dictionary<TokenSymbolsFlags, string>
	{
		{
			TokenSymbolsFlags.CapitalLetters,
			CapitalLetters
		},
		{
			TokenSymbolsFlags.SmallLetters,
			SmallLetters
		},
		{
			TokenSymbolsFlags.Digits,
			Digits
		},
		{
			TokenSymbolsFlags.Symbols,
			SpecialSymbols
		}
	};

	private int _codeLength = 6;

	private TokenSymbolsFlags _symbols = TokenSymbolsFlags.Digits;

	private char[] _tokenSymbols = Digits.ToCharArray();

	public int CodeLength
	{
		get
		{
			return _codeLength;
		}
		set
		{
			_codeLength = value;
		}
	}

	public TokenSymbolsFlags Symbols
	{
		get
		{
			return _symbols;
		}
		set
		{
			if (_symbols == value)
			{
				return;
			}
			_symbols = value;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TokenSymbolsFlags value2 in Enum.GetValues(typeof(TokenSymbolsFlags)))
			{
				if (_symbols.HasFlag(value2))
				{
					stringBuilder.Append(_tokenSymbolsRepo[value2]);
				}
			}
			_tokenSymbols = stringBuilder.ToString().ToCharArray();
		}
	}

	public string Generate()
	{
		int[] array = new int[_codeLength];
		for (int i = 0; i < _codeLength; i++)
		{
			array[i] = GenerateSymbolIndex((byte)_tokenSymbols.Length);
		}
		StringBuilder stringBuilder = new StringBuilder();
		int[] array2 = array;
		foreach (int num in array2)
		{
			stringBuilder.Append(_tokenSymbols[num]);
		}
		return stringBuilder.ToString();
	}

	private static byte GenerateSymbolIndex(byte tokenLength)
	{
		if (tokenLength <= 0)
		{
			throw new ArgumentOutOfRangeException("tokenLength");
		}
		byte[] array = new byte[1];
		RandomGenerator randomGenerator = new RandomGenerator();
		do
		{
			randomGenerator.GetBytes(array);
		}
		while (!IsFairRoll(array[0], tokenLength));
		return (byte)(array[0] % tokenLength);
	}

	private static bool IsFairRoll(byte roll, byte tokenLength)
	{
		int num = 255 / tokenLength;
		return roll < tokenLength * num;
	}
}
