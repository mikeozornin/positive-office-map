CREATE PROCEDURE [dbo].[sp_map_Poi_Delete]
	@PoiId nvarchar(50)
AS
BEGIN
	DELETE FROM [dbo].[tbl_map_poi_to_objects] WHERE [PoiId] = @PoiId
	DELETE FROM [dbo].[tbl_map_poi] WHERE [PoiId] = @PoiId
END
