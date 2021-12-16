using UnityEngine;

//Can probably reuse this for other AI stats too
[CreateAssetMenu(fileName = "PlayerAttribute", menuName = "Player/Attribute", order = 1)]
public class CharacterAttributes : ScriptableObject
{
    public float f_runSpeed = 3f;
    public float f_sneakSpeed = 1f;
    public float f_rollSpeed = 5f;
    public float f_rollDistance = 5f;

    public float f_verticalSpeed = 1;
    public float f_horizontalSpeed = 1;
}
