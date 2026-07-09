using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    #region movement
    // IMovementSubState parentMovementState = xPlayerState.Movement.Locomotion;
    [SerializeReference] public xPlayerPrimaryState exactMovementState = null;    //? ONLY THING CHANGED MANUALLY, REST ON UPDATE
    IAirborneSubState currentAirborneState = xPlayerPrimaryState.Movement.Airborne.Grounded;
    
    #endregion
}
