using Godot;
using System;

public class Rick : KinematicBody2D
{
	[Export]
	public float Speed = 200.0f;
	[Export] 
	public int VidaMaxima = 100;
	
	public int VidaAtual;
	[Export]
public bool TemEspada = false; // Começa como falso (sem espada)

	[Export]
	public NodePath CoracoesPath;
	private HBoxContainer coracoes;
	
	private Vector2 _velocity = Vector2.Zero;
	private Area2D _areaEspada;
	private Sprite _spriteEspada;
	
private AnimatedSprite _anim;
private bool _olhandoParaTras = false;

	public override void _Ready()
	{
		VidaAtual = VidaMaxima;
		_areaEspada = GetNode<Area2D>("AreaEspada");
		
		  if (CoracoesPath != null) 
		coracoes = GetNode<HBoxContainer>(CoracoesPath);
		
		_anim = GetNode<AnimatedSprite>("AnimatedSprite");
		 
		_spriteEspada = GetNode<Sprite>("SpriteEspada");
	}
	
	public override void _PhysicsProcess(float delta)
	{
		AtualizarInterfaceCoracoes();
		
		Vector2 direction = Vector2.Zero;
		direction.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		direction.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		
		direction = direction.Normalized();
		
		_velocity = direction * Speed;
		
		_velocity = MoveAndSlide(_velocity);
		
		// CONTROLE DO SISTEMA DE ANIMAÇÕES (Adicionar abaixo da linha 46)
if (direction != Vector2.Zero)
{
	// 1. Movimento Horizontal (Prioridade Direita / Esquerda)
	if (Math.Abs(direction.x) > Math.Abs(direction.y))
	{
		_olhandoParaTras = false;

		if (direction.x > 0)
		{
			_anim.Play("andar_direita");
			if (_spriteEspada != null) 
			{
				_spriteEspada.Visible = TemEspada;
				_spriteEspada.Position = new Vector2(Math.Abs(_spriteEspada.Position.x), _spriteEspada.Position.y);
			}
		}
		else if (direction.x < 0)
		{
			_anim.Play("andar_esquerda");
			if (_spriteEspada != null) 
			{
				_spriteEspada.Visible = TemEspada;
				_spriteEspada.Position = new Vector2(-Math.Abs(_spriteEspada.Position.x), _spriteEspada.Position.y);
			}
		}
	}
	// 2. Movimento Vertical (Cima / Baixo)
	else
	{
		if (direction.y < 0) // Andando para CIMA
		{
			_olhandoParaTras = true;
			_anim.Play("andar_costas");
			if (_spriteEspada != null) _spriteEspada.Visible = false; 
		}
		else if (direction.y > 0) // Andando para BAIXO
		{
			_olhandoParaTras = false;
			_anim.Play("andar_frente");
			if (_spriteEspada != null) _spriteEspada.Visible = TemEspada;
		}
	}
}
else
{
	// Quando o jogador solta os botões (Fica Parado)
	if (_olhandoParaTras)
	{
		_anim.Play("parado_costas");
		if (_spriteEspada != null) _spriteEspada.Visible = false;
	}
	else
	{
		_anim.Play("parado");
		if (_spriteEspada != null) _spriteEspada.Visible = TemEspada;
	}
}

	}
public override void _Process(float delta)
{
	if (Input.IsActionJustPressed("atacar") && TemEspada)
	{
		BaterNoInimigo();
	}
}

public void ColetarEspada()
{
	TemEspada = true;
	GD.Print("Espada Coletada! Ataque os Zumbies.");
	
	 if (_spriteEspada != null) _spriteEspada.Visible = true;
}

public void tomar_dano(int quantidade)
{
	VidaAtual -= quantidade;
	GD.Print("AI!!!! Vida atual: " + VidaAtual);
	
	if (VidaAtual <= 0)
	{
		GD.Print("Você Morreu!");
		GetTree().ChangeScene("res://MenuGameOver.tscn");
	}
}
private void BaterNoInimigo()
{
	if (_areaEspada == null)
	{
		GD.Print("Erro: O nó AreaEspada não foi encontrado no Rick! Verifique o nome na ávore.");
		return;
	}
	GD.Print("Boa!");
	
	Godot.Collections.Array corposNaArea = _areaEspada.GetOverlappingBodies();
	foreach (Node corpo in corposNaArea)
	{
		if (corpo.HasMethod("tomar_dano"))
		{
			corpo.Call("tomar_dano", 25);
		}
	}
}
private void  AtualizarInterfaceCoracoes()
{
	if (coracoes == null) return;

	Godot.Collections.Array listaCoracoes = coracoes.GetChildren();

	// Calcula quantos corações o jogador deveria ter acesos (ex: 75 de vida / 25 = 3 corações)
	int coracoesAtivos = VidaAtual / 25;

	for (int i = 0; i < listaCoracoes.Count; i++)
	{
		TextureRect coracaoImg = (TextureRect)listaCoracoes[i];
		
		// Se a posição do coração for menor que a quantidade que deve ficar ativa, ele aparece
		if (i < coracoesAtivos)
		{
			coracaoImg.Visible = true;
		}
		else
		{
			coracaoImg.Visible = false;
		}
	}
}
}

