extends Sprite


var lerpa :Vector2
var lerpi :Vector2


func _ready():
	self.set_material(self.get_material().duplicate(true))
	material.set_shader_param("width", get_texture().get_width())
	material.set_shader_param("height", get_texture().get_height())


func _process(delta):
	lerpi = Vector2(0,-350)
	material.set_shader_param("mouse_position", lerpi)
	lerpa = lerpi
