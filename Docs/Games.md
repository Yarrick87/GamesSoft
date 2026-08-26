# Games

Detailed behavior and script map for each mini-game.

---

## 1. Ace of Shadows

**Scene:** `Assets/Scenes/AceOfShadowsGame.unity`

### Gameplay

1. On start, spawn **144** cards onto stack `0` (`_stacks[0]`).
2. Faces come from `CardSpriteLibrary.GetFace(index)` (wraps by face count; empty library → `null`).
3. Every **`_dealInterval`** seconds (default **1s**), pop the top card from stack 0.
4. Fly it to stack 1 over **`_moveDuration`** (default **1.5s**) along a **quadratic Bezier** arc.
5. Multiple cards can be in flight at once; flying cards use elevated sorting order.
6. When all cards have been dispatched and none remain in flight, show status: *All cards have moved*.

Stacks are world-space rows stacked **vertically** for portrait (`Row_0` above `Row_1`). Within each row, cards fan **horizontally** via `_cardOffset` (default `(0.08, 0, 0)`).

### Scripts

| Type | File | Responsibility |
|------|------|----------------|
| `AceOfShadowsController` | `Scripts/AceOfShadows/AceOfShadowsController.cs` | Spawn, deal coroutine, flight coroutines, status |
| `CardStackView` | `CardStackView.cs` | Push / Pop, slot reservation (`BeginIncoming` / `NextSlotIndex`), layout, counter TMP |
| `CardView` | `CardView.cs` | Face sprite + sorting order |
| `CardSpriteLibrary` | `CardSpriteLibrary.cs` | ScriptableObject sprite table |
| `CardArc` | `CardArc.cs` | Pure Bezier evaluation `Evaluate(start, control, end, t)` |

### Prefabs

- `Assets/Prefabs/AceOfShadows/Card.prefab`
- `Assets/Prefabs/AceOfShadows/CardRow.prefab`

### Motion math

Quadratic Bezier (also covered by Edit Mode tests):

\[
B(t) = (1-t)^2 P_0 + 2(1-t)t P_1 + t^2 P_2
\]

Easing on \(t\): smoothstep-style `t * t * (3 - 2 * t)`.

Control point: midpoint between start/end + world **right** offset (`2.2`) so the arc bows sideways while cards move between the upper and lower rows.

### Edge cases

- `Pop()` on empty stack returns `null` → deal routine stops.
- `OnDestroy` stops all coroutines to avoid work after unload.

---

## 2. Magic Words

**Scene:** `Assets/Scenes/MagicWordsGame.unity`

### Gameplay / UX

1. Show *Loading...*
2. `GET` dialogue JSON from the SoftGames Apiary mock API.
3. Build speaker profiles from `avatars[]` (`position: left|right`) and any dialogue-only names.
4. Download avatar textures (first valid URL per speaker); keep textures for cleanup.
5. Instantiate left or right chat bubble prefabs; format text; scroll to top.
6. On failure, show *Failed to load dialogue*.
7. Leaving the scene cancels in-flight requests and destroys loaded textures.

### API

- **Default URL:** `https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords`
- Overridable via Inspector `_endpoint` on `MagicWordsController`

Expected JSON shape (Unity `JsonUtility`):

```json
{
  "dialogue": [
    { "name": "Alice", "text": "Hello {win} and *welcome*" }
  ],
  "avatars": [
    {
      "name": "Alice",
      "url": "https://example.com/a.png",
      "position": "left"
    }
  ]
}
```

DTOs live in `MagicWordsData.cs`:

- `MagicWordsResponse`, `DialogueLine`, `AvatarEntry`
- Runtime `SpeakerProfile` (name, align, sprite, URL list)
- `SpeakerAlignment.IsRight(position)` — case-insensitive `"right"`

### Text formatting (`DialogueTextFormatter`)

| Input | Output |
|-------|--------|
| `{satisfied}` etc. | Mapped Unicode emoji |
| Unknown `{token}` | Fallback `☺` |
| `*text*` | TMP italic `<i>text</i>` |
| `null` / empty | `""` |

Known emoji keys: `satisfied`, `intrigued`, `neutral`, `affirmative`, `laughing`, `win`.

### Scripts

| Type | File | Responsibility |
|------|------|----------------|
| `MagicWordsController` | `MagicWordsController.cs` | Fetch, profiles, avatars, spawn messages, status |
| `ChatMessageView` | `ChatMessageView.cs` | Bind speaker + message, bubble sizing, avatar / initials |
| `DialogueTextFormatter` | `DialogueTextFormatter.cs` | Token + italic transform |
| `MagicWordsData` | `MagicWordsData.cs` | DTOs + alignment helper |

### Prefabs

- `Assets/Prefabs/MagicWords/ChatMessageLeft.prefab`
- `Assets/Prefabs/MagicWords/ChatMessageRight.prefab`

### Cancellation & cleanup

- `Start` is `async void` but always uses `destroyCancellationToken`.
- `UnityWebRequest` aborted via `token.Register(request.Abort)`.
- `OperationCanceledException` is ignored (expected on leave).
- `OnDestroy` destroys all textures in `_loadedTextures`.

### Networking caveats (especially WebGL)

- HTTPS only in builds.
- Browser **CORS** must allow the page origin to call Apiary and avatar hosts (e.g. DiceBear). Failures show the status error path.

---

## 3. Phoenix Flame

**Scene:** `Assets/Scenes/PhoenixFlameGame.unity`

### Gameplay / UX

- Several `ParticleSystem`s render a fire (and related) effect.
- **Next Color** cycles through three colors: orange → green → blue.
- Transition uses `Color.Lerp` over `_transitionDuration` (default **0.85s**) with `SmoothStep`.
- While transitioning, further clicks are ignored (`_isTransitioning`).
- Live particles are recolored via `GetParticles` / `SetParticles` (RGB from target, **alpha preserved**).
- `main.startColor` is updated so new particles spawn in the new color.
- Cancel-safe: `destroyCancellationToken` on the async transition; `OperationCanceledException` ignored.

### Scripts

| Type | File | Responsibility |
|------|------|----------------|
| `PhoenixFlameController` | `PhoenixFlameController.cs` | Button, lerp loop, particle tint |

### Art

- `Assets/Art/PhoenixFlame/FlameParticle.mat`
- `Assets/Art/PhoenixFlame/SmokeParticle.mat`
- `Assets/Art/PhoenixFlame/SoftParticle.png`

---

## Cross-game checklist for reviewers

| Topic | Where |
|-------|--------|
| Scene load debounce | `SceneLoader` |
| Cancel on unload | Magic Words, Phoenix Flame, menu/back loaders |
| Pure logic under test | `DialogueTextFormatter`, `CardArc`, `SpeakerAlignment`, `CardSpriteLibrary`, `CardStackView` |
| Prefabs | `Assets/Prefabs/` |
| Build scenes | `ProjectSettings/EditorBuildSettings.asset` |
