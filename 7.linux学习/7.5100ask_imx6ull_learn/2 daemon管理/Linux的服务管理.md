## 1、说在前面

### 1.1 Linux开发板系统的一般启动过程

​	开发板在上电之后，经历u-boot→使用内核镜像启动内核→内核启动init程序→init程序完成系统服务的启动

### 1.2 init程序的说明

init程序的进程号为1，以daemon的形式存在

在很长一段时间，大部分Linux发行版均使用System V定义的init进程。

后面经过 Lennart Poettering带头开发的systemd成为init进程的主流。

## 2、systemd的介绍

让我们来详细解释一下 `systemd` 及其相关概念。

`systemd` 是现代 Linux 系统中的系统和服务管理器，负责启动、停止、管理系统上的服务和进程。它使用名为 `Unit` 的配置文件来定义和管理这些服务和系统资源。

### Unit 的种类

`Unit` 是 `systemd` 的核心概念，每个 `Unit` 文件描述了一个特定的服务、挂载点、设备或其他资源。常见的 `Unit` 类型包括：

1. **Service（服务）**：表示一个服务，如 `sshd.service` 表示 SSH 服务。
2. **Target（目标）**：表示一个运行模式，类似于传统的运行级别（runlevel），如 `multi-user.target` 表示多用户模式。
3. **Mount（挂载点）**：表示一个文件系统挂载点，如 `/etc/fstab` 中定义的挂载点。
4. **Timer（计时器）**：表示一个定时任务，如 `cron` 定时任务的替代方案。
5. **Snapshot（快照）**：表示系统状态的快照，可以用于恢复系统状态。
6. **Path（路径）**：表示一个路径监控器，当路径下的文件发生变化时触发特定操作。
7. **Socket（套接字）**：表示一个套接字文件，用于在网络服务启动前进行套接字激活。
8. **Swap（交换分区）**：表示一个交换分区或交换文件。

### 通过 systemctl 管理 Unit

`systemctl` 是管理 `systemd` 的命令行工具，可以用来启动、停止、重启和检查 `Unit` 的状态。

#### 常用命令

- **启动服务**：
  ```sh
  sudo systemctl start <service-name>
  ```
  例如：
  ```sh
  sudo systemctl start sshd
  ```

- **停止服务**：
  ```sh
  sudo systemctl stop <service-name>
  ```
  例如：
  ```sh
  sudo systemctl stop sshd
  ```

- **重启服务**：
  ```sh
  sudo systemctl restart <service-name>
  ```
  例如：
  ```sh
  sudo systemctl restart sshd
  ```

- **查看服务状态**：
  ```sh
  sudo systemctl status <service-name>
  ```
  例如：
  ```sh
  sudo systemctl status sshd
  ```

- **启用服务（开机自启）**：
  ```sh
  sudo systemctl enable <service-name>
  ```
  例如：
  ```sh
  sudo systemctl enable sshd
  ```

- **禁用服务（开机不自启）**：
  ```sh
  sudo systemctl disable <service-name>
  ```
  例如：
  ```sh
  sudo systemctl disable sshd
  ```

### 创建自定义 Service

如果你需要添加一个启动程序，可以定义一个 `service` 服务。下面是一个示例：

1. 创建一个新的服务单元文件，例如 `myservice.service`，放在 `/etc/systemd/system/` 目录下：

   ```ini
   [Unit]
   Description=My Custom Service
   After=network.target

   [Service]
   ExecStart=/usr/bin/myprogram
   Restart=always

   [Install]
   WantedBy=multi-user.target
   ```

   - `[Unit]` 部分描述服务的基本信息和依赖关系。
   - `[Service]` 部分定义服务的启动命令和行为。
   - `[Install]` 部分指定服务的安装信息和目标。

2. 重新加载 `systemd` 配置：

   ```sh
   sudo systemctl daemon-reload
   ```

3. 启动并启用服务：

   ```sh
   sudo systemctl start myservice
   sudo systemctl enable myservice
   ```

### 总结

- `Unit` 是 `systemd` 的核心配置文件，用于定义和管理服务及其他系统资源。
- `systemctl` 是管理 `systemd` 的命令行工具，可以用于启动、停止、重启和检查 `Unit` 的状态。
- 创建自定义 `service` 服务可以通过定义一个新的 `.service` 文件并将其放置在 `/etc/systemd/system/` 目录中。

## 3、systemd中service文件的详细使用说明

当然，可以详细解释一下 `service` 文件的内容。一个 `service` 文件主要包含三个部分：`[Unit]`、`[Service]` 和 `[Install]`。每一部分都有其特定的用途和选项。以下是对每个部分和相关选项的详细说明。

### `[Unit]` 部分

`[Unit]` 部分包含与服务相关的元数据和依赖关系。这部分主要用于描述服务的基本信息，以及定义服务的启动顺序和依赖关系。

- **Description**：描述服务的简短文本说明。
- **After**：定义服务启动的顺序。`After=network.target` 表示该服务应在 `network.target` 之后启动。

示例：

```ini
[Unit]
Description=My Custom Service
After=network.target
```

### `[Service]` 部分

`[Service]` 部分定义服务的行为和属性。它包含如何启动、停止和管理服务的详细信息。

- **ExecStart**：指定启动服务的命令。必须是绝对路径。
- **Restart**：定义服务在何种情况下重启。常见的选项包括：
  - `no`：服务不会重启。
  - `on-success`：服务在正常退出时重启。
  - `on-failure`：服务在非正常退出时重启。
  - `always`：无论服务如何退出，总是重启。

其他常见选项：

- **ExecStop**：指定停止服务的命令。
- **ExecReload**：指定重新加载服务配置的命令。
- **User**：指定运行服务的用户。
- **Group**：指定运行服务的用户组。
- **WorkingDirectory**：指定服务运行的工作目录。

示例：

```ini
[Service]
ExecStart=/usr/bin/myprogram
Restart=always
```

### `[Install]` 部分

`[Install]` 部分定义服务的安装信息，包括服务在哪些目标（targets）下启动。这部分通常用于配置服务的启用和禁用行为。

- **WantedBy**：指定服务所属的目标。`multi-user.target` 是一个常用的目标，表示服务在多用户模式下启动。

示例：

```ini
[Install]
WantedBy=multi-user.target
```

### 完整示例解析

让我们再次看看完整的示例，并逐行解释：

```ini
[Unit]
Description=My Custom Service
After=network.target

[Service]
ExecStart=/usr/bin/myprogram
Restart=always

[Install]
WantedBy=multi-user.target
```

#### `[Unit]` 部分

- `Description=My Custom Service`：描述服务为“自定义服务”。
- `After=network.target`：确保此服务在 `network.target` 之后启动，即网络服务启动后再启动此服务。

#### `[Service]` 部分

- `ExecStart=/usr/bin/myprogram`：当启动此服务时，执行 `/usr/bin/myprogram`。
- `Restart=always`：无论服务如何退出（正常或非正常），总是重启该服务。

#### `[Install]` 部分

- `WantedBy=multi-user.target`：指定此服务应在 `multi-user.target` 目标下启动，即在多用户模式下启动。

### 创建和管理自定义服务

1. **创建服务文件**：

   在 `/etc/systemd/system/` 目录下创建一个新的服务文件，例如 `myservice.service`：

   ```sh
   sudo nano /etc/systemd/system/myservice.service
   ```

   将上述配置内容粘贴到文件中并保存。

2. **重新加载 systemd 配置**：

   ```sh
   sudo systemctl daemon-reload
   ```

   这是必要的步骤，使 systemd 识别新的服务文件。

3. **启动服务**：

   ```sh
   sudo systemctl start myservice
   ```

4. **启用服务（使其在启动时自动运行）**：

   ```sh
   sudo systemctl enable myservice
   ```

5. **检查服务状态**：

   ```sh
   sudo systemctl status myservice
   ```

这就完成了自定义 `service` 服务的创建和管理。通过理解和使用 `systemd`，你可以更有效地管理和控制系统上的各种服务和资源。