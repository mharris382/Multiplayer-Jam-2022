extends Node2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
export var tilemapPath : NodePath
export var blockTilemapPath : NodePath
export var seed_quantity = 4
export var source_cell = Vector2(10,10)
export var source_amount = 1
export var source_size = Vector2(3, 2)

enum DirectionBitMap{
	RIGHT = 0b0001
	LEFT = 0b0010
	DOWN = 0b0100
	UP = 0b1000
}

var _blockTileMap : TileMap
var _gasTileMap : TileMap
var _cell_valid_direction_cache = {}

onready var resultTileMap = $TileMap
onready var graph_node = $Graphs

func _ready():
	_gasTileMap = get_node(tilemapPath)
	_gasTileMap.clear()
	assert(_gasTileMap != null)
	for x in range(5, 12):
		for y in range(4, 6):
			_gasTileMap.SetSteam(x, y, seed_quantity)
	

func _process(delta):
	var mp= get_global_mouse_position()
	var gasSpace = _gasTileMap.world_to_map(mp)
	if(is_blocked_gas_space(int(gasSpace.x),int(gasSpace.y))):
		print(gasSpace, "is Blocked")

func get_airflow_directions( xGas, yGas):
	var directionBitMask = _get_possible_blocked_directions(xGas, yGas)
	if directionBitMask <= 0:
		return 0
	
	return directionBitMask

func is_blocked_gas_space( xGas,  yGas):
	var blockSpace = remap_gas_to_block(xGas, yGas)
	return is_blockedv(blockSpace)
	
func is_blocked_gas_spacev( gas):
	var blockSpace = remap_gas_to_block(int(gas.x),int(gas.y))
	return is_blockedv(blockSpace)
	
func is_blocked_world_space( pos:Vector2):
	var block_space = _blockTileMap.world_to_map(Vector2(pos.x, pos.y))
	return is_blockedv(block_space)
	
	
func is_blockedv(v):
	return is_blocked(int(v.x), int(v.y))
	
func is_blocked(xBlock, yBlock):
	if _blockTileMap == null:
		_blockTileMap = $"Block TileMap"
	return _blockTileMap.get_cell(xBlock, yBlock) != -1
	
var visited = {}
var pos = Vector2(5,5)
var sources = {}
func _on_IterationTimer_timeout():
	var usedCells = _gasTileMap.get_used_cells()
	visited.clear()
	apply_sources()
	var clear_queue = []
	for cell in usedCells:
		if is_blocked_gas_space(cell.x, cell.y):
			_gasTileMap.set_cell(cell.x, cell.y, -1)
			if _cell_valid_direction_cache.has(cell):
				_cell_valid_direction_cache.erase(cell)
		else:
			_cell_valid_direction_cache[cell] = 0xF
	
	for cell in _cell_valid_direction_cache.keys():
		
		pass
	
	var firstCell = usedCells[0]
	
	_traverse_graph(firstCell)
	
	count_total_gas()
	
func apply_sources():
	sources.clear()
	for x in range(int(source_size.x)):
		for y in range(int(source_size.y)):
			
			apply_source(source_cell + Vector2(x,y))

func apply_source(cell):
	var cur = _gasTileMap.GetSteam(cell)
	cur += source_amount
	sources[cell] = cur
	_gasTileMap.SetSteam(cell, cur)
func count_total_gas():
	var usedCells = _gasTileMap.get_used_cells()
	var total = 0
	for cell in usedCells:
		total += _gasTileMap.GetSteam(cell)
	return total

func remap_gas_to_block(x, y, subtiles = 4):
	var xc = int(x) / int(subtiles)
	var yc = int(y) / int(subtiles)
	return Vector2(xc, yc)

func remap_block_to_gas(x, y, subtiles = 4):
	var gx0 = x * subtiles
	var gy0 = y * subtiles
	var arr = []
	for i in range(subtiles):
		for j in range(subtiles):
			arr.append(Vector2(i, j))

func _get_possible_blocked_directions(x:int, y:int, subtiles = 4) -> int:
	var x0 = x % subtiles
	var y0 = y % subtiles
	if subtiles <= 1:
		return 0b1111
	var mask = 0
	if x0 == 0:
		mask |= DirectionBitMap.RIGHT;
	elif x0==subtiles-1:
		mask |= DirectionBitMap.LEFT;
	if y0 == 0:
		mask |= DirectionBitMap.UP;
	elif y0 == subtiles-1:
		mask |= DirectionBitMap.DOWN;
	return mask

func _bitmask_to_vector_array(bitmask : int):
	var arr = []
	if bitmask == 0:
		return arr
		
	var hasRight = (DirectionBitMap.RIGHT & bitmask) != 0
	var hasLeft = (DirectionBitMap.LEFT & bitmask) != 0
	var hasDown = (DirectionBitMap.DOWN & bitmask) != 0
	var hasUp = (DirectionBitMap.UP & bitmask) != 0
	if hasRight:
		arr.append(Vector2.RIGHT)
	if hasLeft:
		arr.append(Vector2.LEFT)
	if hasUp:
		arr.append(Vector2.UP)
	if hasDown:
		arr.append(Vector2.DOWN)

func _mask_has_direction(mask : int, direction):
	return mask & direction != 0
	
func _get_valid_neighbors(gasX, gasY):
	var block = Vector2(gasX, gasY)
	var mask = _get_possible_blocked_directions(gasX, gasY)
	#if mask doesn't have direction then it cannot be blocked because the cell is not a neighbor of block grid
	var upBlocked = _mask_has_direction(mask, DirectionBitMap.UP) and is_blocked_gas_spacev(block + Vector2.UP)
	var downBlocked = _mask_has_direction(mask, DirectionBitMap.DOWN) and is_blocked_gas_spacev(block + Vector2.DOWN)
	var rightBlocked =_mask_has_direction(mask, DirectionBitMap.RIGHT) and is_blocked_gas_spacev(block + Vector2.RIGHT)
	var leftBlocked =_mask_has_direction(mask, DirectionBitMap.LEFT) and is_blocked_gas_spacev(block + Vector2.LEFT)
	var arr = []
	var vGas = Vector2(gasX, gasY)
	if not upBlocked:
		arr.append(vGas + Vector2.UP)
	if not downBlocked:
		arr.append(vGas + Vector2.DOWN)
	if not leftBlocked:
		arr.append(vGas + Vector2.LEFT)
	if not rightBlocked:
		arr.append(vGas + Vector2.RIGHT)
	return arr

func _traverse_graph(start):
	
	_traverse2(start)
	print("Found ", visited.size())

func _traverse(node):
	if visited.has(node):
		return
	#_gasTileMap.SetSteam(node.x, node.y, 4)
	resultTileMap.set_cellv(node, 1)
	visited[node] = 0
	var neighbors = _get_valid_neighbors(node.x, node.y)
	for neighbor in neighbors:
		if not visited.has(neighbor):
			_traverse(neighbor)

func _traverse2(node):
	if visited.has(node):
		return
	#_gasTileMap.SetSteam(node.x, node.y, 4)
	
	
	visited[node] = 0
	var neighbors = _get_valid_neighbors(node.x, node.y)
	neighbors.shuffle()
	var curr = _gasTileMap.GetSteam(node.x, node.y)
#	var total1 = count_total_gas()
	for neighbor in neighbors:
		if not visited.has(neighbor):
			var n2 = _gasTileMap.GetSteam(neighbor.x, neighbor.y)
			assert(curr >= 0)
			assert(curr <= 16)
			if n2 < curr:
				if curr > 1:
					
					_gasTileMap.ModifySteam(node, -1)
					_gasTileMap.ModifySteam(neighbor,1)
					
					
					
					curr-=1
					
			elif curr < n2 and not sources.has(node):
				if n2 > 1:
					assert(curr<16)
					
					var actual = _gasTileMap.ModifySteam(node, 1)
					_gasTileMap.ModifySteam(neighbor,-1)
					curr+=1
					
			_traverse2(neighbor)
#	var new_total1 = count_total_gas()
#	assert(total1 == new_total1)
