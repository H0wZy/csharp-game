using Godot;

namespace csharpgame.scripts;

public partial class Camera : Camera2D
{
	private Node2D _target;
	[Export] private float _followSpeed = 5f;

	public override void _Ready()
	{
		GetTarget();
	}

	public override void _Process(double delta)
	{
		if (_target == null)
			return;

		GlobalPosition = GlobalPosition.Lerp(_target.GlobalPosition, _followSpeed * (float)delta);
	}

	private void GetTarget()
	{
		var nodes = GetTree().GetNodesInGroup("player_group");

		if (nodes.Count <= 0)
		{
			GD.PushError("Player not found");
			return;
		}

		_target = nodes[0] as Node2D;
	}
}
