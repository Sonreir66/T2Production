CREATE TABLE [dbo].[Ingredient] (
    [IngredientId]   INT           IDENTITY (1, 1) NOT NULL,
    [IngredientName] VARCHAR (255) NOT NULL,
    [IngredientCost] FLOAT (53)    NOT NULL,
    [TypeId]         INT           NULL,
    CONSTRAINT [PK_Ingredient] PRIMARY KEY CLUSTERED ([IngredientId] ASC)
);

