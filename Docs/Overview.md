# Overview

## Purpose

GamesSoft is a SoftGames-style Unity assignment demonstrating:

- Multi-scene navigation with a shared menu
- 2D gameplay (cards)
- UI + networking (chat from a mock API)
- VFX (particle systems)
- Production-minded details: cancellation on scene unload, null-safety, Edit Mode tests, WebGL readiness

## Feature summary

| Area | Behavior |
|------|----------|
| Main menu | Three buttons load game scenes asynchronously |
| Ace of Shadows | Spawns 144 cards; every 1s moves top card along a Bezier arc to the other stack |
| Magic Words | Fetches JSON dialogue + avatars; left/right bubbles; `{emoji}` and `*italic*` formatting |
| Phoenix Flame | Multi-system particle fire; button cycles orange → green → blue with live recolor |
| FPS | Top-left style counter, singleton + `DontDestroyOnLoad` |
| Back | Prefab button loads `MainMenu` with cancellation support |

## Requirements

- **Unity Editor:** `6000.3.12f1` (see `ProjectSettings/ProjectVersion.txt`)
- **OS:** macOS / Windows / Linux as supported by that editor
- **Optional:** Git, browser for WebGL builds

### Notable packages (`Packages/manifest.json`)

- `com.unity.render-pipelines.universal` — URP  
- `com.unity.ugui` / TextMesh Pro — UI  
- `com.unity.inputsystem` — Input System  
- `com.unity.test-framework` `1.6.0` — tests  
- 2D packages (sprite, animation, etc.)

## How to run in the Editor

1. Open the project in Unity Hub with the matching editor version.
2. Wait for script compile / asset import.
3. Confirm **File → Build Settings** lists all four scenes, **MainMenu** first:

   | Index | Scene |
   |------:|-------|
   | 0 | `Assets/Scenes/MainMenu.unity` |
   | 1 | `Assets/Scenes/AceOfShadowsGame.unity` |
   | 2 | `Assets/Scenes/MagicWordsGame.unity` |
   | 3 | `Assets/Scenes/PhoenixFlameGame.unity` |

4. Open Main Menu and press Play.

### Controls

| Context | Input |
|---------|--------|
| Menu | Click / tap game buttons |
| Games | Back button → menu |
| Phoenix Flame | **Next Color** button |
| Ace / Magic Words | Mostly automatic; Magic Words scrolls |

## Player settings (current)

- **Orientation:** **Portrait** (landscape disabled)  
- **UI reference resolution:** 1080×1920 on scene canvases  
- **WebGL default size:** portrait-oriented in Player Settings  
- **WebGL compression:** Gzip (`webGLCompressionFormat: 2`) — see [Build-and-WebGL.md](Build-and-WebGL.md) if hosting on GitHub Pages  

## UI conventions

- Canvas Scaler: **Scale With Screen Size**, reference **1920×1080**, match **0.5**
- Game canvases use large touch-friendly buttons
- Back control lives in `Assets/Prefabs/BackToMenuButton.prefab`

## Related docs

- [Architecture](Architecture.md) — systems and data flow  
- [Games](Games.md) — per-game design and scripts  
- [Testing](Testing.md) — automated tests  
- [Build and WebGL](Build-and-WebGL.md) — shipping builds  
