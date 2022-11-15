using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float[] position;

    public PlayerData(PlayerController controller)
    {
        health = controller.healthController.tempHit;
        position = new float[3];
        position[0] = controller.gameObject.transform.position.x;
        position[1] = controller.gameObject.transform.position.y;
        position[2] = controller.gameObject.transform.position.z;

        //Will need to store what guns are avalaiable and ammo counts

    }
}
