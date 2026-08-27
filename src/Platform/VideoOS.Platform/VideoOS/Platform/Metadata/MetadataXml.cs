using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace VideoOS.Platform.Metadata;

public static class MetadataXml
{
	public static readonly TimeSpan LogIgnoreTimeSpand = TimeSpan.FromMinutes(1.0);

	public static CultureInfo Culture = CultureInfo.InvariantCulture;

	public static NumberStyles FloatStyle = NumberStyles.Float;

	public static NumberStyles UintStyle = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

	private static readonly Lazy<NameTable> NameTableField = new Lazy<NameTable>(CreateNameTable, isThreadSafe: true);

	public static readonly string OnvifPrefix = "tt";

	public static readonly string OnvifNamespace = "http://www.onvif.org/ver10/schema";

	public static readonly string MetadataStreamElement = "MetadataStream";

	public static readonly string VideoAnalyticsElement = "VideoAnalytics";

	public static readonly string FrameElement = "Frame";

	public static readonly string UtcTimeAttribute = "UtcTime";

	public static readonly string ObjectElement = "Object";

	public static readonly string AppearanceElement = "Appearance";

	public static readonly string BehaviourElement = "Behaviour";

	public static readonly string ObjectIdAttribute = "ObjectId";

	public static readonly string TransformationElement = "Transformation";

	public static readonly string ShapeElement = "Shape";

	public static readonly string ColorClusterElement = "ColorCluster";

	public static readonly string ClassElement = "Class";

	public static readonly string ClassCandidateElement = "ClassCandidate";

	public static readonly string LikelihoodElement = "Likelihood";

	public static readonly string TypeElement = "Type";

	public static readonly string ColorElement = "Color";

	public static readonly string TranslateElement = "Translate";

	public static readonly string ScaleElement = "Scale";

	public static readonly string VectorXAttribute = "x";

	public static readonly string VectorYAttribute = "y";

	public static readonly string BoundingBoxElement = "BoundingBox";

	public static readonly string CenterOfGravityElement = "CenterOfGravity";

	public static readonly string PolygonElement = "Polygon";

	public static readonly string BottomAttribute = "bottom";

	public static readonly string TopAttribute = "top";

	public static readonly string RightAttribute = "right";

	public static readonly string LeftAttribute = "left";

	public static readonly string ColorXAttribute = "X";

	public static readonly string ColorYAttribute = "Y";

	public static readonly string ColorZAttribute = "Z";

	public static readonly string ColorspaceAttribute = "Colorspace";

	public static readonly string PointElement = "Point";

	public static readonly string BehaviourRemovedElement = "Removed";

	public static readonly string BehaviourIdleElement = "Idle";

	public static readonly string ExtensionElement = "Extension";

	public static readonly string OriginalDataElement = "OriginalData";

	public static readonly string NavigationalDataElement = "NavigationalData";

	public static readonly string VersionAttribute = "version";

	public static readonly string LatitudeElement = "Latitude";

	public static readonly string AltitudeElement = "Altitude";

	public static readonly string AzimuthElement = "Azimuth";

	public static readonly string GeodeticSystemElement = "GeodeticSystem";

	public static readonly string HorizontalAccuracyElement = "HorizontalAccuracy";

	public static readonly string LongitudeElement = "Longitude";

	public static readonly string VerticalAccuracyElement = "VerticalAccuracy";

	public static readonly string SpeedElement = "Speed";

	public static readonly string DescriptionElement = "Description";

	public static readonly string BoundingBoxAppearanceElement = "BoundingBoxAppearance";

	public static readonly string FillElement = "Fill";

	public static readonly string LineElement = "Line";

	public static readonly string ColorAttribute = "color";

	public static readonly string DisplayedThicknessInPixelsAttribute = "displayedThicknessInPixels";

	public static readonly string SizeAttribute = "size";

	public static readonly string BoldAttribute = "bold";

	public static readonly string ItalicAttribute = "italic";

	public static readonly string FontFamilyAttribute = "fontFamily";

	public static NameTable NameTable => NameTableField.Value;

	private static NameTable CreateNameTable()
	{
		NameTable nameTable = new NameTable();
		foreach (string item in from field in typeof(MetadataXml).GetFields(BindingFlags.Static | BindingFlags.Public)
			where field.FieldType == typeof(string)
			where field.IsInitOnly
			select (string)field.GetValue(null))
		{
			nameTable.Add(item);
		}
		return nameTable;
	}
}
