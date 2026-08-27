using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.MatchLists;

public class LicensePlateCsvParser
{
	private readonly List<LicensePlatePropertyDefinition> _propertyDefinitions;

	private readonly IDictionary<int, int> _displayOrderToIndexMap;

	private const string _defaultRecordDelimiter = ",";

	public string RecordDelimiter { get; set; }

	public LicensePlateCsvParser(IEnumerable<LicensePlatePropertyDefinition> propertyDefinitions)
	{
		if (propertyDefinitions == null)
		{
			throw new ArgumentNullException("propertyDefinitions");
		}
		_propertyDefinitions = propertyDefinitions.ToList();
		_displayOrderToIndexMap = _propertyDefinitions.ToDictionary((LicensePlatePropertyDefinition pd) => pd.DisplayOrder, (LicensePlatePropertyDefinition pd) => pd.ColumnIndex);
	}

	public IEnumerable<LicensePlateInfo> ReadAll(TextReader textReader)
	{
		if (textReader == null)
		{
			throw new ArgumentNullException("textReader");
		}
		if (string.IsNullOrEmpty(RecordDelimiter))
		{
			RecordDelimiter = DetermineRecordDelimiter(textReader);
		}
		using CsvReader csvReader = new CsvReader(textReader, CultureInfo.CurrentCulture);
		csvReader.Configuration.HasHeaderRecord = false;
		csvReader.Configuration.Delimiter = RecordDelimiter;
		while (csvReader.Read())
		{
			string text = ReadRegistrationNumber(csvReader);
			if (text.Length <= 20)
			{
				LicensePlateProperty[] customFields = ReadLicensePlateProperties(csvReader);
				yield return CreateLicensePlateInfo(text, customFields);
			}
		}
	}

	private static string DetermineRecordDelimiter(TextReader textReader)
	{
		if (textReader is StreamReader streamReader)
		{
			string source = streamReader.ReadLine();
			streamReader.BaseStream.Seek(0L, SeekOrigin.Begin);
			streamReader.DiscardBufferedData();
			char c = source.FirstOrDefault((char c2) => c2 == ',' || c2 == ';');
			if (c != 0)
			{
				return c.ToString();
			}
		}
		return ",";
	}

	private static string ReadRegistrationNumber(CsvReader csvReader)
	{
		return csvReader.GetField<string>(0);
	}

	private LicensePlateProperty[] ReadLicensePlateProperties(CsvReader csvReader)
	{
		LicensePlateProperty[] array = new LicensePlateProperty[_propertyDefinitions.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new LicensePlateProperty();
		}
		foreach (KeyValuePair<int, int> item in _displayOrderToIndexMap)
		{
			LicensePlateProperty licensePlateProperty = array[item.Key];
			licensePlateProperty.ColumnIndex = item.Value;
			if (csvReader.TryGetField(item.Key + 1, out string field))
			{
				licensePlateProperty.Value = field;
			}
		}
		Array.Sort(array, (LicensePlateProperty p1, LicensePlateProperty p2) => p1.ColumnIndex.CompareTo(p2.ColumnIndex));
		return array;
	}

	private static LicensePlateInfo CreateLicensePlateInfo(string registrationNumber, LicensePlateProperty[] customFields)
	{
		return new LicensePlateInfo
		{
			LicensePlate = new LicensePlate
			{
				RegistrationNumber = registrationNumber
			},
			Properties = customFields
		};
	}
}
