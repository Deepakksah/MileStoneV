using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using VideoOS.IM.Common.GlobalDefinitions.UserAccessManagement;
using VideoOS.IM.Common.GlobalDefinitions.Utilities;

namespace VideoOS.IM.Common.GlobalDefinitions.Configuration;

public sealed class IncidentProperties
{
	public class IncidentProperty : INotifyPropertyChanged
	{
		private bool _isOn;

		private string _name;

		private string _title;

		private string _description;

		private int _orderNumber;

		private int _rowNumber;

		private int _columnNumber;

		public string DynamicalPropKey { get; set; }

		public int IncidentIdentificationTypeOrderNumber { get; set; }

		public TypeOfProperty PropType { get; set; }

		public bool IsOn
		{
			get
			{
				return _isOn;
			}
			set
			{
				if (_isOn != value)
				{
					_isOn = value;
					NotifyPropertyChanged("IsOn");
				}
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name != value)
				{
					_name = value;
					NotifyPropertyChanged("Name");
				}
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (_title != value)
				{
					_title = value;
					NotifyPropertyChanged("Title");
				}
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (_description != value)
				{
					_description = value;
					NotifyPropertyChanged("Description");
				}
			}
		}

		public int OrderNumber
		{
			get
			{
				return _orderNumber;
			}
			set
			{
				if (_orderNumber != value)
				{
					_orderNumber = value;
					NotifyPropertyChanged("OrderNumber");
				}
			}
		}

		public int RowNumber
		{
			get
			{
				return _rowNumber;
			}
			set
			{
				if (_rowNumber != value)
				{
					_rowNumber = value;
					NotifyPropertyChanged("RowNumber");
				}
			}
		}

		public int ColumnNumber
		{
			get
			{
				return _columnNumber;
			}
			set
			{
				if (_columnNumber != value)
				{
					_columnNumber = value;
					NotifyPropertyChanged("ColumnNumber");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public IncidentProperty()
		{
			DynamicalPropKey = "";
			PropType = TypeOfProperty.String;
			IsOn = false;
			Name = "Property Name";
			Title = "Prop Title";
			Description = "Prop Description";
			OrderNumber = 0;
		}

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public enum TypeOfProperty
	{
		DynamicalDropDown = 10,
		Long = 20,
		String = 30,
		Bool = 40,
		Decimal = 50,
		DateTime = 60,
		Json = 70
	}

	public IncidentProperty DropDownProp1 { get; set; } = new IncidentProperty
	{
		DynamicalPropKey = DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category1,
		PropType = TypeOfProperty.DynamicalDropDown,
		IncidentIdentificationTypeOrderNumber = 1,
		Title = "Option 1",
		Name = "Extended Property 1",
		Description = "Property Description",
		IsOn = true,
		OrderNumber = 0
	};

	public IncidentProperty DropDownProp2 { get; set; } = new IncidentProperty
	{
		DynamicalPropKey = DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category2,
		PropType = TypeOfProperty.DynamicalDropDown,
		IncidentIdentificationTypeOrderNumber = 2,
		Title = "Option 2",
		Name = "Extended Property 2",
		Description = "Property Description",
		IsOn = false,
		OrderNumber = 1
	};

	public IncidentProperty DropDownProp3 { get; set; } = new IncidentProperty
	{
		DynamicalPropKey = DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category3,
		PropType = TypeOfProperty.DynamicalDropDown,
		IncidentIdentificationTypeOrderNumber = 3,
		Title = "Option 3",
		Name = "Extended Property 3",
		Description = "Property Description",
		IsOn = false,
		OrderNumber = 2
	};

	public IncidentProperty DropDownProp4 { get; set; } = new IncidentProperty
	{
		DynamicalPropKey = DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category4,
		PropType = TypeOfProperty.DynamicalDropDown,
		IncidentIdentificationTypeOrderNumber = 4,
		Title = "Option 4",
		Name = "Extended Property 4",
		Description = "Property Description",
		IsOn = false,
		OrderNumber = 3
	};

	public IncidentProperty DropDownProp5 { get; set; } = new IncidentProperty
	{
		DynamicalPropKey = DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category5,
		PropType = TypeOfProperty.DynamicalDropDown,
		IncidentIdentificationTypeOrderNumber = 5,
		Title = "Option51",
		Name = "Extended Property 5",
		Description = "Property Description",
		IsOn = false,
		OrderNumber = 4
	};

	[JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
	public ObservableCollection<IncidentProperty> Group1Options { get; set; }

	public IncidentProperty Prop_Long1 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Long,
		IncidentIdentificationTypeOrderNumber = 1,
		IsOn = true,
		Title = "Option Whole Number 1",
		OrderNumber = 1,
		Name = "Whole Number 1"
	};

	public IncidentProperty Prop_Long2 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Long,
		IncidentIdentificationTypeOrderNumber = 2,
		IsOn = false,
		Title = "Option Whole Number 2",
		OrderNumber = 2,
		Name = "Whole Number 2"
	};

	public IncidentProperty Prop_Long3 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Long,
		IncidentIdentificationTypeOrderNumber = 3,
		IsOn = false,
		Title = "Option Whole Number 3",
		OrderNumber = 3,
		Name = "Whole Number 3"
	};

	public IncidentProperty Prop_String1 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.String,
		IncidentIdentificationTypeOrderNumber = 1,
		IsOn = true,
		Title = "Option Text 1",
		OrderNumber = 4,
		Name = "Text 1"
	};

	public IncidentProperty Prop_String2 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.String,
		IncidentIdentificationTypeOrderNumber = 2,
		IsOn = false,
		Title = "Option Text 2",
		OrderNumber = 5,
		Name = "Text 2"
	};

	public IncidentProperty Prop_String3 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.String,
		IncidentIdentificationTypeOrderNumber = 3,
		IsOn = false,
		Title = "Option Text 3",
		OrderNumber = 6,
		Name = "Text 3"
	};

	public IncidentProperty Prop_Bool1 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Bool,
		IncidentIdentificationTypeOrderNumber = 1,
		IsOn = true,
		Title = "Option Check Box (True/False) 1",
		OrderNumber = 7,
		Name = "Check Box (True/False) 1"
	};

	public IncidentProperty Prop_Bool2 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Bool,
		IncidentIdentificationTypeOrderNumber = 2,
		IsOn = false,
		Title = "Option Check Box (True/False) 2",
		OrderNumber = 8,
		Name = "Check Box (True/False) 2"
	};

	public IncidentProperty Prop_Bool3 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Bool,
		IncidentIdentificationTypeOrderNumber = 3,
		IsOn = false,
		Title = "Option Check Box (True/False) 3",
		OrderNumber = 9,
		Name = "Check Box (True/False) 3"
	};

	public IncidentProperty Prop_Decimal1 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Decimal,
		IncidentIdentificationTypeOrderNumber = 1,
		IsOn = true,
		Title = "Option Decimal 1",
		OrderNumber = 10,
		Name = "Decimal 1"
	};

	public IncidentProperty Prop_Decimal2 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Decimal,
		IncidentIdentificationTypeOrderNumber = 2,
		IsOn = true,
		Title = "Option Decimal 2",
		OrderNumber = 11,
		Name = "Decimal 2"
	};

	public IncidentProperty Prop_Decimal3 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Decimal,
		IncidentIdentificationTypeOrderNumber = 3,
		IsOn = true,
		Title = "Option Decimal 3",
		OrderNumber = 12,
		Name = "Decimal 3"
	};

	public IncidentProperty Prop_DateTime1 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.DateTime,
		IncidentIdentificationTypeOrderNumber = 1,
		IsOn = true,
		Title = "Option DateTime 1",
		OrderNumber = 13,
		Name = "Date/Time 1"
	};

	public IncidentProperty Prop_DateTime2 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.DateTime,
		IncidentIdentificationTypeOrderNumber = 2,
		IsOn = true,
		Title = "Option DateTime 2",
		OrderNumber = 14,
		Name = "Date/Time 2"
	};

	public IncidentProperty Prop_DateTime3 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.DateTime,
		IncidentIdentificationTypeOrderNumber = 3,
		IsOn = true,
		Title = "Option DateTime 3",
		OrderNumber = 15,
		Name = "Date/Time 3"
	};

	public IncidentProperty Prop_Json1 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Json,
		IncidentIdentificationTypeOrderNumber = 1,
		IsOn = false,
		Title = "Option Json 1",
		OrderNumber = 16,
		Name = "JSon 1"
	};

	public IncidentProperty Prop_Json2 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Json,
		IncidentIdentificationTypeOrderNumber = 2,
		IsOn = false,
		Title = "Option Json 2",
		OrderNumber = 17,
		Name = "JSon 2"
	};

	public IncidentProperty Prop_Json3 { get; set; } = new IncidentProperty
	{
		PropType = TypeOfProperty.Json,
		IncidentIdentificationTypeOrderNumber = 3,
		IsOn = false,
		Title = "Option Json 3",
		OrderNumber = 18,
		Name = "JSon 3"
	};

	[JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
	public ObservableCollection<IncidentProperty> Group2Options { get; set; }

	public bool IncidentImagesFunctionaltyOn { get; set; }

	public string CustomTextLogo { get; set; }

	public int? DaysToKeepIncidents { get; set; }

	public DateTime? StartingIncidentRetentionTime { get; set; }

	public IncidentProperties()
	{
		IncidentImagesFunctionaltyOn = false;
		CustomTextLogo = string.Empty;
		Group1Options = new ObservableCollection<IncidentProperty> { DropDownProp1, DropDownProp2, DropDownProp3, DropDownProp4, DropDownProp5 };
		Group2Options = new ObservableCollection<IncidentProperty>();
		Group2Options.Add(Prop_Long1);
		Group2Options.Add(Prop_Long2);
		Group2Options.Add(Prop_Long3);
		Group2Options.Add(Prop_String1);
		Group2Options.Add(Prop_String2);
		Group2Options.Add(Prop_String3);
		Group2Options.Add(Prop_Bool1);
		Group2Options.Add(Prop_Bool2);
		Group2Options.Add(Prop_Bool3);
		Group2Options.Add(Prop_Decimal1);
		Group2Options.Add(Prop_Decimal2);
		Group2Options.Add(Prop_Decimal3);
		Group2Options.Add(Prop_DateTime1);
		Group2Options.Add(Prop_DateTime2);
		Group2Options.Add(Prop_DateTime3);
	}

	public void ReplacePropsWithObjectsFromList()
	{
		if (Group1Options != null && Group1Options.Count > 0)
		{
			AddDropDown(DropDownProp1, 1);
			AddDropDown(DropDownProp2, 2);
			AddDropDown(DropDownProp3, 3);
			AddDropDown(DropDownProp4, 4);
			AddDropDown(DropDownProp5, 5);
			Group1Options.RemoveAll((IncidentProperty o) => o.IncidentIdentificationTypeOrderNumber <= 0);
			Prop_Long1 = FindProperty(TypeOfProperty.Long, 1, Prop_Long1);
			Prop_Long2 = FindProperty(TypeOfProperty.Long, 2, Prop_Long2);
			Prop_Long3 = FindProperty(TypeOfProperty.Long, 3, Prop_Long3);
			Prop_String1 = FindProperty(TypeOfProperty.String, 1, Prop_String1);
			Prop_String2 = FindProperty(TypeOfProperty.String, 2, Prop_String2);
			Prop_String3 = FindProperty(TypeOfProperty.String, 3, Prop_String3);
			Prop_Bool1 = FindProperty(TypeOfProperty.Bool, 1, Prop_Bool1);
			Prop_Bool2 = FindProperty(TypeOfProperty.Bool, 2, Prop_Bool2);
			Prop_Bool3 = FindProperty(TypeOfProperty.Bool, 3, Prop_Bool3);
			Prop_Decimal1 = FindProperty(TypeOfProperty.Decimal, 1, Prop_Decimal1);
			Prop_Decimal2 = FindProperty(TypeOfProperty.Decimal, 2, Prop_Decimal2);
			Prop_Decimal3 = FindProperty(TypeOfProperty.Decimal, 3, Prop_Decimal3);
			Prop_DateTime1 = FindProperty(TypeOfProperty.DateTime, 1, Prop_DateTime1);
			Prop_DateTime2 = FindProperty(TypeOfProperty.DateTime, 2, Prop_DateTime2);
			Prop_DateTime3 = FindProperty(TypeOfProperty.DateTime, 3, Prop_DateTime3);
			Prop_Json1 = FindProperty(TypeOfProperty.Json, 1, Prop_Json1);
			Prop_Json2 = FindProperty(TypeOfProperty.Json, 2, Prop_Json2);
			Prop_Json3 = FindProperty(TypeOfProperty.Json, 3, Prop_Json3);
		}
	}

	private void AddDropDown(IncidentProperty dropDownProp, int incidentIdentificationTypeOrderNumber)
	{
		IncidentProperty incidentProperty = Group1Options.FirstOrDefault((IncidentProperty o) => o.PropType == TypeOfProperty.DynamicalDropDown && o.IncidentIdentificationTypeOrderNumber == incidentIdentificationTypeOrderNumber);
		if (incidentProperty != null)
		{
			dropDownProp = incidentProperty;
			return;
		}
		if (dropDownProp != null)
		{
			dropDownProp.IncidentIdentificationTypeOrderNumber = incidentIdentificationTypeOrderNumber;
		}
		Group1Options.Add(dropDownProp);
	}

	private IncidentProperty FindProperty(TypeOfProperty propertyType, int incidentIdentificationTypeOrderNumber, IncidentProperty defaultValue)
	{
		return Group2Options.FirstOrDefault((IncidentProperty o) => o.PropType == propertyType && o.IncidentIdentificationTypeOrderNumber == incidentIdentificationTypeOrderNumber) ?? defaultValue;
	}
}
