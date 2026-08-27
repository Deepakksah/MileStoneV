using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VideoOS.LPR.Plugin.ClientResources;

namespace VideoOS.LPR.Common.VehicleProperties;

public static class Constants
{
	public enum VehiclePropertyCategory
	{
		Make,
		Model,
		Type,
		Color,
		Side
	}

	public static readonly Dictionary<string, string> CommonVehicleProperties = new Dictionary<string, string>
	{
		{
			"UNKNOWN",
			ClientStrings.VehiclePropertiesGeneric_Unknown
		},
		{
			"UNDETECTABLE",
			ClientStrings.VehiclePropertiesGeneric_Undetectable
		}
	};

	public static readonly Dictionary<string, string> VehicleTypes = new Dictionary<string, string>
	{
		{
			"BUS",
			ClientStrings.VehiclePropertiesType_Bus
		},
		{
			"PASSENGER_CAR",
			ClientStrings.VehiclePropertiesType_PassengerCar
		},
		{
			"MOTORCYCLE",
			ClientStrings.VehiclePropertiesType_Motorcycle
		},
		{
			"TRUCK",
			ClientStrings.VehiclePropertiesType_Truck
		},
		{
			"VAN",
			ClientStrings.VehiclePropertiesType_Van
		}
	};

	public static readonly Dictionary<string, string> VehicleMakes = new Dictionary<string, string>
	{
		{ "ALFA_ROMEO", "Alfa Romeo" },
		{ "AUDI", "Audi" },
		{ "BMW", "BMW" },
		{ "BURG", "Burg" },
		{ "CHEVROLET", "Chevrolet" },
		{ "CHRYSLER", "Chrysler" },
		{ "CITROEN", "Citroen" },
		{ "DACIA", "Dacia" },
		{ "DAEWOO", "Daewoo" },
		{ "DAF", "DAF" },
		{ "DAIHATSU", "Daihatsu" },
		{ "DODGE", "Dodge" },
		{ "FIAT", "Fiat" },
		{ "FLOOR", "Floor" },
		{ "FORD", "Ford" },
		{ "GROENEWEGEN", "Groenewegen" },
		{ "H.T.F", "H.T.F" },
		{ "HAPERT", "Hapert" },
		{ "HONDA", "Honda" },
		{ "HYUNDAI", "Hyundai" },
		{ "IVECO", "Iveco" },
		{ "JAGUAR", "Jaguar" },
		{ "JEEP", "Jeep" },
		{ "KIA", "Kia" },
		{ "KRONE", "Krone" },
		{ "LAG", "LAG" },
		{ "LANCIA", "Lancia" },
		{ "LAND_ROVER", "Land Rover" },
		{ "LEXUS", "Lexus" },
		{ "MAN", "MAN" },
		{ "MAZDA", "Mazda" },
		{ "MERCEDES_BENZ", "Mercedes-Benz" },
		{ "MINI", "Mini" },
		{ "MITSUBISHI", "Mitsubishi" },
		{ "NISSAN", "Nissan" },
		{ "OPEL", "Opel" },
		{ "PACTON", "Pacton" },
		{ "PEUGEOT", "Peugeot" },
		{ "PORSCHE", "Porsche" },
		{ "RENAULT", "Renault" },
		{ "SAAB", "Saab" },
		{ "SCANIA", "Scania" },
		{ "SCHMITZ", "Schmitz" },
		{ "SCHMITZ_CARGOBULL", "Schmitz Cargobull" },
		{ "SEAT", "Seat" },
		{ "SKODA", "Skoda" },
		{ "SMART", "Smart" },
		{ "SUBARU", "Subaru" },
		{ "SUZUKI", "Suzuki" },
		{ "TESLA", "Tesla" },
		{ "TOYOTA", "Toyota" },
		{ "VAN_ECK", "Van Eck" },
		{ "VAN_HOOL", "Van Hool" },
		{ "VOLKSWAGEN", "Volkswagen" },
		{ "VOLVO", "Volvo" }
	};

	public static readonly Dictionary<string, string> VehicleModels = new Dictionary<string, string>
	{
		{ "ALFA_ROMEO_147", "Alfa Romeo - 147" },
		{ "ALFA_ROMEO_ALFA_GIULIETTA", "Alfa Romeo - Alfa Giulietta" },
		{ "ALFA_ROMEO_MITO", "Alfa Romeo - Mito" },
		{ "AUDI_A1", "Audi - A1" },
		{ "AUDI_A3", "Audi - A3" },
		{ "AUDI_A4", "Audi - A4" },
		{ "AUDI_A5", "Audi - A5" },
		{ "AUDI_A6", "Audi - A6" },
		{ "AUDI_TT", "Audi - TT" },
		{ "BMW_1", "BMW - 1" },
		{ "BMW_116I", "BMW - 116i" },
		{ "BMW_3", "BMW - 3" },
		{ "BMW_320ED", "BMW - 320ed" },
		{ "BMW_320I", "BMW - 320i" },
		{ "BMW_5", "BMW - 5" },
		{ "BMW_520D", "BMW - 520d" },
		{ "BMW_X", "BMW - X" },
		{ "CHEVROLET_SPARK", "Chevrolet - Spark" },
		{ "CITROEN_BERLINGO", "Citroen - Berlingo" },
		{ "CITROEN_C1", "Citroen - C1" },
		{ "CITROEN_C2", "Citroen - C2" },
		{ "CITROEN_C3", "Citroen - C3" },
		{ "CITROEN_C4", "Citroen - C4" },
		{ "CITROEN_C5", "Citroen - C5" },
		{ "CITROEN_DS3", "Citroen - DS3" },
		{ "CITROEN_XSARA_PICASSO", "Citroen - Xsara Picasso" },
		{ "DACIA_LOGAN", "Dacia - Logan" },
		{ "DACIA_SANDERO", "Dacia - Sandero" },
		{ "DAF_CF_400_FT", "DAF - CF 400 FT" },
		{ "DAF_FAR_XF105", "DAF - FAR XF105" },
		{ "DAF_FTG_XF105", "DAF - FTG XF105" },
		{ "DAF_FTP_XF105", "DAF - FTP XF105" },
		{ "DAF_FT_CF85", "DAF - FT CF85" },
		{ "DAF_FT_XF", "DAF - FT XF" },
		{ "DAF_FT_XF105", "DAF - FT XF105" },
		{ "DAF_FT_XF_105", "DAF - FT XF 105" },
		{ "DAF_XF_440_FT", "DAF - XF 440 FT" },
		{ "DAF_XF_460_FT", "DAF - XF 460 FT" },
		{ "DAIHATSU_CUORE", "Daihatsu - Cuore" },
		{ "DAIHATSU_SIRION", "Daihatsu - Sirion" },
		{ "FIAT_500", "Fiat - 500" },
		{ "FIAT_DOBLO", "Fiat - Doblo" },
		{ "FIAT_DUCATO", "Fiat - Ducato" },
		{ "FIAT_PANDA", "Fiat - Panda" },
		{ "FIAT_PUNTO", "Fiat - Punto" },
		{ "FORD_C_MAX", "Ford - C-Max" },
		{ "FORD_FIESTA", "Ford - Fiesta" },
		{ "FORD_FOCUS", "Ford - Focus" },
		{ "FORD_FOCUS_C_MAX", "Ford - Focus C-Max" },
		{ "FORD_FUSION", "Ford - Fusion" },
		{ "FORD_GALAXY", "Ford - Galaxy" },
		{ "FORD_KA", "Ford - Ka" },
		{ "FORD_MONDEO", "Ford - Mondeo" },
		{ "FORD_S_MAX", "Ford - S-Max" },
		{ "FORD_TRANSIT", "Ford - Transit" },
		{ "HONDA_CIVIC", "Honda - Civic" },
		{ "HONDA_INSIGHT", "Honda - Insight" },
		{ "HONDA_JAZZ", "Honda - Jazz" },
		{ "HYUNDAI_GETZ", "Hyundai - Getz" },
		{ "HYUNDAI_I20", "Hyundai - i20" },
		{ "HYUNDAI_IX35", "Hyundai - ix35" },
		{ "HYUNDAI_I_10", "Hyundai - i10" },
		{ "HYUNDAI_I_30", "Hyundai - i30" },
		{ "HYUNDAI_TUCSON", "Hyundai - Tucson" },
		{ "IVECO_AS440T_P", "Iveco - AS440T/P" },
		{ "KIA_CEE_D", "Kia - Cee'd" },
		{ "KIA_PICANTO", "Kia - Picanto" },
		{ "KIA_RIO", "Kia - Rio" },
		{ "KIA_SPORTAGE", "Kia - Sportage" },
		{ "KRONE_N_A", "Krone - N A" },
		{ "KRONE_SD", "Krone - SD" },
		{ "LEXUS_LEXUS_CT200H", "Lexus - CT200h" },
		{ "MAN_TGS", "MAN - TGS" },
		{ "MAN_TGX", "MAN - TGX" },
		{ "MAZDA_2", "Mazda - 2" },
		{ "MAZDA_3", "Mazda - 3" },
		{ "MAZDA_6", "Mazda - 6" },
		{ "MAZDA_CX_5", "Mazda - CX-5" },
		{ "MERCEDES_BENZ_639_VITO_109_CDI", "Mercedes-Benz - 639 Vito 109 CDI" },
		{ "MERCEDES_BENZ_A", "Mercedes-Benz - A" },
		{ "MERCEDES_BENZ_ACTROS", "Mercedes-Benz - Actros" },
		{ "MERCEDES_BENZ_B", "Mercedes-Benz - B" },
		{ "MERCEDES_BENZ_C", "Mercedes-Benz - C" },
		{ "MERCEDES_BENZ_CITAN", "Mercedes-Benz - Citan" },
		{ "MERCEDES_BENZ_E", "Mercedes-Benz - E" },
		{ "MERCEDES_BENZ_SPRINTER", "Mercedes-Benz - Sprinter" },
		{ "MERCEDES_BENZ_VITO", "Mercedes-Benz - Vito" },
		{ "MINI_COOPER", "Mini - Cooper" },
		{ "MINI_MINI", "Mini - Mini" },
		{ "MITSUBISHI_COLT", "Mitsubishi - Colt" },
		{ "MITSUBISHI_OUTLANDER", "Mitsubishi - Outlander" },
		{ "MITSUBISHI_SPACE_STAR", "Mitsubishi - Space Star" },
		{ "NISSAN_MICRA", "Nissan - Micra" },
		{ "NISSAN_NOTE", "Nissan - Note" },
		{ "NISSAN_QASHQAI", "Nissan - Qashqai" },
		{ "OPEL_AGILA", "Opel - Agila" },
		{ "OPEL_AMPERA", "Opel - Ampera" },
		{ "OPEL_ASTRA", "Opel - Astra" },
		{ "OPEL_COMBO_VAN", "Opel - Combo Van" },
		{ "OPEL_CORSA", "Opel - Corsa" },
		{ "OPEL_INSIGNIA_SPORTS_TOURER_SW", "Opel - Insignia Sports Tourer SW" },
		{ "OPEL_MERIVA", "Opel - Meriva" },
		{ "OPEL_MOVANO", "Opel - Movano" },
		{ "OPEL_VIVARO", "Opel - Vivaro" },
		{ "OPEL_ZAFIRA", "Opel - Zafira" },
		{ "PEUGEOT_107", "Peugeot - 107" },
		{ "PEUGEOT_2008", "Peugeot - 2008" },
		{ "PEUGEOT_206", "Peugeot - 206" },
		{ "PEUGEOT_207", "Peugeot - 207" },
		{ "PEUGEOT_208", "Peugeot - 208" },
		{ "PEUGEOT_3008", "Peugeot - 3008" },
		{ "PEUGEOT_307", "Peugeot - 307" },
		{ "PEUGEOT_308", "Peugeot - 308" },
		{ "PEUGEOT_407", "Peugeot - 407" },
		{ "PEUGEOT_5008", "Peugeot - 5008" },
		{ "PEUGEOT_508", "Peugeot - 508" },
		{ "PEUGEOT_EXPERT", "Peugeot - Expert" },
		{ "PEUGEOT_PARTNER", "Peugeot - Partner" },
		{ "RENAULT_CAPTUR", "Renault - Captur" },
		{ "RENAULT_CLIO", "Renault - Clio" },
		{ "RENAULT_ESPACE", "Renault - Espace" },
		{ "RENAULT_KANGOO", "Renault - Kangoo" },
		{ "RENAULT_LAGUNA", "Renault - Laguna" },
		{ "RENAULT_MASTER", "Renault - Master" },
		{ "RENAULT_MEGANE", "Renault - Megane" },
		{ "RENAULT_MEGANE_SCENIC", "Renault - Megane Scenic" },
		{ "RENAULT_MODUS", "Renault - Modus" },
		{ "RENAULT_T", "Renault - T" },
		{ "RENAULT_TRAFIC", "Renault - Trafic" },
		{ "RENAULT_TWINGO", "Renault - Twingo" },
		{ "SAAB_9_3", "Saab - 9-3" },
		{ "SAAB_9_5", "Saab - 9-5" },
		{ "SCANIA_R410", "Scania - R410" },
		{ "SCANIA_R450", "Scania - R450" },
		{ "SCANIA_R_420_A_4X2", "Scania - R 420 A 4x2" },
		{ "SCANIA_R_440_A_4X2", "Scania - R 440 A 4x2" },
		{ "SCHMITZ_UNKNWON", "Schmitz - Unknown" },
		{ "SEAT_ALTEA", "Seat - Altea" },
		{ "SEAT_IBIZA", "Seat - Ibiza" },
		{ "SEAT_LEON", "Seat - Leon" },
		{ "SEAT_MII", "Seat - Mii" },
		{ "SKODA_FABIA", "Skoda - Fabia" },
		{ "SKODA_OCTAVIA", "Skoda - Octavia" },
		{ "SKODA_SUPERB", "Skoda - Superb" },
		{ "SUZUKI_ALTO", "Suzuki - Alto" },
		{ "SUZUKI_SWIFT", "Suzuki - Swift" },
		{ "SUZUKI_SX4", "Suzuki - SX4" },
		{ "TOYOTA_AURIS", "Toyota - Auris" },
		{ "TOYOTA_AVENSIS", "Toyota - Avensis" },
		{ "TOYOTA_AYGO", "Toyota - Aygo" },
		{ "TOYOTA_COROLLA", "Toyota - Corolla" },
		{ "TOYOTA_PRIUS", "Toyota - Prius" },
		{ "TOYOTA_RAV4", "Toyota - RAV4" },
		{ "TOYOTA_YARIS", "Toyota - Yaris" },
		{ "VOLKSWAGEN_CADDY", "Volkswagen - Caddy" },
		{ "VOLKSWAGEN_CRAFTER", "Volkswagen - Crafter" },
		{ "VOLKSWAGEN_FOX", "Volkswagen - Fox" },
		{ "VOLKSWAGEN_GOLF", "Volkswagen - Golf" },
		{ "VOLKSWAGEN_JETTA", "Volkswagen - Jetta" },
		{ "VOLKSWAGEN_LUPO", "Volkswagen - Lupo" },
		{ "VOLKSWAGEN_PASSAT", "Volkswagen - Passat" },
		{ "VOLKSWAGEN_POLO", "Volkswagen - Polo" },
		{ "VOLKSWAGEN_TIGUAN", "Volkswagen - Tiguan" },
		{ "VOLKSWAGEN_TOURAN", "Volkswagen - Touran" },
		{ "VOLKSWAGEN_TRANSPORTER", "Volkswagen - Transporter" },
		{ "VOLKSWAGEN_UP", "Volkswagen - Up" },
		{ "VOLVO_C30", "Volvo - C30" },
		{ "VOLVO_FH", "Volvo - FH" },
		{ "VOLVO_FM", "Volvo - FM" },
		{ "VOLVO_S", "Volvo - S" },
		{ "VOLVO_V", "Volvo - V" },
		{ "VOLVO_XC", "Volvo - XC" }
	};

	public static readonly Dictionary<string, string> VehicleColors = new Dictionary<string, string>
	{
		{
			"BLACK",
			ClientStrings.VehiclePropertiesColor_Black
		},
		{
			"BLUE",
			ClientStrings.VehiclePropertiesColor_Blue
		},
		{
			"GRAY",
			ClientStrings.VehiclePropertiesColor_Gray
		},
		{
			"GREEN",
			ClientStrings.VehiclePropertiesColor_Green
		},
		{
			"RED",
			ClientStrings.VehiclePropertiesColor_Red
		},
		{
			"WHITE",
			ClientStrings.VehiclePropertiesColor_White
		},
		{
			"YELLOW",
			ClientStrings.VehiclePropertiesColor_Yellow
		}
	};

	public static readonly Dictionary<string, string> VehicleSides = new Dictionary<string, string>
	{
		{
			"FRONT",
			ClientStrings.VehiclePropertiesSide_Front
		},
		{
			"REAR",
			ClientStrings.VehiclePropertiesSide_Rear
		}
	};

	public static string GetVehiclePropertyValue(string vehiclePropertyKey, VehiclePropertyCategory vehiclePropertyType, bool ignoreMakeInModel = true)
	{
		if (vehiclePropertyKey == null)
		{
			return ClientStrings.VehiclePropertiesNotAvailableShort;
		}
		Dictionary<string, string> dictionary = GetDictionary(vehiclePropertyType);
		if (CommonVehicleProperties.TryGetValue(vehiclePropertyKey, out var value))
		{
			return value;
		}
		if (dictionary.TryGetValue(vehiclePropertyKey, out var value2))
		{
			if (vehiclePropertyType == VehiclePropertyCategory.Model && ignoreMakeInModel)
			{
				return GetOnlyModelValue(value2);
			}
			return value2;
		}
		return ClientStrings.VehiclePropertiesNotAvailableShort;
	}

	public static Dictionary<string, string> GetDictionaryWithCommonVehicleProperties(VehiclePropertyCategory vehiclePropertyType)
	{
		Dictionary<string, string> result = null;
		switch (vehiclePropertyType)
		{
		case VehiclePropertyCategory.Make:
			result = VehicleMakes.Concat(CommonVehicleProperties).ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
			break;
		case VehiclePropertyCategory.Model:
			result = VehicleModels.Concat(CommonVehicleProperties).ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
			break;
		case VehiclePropertyCategory.Type:
			result = VehicleTypes.Concat(CommonVehicleProperties).ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
			break;
		case VehiclePropertyCategory.Color:
			result = VehicleColors.Concat(CommonVehicleProperties).ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
			break;
		case VehiclePropertyCategory.Side:
			result = VehicleSides.Concat(CommonVehicleProperties).ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
			break;
		}
		return result;
	}

	private static Dictionary<string, string> GetDictionary(VehiclePropertyCategory vehiclePropertyType)
	{
		Dictionary<string, string> result = null;
		switch (vehiclePropertyType)
		{
		case VehiclePropertyCategory.Make:
			result = VehicleMakes;
			break;
		case VehiclePropertyCategory.Model:
			result = VehicleModels;
			break;
		case VehiclePropertyCategory.Type:
			result = VehicleTypes;
			break;
		case VehiclePropertyCategory.Color:
			result = VehicleColors;
			break;
		case VehiclePropertyCategory.Side:
			result = VehicleSides;
			break;
		}
		return result;
	}

	private static string GetOnlyModelValue(string modelValue)
	{
		return Regex.Split(modelValue, " - ").LastOrDefault();
	}
}
