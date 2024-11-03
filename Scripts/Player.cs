using Godot;
using Godot.NativeInterop;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
	private InputSynchronizer input;

	public override void _EnterTree()
	{
		GD.Print("Player ID: ", int.Parse(Name));
		SetMultiplayerAuthority(int.Parse(Name));
	}

	public override void _Ready()
	{
		input = GetNode<InputSynchronizer>("InputSynchronizer");

		if (!Multiplayer.IsServer())
			SetPhysicsProcess(false);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Multiplayer.IsServer())
			Move(delta);
	}

	private void Move(double delta)
	{
		Rotate(input.TurnInput * 2.5f);
		GD.Print($"PL {(Multiplayer.IsServer() ? "(s)" : "(c)")} : {input.TurnInput.ToString()}");
		MoveAndSlide();
	}
}
