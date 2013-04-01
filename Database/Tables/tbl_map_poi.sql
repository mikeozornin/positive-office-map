CREATE TABLE [dbo].[tbl_map_poi] (
    [PoiId]    NVARCHAR (50)  NOT NULL,
    [Name]     NVARCHAR (150) NOT NULL,
    [Title]    NVARCHAR (250) NULL,
    [PortalId] INT            NULL,
    [Email]    NVARCHAR (150) NULL,
    [ImageUrl] NVARCHAR (250) NULL,
    [Phone]    VARCHAR (50)   NULL,
    [Type]     INT            NOT NULL,
    CONSTRAINT [PK_tbl_map_poi] PRIMARY KEY CLUSTERED ([PoiId] ASC)
);

