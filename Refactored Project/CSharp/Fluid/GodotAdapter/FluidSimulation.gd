extends Node



onready var steam_Tilemap = $SteamTilemap
onready var block_Tilemap = $BlockTilemap
onready var timer = $Timer
var fluid_adapter


func _ready():
	var csharp_fluid_adapter_script = load("res://CSharp/Fluid/GodotAdapter/FluidAdapter.cs")
	fluid_adapter = csharp_fluid_adapter_script.new()
	fluid_adapter.Setup(steam_Tilemap, block_Tilemap, timer)
	fluid_adapter._Ready()


