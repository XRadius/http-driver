# http-driver

Linux driver for *`process`/`region`/`memory`* inspection & manipulation via HTTP.

## Installation

This guide is written for *Ubuntu 20.04*.

### [01]: Prerequisites

We'll ensure that the `root` user can login.

1. Configure your `root` user password:

```
sudo passwd
```

See [this FAQ for more information](https://www.cyberciti.biz/faq/how-can-i-log-in-as-root/) on the root user.

### [02] Process Isolation

We'll ensure that non-root users are unable to see the `http-driver` service.

1. Switch to the `root` user:

```
su
```

2. Install dependencies:

```
apt install -y vim
```

3. Open `/etc/fstab` with *vim*:

```
vim /etc/fstab
```

4. Add the following line:

```
proc /proc proc defaults,nosuid,nodev,noexec,relatime,hidepid=2 0 0
```

5. Reboot your system:

```
reboot
```

6. Check that your non-root user cannot see root processes:

```
ps aux
```

See [this FAQ for more information](https://www.cyberciti.biz/faq/linux-hide-processes-from-other-users/) on process isolation.

### [03] Preparing .NET

We'll ensure that `http-driver` can be compiled with *.NET*.

1. Switch to the `root` user:

```
su
```

2. Add the *Microsoft* repositories:

```
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```

3. Install *.NET*:

```
apt update && apt install -y dotnet-sdk-6.0
```

See [this FAQ for more information](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu) on *.NET*.

### [04] Installation

We'l install `http-driver` and register it as a service:

1. Switch to `root` user:

```
su
```

2. Open the `/root` directory: 

```
cd ~
```

3. Install dependencies:

```
apt install -y git
```

4. Clone this repository:

```
git clone https://github.com/XRadius/http-driver
```

5. Open the `http-driver` directory:

```
cd ~/http-driver
```

6. Change `username` and `password` in `appsettings.json`:

```
vim appsettings.json
```

7. Enable execution of the *installation script*:

```
chmod +x http-driver.sh
```

8. Run the *installation script* and follow the instructions:

```
./http-driver.sh
```

Once you've followed the instructions, `http-driver` is ready for use!
