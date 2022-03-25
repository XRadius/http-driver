#!/bin/bash
echo "================================================================================"
echo "This installation script will register a system service. When finished, the name"
echo "of this service is readable by any user. To make sure that it cannot be used for"
echo "detection purposes, you have to enter a random service name. Ensure that it does"
echo "not exist already, and only use characters in [0-9A-Z-]."
echo "================================================================================"
read -p "ServiceName: " serviceName

# ====================
# 
# ====================
rootPath="/root/.${serviceName}"
execPath="/root/.${serviceName}/${serviceName}"
servPath="/etc/systemd/system/${serviceName}.service"

# ====================
# 
# ====================
rm -rf "bin"
rm -rf "$rootPath"

# ====================
# 
# ====================
dotnet publish src --output "${rootPath}" --runtime linux-x64 --self-contained \
  "-p:Configuration=Release;AssemblyName=${serviceName}" \
  "-p:DebugType=None" \
  "-p:GenerateRuntimeConfigurationFiles=true" \
  "-p:PublishSingleFile=true"

# ====================
# 
# ====================
cat > $servPath << EOF
[Unit]
Description=${serviceName}

[Service]
Type=notify
WorkingDirectory=${rootPath}
ExecStart=${execPath}
Environment=ASPNETCORE_URLS=http://*:8080/

[Install]
WantedBy=multi-user.target
EOF

# ====================
# 
# ====================
chmod 770 "$servPath"

# ====================
# 
# ====================
systemctl daemon-reload
systemctl start "${serviceName}"
systemctl enable "${serviceName}"
