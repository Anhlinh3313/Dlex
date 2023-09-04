SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_RanDomCodeCustomer
(
	@Code NVARCHAR(200) = NULL
)
AS
BEGIN

	DECLARE @Message NVARCHAR(200);
	DECLARE @NumberId INT;
	DECLARE @CustomerCode NVARCHAR(100);
	--select customer id asc
	SET @NumberId = (SELECT TOP(1) c.Id
	FROM dbo.Crm_Customer AS c
	ORDER BY c.Id DESC)
	--create code RanDom
	SET @CustomerCode = (@CODE + (SELECT RIGHT(CONVERT(VARCHAR, 1000000 + @NumberId), 6)))
	--
	SELECT @CustomerCode AS [CustomerCode] 
END
GO