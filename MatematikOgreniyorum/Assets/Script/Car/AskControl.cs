using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AskControl : MonoBehaviour
{
    public GameObject BallonPrefab;

    private GameObject[] BallonSpawnPoint;
    [System.NonSerialized] public GameObject[] Ballons = new GameObject[4];

    [Space(10)]
    public TextMeshProUGUI textQuestion;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textTime;

    [System.NonSerialized] public int rightAnswer = 0;
    [System.NonSerialized] public int Score = 0;
    private int defaultTime = 60;
    [System.NonSerialized] public int time;

    private float timer = 0.0f;
    private int seconds=0, lastSecond=0;
    private int totalQuestionCount = 0, rightAnswerCount = 0, wrongAnswerCount = 0;

    [Space(10)]
    public GameObject menuGame;
    public GameObject menuFinish;
    public GameObject menuStart;
    public GameObject menuPause;

    private TextMeshProUGUI[] textDizi;

    [Space(10)]
    public Toggle[] Operations = new Toggle[4];
    public TMP_Dropdown zorlukSeviyesi;
    public Sprite[] cars = new Sprite[6];
    public Image selectedCar;

    private GameObject playerCar;

    private bool[] selectOperations = new bool[4];

    private int lastS1 = 0, lastS2 = 0, lastRandAnswer = -1;

    [Space(10)]
    public AudioSource audioSource;
    public Image[] musicImage;
    public Sprite onMusic, offMusic;

    void Start()
    {
        Time.timeScale = 0;
        time = defaultTime;

        menuStart.SetActive(true);
        menuGame.SetActive(false);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);

        textDizi = menuFinish.GetComponentsInChildren<TextMeshProUGUI>();

        BallonSpawnPoint = GameObject.FindGameObjectsWithTag("ballonSpawnPoint");
        playerCar = GameObject.FindGameObjectWithTag("car");

        playerCar.SetActive(false);

        textScore.SetText(Score.ToString());
        textTime.SetText(time.ToString());

        //NewQuestion();
    }

    void Update()
    {
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);
        if (seconds != lastSecond)
        {
            time--;
            textTime.SetText(time.ToString());

            lastSecond = seconds;
        }

        if (time <= 0 && Time.timeScale != 0)
        {
            Time.timeScale = 0;
            menuStart.SetActive(false);
            menuGame.SetActive(false);
            menuFinish.SetActive(true);
            menuPause.SetActive(false);
            playerCar.SetActive(false);

            for (int i = 0; i < Ballons.Length; i++)
            {
                GameObject.Destroy(Ballons[i]);
            }

            textDizi[1].SetText("Skor: " + Score.ToString());
            textDizi[2].SetText("Toplam Soru: " + totalQuestionCount.ToString());
            textDizi[3].SetText("Doğru Cevap: " + rightAnswerCount.ToString());
            textDizi[4].SetText("Yanlış Cevap: " + wrongAnswerCount.ToString());
        }
    }

    public void NewQuestion()
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

        int s1 = Random.Range(minNumber, maxNumber),
            s2 = Random.Range(minNumber, maxNumber);
        while (true)
        {
            if (selectOperation == 1)
            {
                if (s1 > s2 && (s1 != lastS1 || s2 != lastS2))
                {
                    break;
                }

                s1 = Random.Range(minNumber, maxNumber);
                s2 = Random.Range(minNumber, s1);
            }
            else if (selectOperation == 3)
            {
                if (s1 % s2 == 0 && s1 > s2 && (s1 != lastS1 || s2 != lastS2))
                {
                    break;
                }

                s1 = Random.Range(minNumber, maxNumber);
                s2 = Random.Range(minNumber, (s1 / 2) + 1);
            }
            else if (s1 != lastS1 || s2 != lastS2)
            {
                break;
            }
            else
            {
                s1 = Random.Range(minNumber, maxNumber);
                s2 = Random.Range(minNumber, maxNumber);
            }
        }
        lastS1 = s1;
        lastS2 = s2;

        if (selectOperation == 0)
        {
            textQuestion.SetText(s1 + " + " + s2 + " = ?");
            rightAnswer = s1 + s2;
        }
        else if (selectOperation == 1)
        {
            textQuestion.SetText(s1 + " - " + s2 + " = ?");
            rightAnswer = s1 - s2;

        }
        else if (selectOperation == 2)
        {
            textQuestion.SetText(s1 + " x " + s2 + " = ?");
            rightAnswer = s1 * s2;
        }
        else if (selectOperation == 3)
        {
            textQuestion.SetText(s1 + " / " + s2 + " = ?");
            rightAnswer = s1 / s2;
        }

        int randAnswer = Random.Range(0, 80) % 4;
        int[] answers = new int[4];

        while (lastRandAnswer == randAnswer)
        {
            randAnswer = Random.Range(0, 80) % 4;
        }
        lastRandAnswer = randAnswer;

        for (int i = 0; i < answers.Length; i++)
        {
            int localR = 0;
            if (rightAnswer >= 4)
            {
                localR = Random.Range(rightAnswer / 2, rightAnswer * 2);
            }
            else
            {
                localR = Random.Range(1, rightAnswer * 6);
            }


            if (System.Array.IndexOf(answers, localR) == -1 && localR != rightAnswer)
            {
                answers[i] = localR;
            }
            else
            {
                i--;
                continue;
            }
        }

        answers[randAnswer] = rightAnswer;

        for (int i = 0; i < BallonSpawnPoint.Length; i++)
        {
            Ballons[i] = Instantiate(BallonPrefab, BallonSpawnPoint[i].transform.position, Quaternion.identity);
            Ballons[i].GetComponent<BalonMovement>().newText(answers[i].ToString());
        }
    }

    public void destroyBallons()
    {
        for (int i = 0; i < Ballons.Length; i++)
        {
            GameObject.Destroy(Ballons[i]);
        }
        NewQuestion();
    }

    public void addScore(int score)
    {
        Score += score;

        totalQuestionCount++;
        if (score > 0)
        {
            rightAnswerCount++;
        }
        else
        {
            wrongAnswerCount++;
        }

        textScore.SetText(Score.ToString());
    }

    public void RestartScene()
    {
        time = defaultTime;
        textTime.SetText(time.ToString());

        Score = 0;
        textScore.SetText(Score.ToString());

        totalQuestionCount = 0;
        rightAnswerCount = 0;
        wrongAnswerCount = 0;

        for (int i = 0; i < selectOperations.Length; i++)
        {
            selectOperations[i] = Operations[i].GetComponent<Toggle>().isOn;
        }
        destroyBallons();

        playerCar.SetActive(true);
        menuStart.SetActive(false);
        menuGame.SetActive(true);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void selectCar(int id)
    {
        selectedCar.sprite = cars[id];
        playerCar.GetComponent<SpriteRenderer>().sprite = cars[id];
    }

    public void HomeButton()
    {
        menuStart.SetActive(true);
        menuGame.SetActive(false);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);
    }

    public void MainScene()
    {
        SceneManager.LoadScene(0);
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

    public void PauseButton()
    {
        menuStart.SetActive(false);
        menuGame.SetActive(false);
        menuFinish.SetActive(false);
        menuPause.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueButton()
    {
        menuStart.SetActive(false);
        menuGame.SetActive(true);
        menuFinish.SetActive(false);
        menuPause.SetActive(false);
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
