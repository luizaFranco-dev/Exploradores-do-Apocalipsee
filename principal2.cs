using Godot;
using System;

public class principal2 : Node2D
{
	private Area2D _portal;
	private AnimatedSprite _portalAnim;
	private CollisionShape2D _portalCol;

	public override void _Ready()
	{
		// Encontra o portal e seus componentes dentro do cenário
		_portal = GetNode<Area2D>("PortalFase2");
		_portalAnim = GetNode<AnimatedSprite>("PortalFase2/AnimatedSprite");
		_portalCol = GetNode<CollisionShape2D>("PortalFase2/CollisionShape2D");

		// Conecta as animações e o toque do jogador automaticamente por código
		if (_portalAnim != null)
		{
			_portalAnim.Connect("animation_finished", this, nameof(OnPortalAnimationFinished));
		}

		if (_portal != null)
		{
			_portal.Connect("body_entered", this, nameof(OnPortalBodyEntered));
		}
	}

	// 1. Chamado pelo Gerenciador de Missões ao coletar os 3 itens
	public void Liberar_caminho_fase2()
	{
		GD.Print("Todos os itens coletados! Tornando o portal visível e abrindo...");

		if (_portal != null && _portalCol != null)
		{
			_portal.Visible = true;        // Faz o portal aparecer no mapa
			_portalCol.Disabled = false;   // Ativa a colisão física dele
		}

		if (_portalAnim != null)
		{
			_portalAnim.Play("abrindo");   // Começa com a animação de abertura
			GD.Print("Portal começando a se abrir...");
		}
	}

	// 2. Roda automaticamente quando a animação "abrindo" chega ao fim
	public void OnPortalAnimationFinished()
	{
		if (_portalAnim != null && _portalAnim.Animation == "abrindo")
		{
			_portalAnim.Play("faiscas");   // Troca para o loop contínuo de faíscas
			GD.Print("Portal totalmente aberto e soltando faíscas!");
		}
	}

	// 3. Roda quando o Rick pisa fisicamente no portal aberto
	public void OnPortalBodyEntered(Node body)
	{
		if (body.Name == "Rick")
		{
			GD.Print("Rick entrou no portal! Carregando a Fase 2...");
			GetTree().ChangeScene("res://principal3.tscn");
		}
	}
}
