extends Node


onready var spawns = $"Spawn Points"

# Called when the node enters the scene tree for the first time.
func _ready():
	var remap = "Furnace_1_On"
	print(Blocks.remap_block_name(remap))
	assert(Blocks.remap_block_name(remap) == "Furnace_1")
	remap = "Furnace_1_Off"
	print(Blocks.remap_block_name(remap))
	var children = spawns.get_children()
	for child in children:
		spawn_at_point(child)

func spawn_at_point(node : Node2D):
	var block_name = "Block_%s" % node.name
	if not (Blocks.has_block(block_name)):
		print("Blocks could not find block named ", block_name)
		return
	if not (Blocks.get_block_data(block_name) != null):
		print("Blocks could not find block named ", block_name)
		return
	Blocks.instance_dynamic_block_at_location(block_name, node.position, self)
