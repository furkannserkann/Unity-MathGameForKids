using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    public void GameCar()
    {
        SceneManager.LoadScene(1);
    }

    public void GameTwoPlayer()
    {
        SceneManager.LoadScene(2);
    }
}
