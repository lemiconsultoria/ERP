cd ERP.Gateway
start /d "." dotnet run dotnet run --launch-profile "http"
cd ..

cd ERP.Identity.API
start /d "." dotnet run dotnet run --launch-profile "http"
cd ..

cd ERP.Crud.API
start /d "." dotnet run dotnet run --launch-profile "http"
cd ..

cd ERP.Report.API
start /d "." dotnet run dotnet run --launch-profile "http"
cd ..

cd ERP.Consolidator.API
start /d "." dotnet run dotnet run --launch-profile "http"
cd ..