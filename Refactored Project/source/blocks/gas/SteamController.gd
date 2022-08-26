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

var sources = []

var visited = {}
var unvisited = {}

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
	
	
	_iterate_blocks()
	_iterate_sources()
	
#	for gas in steam_tilemap.get_used_cells():
#		var steam =steam_tilemap.get_steamv(gas)
#		if steam > 1:
#			var neighbors = steam_tilemap.get_lower_neighbors(gas)
#			var cnt = 0
#			for neighbor in neighbors:
#				if block_tilemap.get_cellv(neighbor) == -1:
#					cnt+=1
#			if cnt == 0:
#				continue
#			else:
#				var amount = max(steam / cnt, 1)
#				amount = min(amount, flow_capacity)
#				for neighbor in neighbors:
#					if block_tilemap.get_cellv(neighbor) == -1:
#						steam_tilemap.modify_steam(neighbor, amount)
#						steam_tilemap.modify_steam(gas, -amount)
						
	for cell in steam_tilemap.get_used_cells():
		var steam = steam_tilemap.get_steamv(cell)
		if steam > 0:
			var neighbors = steam_tilemap.get_neighbors(cell, block_tilemap)
			for neighbor in neighbors:
				var neighbor_steam = steam_tilemap.get_steamv(neighbor)
				if steam > neighbor_steam:
					if neighbor_steam == 16:
						continue
					else:
						var flow_amount = steam_tilemap.get_steamv(cell) / 3
						flow_amount = max(flow_amount, steam)
						flow_amount = min(flow_amount, 16 - neighbor_steam)
						steam_tilemap.modify_steam(neighbor, flow_amount)
						steam_tilemap.modify_steam(cell, -flow_amount)
						steam = steam_tilemap.get_steamv(cell)

func _iterate_blocks():
	for block in block_tilemap.get_used_cells():
		steam_tilemap.clear_steamv(block)
		

func _iterate_sources():
	for source in source_tiles.keys():
		steam_tilemap.modify_steam(source, source_tiles[source])

	for source in sources:
		var src = source as GasSource
		if src == null:
			continue
		var rate= src.flow_rate
		var amount_released = src.release_gas_from_source()
		var pos = steam_tilemap.world_to_map(src.position)
		var overflow = steam_tilemap.modify_steam(pos, amount_released)
		if overflow > 0: #this could be changed to push the overflow into adjacent cells
			src.return_gas_to_source(overflow)
		

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
	source_tiles[steam_tilemap.world_to_map(world_position)]=output


func _on_Source4_steam_source_changed(position, output):
	_on_Source_SteamSourceChanged(position, output)


func _on_Source_register_steam_source(source_node):
	sources.append(source_node)
