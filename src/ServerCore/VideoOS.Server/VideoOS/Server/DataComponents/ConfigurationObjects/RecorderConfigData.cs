using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class RecorderConfigData
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public bool Enabled { get; set; }

	public DateTime LastModified { get; set; }

	public static RecorderConfigData[] QueryRecorders(IDataComponentFactory dataComponentFactory)
	{
		using IRecorderConfigDataComponent recorderConfigDataComponent = dataComponentFactory.CreateRecorderConfigDataComponent();
		return recorderConfigDataComponent.QueryRecorderData();
	}

	public static bool RecorderExists(IDataComponentFactory dataComponentFactory, Guid id)
	{
		using IRecorderConfigDataComponent recorderConfigDataComponent = dataComponentFactory.CreateRecorderConfigDataComponent();
		return recorderConfigDataComponent.RecorderExists(id);
	}

	public static bool RecorderEnabled(IDataComponentFactory dataComponentFactory, Guid id)
	{
		using IRecorderConfigDataComponent recorderConfigDataComponent = dataComponentFactory.CreateRecorderConfigDataComponent();
		return recorderConfigDataComponent.RecorderEnabled(id);
	}
}
