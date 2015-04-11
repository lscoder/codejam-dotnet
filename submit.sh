rm submit/*.out
rm submit/*.zip
rm -rf submit/src
cp -rp src/ submit/src


cd submit

ls -t src/LSCoder.CodeJam.ConsoleProcessor/outputFiles/*.out | head -n 1 | xargs -I % cp % ./

rm -rf src/LSCoder.CodeJam.6.0.ReSharper.user
rm -rf src/LSCoder.CodeJam.ConsoleProcessor/bin
rm -rf src/LSCoder.CodeJam.ConsoleProcessor/obj
rm -rf src/LSCoder.CodeJam.ConsoleProcessor/inputFiles
rm -rf src/LSCoder.CodeJam.ConsoleProcessor/outputFiles

zip -r src.zip src/
rm -rf src

# Exit from 'submit' folder
cd ..