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


