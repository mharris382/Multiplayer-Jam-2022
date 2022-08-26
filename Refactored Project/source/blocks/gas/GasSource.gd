class_name GasSource
extends Node2D

signal steam_source_changed(position, output)
signal register_steam_source(source_node)

export var is_source_unlimited = true
export(int, 1, 100) var source_capacity = 9 #fixed value doesn't change
export(int, 1, 16) var maximum_flow_rate = 4
export var start_open = true


var _gas_released = 0

var flow_rate = 0 setget flow_rate_set, flow_rate_get
var stored_amount = 9 setget stored_amount_set, stored_amount_get

func _ready():
	stored_amount_set(source_capacity)
	_gas_released = 0
	flow_rate = maximum_flow_rate if start_open else 0
	emit_signal("steam_source_changed", position, flow_rate)
	emit_signal("register_steam_source", self)

#returns the amount of gas released
func release_gas_from_source() -> int:
	var stored_amt = source_capacity - _gas_released
	if stored_amt <= 0 and not is_source_unlimited:
		return 0
	var to_release = flow_rate if is_source_unlimited else min(flow_rate, stored_amt)
	_gas_released += to_release
	return to_release
	
func return_gas_to_source(amount):
	_gas_released -= max(amount, 0)
	_gas_released = max(_gas_released, 0)
	
#-------------------------------------------------------
#getter/setter

func stored_amount_set(amount):
	amount = clamp(amount, 0, source_capacity)
	
func stored_amount_get():
	return INF if is_source_unlimited else (source_capacity - _gas_released)


func flow_rate_set(rate : int):
	var new_rate = clamp(rate, 0, maximum_flow_rate)
	flow_rate = new_rate
	if new_rate != rate:
		emit_signal("steam_source_changed", position, new_rate)
		emit_signal("register_steam_source", self)

func flow_rate_get() -> int:
	return flow_rate




func _on_Button_button_down():
#	var new_flow = 0 if flow_rate > 0 else maximum_flow_rate
	flow_rate_set(maximum_flow_rate)
	emit_signal("steam_source_changed", position, maximum_flow_rate)
	emit_signal("register_steam_source", self)



func _on_button_down2(extra_arg_0):
	var change_flow = 1 if extra_arg_0 else -1
	var cur_flow = flow_rate_get()
	cur_flow+=change_flow
	cur_flow = clamp(cur_flow, 0, maximum_flow_rate)
	emit_signal("steam_source_changed", position, cur_flow)
