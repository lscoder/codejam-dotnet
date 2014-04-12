REM rmdir submit /q /s
REM mkdir submit
del submit\*.out /f /q
del submit\*.rar /f /q
mkdir submit\src
xcopy src submit\src /s


cd submit\src

for /f "tokens=*" %%a in ('dir LSCoder.CodeJam.ConsoleProcessor\outputFiles\*.out /b /od') do set newest=%%a
copy LSCoder.CodeJam.ConsoleProcessor\outputFiles\%newest% ..
REM copy LSCoder.CodeJam.ConsoleProcessor\outputFiles\Solution.out submit

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