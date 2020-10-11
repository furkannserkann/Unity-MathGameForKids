using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectScore : MonoBehaviour
{
    public int scorePlayerOne = 0, scorePlayerTwo = 0;

    [Space(10)]
    public Toggle[] Operations = new Toggle[4];
    public TMP_Dropdown zorlukSeviyesi;

    private bool[] selectOperations = new bool[4];

    private int rightAnswerP1 = 0, rightAnswerP2 = 0;
    private int lastP1S1 = 0, lastP1S2 = 0, lastP2S1 = 0, lastP2S2 = 0, lastRandAnswerP1 = -1, lastRandAnswerP2 = -1;

    [Space(10)]
    public Sprite[] answerSprites;
    public TextMeshProUGUI textQuestionP1;
    public TextMeshProUGUI textQuestionP2;
    public GameObject[] AnswersP1 = new GameObject[3];
    public GameObject[] AnswersP2 = new GameObject[3];

    [Space(10)]
    public GameObject menuGame;
    public GameObject menuStart;
    public GameObject menuFinish;
    public GameObject menuPause;

    private float timer = 0.0f;
    private int seconds = 0, lastSecond = 0;

    public Image ImgTimeP1, ImgTimeP2;
    private float timeP1 = 1.0f, timeP2 = 1.0f;

    [Space(10)]
    public TextMeshProUGUI textScoreP1;
    public TextMeshProUGUI textScoreP2;

    private bool isPlayingGame = false;

    private float defaultTimer = 60, totalTimer;
    public TextMeshProUGUI textTotalTime;
    public Image imageBarTotalTime;

    [Space(10)]
    public TextMeshProUGUI textP1Win;
    public TextMeshProUGUI textP1Score, textP1TotalAsk, textP1RightAns, textP1WrongAns;
    public TextMeshProUGUI textP2Win;
    public TextMeshProUGUI textP2Score, textP2TotalAsk, textP2RightAns, textP2WrongAns;

    private int totalAskP1 = 0, rightAnsP1 = 0, wrongAnsp1 = 0;
    private int totalAskP2 = 0, rightAnsP2 = 0, wrongAnsp2 = 0;

    private int[] liveAnswersP1, liveAnswersP2;
    private bool isClickTimer = false;

    [Space(10)]
    public AudioSource audioSource;
    public Image[] musicImage;
    public Sprite onMusic, offMusic;

    private void Start()
    {
        menuStart.SetActive(true);
        menuGame.SetActive(false);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);

        totalTimer = defaultTimer;
        textTotalTime.SetText(totalTimer.ToString());
    }

    private void FixedUpdate()
    {
        //timer += Time.deltaTime;
        //seconds = (int)(timer % 60);
        //if (seconds != lastSecond)
        //{
        //    // Zaman Timer!!!

        //    lastSecond = seconds;
        //}

        if (isPlayingGame)
        {
            timeP1 -= 0.00175f;
            timeP2 -= 0.00175f;

            ImgTimeP1.fillAmount = timeP1;
            ImgTimeP2.fillAmount = timeP2;

            if (timeP1 <= 0 && timeP1 == timeP2)
            {
                finishGame();
            }
            else if (timeP1 <= 0)
            {
                finishGame(false);
            }
            else if (timeP2 <= 0)
            {
                finishGame(true);
            }
        }
    }

    public void NewQuestion(bool playerOne)
    {
        int selectOperation = 0;
        while (true)
        {
            int opNumb = Random.Range(0, 80) % 4;
            if (selectOperations[opNumb])
            {
                selectOperation = opNumb;
                break;
            }
        }

        int maxNumber = 10, minNumber = 2;
        if (selectOperation != 2)
        {
            if (zorlukSeviyesi.value == 1)
            {
                if (selectOperation != 3)
                    minNumber = 4;

                maxNumber = 20;
            }
            else if (zorlukSeviyesi.value == 2)
            {
                if (selectOperation != 3)
                    minNumber = 5;

                maxNumber = 25;
            }
            else if (zorlukSeviyesi.value == 3)
            {
                if (selectOperation != 3)
                    minNumber = 6;

                maxNumber = 30;
            }
        }
        else
        {
            minNumber = 2;

            if (zorlukSeviyesi.value == 0)
                maxNumber = 7;
            else if (zorlukSeviyesi.value == 1)
                maxNumber = 8;
            else if (zorlukSeviyesi.value == 2)
                maxNumber = 9;
            else if (zorlukSeviyesi.value == 3)
                maxNumber = 10;
        }

        int[] answers = QuestionGenerate(minNumber, maxNumber, selectOperation, playerOne);

        for (int i = 0; i < answers.Length; i++)
        {
            if (playerOne)
            {
                AnswersP1[i].GetComponent<Image>().sprite = answerSprites[0];
                AnswersP1[i].GetComponentInChildren<TextMeshProUGUI>().SetText(answers[i].ToString());
            }
            else
            {
                AnswersP2[i].GetComponent<Image>().sprite = answerSprites[0];
                AnswersP2[i].GetComponentInChildren<TextMeshProUGUI>().SetText(answers[i].ToString());
            }
        }
    }

    private int[] QuestionGenerate(int minNumber, int maxNumber, int selectOperation, bool playerOne)
    {
        int s1 = Random.Range(minNumber, maxNumber),
            s2 = Random.Range(minNumber, maxNumber);
        while (true)
        {
            if (selectOperation == 1)
            {
                if (playerOne)
                {
                    if (s1 > s2 && (s1 != lastP1S1 || s2 != lastP1S2))
                    {
                        break;
                    }
                }
                else if (s1 > s2 && (s1 != lastP2S1 || s2 != lastP2S2))
                {
                    break;
                }

                s1 = Random.Range(minNumber, maxNumber);
                s2 = Random.Range(minNumber, s1);
            }
            else if (selectOperation == 3)
            {
                if (playerOne)
                {
                    if (s1 % s2 == 0 && s1 > s2 && (s1 != lastP1S1 || s2 != lastP1S2))
                    {
                        break;
                    }
                }
                else if (s1 % s2 == 0 && s1 > s2 && (s1 != lastP2S1 || s2 != lastP2S2))
                {
                    break;
                }

                s1 = Random.Range(minNumber, maxNumber);
                s2 = Random.Range(minNumber, (s1 / 2) + 1);
            }
            else if ((s1 != lastP1S1 || s2 != lastP1S2) && playerOne)
            {
                break;
            }
            else if ((s1 != lastP2S1 || s2 != lastP2S2) && !playerOne)
            {
                break;
            }
            else
            {
                s1 = Random.Range(minNumber, maxNumber);
                s2 = Random.Range(minNumber, maxNumber);
            }
        }

        if (playerOne)
        {
            lastP1S1 = s1;
            lastP1S2 = s2;
        }
        else
        {
            lastP2S1 = s1;
            lastP2S2 = s2;
        }


        if (selectOperation == 0)
        {
            if (playerOne)
            {
                textQuestionP1.SetText(s1 + " + " + s2);
                rightAnswerP1 = s1 + s2;
            }
            else
            {
                textQuestionP2.SetText(s1 + " + " + s2);
                rightAnswerP2 = s1 + s2;
            }
        }
        else if (selectOperation == 1)
        {
            if (playerOne)
            {
                textQuestionP1.SetText(s1 + " - " + s2);
                rightAnswerP1 = s1 - s2;
            }
            else
            {
                textQuestionP2.SetText(s1 + " - " + s2);
                rightAnswerP2 = s1 - s2;
            }
        }
        else if (selectOperation == 2)
        {
            if (playerOne)
            {
                textQuestionP1.SetText(s1 + " x " + s2);
                rightAnswerP1 = s1 * s2;
            }
            else
            {
                textQuestionP2.SetText(s1 + " x " + s2);
                rightAnswerP2 = s1 * s2;
            }
        }
        else if (selectOperation == 3)
        {
            if (playerOne)
            {
                textQuestionP1.SetText(s1 + " / " + s2);
                rightAnswerP1 = s1 / s2;
            }
            else
            {
                textQuestionP2.SetText(s1 + " / " + s2);
                rightAnswerP2 = s1 / s2;
            }
        }

        int randAnswer = Random.Range(0, 80) % 3;
        int[] answers = new int[3];

        if (playerOne)
        {
            while (lastRandAnswerP1 == randAnswer)
            {
                randAnswer = Random.Range(0, 80) % 3;
            }
            lastRandAnswerP1 = randAnswer;
        }
        else
        {
            while (lastRandAnswerP2 == randAnswer)
            {
                randAnswer = Random.Range(0, 80) % 3;
            }
            lastRandAnswerP2 = randAnswer;
        }

        if (playerOne)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                int localR = 0;
                if (rightAnswerP1 >= 4)
                {
                    localR = Random.Range(rightAnswerP1 / 2, rightAnswerP1 * 2);
                }
                else
                {
                    localR = Random.Range(1, rightAnswerP1 * 6);
                }

                if (System.Array.IndexOf(answers, localR) == -1 && localR != rightAnswerP1)
                {
                    answers[i] = localR;
                }
                else
                {
                    i--;
                    continue;
                }
            }

            answers[randAnswer] = rightAnswerP1;
            liveAnswersP1 = answers;
        }
        else
        {
            for (int i = 0; i < answers.Length; i++)
            {
                int localR = 0;
                if (rightAnswerP2 >= 4)
                {
                    localR = Random.Range(rightAnswerP2 / 2, rightAnswerP2 * 2);
                }
                else
                {
                    localR = Random.Range(1, rightAnswerP2 * 6);
                }

                if (System.Array.IndexOf(answers, localR) == -1 && localR != rightAnswerP2)
                {
                    answers[i] = localR;
                }
                else
                {
                    i--;
                    continue;
                }
            }

            answers[randAnswer] = rightAnswerP2;
            liveAnswersP2 = answers;
        }

        return answers;
    }

    public void startGame()
    {
        for (int i = 0; i < selectOperations.Length; i++)
        {
            selectOperations[i] = Operations[i].GetComponent<Toggle>().isOn;
        }

        menuStart.SetActive(false);
        menuGame.SetActive(true);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);

        NewQuestion(true);
        NewQuestion(false);

        isPlayingGame = true;
        if (!isClickTimer)
        {
            isClickTimer = true;
            StartCoroutine(IEnumTotalTimer());
        }
    }

    public void onValueChangedForOperations()
    {
        bool allFalse = true;
        for (int i = 0; i < Operations.Length; i++)
        {
            if (Operations[i].isOn)
            {
                allFalse = false;
                break;
            }
        }

        if (allFalse)
        {
            Operations[0].isOn = true;
        }
    }

    private void resetPlayerOneAnswers()
    {
        for (int i = 0; i < AnswersP1.Length; i++)
        {
            AnswersP1[i].GetComponent<Image>().sprite = answerSprites[0];
            AnswersP1[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        clickAnswerP1 = false;
    }

    private void resetPlayerTwoAnswers()
    {
        for (int i = 0; i < AnswersP2.Length; i++)
        {
            AnswersP2[i].GetComponent<Image>().sprite = answerSprites[0];
            AnswersP2[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        clickAnswerP2 = false;
    }

    private bool clickAnswerP1 = false;
    public void clickAnswerPlayerOne(int clickID)
    {
        if (!clickAnswerP1)
        {
            clickAnswerP1 = true;
            totalAskP1++;

            if (liveAnswersP1[clickID] == rightAnswerP1)
            {
                rightAnsP1++;
                AnswersP1[clickID].GetComponent<Image>().sprite = answerSprites[1];

                timeP1 += 0.25f;
                if (timeP1 > 1)
                    timeP1 = 1;

                ImgTimeP1.fillAmount = timeP1;

                scorePlayerOne += 5;
                textScoreP1.SetText(scorePlayerOne.ToString());

                StartCoroutine(IEnumClickP1());
            }
            else
            {
                wrongAnsp1++;
                AnswersP1[clickID].GetComponent<Image>().color = new Color32(145, 45, 45, 255);
                scorePlayerOne -= 3;
                textScoreP1.SetText(scorePlayerOne.ToString());

                StartCoroutine(IEnumClickP1());
            }
        }
    }

    private bool clickAnswerP2 = false;
    public void clickAnswerPlayerTwo(int clickID)
    {
        if (!clickAnswerP2)
        {
            clickAnswerP2 = true;
            totalAskP2++;

            if (liveAnswersP2[clickID] == rightAnswerP2)
            {
                rightAnsP2++;
                AnswersP2[clickID].GetComponent<Image>().sprite = answerSprites[1];

                timeP2 += 0.25f;
                if (timeP2 > 1)
                    timeP2 = 1;

                ImgTimeP2.fillAmount = timeP2;

                scorePlayerTwo += 5;
                textScoreP2.SetText(scorePlayerTwo.ToString());

                StartCoroutine(IEnumClickP2());
            }
            else
            {
                wrongAnsp2++;
                AnswersP2[clickID].GetComponent<Image>().color = new Color32(145, 45, 45, 255);
                scorePlayerTwo -= 3;
                textScoreP2.SetText(scorePlayerTwo.ToString());

                StartCoroutine(IEnumClickP2());
            }
        }
    }

    private void finishGame(bool isWinP1)
    {
        isPlayingGame = false;
        menuStart.SetActive(false);
        menuGame.SetActive(false);
        menuFinish.SetActive(true);
        menuPause.SetActive(false);

        Color32 colGreen = new Color32(51, 105, 30, 255);
        Color32 colRed = new Color32(183, 28, 28, 255);
        if (isWinP1)
        {
            textP1Win.SetText("1.OYUNCU KAZANDIN");
            textP1Win.color = colGreen;

            textP2Win.SetText("2.OYUNCU KAYBETTİN");
            textP2Win.color = colRed;
        }
        else
        {
            textP1Win.SetText("1.OYUNCU KAYBETTİN");
            textP1Win.color = colRed;

            textP2Win.SetText("2.OYUNCU KAZANDIN");
            textP2Win.color = colGreen;
        }

        writeScoreFinish();
    }

    private void finishGame()
    {
        isPlayingGame = false;
        menuStart.SetActive(false);
        menuGame.SetActive(false);
        menuFinish.SetActive(true);
        menuPause.SetActive(false);

        if (scorePlayerOne > scorePlayerTwo)
        {
            finishGame(true);
        }
        else if (scorePlayerOne < scorePlayerTwo)
        {
            finishGame(false);
        }
        else if (timeP1 > timeP2)
        {
            finishGame(true);
        }
        else if (timeP1 < timeP2)
        {
            finishGame(false);
        }
        else
        {
            Color32 colYellow = new Color32(230, 81, 0, 255);
            textP1Win.SetText("1.OYUNCU BERABERE");
            textP1Win.color = colYellow;

            textP2Win.SetText("2.OYUNCU BERABERE");
            textP2Win.color = colYellow;
        }

        writeScoreFinish();
    }

    private void writeScoreFinish()
    {
        textP1Score.SetText("Skor: " + scorePlayerOne.ToString());
        textP1TotalAsk.SetText("Toplam Soru: " + totalAskP1.ToString());
        textP1RightAns.SetText("Doğru Cevap: " + rightAnsP1.ToString());
        textP1WrongAns.SetText("Yanlış Cevap: " + wrongAnsp1.ToString());

        textP2Score.SetText("Skor: " + scorePlayerTwo.ToString());
        textP2TotalAsk.SetText("Toplam Soru: " + totalAskP2.ToString());
        textP2RightAns.SetText("Doğru Cevap: " + rightAnsP2.ToString());
        textP2WrongAns.SetText("Yanlış Cevap: " + wrongAnsp2.ToString());
    }

    IEnumerator IEnumClickP1()
    {
        yield return new WaitForSeconds(1);
        NewQuestion(true);
        resetPlayerOneAnswers();
    }

    IEnumerator IEnumClickP2()
    {
        yield return new WaitForSeconds(1);
        NewQuestion(false);
        resetPlayerTwoAnswers();
    }

    IEnumerator IEnumTotalTimer()
    {
        yield return new WaitForSeconds(1);

        if (isPlayingGame)
        {
            totalTimer--;

            imageBarTotalTime.fillAmount = totalTimer / defaultTimer;
            textTotalTime.SetText(totalTimer.ToString());

            if (totalTimer <= 0)
            {
                isPlayingGame = false;

                finishGame();
            }
        }

        StartCoroutine(IEnumTotalTimer());
    }

    public void RestartScene()
    {
        totalTimer = defaultTimer;
        textTotalTime.SetText(totalTimer.ToString());
        imageBarTotalTime.fillAmount = totalTimer / defaultTimer;

        scorePlayerOne = 0;
        scorePlayerTwo = 0;
        textScoreP1.SetText(scorePlayerOne.ToString());
        textScoreP2.SetText(scorePlayerTwo.ToString());

        totalAskP1 = 0;
        totalAskP2 = 0;

        rightAnsP1 = 0;
        rightAnsP2 = 0;
        wrongAnsp1 = 0;
        wrongAnsp2 = 0;

        timeP1 = 1;
        timeP2 = 1;

        ImgTimeP1.fillAmount = timeP1;
        ImgTimeP2.fillAmount = timeP2;

        for (int i = 0; i < selectOperations.Length; i++)
        {
            selectOperations[i] = Operations[i].GetComponent<Toggle>().isOn;
        }

        menuStart.SetActive(true);
        menuGame.SetActive(false);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);
    }

    public void MainScene()
    {
        SceneManager.LoadScene(0);
    }

    public void HomeButton()
    {
        menuStart.SetActive(true);
        menuGame.SetActive(false);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);
    }

    public void PauseButton()
    {
        menuStart.SetActive(false);
        menuGame.SetActive(false);
        menuFinish.SetActive(false);
        menuPause.SetActive(true);
        isPlayingGame = false;
        Time.timeScale = 0;
    }

    public void ContinueButton()
    {
        menuStart.SetActive(false);
        menuGame.SetActive(true);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);
        isPlayingGame = true;
        Time.timeScale = 1;
    }

    public void RestartGameButton()
    {
        RestartScene();
        Time.timeScale = 1;
    }

    private bool isMute = false;
    public void clickMute()
    {
        if (isMute)
        {
            isMute = false;
            audioSource.Play();
            for (int i = 0; i < musicImage.Length; i++)
            {
                musicImage[i].sprite = onMusic;
            }
        }
        else
        {
            isMute = true;
            audioSource.Pause();
            for (int i = 0; i < musicImage.Length; i++)
            {
                musicImage[i].sprite = offMusic;
            }
        }
    }
}
