class_name BlockLauncher
extends Node2D

#magnitude of force to launch blocks
export var launch_force = 100

onready var launch_direction :RayCast2D = $"Launch Direction"

var facing_right : bool = true

func get_launch_force():
	return launch_direction.transform.xform(Vector2.RIGHT) * launch_force


	

