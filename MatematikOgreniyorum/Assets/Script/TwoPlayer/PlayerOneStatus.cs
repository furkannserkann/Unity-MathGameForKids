using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOneStatus : MonoBehaviour
{
    public TextMeshProUGUI textQuestion;
    public GameObject[] Answers;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void clickAnswer(int answerID)
    {
        //Answers[answerID].GetComponent<Image>().sprite = answerSprites[1];
    }

    public void newQuestion(string question, string answer1, string answer2, string answer3)
    {
        textQuestion.SetText(question);

        Answers[0].GetComponentInChildren<TextMeshProUGUI>().SetText(answer1);
        Answers[1].GetComponentInChildren<TextMeshProUGUI>().SetText(answer2);
        Answers[2].GetComponentInChildren<TextMeshProUGUI>().SetText(answer3);
    }
}
