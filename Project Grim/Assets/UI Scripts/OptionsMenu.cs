using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] private AudioSource music;


    public void QuitOptions()
    {
        SceneManager.LoadScene("Menu");
    }

    public void MuteSound()
    {
        music.mute = !music.mute;
    }

}
