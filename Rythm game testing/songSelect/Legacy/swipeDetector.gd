extends Camera2D

var length = 150
var startPos: Vector2
var curPos: Vector2
var swiping = false
var threshold = 10

# Declare member variables here. Examples:
# var a = 2
# var b = "text"



# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("press"):
		if !swiping:
			swiping = true
			startPos = get_global_mouse_position()
			print("Start Position: ", startPos)
	if Input.is_action_pressed("press"):
		if swiping: 
			curPos = get_global_mouse_position()
			if startPos.distance_to(curPos) >= length:
				if abs(startPos.y-curPos.y) <= threshold:
					print("Horizontal Swipe!")
					swiping = false
	else:
		swiping = false
