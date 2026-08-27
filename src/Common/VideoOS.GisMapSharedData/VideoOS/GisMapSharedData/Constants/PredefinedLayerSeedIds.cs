using System;

namespace VideoOS.GisMapSharedData.Constants;

public static class PredefinedLayerSeedIds
{
	private static Guid _cameraLayerSeedId = new Guid("00000000-1000-0000-0000-000000000000");

	private static Guid _viewZoneLayerSeedId = new Guid("00000000-2000-0000-0000-000000000000");

	private static Guid _linkLayerSeedId = new Guid("00000000-3000-0000-0000-000000000000");

	private static Guid _legacyLinkLayerSeedId = new Guid("00000000-4000-0000-0000-000000000000");

	private static Guid _locationLayerSeedId = new Guid("00000000-5000-0000-0000-000000000000");

	private static Guid _buildingLayerSeedId = new Guid("00000000-6000-0000-0000-000000000000");

	private static Guid _inputLayerSeedId = new Guid("00000000-7000-0000-0000-000000000000");

	private static Guid _outputLayerSeedId = new Guid("00000000-7500-0000-0000-000000000000");

	private static Guid _microphoneLayerSeedId = new Guid("00000000-8000-0000-0000-000000000000");

	private static Guid _accessControlUnitLayerSeedId = new Guid("00000000-9000-0000-0000-000000000000");

	private static Guid _milestoneWorldMapLayerSeedId = new Guid("00000000-1100-0000-0000-000000000000");

	private static Guid _openStreetMapLayerSeedId = new Guid("00000000-1200-0000-0000-000000000000");

	private static Guid _googleMapsLayerSeedId = new Guid("00000000-1300-0000-0000-000000000000");

	private static Guid _bingMapsLayerSeedId = new Guid("00000000-1400-0000-0000-000000000000");

	public static Guid CameraLayerSeedId => _cameraLayerSeedId;

	public static Guid ViewZoneLayerSeedId => _viewZoneLayerSeedId;

	public static Guid LinkLayerSeedId => _linkLayerSeedId;

	public static Guid LegacyLinkLayerSeedId => _legacyLinkLayerSeedId;

	public static Guid LocationLayerSeedId => _locationLayerSeedId;

	public static Guid BuildingLayerSeedId => _buildingLayerSeedId;

	public static Guid InputLayerSeedId => _inputLayerSeedId;

	public static Guid OutputLayerSeedId => _outputLayerSeedId;

	public static Guid MicrophoneLayerSeedId => _microphoneLayerSeedId;

	public static Guid AccessControlUnitLayerSeedId => _accessControlUnitLayerSeedId;

	public static Guid MilestoneWorldMapLayerSeedId => _milestoneWorldMapLayerSeedId;

	public static Guid OpenStreetMapLayerSeedId => _openStreetMapLayerSeedId;

	public static Guid GoogleMapsLayerSeedId => _googleMapsLayerSeedId;

	public static Guid BingMapsLayerSeedId => _bingMapsLayerSeedId;
}
