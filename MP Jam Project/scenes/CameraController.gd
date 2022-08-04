extends Node


onready var camera : Camera2D = $Camera2D
onready var builder_position : RemoteTransform2D = $BuilderRemoteTransform
onready var transporter_position : RemoteTransform2D = $TranporterRemoteTransform


func _ready():
	
	pass


func _process(delta):
	if builder_position.
