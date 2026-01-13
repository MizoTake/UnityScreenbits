# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Simple Unity package (`com.screenbits.unity`) for controlling Screenbits screen recorder (Microsoft Store version) via URI scheme. Windows-only, Unity 2021.3+.

## Architecture

**ScreenbitsController** (Singleton MonoBehaviour)
- `ScreenbitsController.Instance` - Main API entry point
- Controls recording via `screenbits://` URI scheme executed through Windows explorer
- Checks Screenbits process status via `Process.GetProcessesByName()`
- State: Idle → Starting → Recording ↔ Paused → Stopping → Idle

**API:**
- `IsScreenbitsRunning()` - Check if Screenbits process exists
- `Launch()` - Launch Screenbits application
- `StartRecording(background?)` - Start recording
- `StopRecording()` - Stop recording
- `PauseRecording()` - Pause recording
- `ResumeRecording()` - Resume recording
- `ToggleRecording()` - Toggle start/stop
- `TogglePause()` - Toggle pause/resume

**Property:**
- `BackgroundMode` - Record without Screenbits UI (default: true)

**Event:**
- `OnStateChanged` - Fires when recording state changes

## File Structure

```
Packages/com.screenbits.unity/
└── Runtime/
    ├── Screenbits.Unity.asmdef
    ├── ScreenbitsController.cs
    └── ScreenbitsRecordingState.cs

Assets/Samples/Screenbits/
├── Screenbits.Samples.asmdef
└── ScreenbitsSimpleExample.cs
```

## URI Commands

| Action | URI |
|--------|-----|
| Launch | `screenbits://` |
| Start | `screenbits://start` |
| Start (background) | `screenbits://start/?background` |
| Stop | `screenbits://stop` |
| Pause | `screenbits://pause` |
| Resume | `screenbits://resume` |

## Sample Usage

1. Add `ScreenbitsSimpleExample` to any GameObject
2. Play mode: **L** to launch, **R** to record/stop, **P** to pause/resume

## Design Notes

- Recording output/quality settings are configured in Screenbits UI, not via API
- Platform: Windows Editor and Windows Standalone 64-bit only
- URI executed via `Process.Start("explorer", uri)`
