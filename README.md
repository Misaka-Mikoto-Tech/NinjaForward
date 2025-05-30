## Ninja转发程序

由于新版 Android Studio 无法自定义 ninja.exe 进程的启动参数，但有时确实由此需求，比如想控制 clang++.exe 的工作进程数量（我的电脑默认进程会导致 clang++ 频繁崩溃）

### 使用步骤
1. 编译此工程, 如需 native aot 则命令行执行 `dotnet publish -f net8.0 -r win-x64 -c Release`
2. 打开 ninja.exe 所在目录，比如：`D:\Android_Sdk\cmake\3.18.1\bin`
3. 把 `ninja.exe` 改名为 `ninja_ori.exe`
4. 复制此工程生成的 exe 到目录，改名为 `ninja.exe`，同时创建 `NinjaArg.cfg` 用于写入附加参数, 比如写入 `-j10` 即为限制 clang++.exe 的进程数量为10，其它参数请参考 google 官方文档

### 参考
* https://stackoverflow.com/a/59174124/1062531

>由于 android studio 启动 ninja 时使用的为 `ninja.exe` 全名，因为通过创建 `ninja.bat` 转发的方式无效，故有此项目