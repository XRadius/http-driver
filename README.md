# http-driver

Linux driver for *`process`/`region`/`memory`* inspection & manipulation via HTTP.

# Installation

This guide is written for *Ubuntu*. For other Linux flavors, adapt commands where needed.

## (1) Allow Root Login

We'll ensure that the `root` user can login.

1. Configure your `root` user password:

```
sudo passwd
```

See [this page for more information](https://www.cyberciti.biz/faq/how-can-i-log-in-as-root/) on the root user.

## (2) Enable Process Isolation

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

See [this page for more information](https://www.kernel.org/doc/Documentation/filesystems/proc.txt) on process isolation.

## (3) Disable Process Tracing

We'll ensure that non-root users cannot use `ptrace` capabilities.

1. Switch to the `root` user:

```
su
```

2. Open `/etc/sysctl.d/10-ptrace.conf` with *vim*:

```
vim /etc/sysctl.d/10-ptrace.conf
```

3. Change the `kernel.yama.ptrace_scope` value to `2`:

```
kernel.yama.ptrace_scope = 2
```

4. Reboot your system:

```
reboot
```

5. Check that the `ptrace_scope` is set to `2`:

```
sysctl kernel.yama.ptrace_scope
```

See [this page for more information](https://www.kernel.org/doc/Documentation/security/Yama.txt) on process tracing.

## (4) Install .NET

We'll ensure that `http-driver` can be compiled with *.NET*.

1. Switch to the `root` user:

```
su
```

2. Add the *Microsoft* package repositories:

* See https://docs.microsoft.com/en-us/dotnet/core/install/linux.
* Be sure to carefully follow instructions for your Linux flavor.

3. Install *.NET 6.0*:

```
apt update && apt install -y dotnet-sdk-6.0
```

## (5) Install Service

We'll install `http-driver` and register it as a service:

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
vim src/appsettings.json
```

7. Enable execution of the *installation script*:

```
chmod +x service-install.sh
```

8. Run the *installation script* and follow the instructions:

```
./service-install.sh
```

Once you've followed these instructions, `http-driver` is ready for use!

# Updating

We'll update `http-driver` and register it as a service:

1. Switch to `root` user:

```
su
```

2. Open the `http-driver` directory:

```
cd ~/http-driver
```

3. Enable execution of the *uninstallation script*:

```
chmod +x service-uninstall.sh
```

4. Run the *uninstallation script* and follow the instructions:

```
./service-uninstall.sh
```

5. Remove your changes:

```
git reset --hard
```

6. Update this repository:

```
git pull
```

7. Change `username` and `password` in `appsettings.json`:

```
vim src/appsettings.json
```

8. Run the *installation script* and follow the instructions:

```
./service-install.sh
```

Once you've followed these instructions, `http-driver` is ready for use again!

# Usage

Navigate to [http://0.0.0.0:8080/](http://0.0.0.0:8080/). Replace `0.0.0.0` for your *network-resolvable* IP.
