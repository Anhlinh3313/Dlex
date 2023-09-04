SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_UpdateStationHub
(
	@StationHubId INT = NULL
)
AS
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @tableHubRouting TABLE (Id INT);
	DECLARE @CountStationHub INT = 0;

	SELECT @CountStationHub = COUNT(1) FROM dbo.Core_Hub WHERE Id = @StationHubId AND IsEnabled=1 AND @StationHubId IS NOT NULL;
	IF(@CountStationHub=0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin kho trạm không tồn tại';
	END
	--update kho trạm
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0
	WHERE id = @StationHubId
	--update khu vực khi kho trạm đã xoá
	UPDATE dbo.Core_HubRoute
	SET IsEnabled = 0
	WHERE HubId = @StationHubId
	--update tuyến giao
	UPDATE dbo.Core_HubRouting
	SET IsEnabled = 0
	WHERE HubId = @StationHubId
	INSERT INTO @tableHubRouting (Id) (SELECT Id FROM dbo.Core_HubRouting AS hr WHERE hr.HubId = @StationHubId)
	--update các tuyến giao nhận đã phân
	UPDATE dbo.Core_HubRoutingWard
	SET IsEnabled = 0
	WHERE HubRoutingId IN (SELECT Id FROM @tableHubRouting)
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO
