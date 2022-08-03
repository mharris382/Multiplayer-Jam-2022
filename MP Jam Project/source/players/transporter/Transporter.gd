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
	dynamise_block()
	throw_a_block()
	
func on_player_pressed_interact():
	pick_up_a_block()


func dynamise_block():
	if not holds_block:
		var block = move_and_collide(front_aim_point.position, false, true, true)
		if block != null:
			if "tile_set" in block.collider:
				var pos = block.collider.world_to_map(block.position)
				var tilemap = block.collider.get_parent()
				tilemap.delete_tile(pos)

func pick_up_a_block():
	#if Input.is_action_just_pressed("interact_%s" % player_id):
	var block = move_and_collide(front_aim_point.position, false, true, true)
	if block != null:
		if block.collider.is_in_group("dynamic_block"):
			var new_block = load("res://source/blocks/DynamicBlocks/PickedBlock.tscn").instance()
			block.collider.queue_free()
			front_aim_point.add_child(new_block)
			holds_block = true


func throw_a_block():
	if holds_block:
		#var spawn_position = throw_origin.position
		var impulse_force = aim_transform.transform.xform(Vector2.RIGHT) * throw_force
		print("Impulse is a force of %s." % impulse_force)
		var new_block = test_block.instance()
		get_parent().add_child(new_block)
		new_block.set("should_teleport", true)
		new_block.set("node_pos", to_global(front_aim_point.position))
		new_block.apply_impulse(Vector2(0,0), impulse_force)
		front_aim_point.get_child(0).queue_free()
		holds_block = false
