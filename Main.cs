using Godot;
using System;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	[Export] public PackedScene MobScene { get; set; }
	private int _score;
	public override void _Ready()
	{
		GD.Print("Here we go!");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnMobTimerTimeout()
	{
		var mob = MobScene.Instantiate<Mob>();
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		mob.Position = mobSpawnLocation.Position;

		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);
		GetNode<Timer>("MobTimer").WaitTime = GetNode<Timer>("MobTimer").WaitTime * 0.99;
		AddChild(mob);
	}
	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}
	private void OnStartTimerTimeout()
	{	
		GD.Print("StartTimer");
		GetNode<Timer>("MobTimer").Start();
		GD.Print("MobTimer");
		GetNode<Timer>("ScoreTimer").Start();	
		GD.Print("ScoreTimer");
	}
	private void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
	}

	public void NewGame()
	{
		_score = 0;
		GetTree().CallGroup("mobs",Node.MethodName.QueueFree);
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Visible = true;
		GetNode<Timer>("StartTimer").Start();
		player.Start(startPosition.Position);
	}
}