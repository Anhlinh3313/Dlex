SELECT *
FROM dbo.Core_Page
WHERE ModulePageId = 1 AND ParentPageId = 7

UPDATE dbo.Core_Page
SET Name = 'Nhóm nhân viên'
WHERE id = 250

SELECT *
FROM dbo.Core_Page
WHERE id = 7

UPDATE dbo.Core_Page 
SET AliasPath = '/core-general'
WHERE id = 7

UPDATE dbo.Core_Page 
SET Name = N'Quản lý nhân viên'
WHERE id = 7

UPDATE dbo.Core_Page 
SET IsEnabled = 0
WHERE id = 8

--
SELECT *
FROM dbo.Core_Page
WHERE ModulePageId = 1 AND ParentPageId = 11

SELECT *
FROM dbo.Core_Page
WHERE id=  11

UPDATE dbo.Core_Page 
SET AliasPath = '/core-place'
WHERE id =11

UPDATE dbo.Core_Page 
SET IsEnabled = 0
WHERE id = 146

--
UPDATE dbo.Core_Page 
SET ParentPageId = 7
WHERE id = 23


--
SELECT *
FROM dbo.Core_Page 
WHERE ParentPageId = 16

SELECT *
FROM dbo.Core_Page 
WHERE Name = 'Quản lý hệ thống'

UPDATE dbo.Core_Page
SET Name = N'Quản lý hệ thống'
WHERE id = 16

UPDATE dbo.Core_Page
SET Name = 'Trung tâm'
WHERE id = 17

UPDATE dbo.Core_Page
SET Name = N'Kho/Trạm'
WHERE id = 19

UPDATE dbo.Core_Page
SET Name = N'Phân khu vực phục vụ'
WHERE id = 20

UPDATE dbo.Core_Page
SET Name = N'Phân tuyến phục vụ'
WHERE id = 21

UPDATE dbo.Core_Page 
SET IsEnabled = 0
WHERE id = 216
--
UPDATE dbo.Core_Page 
SET IsEnabled = 0
WHERE id = 22


SELECT *
FROM dbo.Core_ModulePage

UPDATE dbo.Core_ModulePage
SET Code = 'OP'
WHERE id = 1

UPDATE dbo.Core_ModulePage
SET IsEnabled = 0
WHERE id IN (2,3,4)