extends TextureRect

var show_block = false setget show_block_set, show_block_get
var block_name = "" setget block_name_set, block_name_get

func _ready():
	pass



func show_block_set(value : bool):
	show_block = value
	_update_visibility()

func show_block_get() -> bool:
	return show_block and _has_valid_block_name()
	
func block_name_set(value : String):
	block_name = value
	_update_visibility()
	
	
func block_name_get() -> String:
	return block_name

func _has_valid_block_name() -> bool:
	return Blocks.has_block(block_name)

func _update_visibility():
	visible = show_block_get()
	var region = Blocks.block_get_texture_rect(block_name)
	
	var atlas_texture = texture as AtlasTexture
	atlas_texture.atlas = Blocks.block_get_texture(block_name)
	assert(atlas_texture != null)
	atlas_texture.region = region
