# Wall Painter Effect (Experiment)



### How it works

- Init: Each paintable object creates a square RenderTexture
- Update: PaintManager collects up to MAX_HITS (16) hit points and radii. If at least one hit happened, the data is uploaded to the GPU via StructuredBuffer
- Each paintable object draws itself into a RenderTexture, using UV as the result position (to render UV-like map). Then in the fragment it goes through all hit points and compares distances. If a pixel's position in world space falls within the hit radius, it is painted red, otherwise fragment is discarded.

### Media

