extends Area2D

func _ready():
# Conecta o sinal para detectar quando algo encosta no item
		connect("body_entered", self, "_on_body_entered")

func _on_body_entered(body):
# Verifica se quem tocou no item foi o jogador Rick
	if body.name == "Rick" or body.has_method("TomarDano"):
		print("Item coletado!")

# Procura o gerenciador de missões na cena atual e avisa que coletou
	var gerenciador = get_tree().current_scene.find_node("GerenciadorMissao", true, false)
	if gerenciador != null:
		gerenciador.coletar_item()

# Remove o item do mapa (coletado)
	queue_free()
