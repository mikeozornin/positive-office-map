CREATE TABLE [dbo].[tbl_map_poi_to_objects] (
    [ObjectId] INT           NOT NULL,
    [PoiId]    NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tbl_map_poi_to_objects] PRIMARY KEY CLUSTERED ([ObjectId] ASC, [PoiId] ASC),
    CONSTRAINT [FK_tbl_map_poi_to_objects_tbl_map_objects] FOREIGN KEY ([ObjectId]) REFERENCES [dbo].[tbl_map_objects] ([ObjectId]),
    CONSTRAINT [FK_tbl_map_poi_to_objects_tbl_map_poi] FOREIGN KEY ([PoiId]) REFERENCES [dbo].[tbl_map_poi] ([PoiId])
);

