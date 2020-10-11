using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalonMovement : MonoBehaviour
{
    public float speed;

    public TextMeshPro answerText;

    //[System.NonSerialized]
    public int BallonAnswer = 0;

    // Update is called once per frame
    void Update()
    {
        Vector2 temp = transform.position;
        temp.x += speed * Time.deltaTime;
        transform.position = temp;
    }

    public void newText(string text)
    {
        BallonAnswer = int.Parse(text);
        answerText.SetText(text);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "car")
        //{
        //    GameObject.Destroy(gameObject);
        //}
    }
}
