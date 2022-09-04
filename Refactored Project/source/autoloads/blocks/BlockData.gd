class_name BlockData
extends Resource


export var destructable : bool #can this tile be destroyed
export var tile_name : String #name given to the cooresponding tilemap tile
export var dynamic_block : PackedScene #this is required for all blocks which are destructable 
export var static_block : PackedScene #optionally provide a scene which will be instanced on top of the tilemap
