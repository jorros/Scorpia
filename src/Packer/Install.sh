dotnet pack
dotnet tool uninstall -g Scorpia.Engine.Packer
dotnet tool install --global --add-source ./nupkg Scorpia.Engine.Packer
