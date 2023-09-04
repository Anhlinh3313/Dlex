SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_GetListCustomerInfoLog] -- Proc_GetListCustomerInfoLog 4
(
	@SenderId INT = null,
	@ProvinceId INT = null,
	@PageNumber INT = null,
	@PageSize INT = null,
	@SearchText NVARCHAR(200) = NULL,
	@CompanyId INT = null
)
AS
BEGIN

	IF @PageNumber IS NULL SET @PageNumber=1; 
	IF @PageSize IS NULL SET @PageSize = 20;

	WITH _CTE AS(
	 SELECT cuslog.Id, cuslog.SenderId, cuslog.ProvinceId, cuslog.DistrictId, cuslog.WardId, cuslog.Code, cuslog.Name, cuslog.PhoneNumber, cuslog.Address, cuslog.AddressNote,
	 pro.Name as ProvinceName, dis.Name as DistrictName, ward.Name as WardName,
	 (ROW_NUMBER() OVER (ORDER BY cuslog.Id DESC)) AS RowNum 
	 FROM Crm_CustomerInfoLog as cuslog 
	 LEFT JOIN Core_Province as pro ON pro.Id=cuslog.ProvinceId
	 LEFT JOIN Core_District as dis ON dis.Id = cuslog.DistrictId
	 LEFT JOIN Core_Ward as ward on ward.Id = cuslog.WardId
	 WHERE cuslog.IsEnabled=1 
		 AND (@ProvinceId IS NULL OR cuslog.ProvinceId=@ProvinceId)
		 AND (@SenderId IS NULL OR cuslog.SenderId=@SenderId)
		 AND cuslog.Code IS NOT NULL
		 AND cuslog.CompanyId = @CompanyId
		 AND 
		 (@SearchText IS NULL OR cuslog.Code LIKE @SearchText
		 OR cuslog.Name LIKE @SearchText OR cuslog.PhoneNumber LIKE @SearchText
		 )
	 )SELECT *, (SELECT COUNT(1) FROM _CTE) as TotalCount 
	 FROM _CTE WHERE  RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO

