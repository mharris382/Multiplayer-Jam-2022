extends Area2D


func _notification(what):
	if what == NOTIFICATION_PARENTED:
		print("Static Block Area2D, parented to: (%s)" % get_parent().name)

func _on_StaticBlock__Area2D_area_entered(area):
	print("Static Block Area2D, area entered: (%s)"% area.name)
