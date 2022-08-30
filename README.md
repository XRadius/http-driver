# http-driver

Linux driver for *`process`/`region`/`memory`* inspection & manipulation via HTTP.

# Warnings

This driver provides memory access for ***all running processes*** over HTTP. 

### (1) This should not be used on public networks

This driver utilizes [Basic access authentication](https://en.wikipedia.org/wiki/Basic_access_authentication) to verify users. The `Username` and `Password` are passed in clear-text over the network, which is generally regarded as being insecure. While this may be fine on networks you have control over, running this driver on public networks should be avoided.

### (2) Change the default Username / Password

Please change the default `Username` & `Password` from "guest" to something else. It is your last line of defense to prevent malicious users from modifying/reading your system memory. 

### (3) Be wary when using 3rd party tools

This driver is not locked to any single process. Assume that websites utilizing this driver contain malicious code. The only "trusted" tool utilizing this driver is [http-game-apex](https://xradius.github.io/http-game-apex/).

# Installation

This guide is written for *Ubuntu* and *Arch Linux*. For other Linux flavors, adapt commands where needed.

## (1) Allow Root Login

We'll ensure that the `root` user can login.

1. Configure your `root` user password:

```
sudo passwd
```

See [this page for more information](https://www.cyberciti.biz/faq/how-can-i-log-in-as-root/) on the root user.

## (2) Enable Process Isolation

We'll ensure that non-root users are unable to see the `http-driver` service.

1. Install dependencies:

```
sudo apt install -y vim  # Ubuntu based distros
or
sudo pacman -S vim # Arch based distros
```


2. Hide `root` processes for non-root users:

Some components might not work when hiding `/proc`, like mounting a drive via as example Dolphin. This can be bypassed by mounting `/proc` only when necessary (Method 1), other than always hiding `/proc` on boot via fstab (Method2).

### Method 1 (Has to be done again after a restart):


```
sudo mount -o remount,rw,nosuid,nodev,noexec,relatime,hidepid=2 /proc
```


### Method 2 (Will always hide processes at boot, might break things):


Open `/etc/fstab` with *vim*:

```
sudo vim /etc/fstab
```

Add this below in the text file:
```
proc /proc proc defaults,nosuid,nodev,noexec,relatime,hidepid=2 0 0
```

Reboot your system:

```
reboot
```

3. Check that your non-root user cannot see root processes:

```
ps aux
```

See [this page for more information](https://www.kernel.org/doc/Documentation/filesystems/proc.txt) on process isolation.

## (3) Disable Process Tracing

We'll ensure that non-root users cannot use `ptrace` capabilities.

1. Open `/etc/sysctl.d/10-ptrace.conf` with *vim*:

```
sudo vim /etc/sysctl.d/10-ptrace.conf
```

2. Change the `kernel.yama.ptrace_scope` value to `2`:

```
kernel.yama.ptrace_scope = 2
```

3. Reboot your system:

```
reboot
```

4. Check that the `ptrace_scope` is set to `2`:

```
sysctl kernel.yama.ptrace_scope
```

See [this page for more information](https://www.kernel.org/doc/Documentation/security/Yama.txt) on process tracing.

## (4) Install .NET

We'll ensure that `http-driver` can be compiled with *.NET*.

1. Add the *Microsoft* package repositories:

* See https://docs.microsoft.com/en-us/dotnet/core/install/linux.
* Be sure to carefully follow instructions for your Linux flavor.

2. Install *.NET 6.0*:

```
sudo apt update && apt install -y dotnet-sdk-6.0  # Ubuntu based distros
or
sudo pacman -S dotnet-sdk-6.0  # Arch based distros
```

## (5) Install Service

We'll install `http-driver` and register it as a service:

1. Install dependencies:

```
sudo apt install -y git  # Ubuntu based distros
or
sudo pacman -S git  # Arch based distros
```

2. Switch to `root` user:

```
su
```

3. Open the `/root` directory: 

```
cd ~
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

9. Optional (If Method 1 is used):

You should disable the service when using Method 1, as the processes will not be hidden automatically on boot, and just when using the mount command. Disable the automatic starting of the service with the command below. Else this might get you banned.

```
sudo systemctl disable <system name you specified before>
```
and


```
sudo systemctl start <system name you specified before> 
```

Use this every time you want to use the driver again !!! DON'T USE WHEN ROOT PROCESSES AREN'T HIDDEN IN "ps aux" !!!


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
