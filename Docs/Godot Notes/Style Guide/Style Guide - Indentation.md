## Indentation
**Good**:
```
for i in range(10):
    print("hello")
```

**Bad**:
```
for i in range(10):
  print("hello")

for i in range(10):
        print("hello")
```


Exceptions to this rule are [[Array|arrays]], [[Dictionary|dictionaries]], and [[Enums]]. Use a single indentation level to distinguish continuation lines:
**Good**:
```
var party = [
    "Godot",
    "Godette",
    "Steve",
]

var character_dict = {
    "Name": "Bob",
    "Age": 27,
    "Job": "Mechanic",
}

enum Tiles {
    TILE_BRICK,
    TILE_FLOOR,
    TILE_SPIKE,
    TILE_TELEPORT,
}
```

**Bad**:
```
var party = [
        "Godot",
        "Godette",
        "Steve",
]

var character_dict = {
        "Name": "Bob",
        "Age": 27,
        "Job": "Mechanic",
}

enum Tiles {
        TILE_BRICK,
        TILE_FLOOR,
        TILE_SPIKE,
        TILE_TELEPORT,
}
```

