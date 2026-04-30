using Godot;

public partial class Player : CharacterBody2D
{
    private const float Speed = 60.0f;
    private const float JumpVelocity = -300.0f;

    private AnimatedSprite2D _animation;

    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        var direction = Input.GetVector("left", "right", "jump", "fall");

        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        if (IsOnFloor())
        {
            if (direction.X != 0)
            {
                _animation.FlipH = direction.X < 0;
            }

            _animation.Play(direction != Vector2.Zero ? "walk" : "idle");
        }
        else
        {
            _animation.Play("jump");
            // _animation.Play("falling");
        }



        Velocity = velocity;
        MoveAndSlide();
    }
}