using UnityEngine;

public class game_exit : MonoBehaviour
{
    //ƒQ[ƒ€‚ğI—¹‚³‚¹‚é
    public void App_Exit()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
