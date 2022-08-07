class_name Ability
extends Node

signal aiming_mode_changed(aiming)


export var allow_diagnal_aiming = true

const ENABLED_BIT  = 1 << 0
const AIM_MODE_BIT = 1 << 1
const X_AXIS_BITS  = 0b11 << 2 # Reason we use 2 bits here is because there are three possible states for each axis (positive, negative, and zero)
const Y_AXIS_BITS  = 0b11 << 4 # the smaller bit is the magnitude bit, (i.e. if the axis is zero), the other bit is the direction bit is the direction + or -
# this means there is one pattern which is not used: 0b10 is not used because the first bit is off, so we can't have direction without any magnitude

enum AbilityMode{
	ABILITY_NOT_ALLOWED =0,
	ABILITY_ALLOWED =0b00-00-01,
	AIM_MODE_ON 	=0b00-00-11,
	AIM_MODE_OFF	=0b00-00-01,
	AIM_FRONT   	=0b00-00-11,
	AIM_DOWN    	=0b00-10-11,
	AIM_UP      	=0b00-11-11,
	AIM_RIGHT   	=0b11-00-11,
	AIM_LEFT    	=0b10-00-11
}

var ability_mode
# ability mode is essentially the same concept as the build_mode in Builder
#var is_enabled = true setget is_enabled_set, is_enabled_get

var aiming = false setget aiming_set, aiming_get



func aiming_set(aiming):
	var prev_state = ability_mode
	if aiming:
		ability_mode |= AbilityMode.AIM_MODE_ON
		pass
	else:
		ability_mode &= not AbilityMode.AIM_MODE_ON
		pass
	if ability_mode != prev_state:
		
		# we know aiming changed
		pass
	pass
	
func aiming_get():
	return (ability_mode & AbilityMode.AIM_MODE_ON) != 0

func _direction_to_aim_pattern(dir):
	var has_x = int(dir.x != 0) << 3
	var dir_x = int(dir.x > 0) << 4
	var x_axis = has_x | dir_x
	
	if not allow_diagnal_aiming and has_x != 0:
		return x_axis
	
	var has_y = int(dir.y != 0) << 5
	var dir_y = int(dir.y > 0) << 4
	var y_axis = has_y | dir_y
	
	return x_axis | y_axis
#start aiming from either pressing aim button or from aim input axis
