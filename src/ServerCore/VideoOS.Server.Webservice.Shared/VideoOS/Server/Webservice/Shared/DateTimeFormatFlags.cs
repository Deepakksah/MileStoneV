using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Server.Webservice.Shared;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[Flags]
[DataContract(Name = "DateTimeFormatFlags", Namespace = "http://schemas.datacontract.org/2004/07/System.Globalization")]
public enum DateTimeFormatFlags
{
	[EnumMember]
	None = 0,
	[EnumMember]
	UseGenitiveMonth = 1,
	[EnumMember]
	UseLeapYearMonth = 2,
	[EnumMember]
	UseSpacesInMonthNames = 4,
	[EnumMember]
	UseHebrewRule = 8,
	[EnumMember]
	UseSpacesInDayNames = 0x10,
	[EnumMember]
	UseDigitPrefixInTokens = 0x20,
	[EnumMember]
	NotInitialized = -1
}
