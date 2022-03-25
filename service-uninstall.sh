#!/bin/bash
read -p "ServiceName: " serviceName

# ====================
#
# ====================
systemctl disable "${serviceName}"
systemctl stop "${serviceName}"

# ====================
#
# ====================
rm -rf "/root/.${serviceName}"
rm -rf "/etc/systemd/system/${serviceName}.service"
