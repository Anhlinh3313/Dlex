SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_GetHubByUserId]
(
	@UserId INT = NULL

)AS
BEGIN
	SELECT h.Id, h.Name AS HubName
	FROM dbo.Core_Hub AS h
	INNER JOIN dbo.Core_User AS u ON u.HubId = h.Id AND u.IsEnabled = 1
	WHERE h.IsEnabled = 1 AND u.Id = @UserId 
END
GO
