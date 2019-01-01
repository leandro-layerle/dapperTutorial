CREATE TABLE [dbo].[Customer]
(
	[CustomerId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [AddressId] INT NULL, 
    CONSTRAINT [FK_Customer_Address] FOREIGN KEY (AddressId) REFERENCES [Address](AddressId)
)
