using Godot;
using System;

public partial class Player : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		var animated2DSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animated2DSprite.Play();
		}
		else
		{
			animated2DSprite.Stop();
		}
		Position += velocity * (float)delta;
		Position = new Vector2(x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y));
		if (velocity.X != 0)
		{
			animated2DSprite.Animation = "walk";
			animated2DSprite.FlipV = false;
			animated2DSprite.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			animated2DSprite.Animation = "up";
			animated2DSprite.FlipV = velocity.Y > 0;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	[Signal]
	public delegate void HitEventHandler();
	[Export] public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;
}

