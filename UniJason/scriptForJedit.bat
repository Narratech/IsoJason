if exist EmptyFile.txt (
    rem file exists
) else (
   	type NUL > EmptyFile.txt
	start /wait java jason.infra.centralised.RunCentralisedMAS "%~dp0JasonIsoMonks-master\agentes.mas2j
	del EmptyFile.txt
)
pause