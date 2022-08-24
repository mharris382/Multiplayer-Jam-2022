extends Node2D

enum TileIDs{
	SOURCE = -2,
	SINK = -1,
	CLEAR = 0,
	NEUTRAL_PRESSURE = 1,
	LIGHT_PRESSURE = 5,
	MEDIUM_PRESSURE = 10,
	HEAVY_PRESSURE = 15,
	MAXIMUM_PRESSURE = 16}
const SOURCE_OUTPUT = 2

onready var timer = $IterationTimer
onready var steam_tilemap  = $"Steam TileMap"
onready var block_tilemap = $"Block TileMap"

export var iterations_per_sec = 1.0
export var flow_capacity = 1

var source_tiles = {}
#	Vector2.ZERO : SOURCE_OUTPUT,
#	Vector2(10, 10) : 2,
#	Vector2(5, 5) : 1}



func _ready():
	timer.one_shot = false
	timer.wait_time = 1/iterations_per_sec
	timer.connect("timeout", self, "_iterate_gas")
	assert($"Steam TileMap" != null)
	pass

func _iterate_gas():
	print("Gas Iterations")
	var visited = {}
	var unvisited_tiles = {
		Vector2.ZERO: TileIDs.SOURCE
	}
	
	for block in block_tilemap.get_used_cells():
		steam_tilemap.clear_steamv(block)
	
	for source in source_tiles.keys():
		steam_tilemap.modify_steam(source, source_tiles[source])
	
	for gas in steam_tilemap.get_used_cells():
		var steam =steam_tilemap.get_steamv(gas)
		if steam > 1:
			
			var neighbors = steam_tilemap.get_lower_neighbors(gas)
			var cnt = 0
			for neighbor in neighbors:
				if block_tilemap.get_cellv(neighbor) == -1:
					cnt+=1
			if cnt == 0:
				continue
			else:
				var amount = max(steam / cnt, 1)
				amount = min(amount, flow_capacity)
				for neighbor in neighbors:
					if block_tilemap.get_cellv(neighbor) == -1:
						steam_tilemap.modify_steam(neighbor, amount)
						steam_tilemap.modify_steam(gas, -amount)
		
	#var tile_pressures = {}
	
#	for i in range(16):
#		if i == 0:
#			continue
#		else:
#			var found =steam_tilemap.get_used_cells_by_id(i-1)
#			#tile_pressures[i-1] = []
#			if found.size() > 0:
#				#tile_pressures[i-1].append(found)
#				for tile in found:
#					unvisited_tiles[tile] = i-1


func _on_Button_button_down():
	timer.start(1/iterations_per_sec)

func register_gas_source(sourceNode ) -> bool:
	var sourceNode2D = sourceNode as Node2D
	if sourceNode2D == null:
		return false
	var tile_position = steam_tilemap.world_to_map(sourceNode2D.position)
	assert(sourceNode2D.Output != -1)
	return true

func is_position_blocked(tile_position) -> bool:
	return block_tilemap.get_cellv(tile_position) != -1


func _on_Source_SteamSourceChanged(world_position, output):
	if steam_tilemap == null:
		steam_tilemap = $"Steam TileMap"
	var pos = steam_tilemap.world_to_map(world_position)
	source_tiles[pos] = output
	print("Steam Source changed at tile position ", pos)
