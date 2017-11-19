if exist MASRunning.txt (
    rem file exists
) else (
   	type NUL > MASRunning.txt
	start /wait javaw -jar "%~dp0server.jar"
	del EmptyFile.txt
)
