class_name Transporter
extends CharacterBase
export var throw_force : float = 5

var holds_block = false

onready var throw_origin : Node2D #these can be the same transform
onready var aim_transform : Position2D = $ControlPoints/Aim

func _ready():
	._ready()
	assign_player(Players.players[0])
	
func _process(delta):
	._process(delta)
	
func is_direction_valid(aim_direction) -> bool:
	match(aim_direction):
		AimDirection.BELOW:
			return !is_on_floor()
		AimDirection.ABOVE:
			return !is_on_ceiling()
		AimDirection.FRONT:
			return !is_on_floor()
	return true

func on_player_just_pressed_ability(aim):
	.on_player_just_pressed_ability(aim)
	make_block_dynamic()
	throw_a_block()
	
func on_player_just_pressed_interact():
	.on_player_just_pressed_interact()
	pick_up_a_block()


func make_block_dynamic():
	if not holds_block:
		var aim_position: Vector2 = get_aim_direction(facing).position
		var tilemap = get_tree().get_nodes_in_group("Tilemap")[0]
		var block_position = tilemap.world_to_map(position + aim_position)
		var data = tilemap.cell_get_block_data(block_position)
		if data != null and data.destructable != false:
			tilemap.build_dynamic_block(block_position)
		

func get_block_from_pick():
	var block = front_aim_point.get_child(0)
	return Blocks.get_block_data(block.tile_name).dynamic_block

func pick_up_a_block():
	#if Input.is_action_just_pressed("interact_%s" % player_id):
	var block = move_and_collide(front_aim_point.position, false, true, true)
	if block != null:
		if block.collider.is_in_group("dynamic_block"):
			var new_block = load("res://source/blocks/DynamicBlocks/PickedBlock.tscn").instance()
			var dynamic_block = block.collider as RigidBody2D
			new_block.tile_name = dynamic_block.block_data.tile_name
			new_block.set_picked_texture(dynamic_block.get_block_texture())
			front_aim_point.add_child(new_block)
			dynamic_block.queue_free()
			holds_block = true


func throw_a_block():
	if holds_block:
		#var spawn_position = throw_origin.position
		var impulse_force = aim_transform.transform.xform(Vector2.RIGHT) * throw_force
		print("Impulse is a force of %s." % impulse_force)
		var new_block = get_block_from_pick().instance()
		get_parent().add_child(new_block)
		new_block.set("should_teleport", true)
		new_block.set("node_pos", to_global(front_aim_point.position))
		new_block.apply_impulse(Vector2(0,0), impulse_force)
		front_aim_point.get_child(0).queue_free()
		holds_block = false
