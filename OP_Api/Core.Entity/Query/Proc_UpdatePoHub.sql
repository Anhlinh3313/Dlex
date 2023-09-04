SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_UpdatePoHub
(
	@PoHubId INT = NULL
)
AS
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @tableStationHub TABLE (Id INT);
	DECLARE @tableHubRouting TABLE (Id INT);
	DECLARE @CountPoHub INT = 0;

	SELECT @CountPoHub = COUNT(1) FROM dbo.Core_Hub WHERE Id = @PoHubId AND IsEnabled=1 AND @PoHubId IS NOT NULL;
	IF(@CountPoHub=0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin chi nhánh không tồn tại';
	END
	--update chi nhánh
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0
	WHERE Id = @PoHubId
	--update kho trạm
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0
	WHERE POHubId = @PoHubId
	INSERT INTO @tableStationHub (Id) (SELECT Id FROM dbo.Core_Hub AS Hub WHERE Hub.POHubId = @PoHubId)
	--update khu vực khi kho trạm đã xoá
	UPDATE dbo.Core_HubRoute
	SET IsEnabled = 0
	WHERE HubId IN (SELECT Id FROM @tableStationHub)
	--update tuyến giao
	UPDATE dbo.Core_HubRouting
	SET IsEnabled = 0
	WHERE HubId IN (SELECT Id FROM @tableStationHub)
	INSERT INTO @tableHubRouting (Id) (SELECT Id FROM dbo.Core_HubRouting AS hr WHERE hr.HubId IN (SELECT id FROM @tableStationHub))
	--update các tuyến giao nhận đã phân
	UPDATE dbo.Core_HubRoutingWard
	SET IsEnabled = 0
	WHERE HubRoutingId IN (SELECT Id FROM @tableHubRouting)
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO
