using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.CertificateConfigurator.UI.Globalization;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class SettingsViewModel : TabPageViewModel
{
	private readonly IApplicationSettingsQueryModel _queryModel;

	private readonly ICertificateCmdModel _commandModel;

	private readonly ICommonConfigurationProvider _configurationProvider;

	private readonly ICultureProvider _cultureProvider;

	private CultureInfo _selectedCulture;

	public IEnumerable<CultureInfo> AvailableCultures => _cultureProvider.SupportedCultures.OrderBy((CultureInfo ci) => ci.NativeName);

	public CultureInfo SelectedCulture
	{
		get
		{
			return _selectedCulture;
		}
		set
		{
			if (value != null && value != _queryModel.ApplicationUICulture)
			{
				_selectedCulture = value;
				_commandModel.SetApplicationUICulture(_selectedCulture);
				_configurationProvider.PersistCertificateConfiguration(_queryModel);
				this.CultureChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public event EventHandler CultureChanged;

	public SettingsViewModel(IApplicationSettingsQueryModel queryModel, ICertificateCmdModel commandModel, ICommonConfigurationProvider configurationProvider, ISynchronizationService synchronizationService)
		: this(queryModel, commandModel, configurationProvider, synchronizationService, new CultureProvider(Assembly.GetExecutingAssembly()))
	{
	}

	public SettingsViewModel(IApplicationSettingsQueryModel queryModel, ICertificateCmdModel commandModel, ICommonConfigurationProvider configurationProvider, ISynchronizationService synchronizationService, ICultureProvider cultureProvider)
		: base(synchronizationService)
	{
		_queryModel = queryModel ?? throw new ArgumentNullException("queryModel");
		_commandModel = commandModel ?? throw new ArgumentNullException("commandModel");
		_configurationProvider = configurationProvider ?? throw new ArgumentNullException("configurationProvider");
		_cultureProvider = new RegionFreeCultures(cultureProvider ?? throw new ArgumentNullException("cultureProvider"));
		_selectedCulture = _cultureProvider.SupportedCultures.FirstOrDefault((CultureInfo x) => x.Name == _queryModel.ApplicationUICulture.Name) ?? _queryModel.ApplicationUICulture;
	}
}
