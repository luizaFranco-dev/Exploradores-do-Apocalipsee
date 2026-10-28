using Godot;
using System;

public class MenuGameOver : Control
{

	public void _on_BotaoReiniciar_pressed()
	{
		GetTree().ChangeScene("res://MenuInicial.tscn");
	}
}

