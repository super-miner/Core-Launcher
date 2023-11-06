set path=%~1
set path=%path:__= %
"%path%\steamcmd" +login %~2 %~3 +quit