using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public void StartVR()
    {
        SceneManager.LoadScene("Room Experience");
    }
    public void EndGame()
    {
        Application.Quit();
    }
   
}
