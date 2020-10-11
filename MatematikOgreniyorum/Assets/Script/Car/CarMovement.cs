using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Boundry();
    }

    void Boundry()
    {
        Vector2 temp = transform.position;
        if (temp.y < -1.35f)
        {
            temp.y = -1.35f;
            transform.position = temp;
        }
        if (temp.y > 1.38f)
        {
            temp.y = 1.38f;
            transform.position = temp;
        }
    }

    public void up()
    {
        Vector2 temp = transform.position;

        if (temp.y < -0.45f)
        {
            temp.y = -0.45f;
        }
        else if (temp.y < 0.48f)
        {
            temp.y = 0.48f;
        }
        else if (temp.y < 1.38f)
        {
            temp.y = 1.38f;
        }

        transform.position = temp;
    }

    public void down()
    {
        Vector2 temp = transform.position;

        if (temp.y > 0.48f)
        {
            temp.y = 0.48f;
        }
        else if (temp.y > -0.45f)
        {
            temp.y = -0.45f;
        }
        else if (temp.y > -1.35f)
        {
            temp.y = -1.35f;
        }

        transform.position = temp;
    }
}
