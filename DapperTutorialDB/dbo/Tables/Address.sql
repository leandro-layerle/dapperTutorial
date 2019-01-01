CREATE TABLE [dbo].[Address]
(
	[AddressId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Street] NVARCHAR(50) NULL, 
    [Number] NCHAR(10) NULL
)
