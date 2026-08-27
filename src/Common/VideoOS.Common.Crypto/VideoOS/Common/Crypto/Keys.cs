using System;

namespace VideoOS.Common.Crypto;

public static class Keys
{
	private const string ObsoleteMessage = "Use VideoOS.Licensing.Keys for license public keys or OemResourcePublicKey for OEM key instead.";

	[Obsolete("Use VideoOS.Licensing.Keys for license public keys or OemResourcePublicKey for OEM key instead.")]
	public static string CompanyPublicKey => "<RSAKeyValue><Modulus>zmwboAV9rGYf4pa2NW/Tfydh62vDm2YQcekVHPfph8n/Range44GtzFOr7gsSiSJ65ro1IwPJLisroXcF/Eg9RRxsPmh8v9gn6+OqEvDhITp07shmv5N2B1Do921ZnknjDBjxo7Fnki+upIJhScsbNyylF0kUkDMljXOIjis3A8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

	public static string OemResourcePublicKey => "<RSAKeyValue><Modulus>jyGlhWrJ7LCBwqZeD/KEnra5WWQ00JCvP5vdQuAsunWRbP+rJkn2BG4Rr+QtItw3f3fTZ3Ls+jiVgUNQM6Nmg77KsLEcUUFKkVHfYd6cOVlRCGl/R5FLWUcFoVA9ZbTdWltOncrFfAT8OJsbP+9m/1D0fg7JZWNY6CR6G4K1fm3YJ4fuLGTzb5in6+JFWzGKDYziDERSrOwAPE2DKFIWRrj9gIWQxZccNwVcMOrqW1jW2i6A4Cstv63K4Pn01UnrGUO7Q02SVc/colX46y7Bz+S4Eci9/MjxUZ5A7BDBA1otjh3N0qSJyGZhqMTgzBhZjjsCwh0HtulsHsr0K22oCNNTFhAjRtlLWGahD++7yXdmGU5mHpWNG2+hHo/IU8dXfbGxHe/0nGLKsG7pqY/1opODn4NezkzibC+PTlRYOtICChBtw2/1zPclupfv78EaSy443eKfM+TNYna+UMKJhrkIqO6cnudqcY4fhj0cvRZ8JCYgmeunB2ur0UkZGU6LOdHUQCJXCARvQR6gOZUO9Z2MhYNBDg6hPr0vSS6eyilRBIFSmQBu5zikzEdMb09o/WnNkYF49RRQCEBLTBfNyqpHxbPsHN1NQk9YwujwIJl+ju2h3f1qhZFWvYbjEg6jb6SxzSPOwwJo6pRcIFXdC4z6YF0FAatFRxt4vk92p9s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

	public static string SiteSpecificPrivatePublicKey { get; set; }
}
