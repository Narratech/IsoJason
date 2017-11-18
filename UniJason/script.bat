if exist EmptyFile.txt (
    rem file exists
) else (
   	type NUL > EmptyFile.txt
	start /wait javaw -jar "%~dp0server.jar"
=======
	start /wait javaw -jar "%~dp0jason\jedit\jedit.jar"
>>>>>>> d4938ae9be63d7e6104ca1f89ba433d11f55e3d5
	del EmptyFile.txt
