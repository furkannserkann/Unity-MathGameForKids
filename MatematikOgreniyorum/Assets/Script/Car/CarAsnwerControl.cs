using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAsnwerControl : MonoBehaviour
{
    private AskControl askControl;

    void Start()
    {
        askControl = GameObject.FindGameObjectWithTag("scripts").GetComponent<AskControl>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "answerBallon")
        {
            BalonMovement ballon = collision.gameObject.GetComponent<BalonMovement>();
            if (ballon != null)
            {
                if (ballon.BallonAnswer == askControl.rightAnswer)
                {
                    askControl.addScore(5);
                    askControl.destroyBallons();
                }
                else
                {
                    askControl.addScore(-3);
                    askControl.destroyBallons();
                }
            }
        }
    }
}
