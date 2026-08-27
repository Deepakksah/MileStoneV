using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoOS.Common.Integration;

public class PasswordGenerator : IPasswordGenerator
{
	private Random _rng;

	private List<char> _randomGeneratorAlphabet;

	private CharacterSet[] _characterSets;

	private const int _defaultMinPasswordLength = 6;

	private const int _defaultMaxPasswordLength = 16;

	public int MaxGeneratedLength { get; set; }

	public PasswordGenerator()
	{
		_rng = new Random(DateTime.Now.Ticks.GetHashCode());
		MaxGeneratedLength = 32;
	}

	private void InitializeRandomSymbolTable(PasswordRequirements passwordRequirements)
	{
		_randomGeneratorAlphabet = new List<char>();
		_randomGeneratorAlphabet.AddRange(from num in Enumerable.Range(48, 10)
			select (char)num);
		_randomGeneratorAlphabet.AddRange(from num in Enumerable.Range(97, 26)
			select (char)num);
		_randomGeneratorAlphabet.AddRange(from num in Enumerable.Range(65, 26)
			select (char)num);
		foreach (CharacterSet setsOfRequiredCharacter in passwordRequirements.SetsOfRequiredCharacters)
		{
			foreach (char characters in setsOfRequiredCharacter.CharactersList)
			{
				_randomGeneratorAlphabet.Add(characters);
			}
		}
		_randomGeneratorAlphabet = _randomGeneratorAlphabet.Distinct().ToList();
		foreach (char characters2 in passwordRequirements.ForbiddenCharacters.CharactersList)
		{
			_randomGeneratorAlphabet.Remove(characters2);
		}
		_characterSets = passwordRequirements.SetsOfRequiredCharacters.ToArray();
	}

	private static void Swap(ref int intA, ref int intB)
	{
		int num = intB;
		intB = intA;
		intA = num;
	}

	private int[] GenerateCombination(int maxValue, int count)
	{
		int[] array = Enumerable.Range(0, maxValue).ToArray();
		List<int> list = new List<int>(count);
		for (int num = maxValue - 1; num > maxValue - count - 1; num--)
		{
			int num2 = _rng.Next(0, num + 1);
			Swap(ref array[num], ref array[num2]);
			list.Add(array[num]);
		}
		return list.ToArray();
	}

	private char GenerateRandomCharacter(IList<char> source)
	{
		return source[_rng.Next(0, source.Count)];
	}

	public string GeneratePassword(int? minPasswordLength, int? maxPasswordLength, PasswordRequirements passwordRequirements)
	{
		int num = 6;
		int num2 = 16;
		if (minPasswordLength.HasValue)
		{
			num = minPasswordLength.Value;
			if (!maxPasswordLength.HasValue && num > 16)
			{
				num2 = num;
			}
		}
		if (maxPasswordLength.HasValue)
		{
			num2 = maxPasswordLength.Value;
			if (!minPasswordLength.HasValue && num2 < 6)
			{
				num = num2;
			}
		}
		if (MaxGeneratedLength < num2)
		{
			num2 = MaxGeneratedLength;
		}
		PasswordRequirements passwordRequirements2 = null;
		passwordRequirements2 = ((passwordRequirements != null) ? passwordRequirements : new PasswordRequirementsParser().GetRequirements(needsUppercaseLetters: true, needsLowercaseLetters: true, needsDigits: true));
		InitializeRandomSymbolTable(passwordRequirements2);
		if (num > num2)
		{
			throw new ArgumentException("min password length > max password length");
		}
		if (num < 0)
		{
			throw new ArgumentException("min password length should be > 0");
		}
		if (num2 < 1)
		{
			throw new ArgumentException("max password length should be > 1");
		}
		char[] array = GenerateBasePassword(num2);
		if (num2 >= _characterSets.Length)
		{
			array = AddRequiredCharacters(array);
		}
		return new string(array);
	}

	private char[] AddRequiredCharacters(char[] basePassword)
	{
		int[] array = GenerateCombination(basePassword.Length, _characterSets.Length);
		for (int i = 0; i < array.Length; i++)
		{
			basePassword[array[i]] = GenerateRandomCharacter(_characterSets[i].CharactersList);
		}
		return basePassword;
	}

	private char[] GenerateBasePassword(int length)
	{
		StringBuilder stringBuilder = new StringBuilder(length);
		for (int i = 0; i < length; i++)
		{
			char value = GenerateRandomCharacter(_randomGeneratorAlphabet);
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString().ToCharArray();
	}
}
