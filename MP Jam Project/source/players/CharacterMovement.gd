extends Node

var avatar_body: KinematicBody2D

func _ready():
	var parent = get_parent() as KinematicBody2D
	assert(parent != null)
	avatar_body = parent
