SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_GetListBankAccount]
(
	@PageNumber INT = NULL,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL,
	@CompanyId INT = NULL
)
AS
BEGIN
	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT BA.Id, BA.Code, B.Id AS BranchId, Bank.Id AS BankId, B.Name AS BranchName, BA.ConcurrencyStamp,
				 (ROW_NUMBER() OVER (ORDER BY BA.Id DESC)) AS RowNum
				 FROM dbo.Core_BankAccount AS BA
				 INNER JOIN dbo.Core_Branch AS B ON b.Id = BA.BranchId AND B.IsEnabled = 1
				 INNER JOIN dbo.Core_Bank AS Bank ON Bank.Id = B.BankId AND B.IsEnabled = 1
				 WHERE BA.IsEnabled = 1 AND BA.CompanyId = @CompanyId
				 AND ((@SearchText IS NULL OR BA.Code LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR B.Name LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR BA.Name LIKE N'%' + @SearchText + '%')))
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO