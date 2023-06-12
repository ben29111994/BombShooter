using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    GameObject player;
    public GameObject resultPanel;
    public GameObject ground;
    public float hue = 0.5f;
    Color currentColor;
    public Color playerColor;
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public Rigidbody rigidbody;
    Vector3 dir;
    float speed = 8;
    bool isEndGame = true;
    bool isResult = true;
    bool isSpawnSkin = false;
    public Image killPopup;
    //public Image countDownPopup;
    //public Text countDownText;
    bool isCountDown = false;
    public Text killPopupText;
    public Image comboImage;
    public Text timerText;
    public Text killCountText;
    int killCount;
    float timer = 60;
    public Image playerRankImage;
    public GameObject scoringPopup;
    int score;
    public List<Color> skinList = new List<Color>();
    public GameObject spawner;
    public Text nameText;
    public InputField nameInput;
    public Image soundButton;
    public Image vibrateButton;
    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite vibrateOn;
    public Sprite vibrateOff;
    public int isVibrate = 1;
    public GameObject menuCanvas;
    public GameObject mainCanvas;
    public Text bestScoreText;
    float delaySoundButtonTime = 0.3f;
    public Text rankingText;
    public Slider rankingSlider;
    public Image fillColor;
    public GameObject joystick;
    public GameObject hudSystem;
    public int randomSkin;
    public int randomMap;
    public List<GameObject> mapList = new List<GameObject>();
    public float mapLimit = 30;
    public GameObject confettiEffect;
    public float counterClearText;
    public Text KilledByText;
    public List<Vector3> spawnPoints = new List<Vector3>();
    public int totalScore;
    public GameObject aim;
    int combo = 0;
    public List<Sprite> comboList = new List<Sprite>();
    public List<Texture2D> textureList = new List<Texture2D>();
    bool isShoot = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        Physics.gravity = new Vector3(0, -100, 0);
        instance = this;

        hue = Random.Range(0, 1);
        randomMap = Random.Range(0, mapList.Count);
        mapList[randomMap].SetActive(true);
        ground = mapList[randomMap];
        mapLimit = 30;

        var randomTexture = Random.Range(0, textureList.Count);
        ground.GetComponent<Renderer>().material.SetTexture("_MainTex", textureList[randomTexture]);

        if (!isSpawnSkin)
        {
            randomSkin = Random.Range(0, skinList.Count);
            playerColor = skinList[randomSkin];
            GetComponent<Renderer>().material.color = playerColor;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", playerColor * 0.5f);
            playerRankImage.color = playerColor;
            isSpawnSkin = true;
        }
        Camera.main.transform.position = new Vector3(0, 20f, -2);
        transform.rotation = Quaternion.Euler(-45, 180, 0);
        //var trail = transform.GetChild(1);
        //trail.GetComponent<TrailRenderer>().startColor = playerColor;
        //trail.GetComponent<TrailRenderer>().endColor = new Color32((byte)playerColor.r, (byte)playerColor.g, (byte)playerColor.b, 0);

        var oldName = PlayerPrefs.GetString("Name");
        nameInput.text = oldName;

        var isMute = PlayerPrefs.GetInt("Sound");
        if (isMute == 1)
        {
            Camera.main.GetComponent<AudioSource>().enabled = false;
            soundButton.sprite = soundOff;
        }
        else
        {
            Camera.main.GetComponent<AudioSource>().enabled = true;
            soundButton.sprite = soundOn;
        }
        isVibrate = PlayerPrefs.GetInt("Vibrate");
        if (isVibrate == 0)
        {
            vibrateButton.sprite = vibrateOn;
        }
        else
        {
            vibrateButton.sprite = vibrateOff;
        }

        var bestScore = PlayerPrefs.GetInt("BestScore");
        bestScoreText.text = "BEST x" + bestScore.ToString();
        var currentRankingScore = PlayerPrefs.GetInt("CurrentRankingScore");
        //GameAnalytics.Initialize();
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game", currentRankingScore);
        Color rankColor;
        int getRankTitle = 0;
        int getRankExp = 0;
        if (currentRankingScore <= 5000)
        {
            getRankTitle = (int)(currentRankingScore / 1000);
            getRankExp = currentRankingScore % 1000;

            switch (getRankTitle)
            {
                case 0:
                    rankingText.text = "GAMER 1"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 1:
                    rankingText.text = "GAMER 2"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 2:
                    rankingText.text = "GAMER 3"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 3:
                    rankingText.text = "GAMER 4"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 4:
                    rankingText.text = "GAMER 5"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                default:
                    rankingText.text = "GAMER 1"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
            }
        }
        else if (currentRankingScore > 5000 && currentRankingScore <= 15000)
        {
            currentRankingScore -= 5000;
            getRankTitle = (int)(currentRankingScore / 2000);
            getRankExp = currentRankingScore % 2000;
            switch (getRankTitle)
            {
                case 0:
                    rankingText.text = "PRO 1"; rankingSlider.maxValue = 2000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff862d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 1:
                    rankingText.text = "PRO 2"; rankingSlider.maxValue = 2000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff862d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 2:
                    rankingText.text = "PRO 3"; rankingSlider.maxValue = 2000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff862d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 3:
                    rankingText.text = "PRO 4"; rankingSlider.maxValue = 2000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff862d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 4:
                    rankingText.text = "PRO 5"; rankingSlider.maxValue = 2000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff862d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                default:
                    rankingText.text = "GAMER 1"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
            }
        }
        else if (currentRankingScore > 15000 && currentRankingScore <= 30000)
        {
            currentRankingScore -= 15000;
            getRankTitle = (int)(currentRankingScore / 3000);
            getRankExp = currentRankingScore % 3000;
            switch (getRankTitle)
            {
                case 0:
                    rankingText.text = "MASTER 1"; rankingSlider.maxValue = 3000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff2dee", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 1:
                    rankingText.text = "MASTER 2"; rankingSlider.maxValue = 3000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff2dee", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 2:
                    rankingText.text = "MASTER 3"; rankingSlider.maxValue = 3000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff2dee", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 3:
                    rankingText.text = "MASTER 4"; rankingSlider.maxValue = 3000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff2dee", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 4:
                    rankingText.text = "MASTER 5"; rankingSlider.maxValue = 3000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff2dee", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                default:
                    rankingText.text = "GAMER 1"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
            }
        }
        else if (currentRankingScore > 30000 && currentRankingScore <= 50000)
        {
            currentRankingScore -= 30000;
            getRankTitle = (int)(currentRankingScore / 4000);
            getRankExp = currentRankingScore % 4000;
            switch (getRankTitle)
            {
                case 0:
                    rankingText.text = "LEGENDARY 1"; rankingSlider.maxValue = 4000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#b03eff", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 1:
                    rankingText.text = "LEGENDARY 2"; rankingSlider.maxValue = 4000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#b03eff", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 2:
                    rankingText.text = "LEGENDARY 3"; rankingSlider.maxValue = 4000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#b03eff", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 3:
                    rankingText.text = "LEGENDARY 4"; rankingSlider.maxValue = 4000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#b03eff", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 4:
                    rankingText.text = "LEGENDARY 5"; rankingSlider.maxValue = 4000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#b03eff", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                default:
                    rankingText.text = "GAMER 1"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
            }
        }
        else if (currentRankingScore > 50000)
        {
            currentRankingScore -= 50000;
            getRankTitle = (int)(currentRankingScore / 5000);
            getRankExp = currentRankingScore % 5000;
            switch (getRankTitle)
            {
                case 0:
                    rankingText.text = "EPIC 1"; rankingSlider.maxValue = 5000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff0000", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 1:
                    rankingText.text = "EPIC 2"; rankingSlider.maxValue = 5000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff0000", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 2:
                    rankingText.text = "EPIC 3"; rankingSlider.maxValue = 5000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff0000", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 3:
                    rankingText.text = "EPIC 4"; rankingSlider.maxValue = 5000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff0000", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                case 4:
                    rankingText.text = "EPIC 5"; rankingSlider.maxValue = 5000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#ff0000", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
                default:
                    rankingText.text = "AMATEUR 1"; rankingSlider.maxValue = 1000; rankingSlider.value = getRankExp;
                    ColorUtility.TryParseHtmlString("#5eff2d", out rankColor);
                    rankingText.color = rankColor;
                    fillColor.color = rankColor;
                    break;
            }
        }            
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
        if (!isResult)
        {           
            if(spawnPoints.Count == 10)
            {
                var pickPoint = Random.Range(0, spawnPoints.Count);
                transform.position = spawnPoints[pickPoint];
                spawnPoints.RemoveAt(pickPoint);
                spawnPoints.TrimExcess();
                spawnPoints = spawnPoints.OrderBy(x => Random.value).ToList();
            }           
        }
    }

    public void StartGame()
    {
        //FacebookAnalytics.Instance.LogGame_startEvent(1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        SoundManager.instance.PlaySound(SoundManager.instance.button);
        name = nameText.text;
        PlayerPrefs.SetString("Name", name);
        menuCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        spawner.SetActive(true);
        isEndGame = false;
        isResult = false;
        Camera.main.transform.localEulerAngles = new Vector3(45, 0, 0);
        Camera.main.transform.DOMove(new Vector3(transform.position.x, 18 + transform.localScale.y * 2, transform.position.z - 18 - transform.localScale.y), 0.5f);
        Spawner.instance.RefreshLeaderboard();
        aim.SetActive(true);
    }
    
    public void ButtonSettings()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.button);
        delaySoundButtonTime = 0;
        if(!soundButton.gameObject.activeSelf)
        {
            soundButton.gameObject.SetActive(true);
            vibrateButton.gameObject.SetActive(true);
            soundButton.rectTransform.anchoredPosition3D = new Vector3(450, 850, 0);
            soundButton.rectTransform.DOKill();
            soundButton.rectTransform.DOAnchorPos3D(new Vector3(322, 850, 0), 0.5f);
            vibrateButton.rectTransform.anchoredPosition3D = new Vector3(450, 850, 0);
            vibrateButton.rectTransform.DOKill();
            vibrateButton.rectTransform.DOAnchorPos3D(new Vector3(194, 850, 0), 0.3f);
        }
        else
        {
            soundButton.rectTransform.DOKill();
            vibrateButton.rectTransform.DOKill();
            soundButton.rectTransform.DOAnchorPos3D(new Vector3(450, 850, 0), 0.5f);
            vibrateButton.rectTransform.DOAnchorPos3D(new Vector3(450, 850, 0), 0.3f);
            delaySoundButtonTime = 0.3f;
            StartCoroutine(delaySoundButton());
        }
    }

    IEnumerator delaySoundButton()
    {
        while(delaySoundButtonTime > 0)
        {
            delaySoundButtonTime -= 0.025f;
            yield return null;
        }
        soundButton.gameObject.SetActive(false);
        vibrateButton.gameObject.SetActive(false);
    }

    public void ButtonSound()
    {
        var isMute = PlayerPrefs.GetInt("Sound");
        if (isMute == 1)
        {
            Camera.main.GetComponent<AudioSource>().enabled = true;
            soundButton.sprite = soundOn;
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            Camera.main.GetComponent<AudioSource>().enabled = false;
            soundButton.sprite = soundOff;
            PlayerPrefs.SetInt("Sound", 1);
        }
        SoundManager.instance.PlaySound(SoundManager.instance.button);
    }

    public void ButtonVibrate()
    {
        var isVibrate = PlayerPrefs.GetInt("Vibrate");
        if (isVibrate == 0)
        {
            PlayerPrefs.SetInt("Vibrate", 1);
            vibrateButton.sprite = vibrateOff;
        }
        else
        {
            PlayerPrefs.SetInt("Vibrate", 0);
            vibrateButton.sprite = vibrateOn;
        }
        SoundManager.instance.PlaySound(SoundManager.instance.button);
    }

    void FixedUpdate()
    {
        //if (timer <= 0 && !isResult)
        //{
        //    timer = 0;
        //    isEndGame = true;
        //    isResult = true;
        //    Result();
        //}
        //else if(!isResult)
        //{
        //    if(timer <= 5 && !isCountDown)
        //    {
        //        isCountDown = true;
        //        CountDownPopUp();
        //    }
        //    timer -= 0.02f;
        //}
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer % 60);
        string minutesString = "";
        string secondsString = "";

        if (minutes < 10)
        {
            minutesString = "0" + minutes.ToString();
        }
        else
        {
            minutesString = minutes.ToString();
        }
        if (seconds < 10)
        {
            secondsString = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        else
        {
            secondsString = seconds.ToString();
        }
        timerText.text = minutesString + ":" + secondsString;
        //if(isCountDown)
        //{
        //    countDownText.text = minutesString + ":" + secondsString;
        //}

        if(!Joystick.joystickHeld && !isShoot && !isEndGame)
        {
            isShoot = true;
            Vector3 dirPush = aim.transform.position - transform.position;
            spawner.GetComponent<Spawner>().SpawnBomb(aim.transform, dirPush);
            SoundManager.instance.PlaySound(SoundManager.instance.bomb);
            StartCoroutine(delayShoot());
        }
        else if (!isEndGame && Joystick.joystickHeld)
        {
            speed = Mathf.Clamp(8.5f + transform.localScale.x / 2, 9f, 11f);
            dir.x = SimpleInput.GetAxis(horizontalAxis);
            dir.z = SimpleInput.GetAxis(verticalAxis);

            if (dir != Vector3.zero)
            {
                //transform.rotation = Quaternion.Slerp(
                //    transform.rotation,
                //    Quaternion.LookRotation(dir),
                //    Time.deltaTime * 30
                //);
                transform.rotation = Quaternion.LookRotation(dir);
            }
            
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x, 18 + transform.localScale.y * 2, transform.position.z - 18 - transform.localScale.y), Time.deltaTime * 5);
        }
    }

    IEnumerator delayShoot()
    {
        yield return new WaitForSeconds(1.5f);
        isShoot = false;
    }

    void ChangeGroundColor()
    {
        if(hue > 1)
        {
            hue = 0;
        }
        else
        {
            hue += 1 / 5000f;
        }
        currentColor = Color.HSVToRGB(hue, 0.8f, 0.8f);
        Camera.main.backgroundColor = currentColor;
    }

    public void KillText(string text)
    {
        var fadeColor = new Color32(255, 255, 255, 0);
        var color = new Color32(255, 255, 255, 255);
        killPopupText.color = playerColor;
        if (killPopupText.text == "")
        {
            killPopupText.text += text;
            combo++;
        }
        else
        {
            killPopupText.text += "\n" + text;
            combo++;
            if(combo == 2)
            {
                //SoundManager.instance.PlaySound(SoundManager.instance.doubleKill);
                comboImage.sprite = comboList[0];
                comboImage.DOColor(fadeColor, 0);
                comboImage.DOColor(color, 0f);
                comboImage.DOColor(fadeColor, 3f);
            }
            else if (combo == 3)
            {
                //SoundManager.instance.PlaySound(SoundManager.instance.tripleKill);
                comboImage.sprite = comboList[1];
                comboImage.DOColor(fadeColor, 0);
                comboImage.DOColor(color, 0f);
                comboImage.DOColor(fadeColor, 3f);
            }
            else if (combo >= 4)
            {
                //SoundManager.instance.PlaySound(SoundManager.instance.multiKill);
                comboImage.sprite = comboList[2];
                comboImage.DOColor(fadeColor, 0);
                comboImage.DOColor(color, 0f);
                comboImage.DOColor(fadeColor, 3f);
            }
        }
        killPopupText.DOFade(0, 0);
        killPopupText.DOFade(1, 0f);
        killPopupText.DOFade(0, 3f);
        StartCoroutine(delayClearKillText());
    }

    //public void CountDownPopUp()
    //{
    //    SoundManager.instance.PlaySound(SoundManager.instance.ticking);
    //    countDownPopup.rectTransform.anchoredPosition3D = new Vector3(-750, killPopup.rectTransform.anchoredPosition3D.y, 0);
    //    countDownPopup.rectTransform.DOAnchorPos3D(new Vector3(-333, killPopup.rectTransform.anchoredPosition3D.y, 0), 0.5f);
    //}

    IEnumerator delayClearKillText()
    {
        counterClearText = 4;
        while (counterClearText > 0)
        {
            counterClearText -= 0.02f;
            yield return null;
        }
        killPopupText.text = "";
        combo = 0;
    }

    public void UpdateKillCount()
    {
        killCount++;
        killCountText.text = "KILLS: " + killCount.ToString();
    }

    public void Respawn(string botName, Color botColor)
    {       
        isEndGame = true;
        GetComponent<MeshRenderer>().enabled = false;
        var aim = transform.GetChild(0);
        aim.gameObject.SetActive(false);
        //var trail = transform.GetChild(1);
        //trail.gameObject.SetActive(false);
        KilledByText.text = "You was killed by " + botName;
        KilledByText.color = botColor;
        KilledByText.DOFade(1, 0);
        totalScore = 0;
        StartCoroutine(delayRespawn());
    }

    IEnumerator delayRespawn()
    {
        transform.position = new Vector3(Random.Range(-mapLimit, mapLimit), 1.5f, Random.Range(-mapLimit, mapLimit));
        tag = "Env";
        yield return new WaitForSeconds(2);     
        KilledByText.DOFade(0, 1);
        //var trail = transform.GetChild(1);
        //trail.gameObject.SetActive(true);
        isEndGame = false;
        yield return new WaitForSeconds(1);
        tag = "Player";
        GetComponent<MeshRenderer>().enabled = true;
        var aim = transform.GetChild(0);
        aim.gameObject.SetActive(true);
        transform.localScale = new Vector3(1, 1, 1);
        Camera.main.transform.position = new Vector3(transform.position.x, 18 + transform.localScale.y * 2, transform.position.z - 18 - transform.localScale.y);
    }

    public void Result()
    {
        Spawner.instance.RefreshLeaderboard();
        SoundManager.instance.PlaySound(SoundManager.instance.win);

        menuCanvas.SetActive(true);
        resultPanel.SetActive(true);
        var bestScore = PlayerPrefs.GetInt("BestScore");
        if (totalScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", totalScore);
        }
        var currentRankingScore = PlayerPrefs.GetInt("CurrentRankingScore");
        currentRankingScore += totalScore;
        //FacebookAnalytics.Instance.LogGame_endEvent(1);
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", currentRankingScore);
        PlayerPrefs.SetInt("CurrentRankingScore", currentRankingScore);
        for (int i = 0; i < Spawner.instance.rankList.Count; i++)
        {
            var rankPanel = resultPanel.transform.GetChild(i);
            var rankText = rankPanel.transform.GetChild(0).GetComponent<Text>();
            rankPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-1000, rankPanel.GetComponent<RectTransform>().anchoredPosition3D.y, 0);
            rankPanel.GetComponent<RectTransform>().DOAnchorPos3D(new Vector3(0, rankPanel.GetComponent<RectTransform>().anchoredPosition3D.y, 0), 0.5f);
            string space = " ";
            string scoreString = "";
            if (Spawner.instance.rankList[i].tag == "Bot")
            {
                scoreString = Spawner.instance.rankList[i].GetComponent<Bot>().botTotalScore.ToString();
            }
            else
            {
                scoreString = totalScore.ToString();
            }
            int spaceCount = scoreString.Length;
            for (int j = 0; j < 9 - spaceCount; j++)
            {
                space += " ";
            }
            rankText.text = scoreString + space + Spawner.instance.rankList[i].name;
            if (Spawner.instance.rankList[i].tag == "Player")
            {
                if (i < 3)
                {
                    Instantiate(confettiEffect, new Vector3(transform.position.x, transform.position.y + 20, transform.position.z - 10), Quaternion.Euler(-45, 0, 0));
                }
                rankText.color = Color.white;
                rankPanel.GetComponent<Image>().color = playerColor;
            }
            else
            {
                //try
                //{
                    rankText.color = Spawner.instance.rankList[i].GetComponent<Bot>().botColor;
                //}
                //catch { }
            }
        }

        spawner.SetActive(false);
        joystick.SetActive(false);
        Joystick.instance.dynamicJoystickMovementArea.gameObject.SetActive(false);
        hudSystem.SetActive(false);
        mainCanvas.SetActive(false);       
    }

    public void LoadScene()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bot")
        {
            if(isVibrate == 0)
                MMVibrationManager.Vibrate();
            Vector3 dirPush = collision.transform.position - transform.position;
            dirPush *= transform.localScale.x * 50;
            dirPush = new Vector3(Mathf.Clamp(dirPush.x, -80, 80), 0, Mathf.Clamp(dirPush.z, -80, 80));
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dirPush);
        }

        if (collision.gameObject.tag == "MovingBomb")
        {
            score = 0;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            if (isVibrate == 0)
                MMVibrationManager.Vibrate();
            Vector3 dirPush = collision.transform.position - transform.position;
            dirPush *= 1500 * transform.localScale.x;
            dirPush = new Vector3(Mathf.Clamp(dirPush.x, -3000, 3000), 0, Mathf.Clamp(dirPush.z, -3000, 3000));
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dirPush);
            float torque;
            if (dirPush.x > dirPush.z)
            {
                torque = dirPush.x;
            }
            else
            {
                torque = dirPush.z;
            }
            collision.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(0, torque, 0));
        }

        if (collision.gameObject.tag == "Wall")
        {
            if (isVibrate == 0)
                MMVibrationManager.Vibrate();
            Vector3 dirPush = transform.position - collision.contacts[0].point;
            dirPush *= 150;
            dirPush = new Vector3(Mathf.Clamp(dirPush.x, -150, 150), 0, Mathf.Clamp(dirPush.z, -150, 150));
            GetComponent<Rigidbody>().AddForce(dirPush);
        }
    }

    public void Scoring(int currentScore, GameObject target)
    {
        var popupPoint = Instantiate(scoringPopup, new Vector3(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z), Quaternion.Euler(65,0,0));
        var childText = popupPoint.transform.GetChild(0);
        var scoringText = childText.GetComponent<Text>();
        score += currentScore;
        totalScore += score;
        scoringText.rectTransform.anchoredPosition3D = new Vector3(0, 300, 0);
        scoringText.DOFade(1, 0);
        scoringText.DOFade(0, 3);
        scoringText.rectTransform.DOAnchorPos3D(new Vector3(0, 400, 0), 3);
        scoringText.text = "+" + score.ToString();
        Destroy(popupPoint, 3f);
    }
}
