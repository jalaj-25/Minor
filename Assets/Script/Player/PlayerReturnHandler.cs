using UnityEngine;

public class PlayerReturnHandler : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        string returnPointName = PlayerPrefs.GetString("ReturnPoint", "");

        if (returnPointName != "")
        {
            GameObject point = GameObject.Find(returnPointName);

            if (point != null)
            {
                player.position = point.transform.position;
            }
        }
    }
}