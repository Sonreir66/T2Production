CREATE TABLE [dbo].[ProductIngredient] (
    [ProductIngredientId] INT IDENTITY (1, 1) NOT NULL,
    [ProductId]           INT NOT NULL,
    [IngredientId]        INT NOT NULL,
    [Quantity]            INT NOT NULL,
    CONSTRAINT [PK_ProductIngredient] PRIMARY KEY CLUSTERED ([ProductIngredientId] ASC),
    CONSTRAINT [FK_ProductIngredient_Ingredient] FOREIGN KEY ([ProductIngredientId]) REFERENCES [dbo].[Ingredient] ([IngredientId])
);

