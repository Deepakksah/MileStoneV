using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using VideoOS.Common.SharedComponent.Data.ViewLayout;

namespace VideoOS.Common.SharedComponent.Data;

[Serializable]
public class KnownViewLayouts : Dictionary<string, ViewLayoutData>
{
	public static string LayoutGroup4x3 = "7EB5B9CC-D56F-4785-BA72-4E71FADA8C6B";

	public static string LayoutGroup16x9 = "14C3A703-6835-4843-B18C-8DB95436A80C";

	public static string LayoutGroupCustom = "9ABA35C0-6AE1-446f-8CCF-25B2208E14C9";

	private void AddItem(ref VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout layout, int x, int y, int width, int height)
	{
		ViewItem viewItem = new ViewItem();
		viewItem.Position = new Point(x, y);
		viewItem.Size = new Size(width, height);
		layout.ViewItems.Add(viewItem);
	}

	private void GenerateAndAddItems(ref VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout layout, int width, int height)
	{
		_ = new Rectangle[width * height];
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				AddItem(ref layout, 1000 / width * j, 1000 / height * i, 1000 / width, 1000 / height);
			}
		}
	}

	public KnownViewLayouts()
	{
		VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1x1",
			ViewItems = new List<ViewItem>(),
			BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAOxJREFUOE+lk9EKgkAQRf2n/qlfjkAKg6KCykzTrGbasza17UNBKwzrg/fec0fNstRrNBprymSIRUTvoq+Te5vLTZVp3TT9MFUvWnbqg38aYPS3AQlGYukkM7tOvhPYXsygvg7Ye8St6vb8pYL1xoRkTKw3BojXTUSAiGWGS8OgcgaIfLLDRrysh/lYYmhwvolP5gHDtmQvdulFHREggOD9qgYDhJtW/CDEAPGsighCA/qGH5chcy7czCvR/BgR0BUCztJ1NWQ6r569LTk/qk7LiICuGFhnhLYwUouTemySEU8OAUHSv5D6MybrH5/6pi+poiTGAAAAAElFTkSuQmCC")
		};
		AddItem(ref layout, 0, 0, 1000, 1000);
		ViewLayoutGroup viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		ViewLayoutData value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		string key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1p3Wide"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup16x9)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAATVJREFUOE+lk+tOwzAUg8cz8bI8GNIkhBAMNgZovd/SbcfYSRpB4V8quZOmfrbPSXsDYJN33d5hkyPB68v4x5W3MzVfgekCjGdW5bPNDFTOvHzw2sAIeZjgLJAaCHfRoHJAMQX9MVDyJSY7uii5F8zUmqCA02T4GuH1y0CpglXZLcn87ThHQ7hcEtf7ksvP2o4GS+2Wyc1sKGlwigbH3rCPSg3SsiLcM1lwTQXY8Bkr73tg1wYlg6X2wBk0szatmQuCShb8TlDArjM8N0HJYFIyQcEtVSmZoBb1MRph1u5i4n87GDiDNt1yXn9MPOMAAwcm+9rR4LE2PFTwSg10xv4FiZU9PBCm3gi+sPZTEwCB2zIoGQQ4vByCj4KZ+iqYyxKsZAHb0nBfwCsZZH0L+hpzlAUr+BsFrvgedUxPxAAAAABJRU5ErkJggg==");
		AddItem(ref layout, 0, 0, 750, 1000);
		AddItem(ref layout, 750, 0, 250, 333);
		AddItem(ref layout, 750, 333, 250, 333);
		AddItem(ref layout, 750, 666, 250, 333);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1p5"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAANhJREFUOE+tk4EKgkAQRP0n/6lfjiCKoqKCysTMzHa757lyiRSkwnAivtnZQaNo6BXHEx2iCFhE9CXantybHpUq4r2s1FppKZoU/tlPg6cz+9sA2JJYAiajUyHfE1j00KCvq94VDM7d7uyMCfDZTUbHXHV/6yQAoswQNgMgDIgNvM28PkoMDfJKmrb9vmZg8MZNX2VBglG+A3ZlBU4MrenDXdq4xF47LVPR+bXTQdoYcFphREY7J54Re5Gqg1VnSacDJpLAvjCDbSoGHpYanl7G6mDozziYfwP5ZXpeZC+xYwAAAABJRU5ErkJggg==");
		AddItem(ref layout, 0, 0, 666, 666);
		AddItem(ref layout, 666, 0, 333, 333);
		AddItem(ref layout, 666, 333, 333, 333);
		AddItem(ref layout, 0, 666, 333, 333);
		AddItem(ref layout, 333, 666, 333, 333);
		AddItem(ref layout, 666, 666, 333, 333);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1p7"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAANxJREFUOE+tk/0KgkAQxH0n3ykfOYIIIowKLDO/MttpR9njEizoEhbvn5n9zZxGUegTxwuETESxiOApcG+ebe49wKl1qg7DsqIT5O14/mrwULOfDSg2Em4fCRIvcjJPYOhmUKqbYWcNcKo/RJjmpomfm+JDJe8dUMQyfTGLKxT7rIVxspaimQi+Qd3LkJdiYpuBYae6fVtOCCgggZVFsW0/NgIOhbw6ijeF18FfPiRuIwHNcs1KbEPea2lpCYe9vgKrfHILzGoGJjaDHcU3OGyKlxevg6AIoT9jsP4FksJ45fF/i0gAAAAASUVORK5CYII=");
		AddItem(ref layout, 0, 0, 750, 750);
		AddItem(ref layout, 750, 0, 250, 250);
		AddItem(ref layout, 750, 250, 250, 250);
		AddItem(ref layout, 750, 500, 250, 250);
		AddItem(ref layout, 0, 750, 250, 250);
		AddItem(ref layout, 250, 750, 250, 250);
		AddItem(ref layout, 500, 750, 250, 250);
		AddItem(ref layout, 750, 750, 250, 250);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1p8Wide"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup16x9)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAANhJREFUOE/Fk+sKgkAQhX0n3ykfOQIJukJBpeattJn27DQ0qYHgD4WDu+h3Zs6sBsHsVxgueIoCwETEL2Iv7HVt70Ujz5KavS41+f1fg0crhqq8EeAKuGI+l2LoDKJeBAsBVPWjRt8OtCJewrpyyp4iVIXwDJUPuegngjUoW2JkBoy81gDgviDe5p0ZANAhKqzVTxUxhIowALzOTAdDcxh3tGYGqKYdJO6INDOMjp/cWKNynDKvks4MkFUNAOsxAdq5tjd3AeKUPLy8mRmMa3f4i539V5rewBuIffGYxWKF2wAAAABJRU5ErkJggg==");
		AddItem(ref layout, 0, 0, 250, 333);
		AddItem(ref layout, 250, 0, 500, 666);
		AddItem(ref layout, 750, 0, 250, 333);
		AddItem(ref layout, 0, 333, 250, 333);
		AddItem(ref layout, 750, 333, 250, 333);
		AddItem(ref layout, 0, 666, 250, 333);
		AddItem(ref layout, 250, 666, 250, 333);
		AddItem(ref layout, 500, 666, 250, 333);
		AddItem(ref layout, 750, 666, 250, 333);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout2p4Wide"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup16x9)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAANdJREFUOE/Fk9sKwjAQRPtP/Sf9ZBFE8AoKaq29aUvWnMRtY9WX9sHCkDRhZmcnSRT9/YvjiYxBBNkYYyEfUGH28lokq40klTicKuMK/xS4N9I6QwAyOEMuRY6F37cC06/VIYUO+IcUAm7rgIraBvMycEBVoORdJgLeWlABxqIxrmd1EApA3OZG1lkvAwgaFvP00QkcSiMABwhAXqaBA3oZdpRBBloRF4k9Imyr6P5VeXH1a4zzpJcBZ+vvRBeYCmys7dVNHJE1yLNLkMEw+/4G//0pjTfwBNx//nNsuThGAAAAAElFTkSuQmCC");
		AddItem(ref layout, 0, 0, 500, 666);
		AddItem(ref layout, 500, 0, 500, 666);
		AddItem(ref layout, 0, 666, 250, 333);
		AddItem(ref layout, 250, 666, 250, 333);
		AddItem(ref layout, 500, 666, 250, 333);
		AddItem(ref layout, 750, 666, 250, 333);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout2p8"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAMhJREFUOE/FUtEKwjAM7D/1n/STRRBhyEQFtc6tm2Mm9lZio6gPm2jhWNtxubukxoxd1k54DAzIRMRX4vsXe0AKXzpmH1C18b5oiV0T//+mAFShDmVgX3MPa6fvHcC2RBDbmrzzHyKA/KoAckMZ5E1FqQfDp6AiaEXfUZ9ZCh+CMiDnPKhnpXKAMWpCbFgibGtiQAqAvCxUD4ZHUO9AK7qGHiyvQ9PyMjlanJjn7mkK8rLgBnllTDivAjk7pwIgz45fmUKI8Pd1AxlLOMVxFicjAAAAAElFTkSuQmCC");
		AddItem(ref layout, 0, 0, 500, 500);
		AddItem(ref layout, 500, 0, 500, 500);
		AddItem(ref layout, 0, 500, 250, 250);
		AddItem(ref layout, 250, 500, 250, 250);
		AddItem(ref layout, 500, 500, 250, 250);
		AddItem(ref layout, 750, 500, 250, 250);
		AddItem(ref layout, 0, 750, 250, 250);
		AddItem(ref layout, 250, 750, 250, 250);
		AddItem(ref layout, 500, 750, 250, 250);
		AddItem(ref layout, 750, 750, 250, 250);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout2x2"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAOVJREFUOE+1k9EKgkAQRf0n/6lfjkAKg6KCSk1LrGaaszZh+9DLknBZfbj3npnFLEt98nymKcowi4g+RT8n767hoYqupm4Y1QyiVa+h+L8BdyOhBRpvpxkde/lNALbvhoDW0jCeMN9UD9cfI7iZuZ3A5yYA866LCDCxzKnZAzCFZsPGvGlHfS0xDoiv1puD2drLNiJgSRD4smr7RgTtbxKEkQDMyyYimAb4vL5pR+Zcm1aNaFFHBLRBwFnZrI7MzNv33N5c1KqLKiLgigjwq8LoC6O1vGjAphnz/DwhSPoXUn/GZP8LiHGdm8dgk4sAAAAASUVORK5CYII=");
		GenerateAndAddItems(ref layout, 2, 2);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout3x3"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAOdJREFUOE+lk1EKwjAQRHsn7+SVRRClgqKC2tbWai275iUuxICKpDCk+ZiZtwkpitxvMplqjgrMIuKk0cp/0H0M6p26IagZRKub+uKfARbyd0A8FiHWTjM63uQzAWZDx8y+fQTsE+Ze9XD9MoIFGDZ7m5sAzLsuIaCRw7Tm9GZ8s8PGvGmD3g4xDriO4meuney0rdmbXXvZJgQYIGAlGTPCyH7fizcSgHnZJARxAPPGIxgy69pp1Ygu6oSANggCtvhm5kXb19zWvKhV51VCwKwE2FWZ2VrLi3psmjHPzhFB1lvIfYzZ/ieTZ5c5KSQZMAAAAABJRU5ErkJggg==");
		GenerateAndAddItems(ref layout, 3, 3);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout4x3Wide"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup16x9)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAMlJREFUOE/FU1sKwkAM3Dv1TvbIIhTBJyio27p9aNvEzm6DacEf96OFIYQwM3lsjVn8S5IVx8CATETcE3shRIHOyzbUbcMet4Z8/lPg1U0FXRsId5Br5msV6oNAGjFC+u1AHBGBeuygeLN3BeAI55MLmIygBaqOWM+sBUA8lsR7N9uBEBABOMvMl5oYQA4BkLeF6uD/PagdiCOiHU6kZz6Pc6MDOGc588bOdoDbyp1BljOh5cOA3TPUs5w8ef1QO4h6iYv/S9ENfACk3d3luOaB+gAAAABJRU5ErkJggg==");
		GenerateAndAddItems(ref layout, 4, 3);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout4x4"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAPBJREFUOE+lkwkKwjAQRXun3skri1AURUUFta3d3GbMax2JERFJ4ZOk5G9TmiSxT5qONAYJZBHRu+hrZW8431RB61BfBpQX0bzT3vinwNWJ/SWAqrlD9uvhjjPYd/KZwC4jgKsvVjk1zgfIrequ+VKBS9abPa4I0hsygLypgwSQGKYJMLTw6xAb8qoa8DZEX6C5Se/MBfqaM2eIS+c+r4IEECzB8KkGAcjbVnqYAORpGSQwR4vtT9re4b5wmJWiWREkwJEErLnrasOi8/rZ25yzQnWSBwlwRMA6Q7SB4To/aR8bZ8jjo5cg6l+I/Rmj+Q9cAIH3DLovowAAAABJRU5ErkJggg==");
		GenerateAndAddItems(ref layout, 4, 4);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout5x5"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAO9JREFUOE+tk+0KgkAQRX0n36lXjiAKA6OCSs0vzGbao66tS/RnEy7rupy5d2YxikKfOF5piCJgEdGX6LzybtX1qqg2qp6jik40a3Uw/m8BtxUSPI2+tYf7tZVlArcVYPZEZ2VfmmqAN+BG9VL/aMGC9O0noADwqfISADFMF6ZA0Y2ug/MUOy1V0WKItgAf616maesc2zpznhr3pPQScD0cfq5qdOfbuZFBgOyBd4WX4Nu0iQzonh1M9H0hus29BLgxA9bMgDYyAzsa0bN13uaqm8xLwBVRwF4VIALENXnoEBtn4PXdSRD0L4T+jMH8G97KeXS6w59FAAAAAElFTkSuQmCC");
		GenerateAndAddItems(ref layout, 5, 5);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout6x6"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAOxJREFUOE+tk/0KgjAUxX0n36lXjkAKA6MCSc0vrHbbz3ljDuqfJRzmBufjnrEkif3SdCMxSCAbY+Rl5LPyr5ieIqC36CaHZjJSjTIb/1fAH4UEjyWJf44zKEezTuCPApk90VnZt1ZNY5eDyLX/MYISmVsFdO7SukM+d0ECSJSpcSGrgD8C5KJ1WJWoAq5xJ1TbxllxVWf2hXXP2y8d6FX5ApfBCIDIOeR9EySAGLZN0xD986ONfmiMZHWQgLh0wFpZokZm5tMytzpntciuChJwRQi4O3ZNa2G45neZY+MMeXvzEkS9hdjHGM1/A0fCeZKdMmgsAAAAAElFTkSuQmCC");
		GenerateAndAddItems(ref layout, 6, 6);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout7x7"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAOZJREFUOE+1k+0KgkAQRX0n36lXjkAKA6MCS80vrGbao07Y4r8l4bKL7p17ZhajKPSJ442GKMIsIvoW/a7sTcNLFbVOzTCpGkSLXsfg/xSgMgTPmWTZIsko72WdwA4buhWrXTXDzjvVa7vSgh1mtb7t3bJvzOfGI7BhYmTP0FDlhubfUlarop8hrl0lZrBvsww7c+lp7RFwPVwnK8UwW/qlE0UY+YZ5X3kEfgGbNMYl3dGhHyrRpPQISINgwpYRG2R0cqJnS05K1V3hEZBIAevZzBhJTR86YpOMeXtfEAT9C6E/Y7D/AxDFeZ3BrG4ZAAAAAElFTkSuQmCC");
		GenerateAndAddItems(ref layout, 7, 7);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
		layout = new VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout
		{
			ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout8x8"
		};
		viewLayoutGroup = new ViewLayoutGroup
		{
			Id = new Guid(LayoutGroup4x3)
		};
		layout.ViewItems = new List<ViewItem>();
		layout.BinaryIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAOlJREFUOE+tk2sKwjAQhHun3skri1CUiooKalv7omrWfE1X0qC/YmHYtGF2ZjZNksQ+abqQGCSQjTHyMvKprBXDUwS0Fs3gUA1Gil5G4f828KPg4OE5QZl9lMGlN3MHfhTIvGNdo2jzK+RO5NwGEfwGYW72yE2lAeRj88WBH4GhASVBZA15XzvMhqgO3MRd93LKHR7zzqrn9Y8Z6FFpA5RPnRnBN5Qhr6vAgU5Z1XTSEH3LW9tgUxnJysABdvmhqIU9Ih0WmQ9TblXOSpFVEThAkQZUn4w6qvldRtsoQ17ePAdRdyH2Mkbz3+k6eWMaS3m+AAAAAElFTkSuQmCC");
		GenerateAndAddItems(ref layout, 8, 8);
		value = new ViewLayoutData(null, "", layout, viewLayoutGroup);
		key = string.Format(CultureInfo.InvariantCulture, "{0}, VideoOS.RemoteClient.Application", layout.ViewLayoutType);
		Add(key, value);
	}
}
