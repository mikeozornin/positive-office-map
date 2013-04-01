CREATE PROCEDURE [dbo].[sp_map_Poi_GetList]
	@PoiType int = NULL,
	@PoiId nvarchar(50) = NULL
AS
BEGIN
IF @PoiId IS NOT NULL
BEGIN
	SELECT [PoiId]
		,[Name]
		,[Title]
		,[PortalId]
		,[Email]
		,[ImageUrl]
		,[Phone]
		,[Type]
	FROM [dbo].[tbl_map_poi]
	WHERE [PoiId] = @PoiId
	ORDER BY [Name]
END
ELSE
	BEGIN
	IF @PoiType IS NOT NULL
		BEGIN
		SELECT [PoiId]
		  ,[Name]
		  ,[Title]
		  ,[PortalId]
		  ,[Email]
		  ,[ImageUrl]
		  ,[Phone]
		  ,[Type]
	  FROM [dbo].[tbl_map_poi]
	  WHERE [Type] = @PoiType
	  ORDER BY [Name]
		END
	ELSE
		BEGIN
		SELECT [PoiId]
		  ,[Name]
		  ,[Title]
		  ,[PortalId]
		  ,[Email]
		  ,[ImageUrl]
		  ,[Phone]
		  ,[Type]
	  FROM [dbo].[tbl_map_poi]
	  ORDER BY [Name]
		END

	END
END
