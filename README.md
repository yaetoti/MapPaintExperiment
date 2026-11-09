# Wall Painter Effect (Experiment)

<img width="1920" height="1080" alt="Screenshot_2026-11-09_07-37-48" src="https://github.com/user-attachments/assets/fe106bb9-d9e6-41b0-97a2-121e66b60a59" />

### How it works

- Init: Each paintable object creates a square RenderTexture
- Update: PaintManager collects up to MAX_HITS (16) hit points and radii. If at least one hit happened, the data is uploaded to the GPU via StructuredBuffer
- Each paintable object draws itself into a RenderTexture, using UV as the result position (to render a UV-like map). Then in the fragment it goes through all the hit points and compares distances. If a pixel's position in world space falls within the hit radius, it is painted red, otherwise the fragment is discarded.

### Media

https://github.com/user-attachments/assets/0bd7b52b-5e8a-48e6-b722-5106aaaa7bd4
