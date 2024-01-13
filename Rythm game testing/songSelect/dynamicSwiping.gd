extends Path2D

#var length = 150
var startPos: Vector2
var curPos: Vector2
var swiping = false
var threshold = 10
var selected = 0
# 0 = Easy song
# 1 = Medium Song
# 2 = hard
# var b = "text"



# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	
	#comment out these 3 lines of code to disable songcard rotation
	get_node("SongMain/SpriteM").rotation_degrees = -69 + (138*get_node("SongMain").unit_offset)
	get_node("SongLeft/SpriteL").rotation_degrees = -69 + (138*get_node("SongLeft").unit_offset)
	get_node("SongRight/SpriteR").rotation_degrees = -69 + (138*get_node("SongRight").unit_offset)
	
	startPos = curPos
	if Input.is_action_just_pressed("press"):
		startPos = get_global_mouse_position()
		curPos = get_global_mouse_position()
		
	if Input.is_action_pressed("press"):
		curPos = get_global_mouse_position()
	
	var calcOffset = ((curPos.x-startPos.x)/1000)
	get_node("SongMain").unit_offset -= calcOffset
	get_node("SongLeft").unit_offset -= calcOffset
	get_node("SongRight").unit_offset -= calcOffset
