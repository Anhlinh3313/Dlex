SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_GetAllPermissionByRoleId
(
	@RoleId INT = NULL
)
AS
BEGIN

	DECLARE @tablb TABLE (Id INT, RoleId INT, PageId INT, IsAccess BIT, IsAdd BIT, IsEdit BIT, IsDelete BIT, CompanyId INT);
	INSERT INTO @tablb (Id, RoleId,PageId,IsAccess,IsAdd,IsEdit,IsDelete,CompanyId) (SELECT rp.Id, rp.RoleId, rp.PageId, rp.IsAccess, rp.IsAdd, rp.IsEdit, rp.IsDelete, rp.CompanyId
	FROM dbo.Core_RolePage AS rp
	INNER JOIN dbo.Core_Page AS p ON p.Id = rp.PageId AND p.IsEnabled = 1
	WHERE rp.IsEnabled = 1 AND rp.RoleId = @RoleId)

	SELECT Id, RoleId,PageId,IsAccess,IsAdd,IsEdit,IsDelete,CompanyId,
	(CASE 
	WHEN IsAccess = 1 AND IsAdd = 1 AND IsEdit = 1 AND IsDelete = 1 THEN 1
	WHEN IsAccess = 0 OR IsAdd = 0 OR IsEdit = 0 OR IsDelete = 0 THEN 0
	END) AS CheckRolePageId
	FROM @tablb

END
GO







