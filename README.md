---

# CPR SIM

**CPR SIM** — a beginner-friendly, repeatable Virtual Reality simulation for learning basic CPR (30:2 cycles), pulse checking, and emergency response procedures. Built with Unity and designed to run on Google Cardboard or on a desktop for testing.

---

## Table of contents

1. [Overview](#overview)
2. [Quick start (summary)](#quick-start-summary)
3. [Requirements & supported versions](#requirements--supported-versions)
4. [Install / Clone repository](#install--clone-repository)
5. [Open the project in Unity](#open-the-project-in-unity)
6. [Run modes — VR (Cardboard) and Desktop testing](#run-modes---vr-cardboard-and-desktop-testing)
7. [How to use the simulation (step-by-step user flow)](#how-to-use-the-simulation-step-by-step-user-flow)
8. [Controls (desktop) & interactions (VR)](#controls-desktop--interactions-vr)
9. [Project structure & important files](#project-structure--important-files)
10. [Customize & configuration (timers, cycles, assets)](#customize--configuration-timers-cycles-assets)
11. [Build & distribute (PC & Android)](#build--distribute-pc--android)
12. [Common problems & fixes](#common-problems--fixes)
13. [Contributing & code style](#contributing--code-style)
14. [License](#license)

---

## Overview

CPR SIM guides the user through:

* identifying an unresponsive person,
* checking airway/breathing/pulse,
* performing CPR in **30 compressions : 2 breaths** cycles,
* repeating cycles until the patient is revived or the scenario ends.

It’s intended as a training/educational demo — not a replacement for certified CPR training.

---

## Quick start (summary)

1. Install Unity (see versions below).
2. Clone this repo.
3. Open the project in Unity Hub.
4. Save your scene: `File → Save As…` → `Assets/Scenes/edited_pool.unity`.
5. For desktop testing: disable XR (Edit → Project Settings → XR Plug-in Management → uncheck), add Main Camera with `SimpleWalk` script if needed.
6. Press Play to run.
7. For Cardboard: build to Android with `File → Build Settings → Android`, install APK on device.

---

## Requirements & supported versions

* **Unity**: recommended **2020.3 LTS** (the project was tested on 2020.3.x).

  * Check `ProjectSettings/ProjectVersion.txt` to confirm the project version.
* **Platform**: Windows, macOS, Android (for Cardboard).
* **SDKs**: If building to Android, Unity Android Build Support + Android SDK/NDK (Unity Hub installer provides these).
* **Git** (optional) to clone: `git` CLI or GitHub Desktop.

---

## Install / Clone repository

```bash
# clone into a folder
git clone https://github.com/your-username/your-repo.git
cd your-repo
```

> If you forked or pulled from another repo and want a clean history: remove `.git`, then `git init` and push to a new remote (see CONTRIBUTING section below).

---

## Open the project in Unity

1. Open **Unity Hub** → `Add` → select the root folder you cloned.
2. If Unity prompts about version mismatch, install the matching Unity Editor release from Hub.
3. In Unity: `File → Save` (if scene has `*`), `File → Save Project`.
4. Ensure the scene you want to run is the open scene; if you want it included in builds: `File → Build Settings → Add Open Scenes`.

---

## Run modes — VR (Cardboard) and Desktop testing

### Desktop (no headset) — recommended for development / tests

* **Disable XR**:
  `Edit → Project Settings → XR Plug-in Management` → under **PC, Mac & Linux Standalone**, uncheck all plugins (OpenXR/SteamVR/etc).
* **Replace VR rig** (if present):

  * Delete or disable `XR Rig`, `XR Origin`, `CameraRig`.
  * Use the `Main Camera` (GameObject → Camera). Attach the `SimpleWalk` script:

    ```csharp
    // SimpleWalk.cs - put in Assets/Scripts
    using UnityEngine;
    public class SimpleWalk : MonoBehaviour {
      public float speed = 3f;
      public float mouseSensitivity = 2f;
      float rotX=0, rotY=0;
      void Update(){
        float h = Input.GetAxis("Horizontal"), v = Input.GetAxis("Vertical");
        transform.Translate((transform.right*h + transform.forward*v) * speed * Time.deltaTime);
        rotX += Input.GetAxis("Mouse X")*mouseSensitivity;
        rotY -= Input.GetAxis("Mouse Y")*mouseSensitivity;
        rotY = Mathf.Clamp(rotY,-80,80);
        transform.localRotation = Quaternion.Euler(rotY, rotX, 0);
      }
    }
    ```
* **Simulate CPR input**: click the patient or press keyboard key (Space/C) to register compressions depending on script mapping.

### Google Cardboard (VR mobile)

* Ensure `Android Build Support` installed in Unity Hub.
* In Build Settings: `Platform → Android → Switch Platform`.
* Add Cardboard / VR SDK as required (or use simple VR camera prefab).
* Build & install APK on phone, insert phone into Cardboard, run.

---

## How to use the simulation (step-by-step user flow)

**Pre-CPR checklist (manual actions)**

1. Look around and identify the unresponsive person.
2. Check for breathing (look, listen, feel).
3. Tap and call out — check response.
4. Call emergency services (simulate by pressing the on-screen button if present).
5. Check pulse (up to 10 seconds). If pulse absent or faint, begin CPR.

**CPR procedure in simulation**

1. Position yourself beside the patient on a flat surface.
2. Remove clothing covering chest (if simulated).
3. Place heel of one hand on the centre of chest (between nipples). Place the other hand on top, interlace fingers.
4. Compress chest to **~2–2.4 inches** depth (simulated by clicks).
5. Keep compression rate ~**100–120 compressions/min** (timing enforced by game logic).
6. After **30** compressions, give **2 rescue breaths** (2-second pause in simulation).
7. Repeat for **4 cycles** (default) or until “Patient Survived” message.

---

## Controls (desktop) & interactions (VR)

### Desktop

* **W/A/S/D** — move (if `SimpleWalk` attached)
* **Mouse** — look around
* **Left click** — interact / perform compression (or press **C** / **Space**, depending on your script)
* **R** — reset simulation (if reset script present)

### VR (Cardboard)

* **Gaze + click (Cardboard button / screen tap)** — identify, compress, or confirm actions
* **Physical crouch** is usually not used with Cardboard; simulation may have an on-screen crouch action.

---

## Project structure & important files

Typical layout (adjust to your repo):

```
Assets/
  Scenes/
    edited_pool.unity
    VR-CPR1.unity
  Scripts/
    ObjectController.cs       # main CPR logic / cycles
    UserMovement.cs           # crouch detection
    SimpleWalk.cs             # desktop movement
  Prefabs/
  Materials/
  Textures/
ProjectSettings/
Packages/
README.md
```

**Important scripts**

* `ObjectController.cs` — patient identification, pulse check, compression cycle logic. (This is where compression timers and cycle success/failure live.)
* `UserMovement.cs` — tracks crouch state used for pulse check.
* `SimpleWalk.cs` — optional desktop movement.

---

## Customize & configuration (timers, cycles, assets)

Edit these in `ObjectController.cs` (or config script) for quick changes:

* `compressionTimer` — seconds allowed per compression phase (default 15s).
* `compressionsPerCycle` — usually 30.
* `totalCycles` — number of cycles to consider success (default 4).
* `breathingTime` — pause for breaths (default 2s).

To change environment objects:

1. In `Hierarchy`, select the object (e.g., `exterior_swimming_pool`).
2. Disable or delete unwanted meshes.
3. Drag new prefabs or assets into the scene from the `Assets` window.
4. Save scene: **File → Save**.

To change material/color of an object:

1. Select object → Mesh Renderer → Material field.
2. Create new Material (Assets → Create → Material) → set Albedo color/texture → drag onto object.

---

## Build & distribute (PC & Android)

### Build for PC

1. `File → Build Settings` → Platform: **PC, Mac & Linux Standalone**.
2. Check scenes you want in **Scenes In Build**.
3. Click **Build** → choose an empty folder (e.g., `Builds/PC/`).

   * If you get: `Failed to prepare target build directory. Is a built game instance running?` — close previously launched .exe or delete the old build folder and try again.

### Build for Android (Cardboard)

1. `File → Build Settings` → switch to **Android** (install Android module if needed).
2. Check `Development Build` if you want logs.
3. Click **Build & Run** (device must be connected via USB + USB debugging enabled).
4. If Gradle build fails: try clearing `Temp/gradleOut`, delete `~/.gradle` (user folder) and rebuild; ensure JDK/SDK paths (Edit → Preferences → External Tools) are correct.

---

## Common problems & fixes

### Scene changes not appearing in build

* Save scene: `File → Save` (or `Ctrl+S`).
* Add open scene to Build Settings: `File → Build Settings → Add Open Scenes`.
* Use a new build output folder to avoid old build caching.

### Color/material not changing on click

* Ensure you are changing the **Renderer.material** or assigning a material instance (not the shared material if you want per-instance changes). Example:

```csharp
Renderer r = GetComponent<Renderer>();
r.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
```

* If you change `r.material.color`, Unity creates an instance on first write — that's fine for per-object color flashes.

### Build error: “Gradle build failed”

* Delete `Temp/gradleOut` inside project; delete `~/.gradle` if needed.
* Verify `Edit → Preferences → External Tools` paths (JDK/SDK/NDK).
* Reopen Unity and try again.

---

## Contributing & code style

* Keep changes modular: create new scripts rather than editing large monolithic scripts.
* Follow C# naming conventions and comment public APIs.
* Create a branch per feature and open a Pull Request.
* If the repo was forked and you want a fresh history: remove `.git` and reinit:

```bash
rm -rf .git
git init
git add .
git commit -m "Initial commit - clean history"
git remote add origin https://github.com/yourname/yourrepo.git
git push -u origin main --force
```

---

## License

This project uses the original project license. If you forked a different license, keep the original license notices where required. Add your own licensing file if you relicense code.

---
