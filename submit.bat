rmdir submit /q /s
mkdir submit
mkdir submit\src
copy src\LSCoder.CodeJam.ConsoleProcessor\outputFiles\Solution.txt submit
xcopy src submit\src /s

cd submit\src

del LSCoder.CodeJam.6.0.ReSharper.user /q
rmdir LSCoder.CodeJam.ConsoleProcessor\bin /q /s
rmdir LSCoder.CodeJam.ConsoleProcessor\obj /q /s
rmdir LSCoder.CodeJam.ConsoleProcessor\inputFiles /q /s
rmdir LSCoder.CodeJam.ConsoleProcessor\outputFiles /q /s

REM Exits from 'src' folder
cd ..

"C:\Program Files\WinRAR\WinRAR.exe" a -r sourceCode.rar src
rmdir src /q /s

REM Exits from 'submit' folder
cd ..

