using System;
using System.Collections.Generic;
using System.Drawing;

namespace VideoOS.Platform.Client;

public abstract class ViewAndLayoutItem : ConfigItem
{
	public static Guid EmptyBuiltinId = new Guid("57ABE978-8861-4577-8A09-5FA6E43C4109");

	public static Guid CameraBuiltinId = new Guid("CE8EB517-C0CB-45e9-80B3-5ADAB41EB9A5");

	public static Guid HotspotBuiltinId = new Guid("090419A2-009D-4390-BB8A-14C02623A09C");

	public static Guid CarrouselBuiltinId = new Guid("AF5929D4-0986-4de7-928E-C0353CE9F03F");

	public static Guid MatrixBuiltinId = new Guid("7EE8D465-62C6-4839-ABD8-42A67EB53489");

	public static Guid HTMLBuiltinId = new Guid("05332BA1-9F08-4fa8-A11B-7AD8AA6186BC");

	public static Guid MapBuiltinId = new Guid("C27C4F2F-F852-4A91-8516-14CBE6AE302B");

	public static Guid GisMapBuiltinId = new Guid("D1B3FF07-B45C-485A-944B-9BD2814636BA");

	public static Guid SystemMonitorBuiltinId = new Guid("D6504881-C6CF-4BC2-9FEA-F56F6A881EC3");

	[Obsolete("This built in view item is obsolete and will no longer be supported from Smart Client 2026 R1. Use TextBuiltInId instead.", false)]
	public static Guid ImageBuiltInId = new Guid("3A8A7345-1D80-43eb-B556-8982BD3FC64E");

	[Obsolete("This built in view item is obsolete and will no longer be supported from Smart Client 2026 R1. Use TextBuiltInId instead.", false)]
	public static Guid TextBuiltInId = new Guid("769457D3-1F67-4C61-8529-5A428689A7CF");

	public static Guid ImageTextBuiltInId = new Guid("8de0025a-d92e-4c18-b670-9ae0625f8be1");

	public abstract Rectangle[] Layout { get; set; }

	public ViewAndLayoutItem(string name, Rectangle[] layoutArray)
	{
	}

	public ViewAndLayoutItem(string layoutName)
	{
	}

	public ViewAndLayoutItem()
	{
	}

	public abstract void InsertViewItemPlugin(int index, ViewItemPlugin viewItemPlugin, Dictionary<string, string> properties);

	public abstract void InsertBuiltinViewItem(int index, Guid builtinId, Dictionary<string, string> properties);

	public virtual void InsertViewItem(int index, string configurationString)
	{
	}

	public virtual Item GetViewItem(int index)
	{
		return null;
	}

	public virtual Guid ViewItemId(int index)
	{
		return EmptyBuiltinId;
	}

	public virtual string ViewItemConfigurationString(int index)
	{
		return string.Empty;
	}

	public abstract void Save();
}
