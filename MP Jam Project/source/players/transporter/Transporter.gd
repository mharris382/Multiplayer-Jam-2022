class_name Transporter
extends CharacterBase
export var throw_force : float = 5
export (PackedScene) var test_block

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
	var block_name = front_aim_point.get_child(0).current_name
	return Blocks.instance_dynamic_block_at_location(block_name, to_global(front_aim_point.position), get_parent())

func pick_up_a_block():
	#if Input.is_action_just_pressed("interact_%s" % player_id):
	var block = move_and_collide(front_aim_point.position, false, true, true)
	if block != null:
		if block.collider.is_in_group("dynamic_block"):
			var new_block = load("res://source/utils/BlockSprite.tscn").instance()
			var dynamic_block = block.collider as RigidBody2D
			new_block._on_block_changed(dynamic_block.block_name)
			front_aim_point.add_child(new_block)
			dynamic_block.queue_free()
			holds_block = true


func throw_a_block():
	if holds_block:
		var impulse_force = aim_transform.transform.xform(Vector2.RIGHT) * throw_force
		print("Impulse is a force of %s." % impulse_force)
		var new_block = get_block_from_pick()
		new_block.apply_impulse(Vector2(0,0), impulse_force)
		front_aim_point.get_child(0).queue_free()
		holds_block = false
