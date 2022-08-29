extends TileMap


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
const gas_name = "dummy gas"
const tile_name = "AutoTile_Pipe_Template"
var tile_id: int
# Called when the node enters the scene tree for the first time.
func _ready():
	tile_id = self.tile_set.find_tile_by_name(tile_name)
	var gas_id =self.tile_set.find_tile_by_name(gas_name)
	var found = self.get_used_cells_by_id(gas_id)
	assert(tile_id != 0)
	assert(found is Array)
	assert(found.size() > 0)
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
