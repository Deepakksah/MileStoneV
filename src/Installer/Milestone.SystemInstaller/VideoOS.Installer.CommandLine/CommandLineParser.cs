using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VideoOS.Installer.CommandLine;

public class CommandLineParser : ICommandLineParser
{
	public bool ParseArguments(string[] args, object options)
	{
		if (args == null)
		{
			throw new ArgumentNullException("args");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		Dictionary<FieldInfo, OptionAttribute> fields = ValidateFields(options);
		HashSet<FieldInfo> hashSet = new HashSet<FieldInfo>();
		Regex regex = new Regex("^(/|--?)(?<name>\\w+)(=(?<value>.*))?$");
		foreach (string input in args)
		{
			Match match = regex.Match(input);
			if (!match.Success)
			{
				return false;
			}
			string name = match.Groups["name"].Value;
			string value = match.Groups["value"].Value;
			FieldInfo fieldInfo = fields.Keys.FirstOrDefault((FieldInfo p) => name.Equals(fields[p].ShortName, StringComparison.OrdinalIgnoreCase) || name.Equals(fields[p].LongName, StringComparison.OrdinalIgnoreCase));
			if (fieldInfo == null || !hashSet.Add(fieldInfo))
			{
				return false;
			}
			if (fieldInfo.FieldType == typeof(bool) && string.IsNullOrEmpty(value))
			{
				fieldInfo.SetValue(options, true);
				continue;
			}
			if (fieldInfo.FieldType == typeof(string))
			{
				fieldInfo.SetValue(options, value ?? string.Empty);
				continue;
			}
			if (fieldInfo.FieldType == typeof(int) && int.TryParse(value, out var result))
			{
				fieldInfo.SetValue(options, result);
				continue;
			}
			if (fieldInfo.FieldType == typeof(IList<string>) && !string.IsNullOrEmpty(value) && fields[fieldInfo] is OptionListAttribute optionListAttribute)
			{
				fieldInfo.SetValue(options, value.Split(optionListAttribute.Separator).ToList());
				continue;
			}
			return false;
		}
		return true;
	}

	public IEnumerable<OptionAttribute> GetOptionList(object options)
	{
		return ValidateFields(options).Values;
	}

	private static Dictionary<FieldInfo, OptionAttribute> ValidateFields(object options)
	{
		Dictionary<FieldInfo, OptionAttribute> dictionary = (from prop in options.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
			where Attribute.IsDefined(prop, typeof(OptionAttribute))
			select prop).ToDictionary((FieldInfo prop) => prop, (FieldInfo prop) => (OptionAttribute)Attribute.GetCustomAttribute(prop, typeof(OptionAttribute)));
		foreach (KeyValuePair<FieldInfo, OptionAttribute> item in dictionary)
		{
			if (item.Key.FieldType != typeof(bool) && item.Key.FieldType != typeof(string) && item.Key.FieldType != typeof(int) && item.Key.FieldType != typeof(IList<string>))
			{
				throw new NotSupportedException("Type '" + item.Key.FieldType.Name + "' not supported for field '" + item.Key.Name + "'.");
			}
			if (item.Key.FieldType == typeof(IList<string>) && !(item.Value is OptionListAttribute))
			{
				throw new NotSupportedException("Type '" + item.Key.FieldType.Name + "' not supported for field '" + item.Key.Name + "'.");
			}
		}
		return dictionary;
	}
}
