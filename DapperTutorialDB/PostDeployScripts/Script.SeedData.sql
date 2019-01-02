DELETE FROM dbo.Customer
INSERT INTO dbo.Customer ([Name],[LastName])
VALUES
('Fernando','Marquez'),
('Sergi','Torres'),
('Sergio','Fernandez'),
('Federico','Estevez')

DELETE FROM dbo.[Address]
INSERT INTO dbo.[Address] (Street, Number)
VALUES
('Calle Falsa','1234')

UPDATE dbo.Customer
SET AddressId = @@IDENTITY
WHERE [Name]= 'Fernando' AND LastName = 'Marquez'

INSERT INTO dbo.[Address] (Street, Number)
VALUES
('Calle Falsa','1235')

UPDATE dbo.Customer
SET AddressId = @@IDENTITY
WHERE [Name]= 'Sergio' AND LastName = 'Fernandez'
