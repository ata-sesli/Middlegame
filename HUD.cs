using Godot;
using System;

public partial class HUD : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	[Signal] public delegate void StartGameEventHandler();

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		
		GetNode<Timer>("Message Timer").Start();
	}
	async public void ShowGameOver()
	{
			ShowMessage("Game over!");
			var messageTimer = GetNode<Timer>("Message Timer");
			await ToSignal(messageTimer, Timer.SignalName.Timeout);
			var message = GetNode<Label>("Message");
			message.Text = "Dodge the Creeps!";
			message.Show();

			await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
			GetNode<Button>("StartButton").Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	public override void _Ready()
	{
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}
}
