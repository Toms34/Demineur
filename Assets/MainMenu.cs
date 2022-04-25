using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using System.Threading;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    public Button currentButton;
    public Sprite push;
    private Sprite baseSprite;
    public void Newgame(){
        SceneManager.LoadScene("Base",LoadSceneMode.Single);
    }

    public void Setting(){
        SceneManager.LoadScene("Settings",LoadSceneMode.Single);
    }
}
