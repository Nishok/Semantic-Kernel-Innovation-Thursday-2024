-- Create the database
CREATE DATABASE StoreDB;
GO

-- Use the newly created database
USE StoreDB;
GO

-- Create the Items table
CREATE TABLE [dbo].[Items] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Price] DECIMAL(18, 2) NOT NULL
);

-- Create the PurchasedItems table
CREATE TABLE [dbo].[PurchasedItems] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ItemName] NVARCHAR(100) NOT NULL,
    [Quantity] INT NOT NULL,
    [PurchasedDate] DATETIME NOT NULL,
    [ItemId] INT NOT NULL,
    CONSTRAINT [FK_PurchasedItems_Items] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items]([Id])
);

-- Add a few sample items to the Items table
INSERT INTO [dbo].[Items] (Name, Price) VALUES ('Laptop', 999.99);
INSERT INTO [dbo].[Items] (Name, Price) VALUES ('Smartphone', 699.99);
INSERT INTO [dbo].[Items] (Name, Price) VALUES ('Headphones', 199.99);
INSERT INTO [dbo].[Items] (Name, Price) VALUES ('Monitor', 299.99);
INSERT INTO [dbo].[Items] (Name, Price) VALUES ('Keyboard', 49.99);
GO