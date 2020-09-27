# UnitySpriteSwapper

Rough version - if this turns out to be something people find useful, I may polish it up.
Uses OdinInspector, but it isn't required for the functionality, so feel free to strip it out.

Usage instructions: 
- Create a folder with only the base sprites you wish to swap out. This is your "originals" folder.
- Create a second folder with the sprites that will replace them. This is your "variants" folder.
- Ensure that for every texture in originals, there is a replacement texture of the same name, dimensions, and sprite slicing.
- Add the "SpriteSwapper" component to your SpriteRenderer's gameobject.
- Fill in the original and variant path directories (starting with "Assets/...").
- Hit populate.
- Manually modify one of the swapsets so they are marked dirty and saved.
- You're done! Now, any time this SpriteSwapper is enabled, it will swap sprites on the fly.

KNOWN BUG: The populate function doesn't mark the data as dirty, so it won't save unless you manually change one of the swap sets after populating them.
