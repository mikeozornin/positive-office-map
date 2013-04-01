CREATE PROCEDURE [dbo].[sp_map_Poi_Unbind]
	@PoiId		nvarchar(50),
	@ObjectId	int = NULL
AS
BEGIN
IF @ObjectId IS NOT NULL
	BEGIN
		IF EXISTS(SELECT 1 FROM  [dbo].[tbl_map_poi_to_objects] WHERE ObjectId = @ObjectId AND PoiId = @PoiId)
			DELETE FROM [dbo].[tbl_map_poi_to_objects] WHERE ObjectId = @ObjectId AND PoiId = @PoiId
	END
ELSE
	BEGIN
		DELETE FROM [dbo].[tbl_map_poi_to_objects] WHERE PoiId = @PoiId
	END

END
