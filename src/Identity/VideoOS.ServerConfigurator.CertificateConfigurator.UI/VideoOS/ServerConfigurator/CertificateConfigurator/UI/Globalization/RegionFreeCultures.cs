using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI.Globalization;

public sealed class RegionFreeCultures : ICultureProvider
{
	private sealed class TitleCaseDisplayCultureInfo : CultureInfo
	{
		public override string NativeName => TextInfo.ToTitleCase(base.NativeName);

		public TitleCaseDisplayCultureInfo(string name)
			: base(name)
		{
		}

		public TitleCaseDisplayCultureInfo(CultureInfo baseInfo)
			: base(baseInfo.Name)
		{
		}
	}

	private sealed class RegionFreeDisplayCultureInfo : CultureInfo
	{
		private readonly TitleCaseDisplayCultureInfo _displayCulture;

		public override string EnglishName => _displayCulture?.EnglishName;

		public override string DisplayName => _displayCulture?.DisplayName;

		public override string NativeName => _displayCulture?.NativeName;

		public RegionFreeDisplayCultureInfo(CultureInfo baseInfo)
			: base(baseInfo.Name)
		{
			_displayCulture = new TitleCaseDisplayCultureInfo(baseInfo.TwoLetterISOLanguageName);
		}
	}

	private readonly ICultureProvider _supportedCultureProvider;

	private IEnumerable<CultureInfo> _supportedCultures;

	public IEnumerable<CultureInfo> SupportedCultures => _supportedCultures ?? (_supportedCultures = GetRegionFreeCultures(_supportedCultureProvider.SupportedCultures));

	public IEnumerable<Exception> Exceptions => _supportedCultureProvider.Exceptions;

	public RegionFreeCultures(ICultureProvider supportedCultureProvider)
	{
		_supportedCultureProvider = supportedCultureProvider ?? throw new ArgumentNullException("supportedCultureProvider");
	}

	private static CultureInfo[] GetRegionFreeCultures(IEnumerable<CultureInfo> supportedCultures)
	{
		return (from ci in supportedCultures
			group ci by ci.TwoLetterISOLanguageName).SelectMany((IGrouping<string, CultureInfo> g) => (g.Count() <= 1) ? new RegionFreeDisplayCultureInfo[1]
		{
			new RegionFreeDisplayCultureInfo(g.First())
		} : ((IEnumerable<CultureInfo>)g).Select((Func<CultureInfo, CultureInfo>)((CultureInfo ci) => new TitleCaseDisplayCultureInfo(ci)))).ToArray();
	}
}
