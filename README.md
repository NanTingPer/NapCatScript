# NapCatScript
基于NapCat的QQBot

# 插件开发

目前，`NapCatScript`可以加载基于`NapCatScript`开发的插件，具体如何开发可以查看源码[TestPlugin](https://github.com/NanTingPer/NapCatScript/blob/main/TestPlugin/TestClass.cs)

1. 下载源码

2. 创建项目

3. 添加项目引用，引用本项目的`Start` (`NapCatScript.Start`)

4. 创建类，继承`PluginType`，此类位于 `NapCatScript.Start.PluginType`

5. 实现给定方法，在`Run`方法中，是主要操作

6. 生成项目，在`TestPlugin\bin\Debug\net8.0`中找到 `项目名称.dll` `项目名称.pdb` `项目名称.deps.json`

   拷贝到 `NapCatScript`程序目录下的`Plugin\项目名称`下，如果不存在需要自己创建

7. 运行 `NapCatScript.Start`，如果你在`Init`方法中进行了控制台输出，那应该可以看到内容



# `API`支持

> 目前仅对少数API进行了封装

在`PluginType`中，有一个`Send`属性，其中包含了部分已经封装的方法
手动发送请求，可以调用`NapCatScript.Handle.Parses.SendMesg`类中的方法
发送纯文本，可以调用`NapCatScript.Handle.Utils.SendTextAsync`

# 使用

首次打开会在应用程序根目录下生成`Conf.conf`文件，可以像这样填写

- `SocketUri` 需要在 `NapCat` 的网络配置中添加`WebSocket服务器`
- `HttpServerUri` 需要在 `NapCat` 的网络配置中添加 `Http服务器`

```cs
SocketUri=ws://127.0.0.1:9999
HttpServerUri=http://127.0.0.1:9998/
```

设置完成后再次打开即可

日志位于应用根目录的`Log.log`中

对于其他配置项目，都是可选项



# 灾厄Wiki页

对于灾厄Wiki的支持，需要将灾厄Wiki页面存放在程序根目录的`Cal`下，对于灾厄Wiki图片的获取，需要自行解决
或者使用`https://github.com/NanTingPer/Learning-Notes/blob/master/codeor/Cshps/HTML2JPG/Program.cs` 截取网页，最终全部文件需要统一存放到根目录`Cal`下
