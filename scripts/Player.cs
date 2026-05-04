using Godot;

public partial class Player : CharacterBody2D
{
    private const float Speed = 60.0f;
    private const float JumpVelocity = -300.0f;

    private const float Acceleration = 800.0f;
    private const float Friction = 1000.0f;

    private const float FastFallMultiplier = 2.5f;
    private const float MaxFallSpeed = 500.0f;

    private const float JumpCutMultiplier = 0.5f;

    // TODO: Implementar double jump
    private const int MaxJumps = 2;
    private int _jumpsRemaining = MaxJumps;

    private AnimatedSprite2D _animation;

    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaTime = (float)delta;
        var velocity = Velocity;

        bool isFallPressed = Input.IsActionPressed("down");
        bool isJumpJustPressed = Input.IsActionJustPressed("jump");
        bool isJumpJustReleased = Input.IsActionJustReleased("jump");

        // Movimento horizontal
        var direction = Input.GetAxis("left", "right");

        // Gravidade
        if (!IsOnFloor())
        {
            Vector2 gravity = GetGravity();

            if (isFallPressed)
                gravity *= FastFallMultiplier;

            velocity += gravity * deltaTime;

            if (velocity.Y > MaxFallSpeed)
                velocity.Y = MaxFallSpeed;
        }

        // Pulo
        if (isJumpJustPressed && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Cortar o pulo ao soltar o botão
        if (isJumpJustReleased && velocity.Y < 0)
            velocity.Y *= JumpCutMultiplier;

        // Movimento horizontal
        if (direction != 0)
        {
            velocity.X = Mathf.MoveToward(velocity.X, direction * Speed, Acceleration * deltaTime);
            _animation.FlipH = direction < 0;
        }

        else
            velocity.X = Mathf.MoveToward(velocity.X, 0, Friction * deltaTime);

        UpdateAnimation(direction, velocity, isFallPressed);

        Velocity = velocity;
        MoveAndSlide();
    }

    private void UpdateAnimation(float direction, Vector2 velocity, bool isFallPressed)
    {
        if (IsOnFloor())
        {
            _animation.Play(direction != 0 ? "walk" : "idle");
            return;
        }

        if (isFallPressed)
        {
            _animation.Play("fall");
            return;
        }

        _animation.Play(velocity.Y < 0 ? "jump" : "fall");
    }
}