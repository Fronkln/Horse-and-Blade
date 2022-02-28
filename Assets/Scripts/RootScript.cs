using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Everything core about the game is configured here.

public class RootScript : MonoBehaviour
{

#if UNITY_EDITOR

    public int DEBUG_LEVELTOLOAD = 1;

#endif

    public static RootScript instance = null;
    public PlayerScript player = null;

    [HideInInspector] public LevelCompletorScript currentLevelFinisher = null;

    public GameObject[] randomPropObjects = null;

    [Header("UI Elements")]
    public GameObject playButton = null;
    public Image gameOverRoot = null;
    public Image successRoot = null;
    public GameObject settingsRoot = null;

    [Header("Settings Stuff")]
    public Dropdown graphicSettings = null;
    public Toggle soundEnabledToggle = null;
    public Toggle hapticsEnabledToggle = null;
    public Text levelCompletedText = null;

    [Header("Level Info")]

    public int curLevel = 1;
    public bool levelStarted = false;
    public bool levelFinished = false;

    private Transform mapRoot = null;

    private bool playerDeadDoOnce = false;

    private bool haptics = true;


    private Transform currentLevelTransform;
    [Header("Levels")]
    public Transform[] allMaps = null;
    [HideInInspector] public BaseEnemy[] allEnemies = null;
    [HideInInspector] public RandomProp[] rndProps = null;


    private Transform levelsRoot = null;


    public void OnOpenSettings()
    {
        playButton.SetActive(false);
        soundEnabledToggle.isOn = (AudioListener.volume == 1 ? true : false);
        soundEnabledToggle.isOn = (PlayerPrefs.GetInt("hapticsEnabled") == 1 ? true : false);

        settingsRoot.gameObject.SetActive(true);

        string[] options = QualitySettings.names;

        List<Dropdown.OptionData> graphicOptions = new List<Dropdown.OptionData>();

        for (int i = 0; i < options.Length; i++)
            graphicOptions.Add(new Dropdown.OptionData() { text = options[i] });

        graphicSettings.options = graphicOptions;
        graphicSettings.value = QualitySettings.GetQualityLevel();
    }

    public void OnSoundSettingChanged()
    {
        AudioListener.volume = (soundEnabledToggle.isOn ? 1 : 0);
    }

    public void OnGraphicSettingsChanged() => QualitySettings.SetQualityLevel(graphicSettings.value);

    public void OnPlayerCloseSettings()
    {
        PlayerPrefs.SetInt("soundEnabled", (soundEnabledToggle.isOn ? 1 : 0));
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("currentGraphicLevel", graphicSettings.value);
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("hapticsEnabled", (hapticsEnabledToggle.isOn ? 1 : 0));
        PlayerPrefs.Save();

        haptics = hapticsEnabledToggle.isOn;

        playButton.SetActive(true);
        settingsRoot.gameObject.SetActive(false);
    }

    public void EraseProgress()
    {

        PlayerPrefs.SetInt("currentLevel", 0);
        PlayerPrefs.Save();


        Application.Quit();
    }

    void GetAndSetPlayerPrefs()
    {

        if (PlayerPrefs.HasKey("soundEnabled"))
            AudioListener.volume = (PlayerPrefs.GetInt("soundEnabled") == 1 ? 1 : 0);
        else
        {
            PlayerPrefs.SetInt("soundEnabled", 1);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("hapticsEnabled"))
            AudioListener.volume = (PlayerPrefs.GetInt("hapticsEnabled") == 1 ? 1 : 0);
        else
        {
            PlayerPrefs.SetInt("soundEnabled", 1);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("currentGraphicLevel"))
        {
            int qualityLevel = PlayerPrefs.GetInt("currentGraphicLevel");

            if (qualityLevel != QualitySettings.GetQualityLevel())
                QualitySettings.SetQualityLevel(qualityLevel);
        }
    }

    void Awake()
    {
        Application.targetFrameRate = 60;

#if !MOBILE_INPUT

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif

        for (int i = 0; i < allMaps.Length; i++) allMaps[i].gameObject.SetActive(false);

        if (instance == null)
            instance = this;

        if (!playButton.transform.root.gameObject.activeSelf)
            playButton.transform.root.gameObject.SetActive(true);


        mapRoot = GameObject.Find("map").transform;
        levelsRoot = mapRoot.transform.Find("LEVELPREFABS");

        player = GameObject.FindObjectOfType<PlayerScript>();

#if !UNITY_EDITOR
        if (!PlayerPrefs.HasKey("currentLevel"))
        {
            PlayerPrefs.SetInt("currentLevel", 1);
            PlayerPrefs.Save();
        }
        else
        {
            curLevel = PlayerPrefs.GetInt("currentLevel");
        }
#else
        curLevel = DEBUG_LEVELTOLOAD;
#endif

        GetAndSetPlayerPrefs();


        settingsRoot.SetActive(false);
        successRoot.gameObject.SetActive(false);
        gameOverRoot.gameObject.SetActive(false);

        playButton.SetActive(true);

        LoadLevel();
    }



    void LoadLevel()
    {

        currentLevelTransform = allMaps[curLevel];
        currentLevelTransform.gameObject.SetActive(true);
        allEnemies = GameObject.FindObjectsOfType<BaseEnemy>();
        rndProps = GameObject.FindObjectsOfType<RandomProp>();
    }

    public void OnPlayerClickStart()
    {
        levelFinished = false;

        levelStarted = true;
        player.OnLevelStarted();
        playButton.gameObject.SetActive(false);

#if !MOBILE_INPUT

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
#endif
    }

    public void OnPlayerRetry()
    {
        player.transform.position = player.startPos;
        player.transform.rotation = player.startRot;

        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].gameObject.SetActive(true);

            BaseEnemy enemy = allEnemies[i];

            enemy.transform.localPosition = enemy.startPos;
            enemy.transform.rotation = enemy.startRot;
            enemy.attackCooldown = 0;
        }

        for (int i = 0; i < rndProps.Length; i++)
            rndProps[i].PickProp();


        EnemyArrow[] enemyArrows = GameObject.FindObjectsOfType<EnemyArrow>();

        for (int i = 0; i < enemyArrows.Length; i++)
            DestroyImmediate(enemyArrows[i].gameObject);

        successRoot.gameObject.SetActive(false); //Just in case
        gameOverRoot.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);

        if (currentLevelFinisher != null)
            currentLevelFinisher.completeLevelDoOnce = false;
    }


    public void OnPlayerCompleteLevel()
    {

#if !MOBILE_INPUT

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif

        levelFinished = true;
        successRoot.gameObject.SetActive(true);
        levelCompletedText.text = "Level " + (curLevel + 1).ToString() + "\nCompleted!";

        player.OnLevelFinished();
    }

    private void ClearMap()
    {
        currentLevelTransform.gameObject.SetActive(false);

        for (int i = 0; i < allEnemies.Length; i++) // return enemies to their old state
        {
            BaseEnemy enemy = allEnemies[i];
            enemy.gameObject.SetActive(true);
            enemy.attackCooldown = 0;
            enemy.transform.position = enemy.startPos;
            enemy.transform.rotation = enemy.startRot;
        }

        EnemyArrow[] enemyArrows = GameObject.FindObjectsOfType<EnemyArrow>();

        for (int i = 0; i < enemyArrows.Length; i++)
            DestroyImmediate(enemyArrows[i].gameObject);

        allEnemies = null;

        player.transform.position = player.startPos;
        player.transform.rotation = player.startRot;

    }

    public void OnPlayerGoNextLevel()
    {

        if (curLevel + 1 == allMaps.Length) // if there are no more levels left to play
        {
            OnPlayerRetry();
            return;
        }

        ClearMap();

#if UNITY_EDITOR
        DEBUG_LEVELTOLOAD++;
#else

        curLevel++;

        PlayerPrefs.SetInt("currentLevel", curLevel);
        PlayerPrefs.Save();
#endif
        Awake();
    }

    public void OnPlayerDeath()
    {
        gameOverRoot.gameObject.SetActive(true);

#if MOBILE_INPUT
        if(haptics)
     Handheld.Vibrate();
#else
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif
    }
}
