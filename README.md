# Unity Screenbits

Screenbits スクリーンレコーダー（Microsoft Store版）をUnityから制御するためのシンプルなパッケージです。

## 機能

- Screenbitsの起動
- 録画の開始/停止
- 録画の一時停止/再開
- バックグラウンドモード対応

## 動作要件

- Windows 10/11
- [Screenbits（Microsoft Store版）](https://apps.microsoft.com/detail/9p65dckjftj3?hl=ja-JP&gl=JP)
- Unity 2021.3以降

## インストール

### Unity Package Manager (UPM) を使用

1. Unityエディタで `Window > Package Manager` を開く
2. 左上の `+` ボタンをクリック
3. `Add package from git URL...` を選択
4. 以下のURLを入力:

```
https://github.com/MizoTake/UnityScreenbits.git?path=Packages/com.screenbits.unity
```

### manifest.json を直接編集

`Packages/manifest.json` の `dependencies` に以下を追加:

```json
{
  "dependencies": {
    "com.screenbits.unity": "https://github.com/MizoTake/UnityScreenbits.git?path=Packages/com.screenbits.unity"
  }
}
```

### バージョンを指定する場合

タグを使用してバージョンを固定できます:

```
https://github.com/MizoTake/UnityScreenbits.git?path=Packages/com.screenbits.unity#v1.0.0
```

## 使い方

```csharp
using Screenbits.Unity;

var controller = ScreenbitsController.Instance;

// Screenbitsの起動確認・起動
controller.IsScreenbitsRunning();
controller.Launch();

// 録画制御
controller.StartRecording();
controller.StopRecording();
controller.PauseRecording();
controller.ResumeRecording();

// トグル操作
controller.ToggleRecording();
controller.TogglePause();
```

## オプション

```csharp
// バックグラウンドモード（Screenbits UIを表示しない）
controller.BackgroundMode = true;

// 呼び出し時に上書き
controller.StartRecording(background: false);
```

## イベント

```csharp
controller.OnStateChanged += (state) => {
    Debug.Log($"録画状態: {state}");
};
```

## 出力設定について

録画の出力先フォルダ、画質、キャプチャ設定などはScreenbitsアプリのUI側で行ってください。

## ライセンス

MIT License
