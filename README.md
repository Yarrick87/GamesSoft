# GamesSoft

Unity 6 take-home project for SoftGames: a main menu and three independent mini-games.

| | |
|---|---|
| **Engine** | Unity `6000.3.12f1` (Unity 6) |
| **Render pipeline** | URP 2D |
| **Product** | GamesSoft `1.0` |
| **Primary platform** | Editor + WebGL |
| **Orientation** | Portrait |

## Documentation map

| Document | Contents |
|----------|----------|
| [Docs/Overview.md](Docs/Overview.md) | Goals, features, how to open and play |
| [Docs/Architecture.md](Docs/Architecture.md) | Assemblies, scene flow, shared systems |
| [Docs/Games.md](Docs/Games.md) | Ace of Shadows, Magic Words, Phoenix Flame in depth |
| [Docs/Testing.md](Docs/Testing.md) | Edit Mode tests and editor tooling |
| [Docs/Build-and-WebGL.md](Docs/Build-and-WebGL.md) | WebGL build, GitHub Pages, CORS notes |

## Quick start

1. Install **Unity Hub** and editor **6000.3.12f1**.
2. Open this repository folder as a Unity project.
3. Open `Assets/Scenes/MainMenu.unity`.
4. Press **Play**.

From the main menu:

- **Ace of Shadows** — card dealing / flying stacks  
- **Magic Words** — networked chat UI with emoji formatting  
- **Phoenix Flame** — particle fire color cycling  

Each game scene has a **Back** button (top-right) to return to the menu. An **FPS** counter persists across scenes.

## Repository layout (high level)

```
Assets/
  Art/                 Card sprites, flame textures, ScriptableObjects
  Prefabs/             Cards, chat bubbles, Back button
  Scenes/              MainMenu + 3 games
  Scripts/             Runtime C# (GamesSoft assembly)
  Editor/              GamesSoft menu tools
  Tests/EditMode/      NUnit Edit Mode tests
  Settings/            URP assets
Docs/                  Detailed documentation
Packages/manifest.json Unity packages
ProjectSettings/       Player / build settings
```

## Scripts at a glance

```
Assets/Scripts/
  Core/           SceneLoader, BackToMenuButton, FpsCounter
  Menu/           MainMenuController
  AceOfShadows/   Controller, stacks, cards, CardArc, sprite library
  MagicWords/     API load, chat UI, text formatting, DTOs
  PhoenixFlame/   Particle color transitions
```

## Tests (short)

- Window → General → **Test Runner** → EditMode → Run All  
- or menu **GamesSoft → Run EditMode Tests**

See [Docs/Testing.md](Docs/Testing.md).

## License / attribution

Assignment project. Card art and SoftParticle textures are project assets. TextMesh Pro ships with Unity (see TMP license files under `Assets/TextMesh Pro/`).
