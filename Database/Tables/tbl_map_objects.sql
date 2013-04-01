CREATE TABLE [dbo].[tbl_map_objects] (
    [ObjectId] INT           IDENTITY (1, 1) NOT NULL,
    [Type]     VARCHAR (50)  NOT NULL,
    [Coords]   VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_tbl_map_objects] PRIMARY KEY CLUSTERED ([ObjectId] ASC)
);

