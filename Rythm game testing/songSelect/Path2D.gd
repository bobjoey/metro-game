tool
extends Path2D

const SIZE = 400
const NUM_POINTS = 128


func _ready() -> void:
	print("Is this running?")
	curve = Curve2D.new()
	for i in NUM_POINTS:
		curve.add_point(Vector2(0, -SIZE).rotated((i / float(NUM_POINTS)) * TAU))

	# End the circle.
	curve.add_point(Vector2(0, -SIZE))
