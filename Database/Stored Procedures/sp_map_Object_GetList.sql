/****** Object:  StoredProcedure [dbo].[sp_map_Poi_GetList]    Script Date: 23.10.2012 13:32:50 ******/
CREATE PROCEDURE [dbo].[sp_map_Object_GetList]
	@ObjectId int = NULL,
	@PoiId varchar(50) = NULL
AS
BEGIN

IF @ObjectId IS NOT NULL
	BEGIN
	SELECT [ObjectId]
      ,[Type]
      ,[Coords]
  FROM [PTMap].[dbo].[tbl_map_objects]
  WHERE [ObjectId] = @ObjectId
	END
ELSE
	IF @PoiId IS NOT NULL
	BEGIN
		SELECT o.[ObjectId]
		  ,[Type]
		  ,[Coords]
		FROM [PTMap].[dbo].[tbl_map_objects] o
			INNER JOIN [dbo].[tbl_map_poi_to_objects] pto ON pto.[ObjectId] = o.ObjectId
		WHERE pto.[PoiId] = @PoiId
	END
	ELSE
	BEGIN
	SELECT [ObjectId]
      ,[Type]
      ,[Coords]
  FROM [PTMap].[dbo].[tbl_map_objects]
	END

END
