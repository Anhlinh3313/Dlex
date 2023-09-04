SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER Proc [dbo].[Proc_Hub_RoutingWard_Availability]
(
  @HubId INT,
  @HubRoutingId INT,
  @IsTruckDelivery BIT = NULL
)
AS
SET NOCOUNT ON
	IF (@IsTruckDelivery IS NULL) SET @IsTruckDelivery = 0
  IF(@HubId = '')
    SET @HubId = 0;
  IF(@HubRoutingId = '')
    SET @HubRoutingId = 0;
  DECLARE @TableHub TABLE(Id INT)
  DECLARE @TableWard TABLE(Id INT)
  INSERT INTO @TableHub SELECT hub.Id FROM Core_Hub AS hub WHERE hub.IsEnabled=1 AND (hub.CenterHubId = @HubId OR hub.POHubId = @HubId OR hub.Id=@HubId)
  INSERT INTO @TableWard SELECT HubRoutingWard.WardId
    FROM dbo.Core_HubRoutingWard HubRoutingWard WITH(NOLOCK)
	INNER JOIN dbo.Core_HubRouting as HubRouting ON HubRouting.Id = HubRoutingWard.HubRoutingId AND HubRouting.IsEnabled=1
    WHERE HubRouting.Id <> @HubRoutingId 
	AND HubRoutingWard.IsEnabled=1  AND HubRouting.IsTruckDelivery = @IsTruckDelivery
  
  SELECT 
    HubRoute.Id,
    HubRoute.HubId,
    HubRoute.WardId,
    Ward.Name AS WardName,
    Ward.DistrictId,
    District.Name AS DistrictName,
    District.ProvinceId AS ProvinceId,
    Province.Name AS ProvinceName,
    (SELECT TOP 1 HubRoutingId
	FROM dbo.Core_HubRoutingWard HubRoutingWard 
	WHERE HubRoutingWard.WardId = HubRoute.WardId
	AND HubRoutingWard.IsEnabled=1 AND HubRoutingWard.HubRoutingId=@HubRoutingId) AS HubRoutingId
    FROM dbo.Core_HubRoute AS HubRoute WITH(NOLOCK) 
	INNER JOIN dbo.Core_Ward Ward ON Ward.Id = HubRoute.WardId and Ward.IsEnabled=1 
	INNER JOIN dbo.Core_District District ON District.Id = Ward.DistrictId and District.IsEnabled=1 
	INNER JOIN dbo.Core_Province Province ON Province.Id = District.ProvinceId and Province.IsEnabled=1
    WHERE HubRoute.IsEnabled=1
	and (exists (SELECT Id FROM @TableHub where Id = HubRoute.HubId)) 
	AND (not exists (SELECT Id FROM @TableWard where Id = WardId)) 
  ORDER BY Province.Name,District.Name,Ward.Name ASC
GO



