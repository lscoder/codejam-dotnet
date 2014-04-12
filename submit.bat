del submit\*.out /f /q
del submit\*.zip /f /q
mkdir submit\src
xcopy src submit\src /s


cd submit\src

for /f "tokens=*" %%a in ('dir LSCoder.CodeJam.ConsoleProcessor\outputFiles\*.out /b /od') do set newest=%%a
copy LSCoder.CodeJam.ConsoleProcessor\outputFiles\%newest% ..

del LSCoder.CodeJam.6.0.ReSharper.user /q
rmdir LSCoder.CodeJam.ConsoleProcessor\bin /q /s
rmdir LSCoder.CodeJam.ConsoleProcessor\obj /q /s
rmdir LSCoder.CodeJam.ConsoleProcessor\inputFiles /q /s
rmdir LSCoder.CodeJam.ConsoleProcessor\outputFiles /q /s

REM Exits from 'src' folder
cd ..

"C:\Program Files\WinRAR\WinRAR.exe" a -afzip -r sourceCode.zip src
rmdir src /q /s

REM Exits from 'submit' folder
cd ..