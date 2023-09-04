(robocopy ..\Publish\runtimes D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\runtimes /e) ^& IF %ERRORLEVEL% LSS 8 SET ERRORLEVEL = 0
(robocopy ..\Publish\Api D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\Api /e) ^& IF %ERRORLEVEL% LSS 8 SET ERRORLEVEL = 0
copy ..\Publish\*.dll D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\
copy ..\Publish\*.pdb D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\
copy ..\Publish\Core.Api.exe D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\Core.Api.exe
copy ..\Publish\Core.Api.deps.json D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\Core.Api.deps.json
copy ..\Publish\Core.Api.runtimeconfig.json D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\Core.Api.runtimeconfig.json
copy ..\Publish\dev-dlex-pub.json D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\dev-dlex-pub.json
copy ..\Publish\web.config D:\inetpub\vhosts\GSDP_Dev\GSDPstg-PostApi.dsc.vn\web.config
