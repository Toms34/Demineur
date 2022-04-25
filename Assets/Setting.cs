using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{

    private int taille;
    public TMPro.TMP_Dropdown dropdown;
    public Button button;
    // Start is called before the first frame update

    public void change(){
        Debug.Log("test ");
        taille=int.Parse(dropdown.options[dropdown.value].text);
        Debug.Log("La value c : "+ taille);
        PlayerPrefs.SetInt("Taille",taille);
        PlayerPrefs.Save();
    }
    public void exit(){
        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
    }

    // Update is called once per frame
}
