cp http-driver.service /etc/systemd/system/http-driver.service
dotnet publish -c Release -r linux-x64 --self-contained=true -p:PublishSingleFile=true -p:GenerateRuntimeConfigurationFiles=true -o bin
systemctl daemon-reload
systemctl start http-driver
systemctl enable http-driver
