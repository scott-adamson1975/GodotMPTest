using Godot;
using System;

public partial class Network : Node
{


	string ip = "localhost";
	int port = 8080;
	int MAX_CLIENTS = 4;
	PackedScene player_scene;
	Node spawned_nodes;
	Panel network_ui;

	public override void _Ready()
	{
		player_scene = GD.Load<PackedScene>("res://Scenes/Player.tscn");
		spawned_nodes = GetNode("SpawnedNodes");
		//spawned_nodes = $SpawnedNodes;
		network_ui = GetNode<Panel>("NetworkUI");
	}
	public void BtnHostPressed()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateServer(port, MAX_CLIENTS);
		Multiplayer.MultiplayerPeer = peer;

		Multiplayer.PeerConnected += PlayerJoined;
		Multiplayer.PeerDisconnected += PlayerDisconnected;

		//_on_player_connected(multiplayer.get_unique_id())

		network_ui.Visible = false;

	}


	private void PlayerJoined(long id)
	{
		GD.Print($"Player {id} joined the game.");


		var player = player_scene.Instantiate();
		player.Name = id.ToString();
		spawned_nodes.AddChild(player, true);
	}

	private void PlayerDisconnected(long id)
	{
		GD.Print($"Player {id} left the game.");


		if (!spawned_nodes.HasNode(id.ToString()))
			return;

		spawned_nodes.GetNode(id.ToString()).QueueFree();
	}

	public void BtnJoinPressed()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateClient(ip, port);
		Multiplayer.MultiplayerPeer = peer;

		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
		Multiplayer.ServerDisconnected += ServerDisconnected;
	}



	private void ConnectedToServer()
	{
		GD.Print("Connected to server.");
		network_ui.Visible = false;
	}

	private void ConnectionFailed()
	{
		GD.Print("Connection failed!");
	}

	private void ServerDisconnected()
	{
		GD.Print("Server disconnected.");
		network_ui.Visible = true;
	}

}
