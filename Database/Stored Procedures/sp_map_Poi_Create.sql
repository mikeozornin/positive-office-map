CREATE PROCEDURE sp_map_Poi_Create 
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
	INSERT INTO [dbo].[tbl_map_poi]
           ([PoiId]
           ,[Name]
           ,[Title]
           ,[PortalId]
           ,[Email]
           ,[ImageUrl]
           ,[Phone]
           ,[Type])
     VALUES
           (@PoiId,
           @Name,
           @Title,
           @PortalId,
           @Email,
           @ImageUrl,
           @Phone,
           @Type)
END
