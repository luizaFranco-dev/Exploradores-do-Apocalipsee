using Godot;
using System;

public class espadachao : Area2D
{
	public override void _Ready()
	{
		// Inicialização da espada
	}

	public void _on_espadachao_body_entered(Node body)
	{
		if (body.HasMethod("ColetarEspada"))
		{
			body.Call("ColetarEspada");
			QueueFree();
		}
	}
}
