using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI;

public sealed class ThemeManager : INotifyPropertyChanged
{
	private static bool _initCalled;

	private static bool _themingSystemInitialized;

	private ThemeType __selectedTheme = ThemeType.Light;

	private static ResourceDictionary _staticResources;

	private static readonly IDictionary<ThemeType, ResourceDictionary> _themedResources = new Dictionary<ThemeType, ResourceDictionary>();

	private static IDictionary<string, Color> _brandColorsResourceDictionary;

	public static ThemeManager Instance { get; } = new ThemeManager();

	public ThemeType SelectedTheme
	{
		get
		{
			return __selectedTheme;
		}
		private set
		{
			__selectedTheme = value;
			UpdateThemeResources();
			this.SelectedThemeChanged?.Invoke(this, EventArgs.Empty);
			OnPropertyChanged("SelectedTheme");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public event EventHandler SelectedThemeChanged;

	public static void Init(IDictionary<string, Color> brandColors)
	{
		if (!_initCalled)
		{
			EnvironmentManager.Instance.RegisterReceiver(ChangeThemeHandler, new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));
			_brandColorsResourceDictionary = brandColors;
			UpdateThemeResources();
			InitializeThemingSystem();
			_initCalled = true;
		}
	}

	private static object ChangeThemeHandler(Message message, FQID dest, FQID source)
	{
		if (message.Data is ThemeType selectedTheme)
		{
			Instance.SelectedTheme = selectedTheme;
		}
		return null;
	}

	private static string GetPackUri(string relativePath)
	{
		return "pack://application:,,,/VideoOS.Platform;component/" + relativePath;
	}

	private static ResourceDictionary GetStaticResourcesResourceDictionary()
	{
		return new ResourceDictionary
		{
			Source = new Uri(GetPackUri(FormattableString.Invariant($"Themes/StaticResources.xaml")))
		};
	}

	private static ResourceDictionary GetDesignSystemColorsResourceDictionary()
	{
		return new ResourceDictionary
		{
			Source = new Uri(GetPackUri(FormattableString.Invariant($"Themes/DesignSystemColors.xaml")))
		};
	}

	private static ResourceDictionary GetThemeResourceDictionary(ThemeType theme)
	{
		ResourceDictionary resourceDictionary = new ResourceDictionary();
		resourceDictionary.Source = new Uri(GetPackUri(FormattableString.Invariant($"Themes/{theme}/Theme.xaml")));
		resourceDictionary.PopulateColorsFromSolidColorBrushes<VideoOSResourceKeys>();
		return resourceDictionary;
	}

	private static void UpdateThemeResources()
	{
		foreach (ThemeType value2 in Enum.GetValues(typeof(ThemeType)))
		{
			if (!_themedResources.TryGetValue(value2, out var value))
			{
				value = new ResourceDictionary();
				_themedResources[value2] = value;
			}
			ResourceDictionary themeResourceDictionary = GetThemeResourceDictionary(value2);
			ResourceDictionary designSystemColorsResourceDictionary = GetDesignSystemColorsResourceDictionary();
			if (_brandColorsResourceDictionary != null)
			{
				foreach (string item in designSystemColorsResourceDictionary.Keys.OfType<string>())
				{
					if (_brandColorsResourceDictionary.ContainsKey(item))
					{
						designSystemColorsResourceDictionary[item] = _brandColorsResourceDictionary[item];
					}
				}
			}
			themeResourceDictionary.MergedDictionaries.Clear();
			themeResourceDictionary.MergedDictionaries.Add(designSystemColorsResourceDictionary);
			value.MergedDictionaries.Add(themeResourceDictionary);
		}
		foreach (ResourceDictionary value3 in _themedResources.Values)
		{
			Application.Current.Resources.MergedDictionaries.Remove(value3);
		}
		Application.Current.Resources.MergedDictionaries.Add(_themedResources[Instance.SelectedTheme]);
	}

	private static void ValidateThemes()
	{
		List<object> list = Enum.GetValues(typeof(VideoOSResourceKeys)).OfType<object>().ToList();
		List<object> list2 = (from fi in typeof(VideoOSResourceKeysInternal).GetFields(BindingFlags.Static | BindingFlags.Public)
			where fi.FieldType == typeof(string)
			select fi.GetValue(null)).ToList();
		ResourceDictionary staticResourcesResourceDictionary = GetStaticResourcesResourceDictionary();
		foreach (ThemeType value in Enum.GetValues(typeof(ThemeType)))
		{
			if (TryFindMissingKey(new HashSet<object>(GetThemeResourceDictionary(value).Keys.OfType<object>().Concat(staticResourcesResourceDictionary.Keys.OfType<object>())), out var missingKey, list, list2))
			{
				throw new ConfigurationErrorsException(FormattableString.Invariant($"{value} theme is missing resource key: '{missingKey}'"));
			}
		}
	}

	private static bool TryFindMissingKey(HashSet<object> keys, out object missingKey, params List<object>[] resourceKeys)
	{
		missingKey = null;
		foreach (List<object> source in resourceKeys)
		{
			missingKey = source.FirstOrDefault((object key) => !keys.Contains(key));
			if (missingKey != null)
			{
				return true;
			}
		}
		return false;
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public static void InitializeThemingSystem()
	{
		if (!_themingSystemInitialized)
		{
			_themingSystemInitialized = true;
			if (Application.Current == null)
			{
				new Application().ShutdownMode = ShutdownMode.OnExplicitShutdown;
			}
			_staticResources = GetStaticResourcesResourceDictionary();
			ValidateThemes();
			UpdateThemeResources();
		}
	}

	public object FindStaticResource(object resourceKey)
	{
		if (_staticResources == null)
		{
			throw new InvalidOperationException($"ThemeManager has not been initialized. Call ThemeManager.InitializeThemingSystem() before using FindStaticResource. Key: '{resourceKey}'");
		}
		object obj = _staticResources[resourceKey];
		if (obj == null)
		{
			throw new ArgumentException(FormattableString.Invariant($"Static resource not found: '{resourceKey}'"), "resourceKey");
		}
		return obj;
	}

	public void ApplyThemeOnElement(FrameworkElement frameworkElement, ThemeType theme)
	{
		foreach (KeyValuePair<ThemeType, ResourceDictionary> themedResource in _themedResources)
		{
			frameworkElement.Resources.MergedDictionaries.Remove(themedResource.Value);
		}
		ResourceDictionary item = _themedResources[theme];
		frameworkElement.Resources.MergedDictionaries.Add(item);
		OnPropertyChanged("SelectedTheme");
	}
}
