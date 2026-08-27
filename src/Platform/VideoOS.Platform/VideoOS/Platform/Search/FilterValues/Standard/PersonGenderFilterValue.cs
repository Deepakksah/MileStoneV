using System.Collections.Generic;

namespace VideoOS.Platform.Search.FilterValues.Standard;

public class PersonGenderFilterValue
{
	public enum Gender
	{
		Female,
		Male
	}

	public IEnumerable<Gender> Genders { get; public set; }
}
