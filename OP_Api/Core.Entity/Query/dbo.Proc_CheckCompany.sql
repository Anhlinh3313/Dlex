SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_CheckCompany]
(
	@CompanyCode NVARCHAR(100) = NULL
)

AS
BEGIN
	SELECT 
		_com.Id, _com.Code, _com.Name AS CompanyName
	FROM dbo.Core_Company as _com 
	WHERE _com.IsEnabled=1 
		 AND _com.Code = @CompanyCode AND @CompanyCode IS NOT NULL
END
GO

