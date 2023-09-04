SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER Proc [dbo].[Proc_GetWardByHubId]
(
  @HubId INT
)
AS
	BEGIN
		SELECT w.Id, p.Name AS ProvinceName, d.Name AS DistrictName, w.Name AS WardName 
		FROM dbo.Core_HubRoute AS hr
		INNER JOIN dbo.Core_Ward AS w ON w.Id = hr.WardId AND w.IsEnabled = 1
		INNER JOIN dbo.Core_District AS d ON d.id = w.DistrictId AND d.IsEnabled = 1
		INNER JOIN dbo.Core_Province AS p ON p.id = d.ProvinceId
		WHERE hr.HubId = @HubId
		GROUP BY p.Name, d.Name, w.Name, w.Id
	END
GO

