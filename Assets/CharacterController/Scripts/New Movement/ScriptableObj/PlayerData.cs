using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    #region movement
    public MovementStateEnum movementState = MovementStateEnum.Idle;
    #endregion
}
