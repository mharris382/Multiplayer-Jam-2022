# [TileMap](https://docs.godotengine.org/en/stable/classes/class_tilemap.html)

**Inherits:** [[Node2D]] < [[CanvasItem]] < [[Node]] **<** [Object](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object)

references a [[TileSet]]



## Properties[¶](https://docs.godotengine.org/en/stable/classes/class_tilemap.html#properties "Permalink to this headline")



  return | method name
  ---|----
`void` | `clear ( )`
`void` | `fix_invalid_tiles ( )`
`int` | `get_cell ( int x, int y ) const`
`Vector2` | `get_cell_autotile_coord ( int x, int y ) const`
`int` | `get_cellv ( Vector2 position ) const`
`bool` | `get_collision_layer_bit ( int bit ) const`
`bool` | `get_collision_mask_bit ( int bit ) const`
`Array` | `get_used_cells ( ) const`
`Array` | `get_used_cells_by_id ( int id ) const`
`Rect2` | `get_used_rect ( )`
`bool` | `is_cell_transposed ( int x, int y ) const`
`bool` | `is_cell_x_flipped ( int x, int y ) const`
`bool` | `is_cell_y_flipped ( int x, int y ) const`
`Vector2` | `map_to_world ( Vector2 map_position, bool ignore_half_ofs=false ) const`
`void` | `set_cell ( int x, int y, int tile, bool flip_x=false, bool flip_y=false, bool transpose=false, Vector2 autotile_coord=Vector2( 0, 0 ) )`
`void` | `set_cellv ( Vector2 position, int tile, bool flip_x=false, bool flip_y=false, bool transpose=false, Vector2 autotile_coord=Vector2( 0, 0 ) )`
`void` | `set_collision_layer_bit ( int bit, bool value )`
`void` | `set_collision_mask_bit ( int bit, bool value )`
`void` | `update_bitmask_area ( Vector2 position )`
`void` | `update_bitmask_region ( Vector2 start=Vector2( 0, 0 ), Vector2 end=Vector2( 0, 0 ) )`
`void` | `update_dirty_quadrants ( )`
`Vector2` | `world_to_map ( Vector2 world_position ) const`

