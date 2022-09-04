extends Node2D



#* apply jump boost for all body2d
func _on_Area2D_body_entered(body:Node):
	print(body)
	assert(body!=null)
	if body.has_method("block_jumper"):
		body.block_jumper()
	$AnimationTree.set("parameters/OneShot/active",true)
