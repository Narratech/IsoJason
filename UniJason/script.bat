if exist EmptyFile.txt (
    rem file exists
) else (
   	type NUL > EmptyFile.txt
	start /wait javaw -jar "%~dp0jason\jedit\jedit.jar"
	del EmptyFile.txt
)