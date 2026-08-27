namespace VideoOS.GisMapSharedData.CommandExceptions;

public static class MapCommandErrorReason
{
	private static string _unknown = "Unknown";

	private static string _invalidAuthentication = "InvalidAuthentication";

	private static string _invalidAuthorization = "InvalidAuthorization";

	private static string _databaseExtraRows = "DatabaseExtraRows";

	private static string _itemNotFound = "ItemNotFound";

	private static string _layerNotFound = "LayerNotFound";

	private static string _mapNotFound = "MapNotFound";

	private static string _repositoryNotFound = "RepositoryNotFound";

	private static string _viewPointNotFound = "ViewPointNotFound";

	private static string _invalidType = "InvalidType";

	private static string _sqlDatabase = "SqlDatabase";

	private static string _argumentIsNull = "ArgumentIsNull";

	private static string _operationNotSupported = "OperationNotSupported";

	private static string _sanityCheckFailed = "SanityCheckFailed";

	private static string _mapAlreadyExists = "MapAlreadyExists";

	private static string _layerAlreadyExists = "LayerAlreadyExists";

	private static string _itemAlreadyExists = "ItemAlreadyExists";

	private static string _repositoryAlreadyExists = "RepositoryAlreadyExists";

	private static string _viewPointAlreadyExists = "ViewPointAlreadyExists";

	private static string _layerOrderDuplicateFound = "LayerOrderDuplicateFound";

	private static string _layerOrderGapFound = "LayerOrderGapFound";

	private static string _obsoleteItemType = "ObsoleteItemType";

	private static string _buildingNotFound = "BuildingNotFound";

	private static string _buildingAlreadyExists = "BuildingAlreadyExists";

	private static string _levelAlreadyExists = "LevelAlreadyExists";

	private static string _nonUniqueBuildingId = "NonUniqueBuildingId";

	private static string _nonUniqueMapId = "NonUniqueMapId";

	private static string _orderDuplicateFound = "OrderDuplicateFound";

	private static string _orderGapFound = "OrderGapFound";

	private static string _levelNotFound = "LevelNotFound";

	public static string Unknown => _unknown;

	public static string InvalidAuthentication => _invalidAuthentication;

	public static string InvalidAuthorization => _invalidAuthorization;

	public static string DatabaseExtraRows => _databaseExtraRows;

	public static string ItemNotFound => _itemNotFound;

	public static string LayerNotFound => _layerNotFound;

	public static string MapNotFound => _mapNotFound;

	public static string RepositoryNotFound => _repositoryNotFound;

	public static string ViewPointNotFound => _viewPointNotFound;

	public static string InvalidType => _invalidType;

	public static string SqlDatabase => _sqlDatabase;

	public static string ArgumentIsNull => _argumentIsNull;

	public static string OperationNotSupported => _operationNotSupported;

	public static string SanityCheckFailed => _sanityCheckFailed;

	public static string MapAlreadyExists => _mapAlreadyExists;

	public static string LayerAlreadyExists => _layerAlreadyExists;

	public static string ItemAlreadyExists => _itemAlreadyExists;

	public static string RepositoryAlreadyExists => _repositoryAlreadyExists;

	public static string ViewPointAlreadyExists => _viewPointAlreadyExists;

	public static string LayerOrderDuplicateFound => _layerOrderDuplicateFound;

	public static string LayerOrderGapFound => _layerOrderGapFound;

	public static string ObsoleteItemType => _obsoleteItemType;

	public static string BuildingNotFound => _buildingNotFound;

	public static string BuildingAlreadyExists => _buildingAlreadyExists;

	public static string LevelAlreadyExists => _levelAlreadyExists;

	public static string NonUniqueBuildingId => _nonUniqueBuildingId;

	public static string NonUniqueMapId => _nonUniqueMapId;

	public static string OrderDuplicateFound => _orderDuplicateFound;

	public static string OrderGapFound => _orderGapFound;

	public static string LevelNotFound => _levelNotFound;
}
