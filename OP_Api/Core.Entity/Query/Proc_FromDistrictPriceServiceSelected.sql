SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_FromDistrictPriceServiceSelected
	@priceServiceId INT
AS
BEGIN
--
DECLARE @TableAreaIds TABLE(Id INT)
INSERT INTO @TableAreaIds SELECT psdt.AreaId FROM Post_PriceServiceDetail psdt 
WHERE psdt.PriceServiceId=@PriceServiceId AND psdt.IsEnabled=1
--
    SELECT 
           farea.Id,
           farea.DistrictId,
           farea.AreaId
    FROM dbo.Post_FromDistrictArea farea
    WHERE (farea.AreaId in(SELECT * FROM @TableAreaIds))
          AND farea.IsEnabled = 1;
END;
GO
