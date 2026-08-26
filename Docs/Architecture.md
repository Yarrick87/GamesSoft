# Architecture

## Assembly layout

Runtime gameplay code is compiled into a dedicated assembly:

| Assembly | Path | Role |
|----------|------|------|
| `GamesSoft` | `Assets/Scripts/GamesSoft.asmdef` | All runtime scripts under `Assets/Scripts/` |
| `GamesSoft.Editor` | `Assets/Editor/GamesSoft.Editor.asmdef` | Editor menu tools |
| `GamesSoft.EditModeTests` | `Assets/Tests/EditMode/GamesSoft.EditModeTests.asmdef` | Edit Mode NUnit tests |

`GamesSoft` references:

- `Unity.TextMeshPro`
- `Unity.ugui`

Namespaces mirror folders:

- `GamesSoft.Core`
- `GamesSoft.Menu`
- `GamesSoft.AceOfShadows`
- `GamesSoft.MagicWords`
- `GamesSoft.PhoenixFlame`
- `GamesSoft.EditorTools` (Editor only)
- `GamesSoft.Tests.EditMode` (tests)

## Scene flow

```
                 ┌─────────────┐
                 │  MainMenu   │
                 └──────┬──────┘
          ┌─────────────┼─────────────┐
          ▼             ▼             ▼
 AceOfShadowsGame  MagicWordsGame  PhoenixFlameGame
          │             │             │
          └──────► MainMenu ◄─────────┘
                     (Back)
```

All transitions go through `GamesSoft.Core.SceneLoader`:

```csharp
public static async Awaitable Load(string sceneName, CancellationToken cancellationToken = default)
```

### SceneLoader behavior

1. If a load is already in progress (`_isLoading`), additional calls **return immediately** (debounce / double-click guard).
2. Throws if cancellation was requested before starting.
3. Uses `SceneManager.LoadSceneAsync` and waits with `Awaitable.NextFrameAsync` until `isDone`.
4. Clears `_isLoading` in `finally` so the lock is always released.

Callers (`MainMenuController`, `BackToMenuButton`) pass `destroyCancellationToken` and swallow `OperationCanceledException` so destroying the UI mid-load does not error.

Scene name constants:

| Constant | Scene name |
|----------|------------|
| `SceneLoader.Menu` | `MainMenu` |
| `SceneLoader.AceOfShadows` | `AceOfShadowsGame` |
| `SceneLoader.MagicWords` | `MagicWordsGame` |
| `SceneLoader.PhoenixFlame` | `PhoenixFlameGame` |

## Shared systems

### BackToMenuButton

- Prefab: `Assets/Prefabs/BackToMenuButton.prefab`
- Listens to a UI `Button`, loads `SceneLoader.Menu`
- Same cancel / error handling pattern as the menu

### FpsCounter

- Lives on Main Menu (`FpsCounter` → `FpsCanvas` → label)
- Singleton: duplicate instances destroy themselves
- `DontDestroyOnLoad` so the counter survives scene changes
- Updates label about every `0.25s` with `FPS: N`

### UI stack

Typical game scene:

```
Canvas (Screen Space Overlay + CanvasScaler 1920×1080)
  Title / Status / game-specific widgets
  BackToMenuButton (prefab instance)
EventSystem
Main Camera (orthographic for 2D)
Game controller MonoBehaviour
```

## Design principles used in code

1. **SerializeField wiring** — scene references set in the Inspector, not `Find` in hot paths (except rare Editor tooling).
2. **Cancellation** — async flows that outlive a scene use `destroyCancellationToken` and/or `UnityWebRequest.Abort` via `CancellationToken.Register`.
3. **Null-safety** — empty card stacks return `null` from `Pop()`; empty sprite libraries return `null` from `GetFace`.
4. **Small pure helpers** — `CardArc`, `DialogueTextFormatter`, `SpeakerAlignment` are easy to unit-test without Play Mode.
5. **No ThreadPool usage** — WebGL-safe `async`/`await` only on Unity operations (`Awaitable`, `UnityWebRequest`).

## Data / art assets

| Asset | Path | Use |
|-------|------|-----|
| Card faces | `Assets/Art/Cards/*.png` (~52 ranks × suits set) | Ace of Shadows |
| CardSpriteLibrary | `Assets/Art/CardSpriteLibrary.asset` | Maps card index → sprite |
| Flame / smoke mats | `Assets/Art/PhoenixFlame/` | Particle materials |
| SoftParticle | `Assets/Art/PhoenixFlame/SoftParticle.png` | Particle texture |

## Extension points

| Goal | Where to change |
|------|-----------------|
| Add a fourth mini-game | New scene + scripts folder; register in Build Settings; add menu button + `SceneLoader` constant |
| Change Magic Words API | `_endpoint` on `MagicWordsController` or default constant |
| Tune Ace timing | `_cardCount`, `_dealInterval`, `_moveDuration` on `AceOfShadowsController` |
| Tune flame colors / duration | `FireColor_*.anim` clips and transition duration on `FireColor.controller` |
