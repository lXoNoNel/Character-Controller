using UnityEngine;

public enum MovementStateEnum
{
    Idle,
    Walking,
    Running,
    Sprinting,

}

// This static class adds a helper function to your enum
public static class MovementStateExtensions
{
    public static float GetSpeed(this MovementStateEnum state)
    {
        return state switch
        {
            MovementStateEnum.Idle => 0f,
            MovementStateEnum.Walking => 1.5f,
            MovementStateEnum.Running => 6f,
            MovementStateEnum.Sprinting => 8f,
            _ => 0.0f // Default fallback
        };
    }
}