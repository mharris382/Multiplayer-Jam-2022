extends Node


onready var spawns = $"Spawn Points"
var my_coroutine
var done = false
var rng
var last
# Called when the node enters the scene tree for the first time.
func _ready():
	rng = RandomNumberGenerator.new()
	

func spawn():
	var children = spawns.get_children()
	var cnt = spawns.get_child_count()
	var index = rng.randi_range(0, cnt-1)
	if index == last:
		if index == 0:
			index = cnt-1
		elif index == cnt-1:
			index = 0
		else:
			index+=1
	spawn_at_point(spawns.get_child(index))
	done = true

func spawn_at_point(node : Node2D):
	var block_name = "Block_%s" % node.name
	if not (Blocks.has_block(block_name)):
		print("Blocks could not find block named ", block_name)
		return
	if not (Blocks.get_block_data(block_name) != null):
		print("Blocks could not find block named ", block_name)
		return
	print("Spawned ", block_name)
	var block =  Blocks.instance_dynamic_block_at_location(block_name, node.position, self)
	if node.get_child_count() > 0:
		block.add_child(node.get_child(0))


func _on_Button_button_down():
	spawn()
