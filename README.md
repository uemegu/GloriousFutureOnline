# Glorious Future Online

これは [続：どこまでショボいアプリがAppleの審査に通るのか試してみた](https://qiita.com/megumu-u/items/29c28ed9b2b3830769f7) で作ったXamarinアプリをBlazor(client-side)でWEBアプリ化したものです。

## Blazorのビルド

BlazorProject内でビルド

````
$ dotnet --version
3.0.100-preview6-012264

$ dotnet build
bin配下にビルドされる

$ dotnet run
Blazorアプリの単体確認
````


## Firebaseと連携

bin配下の_frameworkをFirebaseプロジェクトに移動して動かす

````
$ firebase --version
6.10.0

$ mv bin/Debug/netstandard2.0/dist/_framework Firebase/public

$ cd Firebase

$ firebase serve
Firebaseと連携した確認
````



## Blazor→JavaScriptへの連携

こんな感じで連携できます。
単純にJavaScriptのメソッドを呼び出すだけです。

https://github.com/uemegu/GloriousFutureOnline/blob/master/BlazorProject/ViewModels/PredictModel.cs#L81

````csharp
_jsRuntime.InvokeAsync<object>("notifyPredict", Word.Day, Word.ToString());
````

## JavaScript→Blazorへの連携

`DotNetObjectRef.Create(ラップしたいオブジェクト)`でJavaScriptから呼び出したいメソッドを持ったインスタンスをラップして、`JSRuntime.InvokeAsync`でオブジェクトをJavaScriptの世界に渡します。

https://github.com/uemegu/GloriousFutureOnline/blob/master/BlazorProject/Pages/Index.razor#L23

````csharp
await JSRuntime.InvokeAsync<object>( "passHelper", DotNetObjectRef.Create(viewModel));
````

https://github.com/uemegu/GloriousFutureOnline/blob/master/BlazorProject/Firebase/public/index.html#L57

````javascript
function passHelper(helper) {
    window.helper = helper;
````


JavaScriptの世界では受け取ったオブジェクトの`invokeMethod`でBlazorのメソッドが呼べるようなります。
https://github.com/uemegu/GloriousFutureOnline/blob/master/BlazorProject/Firebase/public/index.html#L61

````javascript
window.helper.invokeMethod("SetWord", window.predict);
````


