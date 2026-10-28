extends KinematicBody2D

export var velocidade = 100
var vetor_movimento = Vector2.ZERO
var jogador = null
var vida = 25

var tempo_animacao = 0.0
var frame_atual = 0

# Configuração da spritesheet do zumbi:
# Linha 0 = Frente/Baixo, Linha 1 = Esquerda, Linha 2 = Direita, Linha 3 = Costas/Cima
var linha_animacao = 0 

func _physics_process(delta):
	vetor_movimento = Vector2.ZERO

	if jogador != null:
		vetor_movimento = global_position.direction_to(jogador.global_position) * velocidade

	move_and_slide(vetor_movimento)
	
		# --- SISTEMA DE ANIMAÇÃO DO ZUMBI ---
	# Se o zumbi estiver se movendo
	if vetor_movimento != Vector2.ZERO:
		# 1. Descobre a direção para escolher a linha correta da Spritesheet
		if abs(vetor_movimento.x) > abs(vetor_movimento.y):
			if vetor_movimento.x > 0:
				linha_animacao = 2 # Direita
			else:
				linha_animacao = 1 # Esquerda
		else:
			if vetor_movimento.y > 0:
				linha_animacao = 0 # Frente/Baixo
			else:
				linha_animacao = 3 # Costas/Cima

		# 2. Controla o tempo para mudar de frame (Velocidade da animação)
		tempo_animacao += delta
		if tempo_animacao >= 0.15: # Muda de frame a cada 0.15 segundos
			tempo_animacao = 0.0
			frame_atual = (frame_atual + 1) % 4 # Alterna entre os 4 frames da linha
			
		# 3. Aplica o cálculo matemático de frame final no Sprite
		$Sprite.frame = (linha_animacao * 4) + frame_atual
	else:
		# Se o zumbi parar por algum motivo, reseta para o frame parado da linha atual
		$Sprite.frame = (linha_animacao * 4) + 0


func _on_AreaDetecao_body_entered(body):
	if body.name == "Rick":
		jogador = body

func _on_AreaDetecao_body_exited(body):
	if body.name == "Rick":
		jogador = null


func _on_areaAtaque_body_entered(body):
	if body.name == "Rick":
		print("ATAQUE ZUMBI")
		body.call("tomar_dano", 25)

func tomar_dano(quantidade):
	vida -= quantidade
	print("Zumbi levou dano! Vida atual: ", vida)
	
	if vida <= 0:
		print("Zumbi morreu!")
		queue_free()

