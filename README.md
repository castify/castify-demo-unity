# castify-demo-unity

Unity から Castify プラットフォーム上に配信を行うサンプルです。

## 使い方

[BroadcastingDriver.cs](Assets/CastifyDemo/Scripts/BroadcastingDriver.cs) を開き以下の箇所に API トークンを設定します。

```csharp
private static API_TOKEN = /* ... */;
```

Unity のメニューから File → Build Settings → iOS: Build を実行し Xcode プロジェクトを生成＆開きます。そして、適切な Signing の設定を行ってから実機にインストールします。
