
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'FirstLockDate'
          AND Object_ID = Object_ID(N'dbo.Post_ListReceiptMoney'))
BEGIN
	ALTER TABLE Post_ListReceiptMoney ADD FirstLockDate DATETIME DEFAULT GETDATE()
END
GO
--UPDATE Post_ListReceiptMoney SET FirstLockDate = CreatedWhen WHERE FirstLockDate IS NULL
--GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'DataChanged'
          AND Object_ID = Object_ID(N'dbo.Post_ListReceiptMoneyLog'))
BEGIN
	ALTER TABLE Post_ListReceiptMoneyLog
	ADD DataChanged NVARCHAR(250)
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'ReasonListGoodsId'
          AND Object_ID = Object_ID(N'dbo.Post_ListReceiptMoney'))
BEGIN
	ALTER TABLE Post_ListReceiptMoney 
	ADD ReasonListGoodsId INT
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AcceptDateFrom'
          AND Object_ID = Object_ID(N'dbo.Post_ListCustomerPayment'))
BEGIN
	alter table Post_ListCustomerPayment
	add AcceptDateFrom DATETIME
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AcceptDateTo'
          AND Object_ID = Object_ID(N'dbo.Post_ListCustomerPayment'))
BEGIN
	alter table Post_ListCustomerPayment
	add AcceptDateTo DATETIME
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'IsUnlockListGood'
          AND Object_ID = Object_ID(N'dbo.Post_Reason'))
BEGIN
	alter table Post_Reason
	add IsUnlockListGood bit
END
GO
update Post_Reason set IsUnlockListGood = 0 WHERE IsUnlockListGood IS NULL
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'FromHubId'
          AND Object_ID = Object_ID(N'dbo.Post_ListReceiptMoneyLog'))
BEGIN
	alter table Post_ListReceiptMoneyLog
add FromHubId INT, ToHubId INT, PaidByEmpId INT, TotalShipment INT, TotalCOD FLOAT, TotalPrice FLOAT, 
GrandTotal FLOAT, ListReceiptMoneyStatusId INT, AttachmentId INT, ListReceiptMoneyTypeId INT, 
Note NVARCHAR(MAX), ImagePathDOC NVARCHAR(MAX), BankAccount NVARCHAR(50), IsTransfer BIT, 
GrandTotalReal FLOAT, CancelReason NVARCHAR(MAX), FirstLockDate DATETIME, ReasonListGoodsId INT, WarningNote NVARCHAR(MAX), AccountingAccountId INT,
CashFlowId INT, FeeBank FLOAT, AcceptDate DATETIME
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Flag'
          AND Object_ID = Object_ID(N'dbo.Post_Shipment'))
BEGIN
	alter table Post_Shipment
	add Flag INT 
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Flag'
          AND Object_ID = Object_ID(N'dbo.Post_PushVSELog'))
BEGIN
	alter table Post_PushVSELog
	add Flag INT 
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'IsTransfer'
          AND Object_ID = Object_ID(N'dbo.Post_ListReceiptMoneyLog'))
BEGIN
	alter table Post_ListReceiptMoneyLog add IsTransfer BIT
END
GO
UPDATE Post_ListReceiptMoneyLog SET IsTransfer=0 WHERE IsTransfer IS NULL
GO
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Post_ListReceiptMoneyLogShipment'))
BEGIN
   CREATE TABLE Post_ListReceiptMoneyLogShipment(
   Id INT PRIMARY KEY IDENTITY,
   IsEnabled BIT DEFAULT 1,
   ShipmentId INT CONSTRAINT fk_ListReceiptMoneyLogShipment_Shipemnt FOREIGN KEY(ShipmentId) REFERENCES Post_Shipment(Id),
   ListReceiptMoneyLogId INT CONSTRAINT fk_ListReceiptMoneyLogShipment_ListReceiptMoneyLog 
   FOREIGN KEY(ListReceiptMoneyLogId) REFERENCES Post_ListReceiptMoneyLog(Id)
   )
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'IsMustReceiverCode2'
          AND Object_ID = Object_ID(N'dbo.Crm_Customer'))
BEGIN
	alter table Crm_Customer add IsMustReceiverCode2 BIT DEFAULT 0
END
GO
UPDATE Crm_Customer SET IsMustReceiverCode2 = 0 WHERE IsMustReceiverCode2 IS NULL
GO
UPDATE Crm_Customer SET IsMustReceiverCode2 = 1 WHERE VSEOracleCode = 'CS010325'
GO
DECLARE @NamePage NVARCHAR(200) = N'Báo cáo bảng kê đã xác nhận';
DECLARE @CodePage NVARCHAR(200) = N'bao-cao-bang-ke-da-xac-nhan';
DECLARE @PathPage NVARCHAR(500) =  '/bao-cao/bao-cao-bang-ke-da-xac-nhan';
DECLARE @ParentPageCode NVARCHAR(500) = 'Report';
DECLARE @ModulePageId INT = 3; --1 core --2 crm --3 post
IF(SELECT COUNT(1) FROM Core_Page WHERE Code=@CodePage)=0
BEGIN
	DECLARE @ParentPageId INT = NULL;
	DECLARE @PageOrder INT = 0;
	SELECT TOP 1 @ParentPageId=Id FROM Core_Page WHERE IsEnabled=1 AND Code=@ParentPageCode
	IF @ParentPageId IS NOT NULL
	BEGIN
		SELECT @PageOrder=(MAX(PageOrder)+1) FROM Core_Page WHERE IsEnabled=1 AND ParentPageId=@ParentPageId
		IF(@PageOrder IS NULL) SET @PageOrder = 1
		INSERT INTO Core_Page (IsEnabled,ModulePageId,ParentPageId,PageOrder,IsAccess,IsAdd,IsEdit,IsDelete,Icon,Code,Name,AliasPath)
		VALUES(1,@ModulePageId,@ParentPageId,@PageOrder,1,1,1,1,null,@CodePage,@NamePage,@PathPage)
	END
	ELSE 
	BEGIN	
		SELECT @PageOrder=(MAX(PageOrder)+1) FROM Core_Page WHERE IsEnabled=1 AND ParentPageId=@ParentPageId
		IF(@PageOrder IS NULL) SET @PageOrder = 1
		INSERT INTO Core_Page (IsEnabled,ModulePageId,ParentPageId,PageOrder,IsAccess,IsAdd,IsEdit,IsDelete,Icon,Code,Name,AliasPath)
		VALUES(1,@ModulePageId,@ParentPageId,@PageOrder,1,1,1,1,null,@CodePage,@NamePage,@PathPage)
	END
END
GO
UPDATE Post_ListCustomerPaymentType SET IsEnabled=0 WHERE Id=3
GO
update Core_Page set AliasPath='/bao-cao/bao-cao-cod-phai-nop',Code='bao-cao-cod-phai-nop' WHERE AliasPath LIKE '/bao-cao/bao-cao-cod-phai-thu'
GO