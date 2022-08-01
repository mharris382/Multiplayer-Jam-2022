# Trailing comma[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#trailing-comma "Permalink to this headline")

Use a trailing comma on the last line in [[Array|arrays]], [[Dictionary|dictionaries]], and [[Enums]] This results in easier refactoring and better diffs in version control as the last line doesn't need to be modified when adding new elements.

**Good**: (see [[Enums]])
```
enum Tiles {
    TILE_BRICK,
    TILE_FLOOR,
    TILE_SPIKE,
    TILE_TELEPORT,
}
```

**Bad**:
```
enum Tiles {
    TILE_BRICK,
    TILE_FLOOR,
    TILE_SPIKE,
    TILE_TELEPORT
}
```

Trailing commas are unnecessary in single-line lists, so don't add them in this case.