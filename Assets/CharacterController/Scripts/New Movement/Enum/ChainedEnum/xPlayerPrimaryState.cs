using System;

// --- MARKER INTERFACES FOR TYPE RESTRICTION ---
public interface IMovementSubState { }
public interface IAirborneSubState { }

public class xPlayerPrimaryState : ChainedEnum
{
    public float MovementSpeed { get; }

    private xPlayerPrimaryState(string name, ChainedEnum? parent = null, float movementSpeed = 0f) : base(name, parent)
    {
        MovementSpeed = movementSpeed;
    }


    // ==========================================
    // LAYER 1: ROOT STATES
    // ==========================================
    public static readonly xPlayerPrimaryState Dead = new("Dead");
    public static readonly MovementStateGroup Movement = new();

    // ==========================================
    // LAYER 2: MOVEMENT SUB-STATES
    // ==========================================
    public class MovementStateGroup : ChainedEnum
    {
        public MovementStateGroup() : base("Movement") { }

        // Keeping these as raw classes preserves IntelliSense dot-navigation!
        public readonly LocomotionGroup Locomotion = new(parent: null); 
        public readonly AirborneGroup Airborne = new(parent: null); 

        public static implicit operator xPlayerPrimaryState(MovementStateGroup g) => new xPlayerPrimaryState(g.Name);
    }

    // ==========================================
    // LAYER 3: LOCOMOTION DETAILED STATES
    // ==========================================
    // Inherits from IMovementSubState so the parent container can be assigned cleanly
    public class LocomotionGroup : ChainedEnum, IMovementSubState
    {
        public readonly xPlayerPrimaryState Idle;
        public readonly xPlayerPrimaryState Walking;
        public readonly xPlayerPrimaryState Running;
        public readonly xPlayerPrimaryState Sprinting;

        public LocomotionGroup(ChainedEnum? parent) : base("Locomotion", parent)
        {
            Idle = new xPlayerPrimaryState("Idle", this, 0f);
            Walking = new xPlayerPrimaryState("Walking", this, 1.5f);
            Running = new xPlayerPrimaryState("Running", this, 6f);
            Sprinting = new xPlayerPrimaryState("Sprinting", this, 8f);
        }

        public static implicit operator xPlayerPrimaryState(LocomotionGroup g) => new xPlayerPrimaryState(g.Name);
    }

    // ==========================================
    // LAYER 4: AIRBORNE DETAILED STATES
    // ==========================================
    // Inherits from IMovementSubState so the parent container can be assigned cleanly
    public class AirborneGroup : ChainedEnum, IMovementSubState
    {
        // Items inherit from IAirborneSubState to restrict the leaf variable
        public readonly AirborneLeaf Grounded;
        public readonly AirborneLeaf Jumping;
        public readonly AirborneLeaf Falling;
        public readonly AirborneLeaf Landing;

        public AirborneGroup(ChainedEnum? parent) : base("Airborne", parent)
        {
            Grounded = new AirborneLeaf("Grounded", this);
            Jumping = new AirborneLeaf("Jumping", this);
            Falling = new AirborneLeaf("Falling", this);
            Landing = new AirborneLeaf("Landing", this);
        }

        public static implicit operator xPlayerPrimaryState(AirborneGroup g) => new xPlayerPrimaryState(g.Name);

        // A tiny subclass for Airborne leaves that carries the interface tag
        public class AirborneLeaf : xPlayerPrimaryState, IAirborneSubState
        {
            public AirborneLeaf(string name, ChainedEnum? parent) : base(name, parent) { }
        }
    }
}