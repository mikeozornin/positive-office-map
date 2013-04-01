CREATE PROCEDURE sp_map_Poi_Update
	@PoiId nvarchar(50),
    @Name nvarchar(150),
    @Title nvarchar(250) = NULL,
    @PortalId int = NULL,
    @Email nvarchar(150) = NULL,
    @ImageUrl nvarchar(250) = NULL,
    @Phone varchar(50) = NULL,
    @Type int
AS
BEGIN
UPDATE [dbo].[tbl_map_poi]
   SET [Name] = @Name
      ,[Title] = @Title
      ,[PortalId] = @PortalId
      ,[Email] = @Email
      ,[ImageUrl] = @ImageUrl
      ,[Phone] = @Phone
      ,[Type] = @Type
 WHERE [PoiId] = @PoiId
END
