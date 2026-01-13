# Screenbits Unity Controller

Simple Unity integration for Screenbits screen recorder (Microsoft Store version).

## Features

- Launch Screenbits
- Start/Stop recording
- Pause/Resume recording
- Background mode support

## Requirements

- Windows 10/11
- Screenbits (Microsoft Store version)
- Unity 2021.3+

## Usage

```csharp
using Screenbits.Unity;

var controller = ScreenbitsController.Instance;

// Check if Screenbits is running
bool running = controller.IsScreenbitsRunning();

// Launch Screenbits
controller.Launch();

// Start/Stop recording
controller.StartRecording();
controller.StopRecording();

// Pause/Resume
controller.PauseRecording();
controller.ResumeRecording();

// Toggle shortcuts
controller.ToggleRecording();
controller.TogglePause();
```

## Options

```csharp
// Background mode (no Screenbits UI window)
controller.BackgroundMode = true;

// Override per-call
controller.StartRecording(background: false);
```

## Events

```csharp
controller.OnStateChanged += (state) => {
    Debug.Log($"State: {state}");
};
```

## Output/Settings

Configure recording output folder, video quality, and capture settings in the Screenbits application UI.
