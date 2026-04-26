using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminBuilding : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Admin Building Clicked");

        SceneManager.LoadScene("AdminScene");
    }
}