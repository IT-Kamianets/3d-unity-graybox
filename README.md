# 3d-unity-graybox

Building a game's first playable from Unity primitives, on the IT Kamianets 3D Engine.

### Logic

- `AssetIdTag` (runtime) puts a stable id such as `monsters/goblin` or `props/chest` on anything that will later get a real model. Whatever shape draws it today, the id says what it is, so a model can replace the primitive without touching the gameplay.
- `GrayboxBuilder` (Editor) holds the three helpers every scene builder needs: `Prim` (a primitive in a place and size, with no collider unless asked), `Box` (a cube that keeps its collider, for floors and walls) and `Mat` (a plain URP Lit material of one colour, saved as an asset and updated on the next run).

- `GrayboxBuilder.Text` (Editor) puts a line of text in the world (a sign, a plaque) with the depth-tested `Temnorid/WorldText` shader from this package, because the built-in font shader draws text through walls.

Scene builders written with these stay short and can be run again at any time to rebuild a level from scratch.

### Status

v0.1.1: `AssetIdTag`, `GrayboxBuilder` (with world text). Used by every Temnorid game with a built scene.

### Roadmap

- A catalog that maps asset ids to model files, and a swapper that replaces tagged primitives with them (together with `3d-unity-assets`).
