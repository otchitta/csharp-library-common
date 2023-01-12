dotnet tool install --global dotnet-reportgenerator-globaltool
dotnet tool update --global dotnet-reportgenerator-globaltool

dotnet add package converlet.msbuild

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./Result.File/20221004_212600/NUnitResult.xml
reportgenerator -reports:./Result.File/20221004_212600/NUnitResult.xml -targetdir:./Result.File/20221004_212600 -reporttypes:Html

出典: https://qiita.com/suganury/items/62e0770d16eb579799d8

