CREATE TABLE [dbo].[Product] (
    [ProductId]          INT           IDENTITY (1, 1) NOT NULL,
    [ProductName]        VARCHAR (255) NOT NULL,
    [SellPrice]          FLOAT (53)    NOT NULL,
    [BuildTimeInSeconds] INT           NOT NULL,
    [OutputCount]        INT           NOT NULL,
    [TypeId]             INT           NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);

