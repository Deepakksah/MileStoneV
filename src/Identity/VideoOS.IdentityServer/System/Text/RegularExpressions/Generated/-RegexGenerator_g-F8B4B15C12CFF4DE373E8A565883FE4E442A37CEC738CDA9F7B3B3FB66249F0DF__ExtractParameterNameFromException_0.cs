using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "8.0.12.26602")]
public sealed class _003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__ExtractParameterNameFromException_0 : Regex
{
	private sealed class RunnerFactory : RegexRunnerFactory
	{
		private sealed class Runner : RegexRunner
		{
			protected override void Scan(ReadOnlySpan<char> inputSpan)
			{
				while (TryFindNextPossibleStartingPosition(inputSpan) && !TryMatchAtCurrentPosition(inputSpan) && runtextpos != inputSpan.Length)
				{
					runtextpos++;
					if (_003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
				}
			}

			private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				if (num <= inputSpan.Length - 33)
				{
					int num2 = MemoryExtensions.IndexOf<char>(inputSpan.Slice(num), (ReadOnlySpan<char>)"Failed to bind parameter");
					if (num2 >= 0)
					{
						runtextpos = num + num2;
						return true;
					}
				}
				runtextpos = inputSpan.Length;
				return false;
			}

			private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				int start = num;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
				if (!MemoryExtensions.StartsWith<char>(readOnlySpan, (ReadOnlySpan<char>)"Failed to bind parameter"))
				{
					UncaptureUntil(0);
					return false;
				}
				num += 24;
				readOnlySpan = inputSpan.Slice(num);
				int i;
				for (i = 0; (uint)i < (uint)readOnlySpan.Length && char.IsWhiteSpace(readOnlySpan[i]); i++)
				{
				}
				if (i == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(i);
				num += i;
				if (readOnlySpan.IsEmpty || readOnlySpan[0] != '"')
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				readOnlySpan = inputSpan.Slice(num);
				num2 = num;
				if (readOnlySpan.IsEmpty || readOnlySpan[0] == '\n')
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				readOnlySpan = inputSpan.Slice(num);
				num4 = num;
				while (true)
				{
					num3 = Crawlpos();
					Capture(1, num2, num);
					if (!readOnlySpan.IsEmpty && readOnlySpan[0] == '"')
					{
						num++;
						readOnlySpan = inputSpan.Slice(num);
						int j;
						for (j = 0; (uint)j < (uint)readOnlySpan.Length && char.IsWhiteSpace(readOnlySpan[j]); j++)
						{
						}
						if (j != 0)
						{
							readOnlySpan = readOnlySpan.Slice(j);
							num += j;
							if (MemoryExtensions.StartsWith<char>(readOnlySpan, (ReadOnlySpan<char>)"from"))
							{
								break;
							}
						}
					}
					UncaptureUntil(num3);
					if (_003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
					num = num4;
					readOnlySpan = inputSpan.Slice(num);
					if (readOnlySpan.IsEmpty || readOnlySpan[0] == '\n')
					{
						UncaptureUntil(0);
						return false;
					}
					num++;
					readOnlySpan = inputSpan.Slice(num);
					num4 = MemoryExtensions.IndexOfAny<char>(readOnlySpan, '\n', '"');
					if ((uint)num4 >= (uint)readOnlySpan.Length || readOnlySpan[num4] == '\n')
					{
						UncaptureUntil(0);
						return false;
					}
					num += num4;
					readOnlySpan = inputSpan.Slice(num);
					num4 = num;
				}
				Capture(0, start, runtextpos = num + 4);
				return true;
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				void UncaptureUntil(int capturePosition)
				{
					while (Crawlpos() > capturePosition)
					{
						Uncapture();
					}
				}
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	public static readonly _003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__ExtractParameterNameFromException_0 Instance = new _003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__ExtractParameterNameFromException_0();

	private _003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__ExtractParameterNameFromException_0()
	{
		pattern = "Failed to bind parameter\\s+\"(.+?)\"\\s+from";
		roptions = RegexOptions.None;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 2;
	}
}
