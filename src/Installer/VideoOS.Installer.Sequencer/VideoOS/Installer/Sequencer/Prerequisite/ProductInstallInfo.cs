using System;
using Microsoft.Installer.ProductConfig;
using VideoOS.Installer.Common;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Sequencer.Prerequisite;

public class ProductInstallInfo
{
	private string _displayName;

	private CommandsTypeCommand _command;

	private string _errorString;

	private int _summEstimatedInstallSecounds;

	private int _estimatedInstalledBytes;

	private PlatformID _platform;

	private Version _version;

	private string _checkVersion;

	private string _property;

	private string _productCode;

	public string DisplayName => _displayName;

	public CommandsTypeCommand Command => _command;

	public string ErrorString => _errorString;

	public int SummEstimatedInstallSecounds => _summEstimatedInstallSecounds;

	public int EstimatedInstalledBytes => _estimatedInstalledBytes;

	public string ProductCode => _productCode;

	public ProductInstallInfo(string displayName)
	{
		RetriveOSProperties();
		_displayName = displayName;
	}

	public ProductInstallInfo(string displayName, Product product, string property, object checkValue)
	{
		_checkVersion = checkValue?.ToString();
		_productCode = product.ProductCode;
		_property = property;
		RetriveOSProperties();
		FindCommandLine(product);
		_displayName = displayName;
		if (Command != null && Command.EstimatedInstalledBytes != null)
		{
			_estimatedInstalledBytes = Convert.ToInt32(Command.EstimatedInstalledBytes);
		}
	}

	private void RetriveOSProperties()
	{
		_platform = Environment.OSVersion.Platform;
		_version = Environment.OSVersion.Version;
		LogManager.Write("Platform = " + _platform);
		LogManager.Write("Version = " + _version.ToString());
	}

	private void FindCommandLine(Product product)
	{
		string text = null;
		if (product.Commands == null)
		{
			return;
		}
		CommandsTypeCommand[] command = product.Commands.Command;
		foreach (CommandsTypeCommand commandsTypeCommand in command)
		{
			text = ValidateCommand(commandsTypeCommand);
			if (text == null)
			{
				_command = commandsTypeCommand;
				LogManager.Write("Command found: " + commandsTypeCommand.PackageFile);
				break;
			}
		}
		if (Command != null)
		{
			if (Command.EstimatedInstallSeconds != null)
			{
				int num = Convert.ToInt32(Command.EstimatedInstallSeconds);
				if (num != 0)
				{
					_summEstimatedInstallSecounds = num;
				}
			}
		}
		else
		{
			_errorString = text;
		}
	}

	private string ValidateCommand(CommandsTypeCommand cmd)
	{
		object[] installConditions = cmd.InstallConditions;
		foreach (object obj in installConditions)
		{
			string text = ValidateProperty(obj);
			if (text != null)
			{
				return text;
			}
		}
		return null;
	}

	private string ValidateProperty(object obj)
	{
		if (obj is CommandsTypeCommandBypassIf)
		{
			CommandsTypeCommandBypassIf commandsTypeCommandBypassIf = obj as CommandsTypeCommandBypassIf;
			if (CompareProperty(commandsTypeCommandBypassIf.Property, commandsTypeCommandBypassIf.Value, commandsTypeCommandBypassIf.Compare))
			{
				return commandsTypeCommandBypassIf.Property;
			}
		}
		else if (obj is CommandsTypeCommandFailIf)
		{
			CommandsTypeCommandFailIf commandsTypeCommandFailIf = obj as CommandsTypeCommandFailIf;
			if (CompareProperty(commandsTypeCommandFailIf.Property, commandsTypeCommandFailIf.Value, commandsTypeCommandFailIf.Compare))
			{
				return commandsTypeCommandFailIf.String;
			}
		}
		return null;
	}

	private bool CompareProperty(string property, string propValue, CompareType compareType)
	{
		LogManager.Write($"Compare property '{property} value='{propValue}' CompareType='{compareType}'");
		if (string.Compare(property, "ProcessorArchitecture", StringComparison.InvariantCultureIgnoreCase) == 0)
		{
			return CompareHelper.ComparePlatform(ProcessorArchitectureHelper.GetPlatformType(), propValue, compareType);
		}
		if (string.Compare(property, "versionnt", StringComparison.InvariantCultureIgnoreCase) == 0)
		{
			if (_platform != PlatformID.Win32NT)
			{
				return false;
			}
			return CompareHelper.CompareVersions(_version, CompareHelper.ConvertVersionToString(propValue), compareType);
		}
		if (string.Compare(property, "version9x", StringComparison.InvariantCultureIgnoreCase) == 0)
		{
			if (_platform != PlatformID.Win32Windows)
			{
				return false;
			}
			return CompareHelper.CompareVersions(_version, CompareHelper.ConvertVersionToString(propValue), compareType);
		}
		if (string.Compare(property, "versionmsi", StringComparison.InvariantCultureIgnoreCase) == 0 || property == _property || string.Compare(property, "versionmsidll", StringComparison.InvariantCultureIgnoreCase) == 0)
		{
			return CompareHelper.Compare(_checkVersion, propValue, compareType);
		}
		if (property != _property && string.Compare(property, "AdminUser", StringComparison.InvariantCultureIgnoreCase) != 0 && string.Compare(property, "InstallMode", StringComparison.InvariantCultureIgnoreCase) != 0)
		{
			return true;
		}
		return false;
	}
}
