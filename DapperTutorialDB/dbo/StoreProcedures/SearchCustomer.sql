CREATE PROCEDURE [dbo].[SearchCustomer]
	@searchTerm VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT [CustomerId], [Name], [LastName]
	FROM dbo.Customer
	WHERE [Name] LIKE '%' + @searchTerm + '%'
		OR LastName LIKE '%' + @searchTerm + '%';

END
