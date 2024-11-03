using Godot;
using System;
using System.Diagnostics;

public partial class InputSynchronizer : MultiplayerSynchronizer
{
	[Export] public float TurnInput { get; set; }


	public override void _Ready()
	{
		if (GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
			SetPhysicsProcess(false);
	}
	public override void _PhysicsProcess(double delta)
	{
		TurnInput = Input.GetAxis("Left", "Right");
		GD.Print($"IS {(Multiplayer.IsServer()?"(s)":"(c)")} : {TurnInput.ToString()}");
	}
}
