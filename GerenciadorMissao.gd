extends Node2D
# Quantidade de itens que o jogador precisa pegar para vencer
export var itens_necessarios : int = 3
var itens_coletados : int = 0

func _ready():
	print("Gerenciador de Missão Pronto! Itens necessários: ", itens_necessarios)

func coletar_item():
	itens_coletados += 1
	print("Progresso da Missão: ", itens_coletados, " / ", itens_necessarios)

# Se coletou todos os itens necessários, finaliza o jogo
	if itens_coletados >= itens_necessarios:
		finalizar_jogo()

func finalizar_jogo():
	# VERIFICAÇÃO DA FASE 1 (principal2)
	if get_tree().current_scene.name == "principal2":
		print("Itens da Fase 1 coletados! Chamando o portal...")
		# Em vez de mudar de cena, avisa o C# da Fase 1 para tocar a animação do portal
		if get_tree().current_scene.has_method("Liberar_caminho_fase2"):
			get_tree().current_scene.Liberar_caminho_fase2()

	# VERIFICAÇÃO DA FASE 2 (principal3)
	elif get_tree().current_scene.name == "principal3":
		print("Todos os 4 itens da Fase 2 coletados! Indo para a terceira tela...")
		# Como a terceira tela ainda não existe, vamos mandar provisoriamente para o Menu
		# Quando você criar a terceira tela, basta mudar o nome do arquivo abaixo!
		get_tree().change_scene("res://MenuVencedor.tscn")

