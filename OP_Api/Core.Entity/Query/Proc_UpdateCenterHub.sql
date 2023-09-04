SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_UpdateCenterHub
(
	@CenterHubId INT = NULL
)
AS
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @tableStationHub TABLE (Id INT);
	DECLARE @tableHubRouting TABLE (Id INT);
	DECLARE @CountCenterHub INT = 0;

	SELECT @CountCenterHub = COUNT(1) FROM dbo.Core_Hub WHERE Id=@CenterHubId AND IsEnabled=1 AND @CenterHubId Is NOT NULL;
	IF(@CountCenterHub=0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin trung tâm không tồn tại';
	END
	--update trung tâm
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0 
	WHERE id = @CenterHubId
	--update chi nhánh
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0
	WHERE CenterHubId = @CenterHubId
	INSERT INTO @tableStationHub (Id) (SELECT Id FROM dbo.Core_Hub AS Hub WHERE Hub.CenterHubId  = @CenterHubId AND Hub.POHubId IS NOT NULL)
	--update kho trạm
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0
	WHERE id IN (SELECT Id FROM @tableStationHub)
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
