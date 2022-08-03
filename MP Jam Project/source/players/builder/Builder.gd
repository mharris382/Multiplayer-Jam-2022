class_name Builder
extends CharacterBase

# Called when the node enters the scene tree for the first time.
func _ready():
	._ready()
	assign_player(Players.players[1])

func is_direction_valid(aim_direction) -> bool:
	match(aim_direction):
		AimDirection.BELOW:
			return !is_on_floor()
		AimDirection.ABOVE:
			return !is_on_ceiling()
		AimDirection.FRONT:
			return !is_on_floor()
	return true

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
