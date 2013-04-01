CREATE PROCEDURE [dbo].[sp_map_Poi_Bind]
	@PoiId		nvarchar(50),
	@ObjectId	int
AS
BEGIN
IF NOT EXISTS(SELECT 1 FROM  [dbo].[tbl_map_poi_to_objects] WHERE ObjectId = @ObjectId AND PoiId = @PoiId)
	INSERT INTO [dbo].[tbl_map_poi_to_objects] ([ObjectId], [PoiId]) VALUES (@ObjectId, @PoiId)
END
