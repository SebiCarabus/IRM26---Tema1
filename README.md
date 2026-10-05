# IMR 2026 — Tema 1

Augmented-reality Unity project using the **Vuforia Engine 11.4.4** with image targets.

## Requirements

| | |
|---|---|
| Unity | **6000.6.3f1** (`ProjectSettings/ProjectVersion.txt`) |
| Render pipeline | URP 17.6.0 |
| Target platform | Android, **ARM64 only**, min API level 26 |
| Device | Must have **Google Play Services for AR** (ARCore) installed |

## Setup

```bash
git lfs install      # only needed once per machine
git clone <this repo>
```

Open `IMR_tema1/` in Unity Hub with version **6000.6.3f1**.

The Vuforia Engine package ships in the repo as a Git LFS object at
`IMR_tema1/Packages/com.ptc.vuforia.engine-11.4.4.tgz` and is referenced from
`Packages/manifest.json`. If you see

```
An error occurred while resolving dependencies: Project has invalid dependencies
```

then the LFS objects were not pulled. Fix it with:

```bash
git lfs pull
```

### License key (required)

The license key is **not** committed — it must be your own, from
<https://developer.vuforia.com> (free "Developer" plan is enough for this project).

1. Create an app on the Vuforia Developer Portal and copy its license key.
2. In Unity: `Window > Package Manager > Vuforia > Configure`
3. Paste the key into **License Key** and press **Activate**.

The key belongs in `Assets/Resources/VuforiaConfiguration.asset` under
`vuforia.vuforiaLicenseKey`. That file is tracked, but the key inside it is blank
on purpose — don't commit your key back.

Until a valid key is activated, Vuforia initialises but tracking does not work.

### Image targets

The trained target data lives in `Assets/StreamingAssets/Vuforia/`:

| Dataset | Targets |
|---|---|
| `Tema1` | `AU` |
| `Tema1-2` | `BU` |
| `Tema1-3` | `reverse_rosu` |
| `Tema1-4` | `2verde`, `plus2albastru` |

Each dataset needs both `<name>.xml` and `<name>.dat`. The `.dat` files hold the
actual feature data, so **both** are required — a missing `.dat` means the target
never gets detected, with no error message. Source images for re-training are in
`Assets/Editor/Vuforia/ImageTargetTextures/`.

## Scene

`Assets/Scenes/SampleScene.unity` (already in Build Settings):

- `ARCamera` — carries `VuforiaBehaviour`; replaces the stock `Main Camera` at runtime
- `ImageTarget` — two instances, each an `ImageTargetBehaviour` + `DefaultObserverEventHandler`
- `Main Camera`, `Directional Light` — unused, left from the URP template

`Assets/Attack_retreat.cs` is the only custom gameplay script: it flips the
animator's `Attacking` bool when the target comes within `attackDistance`.

## License notes

Third-party assets (Kevin Iglesias human animations, etc.) keep their original
licenses. Vuforia itself requires accepting PTC's EULA, which is recorded in
`VuforiaConfiguration.asset` via `eulaAcceptedVersions`.