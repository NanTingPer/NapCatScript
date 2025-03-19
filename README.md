# NapCatSprcit
使用NapCat作为前端的一个Bot后端，算脚本吗？

# 使用

首次打开会在应用程序根目录下生成`Conf.conf`文件，可以像这样填写

- `SocketUri` 需要在 `NapCat` 的网络配置中添加`WebSocket服务器`
- `HttpServerUri` 需要在 `NapCat` 的网络配置中添加 `Http服务器`

```cs
SocketUri=ws://127.0.0.1:9999
HttpServerUri=http://127.0.0.1:9998/
```

设置完成后再次打开即可



# 灾厄Wiki页

对于灾厄Wiki的支持，需要将灾厄Wiki页面存放在程序根目录的`Cal`下，对于灾厄Wiki图片的获取，需要自行解决
或者使用`https://github.com/NanTingPer/Learning-Notes/blob/master/codeor/Cshps/HTML2JPG/Program.cs` 截取网页，最终全部文件需要统一存放到根目录`Cal`下
