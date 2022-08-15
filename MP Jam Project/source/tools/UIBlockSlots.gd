extends TextureRect

export var run_test = true
export var test_block_name_1 = "Block_Plate"
export var test_block_count = 1
export var test_block_name_2 = "Block_Plate"
export var test_block_name_3 = "Block_Plate"

onready var center_block_count = $"Center Slot/Count"
onready var center_block_texture = $"Center Slot/Block"
onready var next_block_texture = $"Next Slot/Block"
onready var previous_block_texture = $"Previous Slot/Block"



func _TEST():
	center_block_texture.block_name = test_block_name_1
	center_block_texture.show_block = true
	center_block_count.text = String(test_block_count)
	
	next_block_texture.block_name = test_block_name_2
	next_block_texture.show_block = true
	
	previous_block_texture.block_name = test_block_name_3
	previous_block_texture.show_block = true
	
	assert(center_block_texture.show_block == Blocks.has_block(test_block_name_1))
	assert(next_block_texture.show_block == Blocks.has_block(test_block_name_2))
	assert(previous_block_texture.show_block == Blocks.has_block(test_block_name_3))

func _ready():
	if run_test:
		_TEST()
	pass
