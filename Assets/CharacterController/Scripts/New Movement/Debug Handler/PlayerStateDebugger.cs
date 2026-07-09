using UnityEngine;

public class PlayerStateDebugger : MonoBehaviour
{
    // Reference to your ScriptableObject data asset
    public PlayerData data;

    /// <summary>
    /// Converts the current state stored in PlayerData to a readable string path.
    /// </summary>
    /// <returns>The full hierarchical string path of the state, or "No State Assigned" if null.</returns>
    public string GetCurrentStateAsString()
    {
        // 1. Safety check to make sure the ScriptableObject reference isn't empty
        if (data == null)
        {
            Debug.LogWarning("PlayerData asset reference is missing on this component!");
            return "Missing PlayerData Asset";
        }

        // 2. Safety check to make sure a state is actually assigned inside the ScriptableObject
        if (data.exactMovementState == null)
        {
            return "No State Assigned";
        }

        // 3. Return the full hierarchical path string (e.g., "Movement/Locomotion/Idle")
        return data.exactMovementState.FullPath;
    }

    // Example Unity Test Usage
    private void Update()
    {
        // Press Space bar to log the current string state to the console
        // string stateString = GetCurrentStateAsString();
        // Debug.Log($"[Player State]: {stateString}");
    }
}