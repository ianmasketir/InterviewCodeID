# InterviewCodeID
1. Deploy API before deploy the apps. Please change the "BaseUrl" section in the apps file "appsettings.json" with following:
1a. If API deployed on http, change the https://localhost:7278/ to the api location. Example: http://codeiddev:8322/
1b. If API deployed on https, please deploy it in folder "porect" under "Default Web Site". Change the https://localhost:7278/ to the api location. Example: https://codeiddev:8322/porect/
2. Restore database "Tes.bak" to the db server (MS SQL Server). Change the "SQLConnection" section to the db server connection in file "appsettings.json" in "PORECT.API" project.
3. Log file location setting can be found in "nlog.config" both in "PORECT.API" and "PORECT" projects.
