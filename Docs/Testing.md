# Testing

## Framework

- **Unity Test Framework** `1.6.0` (`com.unity.test-framework`)
- **Mode:** Edit Mode only (no Play Mode suite yet)
- **Assembly:** `GamesSoft.EditModeTests`
- **Namespace:** `GamesSoft.Tests.EditMode`

Tests reference `GamesSoft`, `UnityEngine.TestRunner`, `UnityEditor.TestRunner`, TMP, and uGUI. They use `nunit.framework.dll` with `overrideReferences`.

## How to run

### Test Runner window

1. **Window → General → Test Runner**
2. Select **EditMode**
3. **Run All** (or run a single fixture)

### Project menu

**GamesSoft → Run EditMode Tests**

- Starts the Edit Mode run via `TestRunnerApi`
- Writes a plain-text report to project root: `TestResults-EditMode.txt`
- That file is gitignored (`/TestResults*.txt`, `/TestResults*.xml`)

Implementation: `Assets/Editor/EditModeTestRunner.cs`.

## Test inventory

| Fixture | File | What it verifies |
|---------|------|------------------|
| `DialogueTextFormatterTests` | `DialogueTextFormatterTests.cs` | Empty input, emoji tokens, unknown token fallback, italics, combined |
| `MagicWordsDataTests` | `MagicWordsDataTests.cs` | `SpeakerAlignment.IsRight`, `JsonUtility` parse / missing arrays |
| `CardSpriteLibraryTests` | `CardSpriteLibraryTests.cs` | Empty library → null; index wrap via SerializedObject faces |
| `CardArcTests` | `CardArcTests.cs` | Bezier endpoints and midpoint formula |
| `CardStackViewTests` | `CardStackViewTests.cs` | Empty `Pop`, push/pop, `BeginIncoming` / `NextSlotIndex` |
| `SceneLoaderTests` | `SceneLoaderTests.cs` | Scene name constants match Build Settings names |

## Editor utilities related to hygiene

**GamesSoft → Clear Logs** (`Assets/Editor/LogCleaner.cs`):

- Deletes files under `Logs/`
- Deletes root `TestResults*.txt` / `*.xml`
- Clears the Unity Console (`LogEntries.Clear`)
- Skips files locked by the Editor and reports counts in the Console

## What is not covered (yet)

- Play Mode integration (full Ace deal, network Magic Words against live Apiary)
- UI layout / Canvas regression tests
- WebGL player automated runs

Adding Play Mode tests would typically live under `Assets/Tests/PlayMode/` with a separate asmdef and `[UnityTest]` coroutines / `Awaitable` helpers.
