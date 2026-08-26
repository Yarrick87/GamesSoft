# Build and WebGL

## Editor play vs player build

| | Editor Play | WebGL Player |
|--|-------------|--------------|
| Scenes | Same Build Settings list | Same |
| Networking | Usually works | Needs HTTPS + CORS |
| Async | `Awaitable` / UWR OK | Avoid ThreadPool APIs |
| Input | Mouse / trackpad | Mouse + touch |

## Build Settings checklist

1. **File → Build Settings**
2. Platform: **WebGL** (Switch Platform if needed)
3. Scenes enabled in order: MainMenu → Ace → Magic Words → Phoenix
4. **Player Settings** review:
   - Company / Product: GamesSoft
   - Resolution / Presentation: landscape autorotation
   - Publishing Settings → compression (see below)

## Compression and GitHub Pages

Current project setting uses **Gzip** compression (`webGLCompressionFormat: 2`).

| Host | Recommendation |
|------|----------------|
| Local `python -m http.server` / proper static host with `Content-Encoding: gzip` | Gzip OK |
| **GitHub Pages** | Prefer **Disabled** compression, or enable **Decompression Fallback** in Player Settings |

GitHub Pages often serves `.gz` / `.br` files as raw downloads without the correct encoding headers, which breaks the loader.

### Suggested Pages-friendly settings

- Compression Format: **Disabled**
- (Optional) Decompression Fallback: **On** if you keep compressed builds

## Building

1. Switch to WebGL.
2. **Build** (or Build And Run) to an empty folder, e.g. `Build/WebGL/`.
3. Output typically contains:

```
index.html
Build/
  GamesSoft.data[.gz]
  GamesSoft.framework.js[.gz]
  GamesSoft.loader.js
  GamesSoft.wasm[.gz]
TemplateData/
```

4. Open `index.html` via a **local HTTP server** (not always `file://`).

Example:

```bash
cd Build/WebGL
python3 -m http.server 8000
# visit http://localhost:8000
```

## Deploying to GitHub Pages (pattern used in this repo)

Historical deploy branches placed player files at the **repo root** (`index.html`, `Build/`, `TemplateData/`) and pointed Pages at that branch.

Checklist:

1. Produce a complete WebGL build (all four files under `Build/`, not only `.wasm` / `.data`).
2. Commit them on a dedicated branch (e.g. `release/deploy_*`).  
   Note: Unity’s default `.gitignore` ignores `/[Bb]uild/` — force-add or adjust ignore on that branch.
3. Add `.nojekyll` so GitHub Pages does not process the site as Jekyll.
4. Settings → Pages → Deploy from branch → `/` (root).
5. Wait for Pages to update; verify:

   `https://<user>.github.io/<repo>/Build/GamesSoft.loader.js` returns **200**, not 404.

If `index.html` loads but the loader 404s, the branch is incomplete (common failure mode).

## Magic Words on WebGL

The chat feature calls:

- Apiary mock API  
- External avatar image URLs  

The **browser** enforces CORS. If the hosting origin is not allowed, Unity will report request failures and the UI shows *Failed to load dialogue*. That is an environment constraint, not a missing `await`.

## Memory

Player Settings include a modest WebGL memory configuration (`webGLMemorySize: 32` in the asset file; Unity 6 may also use newer WASM memory options). If a build runs out of memory with all three games’ assets, increase WebGL memory in Player Settings and rebuild.

## CI / batch builds (optional)

Unity batchmode example:

```bash
/Applications/Unity/Hub/Editor/6000.3.12f1/Unity.app/Contents/MacOS/Unity \
  -batchmode -nographics -quit \
  -projectPath "/path/to/GamesSoft" \
  -buildTarget WebGL \
  -executeMethod WebGLBuilder.Build
```

Only works if you add an Editor entry point (not required for manual builds). Close the Editor or use a separate project copy so the Library lock is free.
