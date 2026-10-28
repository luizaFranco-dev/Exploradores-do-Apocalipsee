eextends KinematicBody2D

export var velocidade = 100
var vetor_movimento = Vector2.ZERO
var jogador = null

func _physics_process(delta):
	vetor_movimento = Vector2.ZERO

	if jogador != null:
		vetor_movimento = position.direction_to(jogador.position) * velocidade
	
	vetor_movimento = move_and_slide(vetor_movimento)
	 
func _on_AreaDetecao_body_entered(body):
		if body.name =="Rick":
			jogador = body 
	
func _on_AreaDetecao_body_exited(body):
			if body.name =="Rick":
				jogador = null
		
