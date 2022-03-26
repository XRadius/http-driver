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

See [this FAQ for more information](https://www.cyberciti.biz/faq/how-can-i-log-in-as-root/) on the root user.

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

See [this FAQ for more information](https://www.cyberciti.biz/faq/linux-hide-processes-from-other-users/) on process isolation.

## (3) Install .NET

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

## (4) Install Service

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

# Usage

Open [http://127.0.0.1:8080/swagger](http://localhost:8080/swagger) to view the *OpenAPI* specification.
