using System.Linq;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.Converters;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Builders;

public class LicensePlateItemsBuilder : ILicensePlateItemsBuilder
{
	private readonly IFQIDConverter _fqidConverter;

	private readonly IStringCollectionConverter _stringCollectionConverter;

	public LicensePlateItemsBuilder(IFQIDConverter fqidConverter, IStringCollectionConverter stringCollectionConverter)
	{
		_fqidConverter = fqidConverter;
		_stringCollectionConverter = stringCollectionConverter;
	}

	public LicensePlateInfo[] BuildLicensePlateInfos(string registrationNumbersInfoSeparatedList)
	{
		return _stringCollectionConverter.StringTo2DimensionalArray(registrationNumbersInfoSeparatedList).Select(BuildLicensePlateInfo).ToArray();
	}

	public LicensePlate[] BuildLicensePlates(string registrationNumbersSeparatedList)
	{
		return (from s in _stringCollectionConverter.StringToArray(registrationNumbersSeparatedList)
			where !string.IsNullOrEmpty(s)
			select new LicensePlate
			{
				RegistrationNumber = s
			}).ToArray();
	}

	public LicensePlatePropertyDefinition[] BuildCustomFields(string[] customFields)
	{
		int displayOrder = 0;
		int columnIndex = 0;
		return customFields.Select((string s) => new LicensePlatePropertyDefinition
		{
			ColumnIndex = columnIndex++,
			DisplayOrder = displayOrder++,
			Name = s
		}).ToArray();
	}

	public FQID[] BuildTriggeredEventsIds(IGeminiRequestContext request, string[] triggeredEvents)
	{
		return _fqidConverter.ConvertPathsToFQIDs(request, triggeredEvents.Select((string s) => ConfigurationItemPath.CreatePath(s)));
	}

	private static LicensePlateInfo BuildLicensePlateInfo(string[] licensePlateRow)
	{
		int i = 0;
		return new LicensePlateInfo
		{
			LicensePlate = new LicensePlate
			{
				RegistrationNumber = licensePlateRow[0]
			},
			Properties = (from s in licensePlateRow.Skip(1)
				select new LicensePlateProperty
				{
					ColumnIndex = i++,
					Value = s
				}).ToArray()
		};
	}
}
