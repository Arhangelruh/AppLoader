# AppLoader
This App copies cyrrent folder to destination from json config, and then start some application.

This App can be usful for apps which work in local network, and with for example oracle client.
1) Share folder with app
2) Put Loader into folder with app
3) Put oracle client into folder with app
4) create bat file with paths for oracle and with command for starting app
5) create apploader.json with destination folder and .bat which going to start after copying.
6) create link to Apploader
7) Apploader will copy folder with app and oracle client, then going to start .bat which going to start .exe

   If you need to update app you can update only share folder, every times clients chek files from share folder and update their own folders.

   There is client for .NetFramework 4.8 in the brunch AppLoderNet. 

Apploader.json example:
{
  "TargetPath": "C:\\some-programm",
  "ProcessName": "some.bat"
}

.bat example
@echo off
set PATH=C:\some-programm\instantclient_21_8
set ORACLE_HOME=C:\some-programm\instantclient_21_8
set TNS_ADMIN=C:\some-programm\instantclient_21_8\network\admin
set NLS_LANG=AMERICAN_AMERICA.CL8MSWIN1251
start Some-programm.exe /aliasname=SomeProgr
exit
