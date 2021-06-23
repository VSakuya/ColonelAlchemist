using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public enum EDropItemType {Jet, Tank, Soldier}

public enum ERatioStat {Bad, Normal, Good, Perfect}

public enum EEndGameType {NextLevel, BadEnding, HappyEnding, HiddenEnding}

public enum EGameStat {MainMenu, Story, Endless, Ending }

public class L_GameManager : MonoBehaviour {

	//public parameters
	public static L_GameManager instance = null;
	public float loadingTime = 0.5f;

    //gamerule
    public int perfectTtoJ = 3;
	public int perfectStoJ = 6;
	public float normalRatioPara = 1.0f;
	public float goodRatioPara = 0.5f;

    public float multiWhenBad = 0.5f;
    public float multiWhenNormal = 0.8f;
    public float multiWhenGood = 1.0f;
    public float multiWhenPerfect = 1.5f;

	public float enemyHPRegenPerSec = 15f;
	public float SideHP = 300f;
    public float MaxPoolSize = 600f;
	public float maxPlayerHPRegenPerSec = 20f;
	public int maxADCountPerSec = 50;
	public float detectDeltaTime = 0.3f;
 

	public GameObject loadingView;
    public GameObject mainGameField;
    public GameObject gameUI;
    public GameObject nextLevelView;
    public GameObject jetPlatform;
    public GameObject tankPlatform;
    public GameObject soldierPlatform;

	//UI
	public GameObject mainMenuOb;
	public Scrollbar pointRatioSB;
    public Slider poolPointSlider;
	public Image ratioStatImg;
	public GameObject spoon;

    [HideInInspector]public bool isStirring = false;

    //private parameters
    private bool isDoingSetup = false;
    private bool isPlayingStory = false;
    private bool isStartBattle = false;
    private EGameStat curGameStat = EGameStat.MainMenu;
    private int curLevel = 1;
	private bool endStoryAndRetry = false;

    //gamerule
    private int gameOpenedType = 3;
	private float PointInPool = 0.0f;
	private int jetCount;
	private int tankCount;
	private int soldierCount;
	private ERatioStat CurRatio;

	private L_StirringSpoon spoonComp;
	private int keyADownCount;
	private int keyDDownCount;
	private float tempTimeCount;
	private float tempTapSpeedRatio;
    private float enemyHP;

    private float startTimeThisLvl = 0.0f;
    private float playTimeThisLevel = 0.0f;
    private float sumPlayTime = 0.0f;

    

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
	}

	void InitGame()
	{
        PointInPool = 0f;
        soldierCount = 0;
        tankCount = 0;
        jetCount = 0;
        if (gameOpenedType > 3 || gameOpenedType < 1)
        {
            gameOpenedType = 3;
        }
		if(spoon != null && spoonComp == null)
			spoonComp = spoon.GetComponent<L_StirringSpoon> ();
        
        mainGameField.SetActive(true);
        switch (gameOpenedType)
        {
            case 1:
                soldierPlatform.SetActive(true);
                break;
            case 2:
                soldierPlatform.SetActive(true);
                tankPlatform.SetActive(true);
                break;
            case 3:
                soldierPlatform.SetActive(true);
                tankPlatform.SetActive(true);
                jetPlatform.SetActive(true);
                break;
        }
        startTimeThisLvl = Time.time;
	}

    //Main menu functions
    public void StartStoryMode()
    {
		StartCoroutine (StartStoryModeCR ());
    }

	private IEnumerator StartStoryModeCR()
	{
        ShowLoading();
        yield return new WaitForSeconds(loadingTime);
		while (!L_DiagManager.instance.GetIsReady ()) 
		{
            yield return null;
		}
        HideLoading();
        curLevel = 1;
        gameOpenedType = 1;
        L_DiagManager.instance.PlayStory("story1");
        isPlayingStory = true;
        curGameStat = EGameStat.Story;
        InitGame();
		HideMainMenu ();
	}

	public void EndGame ()
	{
		Debug.Log ("End game.");
		Application.Quit();
	}

	void HideMainMenu()
	{
		mainMenuOb.SetActive (false);
	}

    public void PlayStoryEnd()
    {
        curGameStat = EGameStat.Story;
        StartBattle();
    }

    public void StartBattle()
    {
        gameUI.SetActive(true);
        enemyHP = SideHP;
        isStartBattle = true;
    }

    public void EndBattle(EEndGameType endBattleType)
    {
        GameObject[] clearObjs = GameObject.FindGameObjectsWithTag("DropItem");
        for (int i = 0; i < clearObjs.Length; i++)
        {
            Destroy(clearObjs[i]);
        }
        if (endBattleType == EEndGameType.NextLevel)
        {
            curLevel++;
            isStartBattle = false;
            nextLevelView.SetActive(true);
        }
        else
        {
            isStartBattle = false;
            mainGameField.SetActive(false);
            jetPlatform.SetActive(false);
            tankPlatform.SetActive(false);
            soldierPlatform.SetActive(false);
            gameUI.SetActive(false);
            StartCoroutine(PlayEnding(endBattleType));
        }

    }
    
    private IEnumerator PlayEnding(EEndGameType endBattleType)
    {
        ShowLoading();
        yield return new WaitForSeconds(loadingTime);
        HideLoading();
        switch (endBattleType)
        {
            case EEndGameType.BadEnding:
                L_DiagManager.instance.PlayStory("ending1");
                isPlayingStory = true;
                curGameStat = EGameStat.Ending;
                break;
            case EEndGameType.HappyEnding:
                L_DiagManager.instance.PlayStory("ending2");
                isPlayingStory = true;
                curGameStat = EGameStat.Ending;
                break;
            case EEndGameType.HiddenEnding:
                L_DiagManager.instance.PlayStory("ending3");
                isPlayingStory = true;
                curGameStat = EGameStat.Ending;
                break;

        }
    }

    public void Restart()
	{
        StartCoroutine(RestartCR());
	}

    private IEnumerator RestartCR()
    {
        ShowLoading();
        yield return new WaitForSeconds(loadingTime);
        HideLoading();
        nextLevelView.SetActive(false);
        PointInPool = 0f;
        soldierCount = 0;
        tankCount = 0;
        jetCount = 0;
        if(curGameStat == EGameStat.Story)
        {
            string shouldPlayStoryName = string.Empty;
            switch (curLevel)
            {
                case 1:
                    shouldPlayStoryName = "story1";
                    gameOpenedType = 1;
                    break;
                case 2:
                    shouldPlayStoryName = "story2";
                    gameOpenedType = 2;
                    break;
                case 3:
                    shouldPlayStoryName = "story3";
                    gameOpenedType = 3;
                    break;
                case 4:
                    shouldPlayStoryName = "story4";
                    gameOpenedType = 3;
                    SideHP = 250f;
                    break;
                case 5:
                    shouldPlayStoryName = "story5";
                    gameOpenedType = 3;
                    SideHP = 350f;
                    break;
            }

            L_DiagManager.instance.PlayStory(shouldPlayStoryName);
            isPlayingStory = true;
            curGameStat = EGameStat.Story;
            InitGame();
        }
    }

    public void ResetGame()
    {
        if (endStoryAndRetry)
        {
            endStoryAndRetry = false;
            curGameStat = EGameStat.Story;
            InitGame();
            StartBattle();
        }
        else
        {
            mainMenuOb.SetActive(true);
            PointInPool = 0f;
            soldierCount = 0;
            tankCount = 0;
            jetCount = 0;
            isPlayingStory = false;
            isStartBattle = false;
            curLevel = 1;
            curGameStat = EGameStat.MainMenu;
        }
    }

	//Game rules
	public void AddItemInPool(EDropItemType itemType, float itemPoint)
	{
        if (PointInPool < MaxPoolSize)
        {
            switch (itemType)
            {
                case EDropItemType.Jet:
                    jetCount += 1;
                    PointInPool += itemPoint;
                    break;
                case EDropItemType.Tank:
                    tankCount += 1;
                    PointInPool += itemPoint;
                    break;
                case EDropItemType.Soldier:
                    soldierCount += 1;
                    PointInPool += itemPoint;
                    break;
            }

        }
		CheckRatio ();
        UpdatePointPoolUI();
        Debug.Log(PointInPool);
	}

    void UpdatePointPoolUI()
    {
        if(poolPointSlider != null)
        {
            poolPointSlider.value = PointInPool / MaxPoolSize;
        }
    }

	void CheckRatio()
	{
        switch (gameOpenedType)
        {
            case 1:
                CurRatio = ERatioStat.Perfect;
                break;

            case 2:
                if (tankCount <= 0)
                {
                    CurRatio = ERatioStat.Bad;
                }
                else
                {
                    float soldierBase = tankCount * (perfectStoJ / perfectTtoJ);
                    float curSoldierPara = Mathf.Abs(soldierCount - soldierBase) / soldierBase;
                    if (curSoldierPara > normalRatioPara * tankCount)
                    {
                        CurRatio = ERatioStat.Bad;
                    }
					else if (curSoldierPara <= normalRatioPara && curSoldierPara > goodRatioPara)
                    {
                        CurRatio = ERatioStat.Normal;
                    }
					else if (curSoldierPara <= goodRatioPara  && curSoldierPara > 0)
                    {
                        CurRatio = ERatioStat.Good;
                    }
                    else
                    {
                        CurRatio = ERatioStat.Perfect;
                    }
                }
                break;

            case 3:
                if (jetCount <= 0)
                {
                    CurRatio = ERatioStat.Bad;
                }
                else
                {
                    int soldierBase = jetCount * perfectStoJ;
                    int tankBase = jetCount * perfectTtoJ;
                    float curSumPara = (Mathf.Abs(soldierBase - soldierCount) / soldierBase) + (Mathf.Abs(tankBase - tankCount) / tankBase);
                    if (curSumPara > normalRatioPara)
                    {
                        CurRatio = ERatioStat.Bad;
                    }
                    else if (curSumPara <= normalRatioPara && curSumPara > goodRatioPara)
                    {
                        CurRatio = ERatioStat.Normal;
                    }
                    else if (curSumPara <= goodRatioPara && curSumPara > 0)
                    {
                        CurRatio = ERatioStat.Good;
                    }
                    else
                    {
                        CurRatio = ERatioStat.Perfect;
                    }
                }
               
                break;

        }

        Debug.Log(CurRatio + "Jet: "+jetCount + ", Tank: "+ tankCount + ", Soldier: " + soldierCount);
        UpdateRatioUI();
	}

    void UpdateRatioUI()
    {
		if (ratioStatImg != null) {
			L_SwitchRatioStat SRS = ratioStatImg.GetComponent<L_SwitchRatioStat>();
			if (SRS != null) {
				SRS.SwitchSprite(CurRatio);
			}
		}
        
    }

	public void StartStirring()
	{
		isStirring = true;
		spoon.SetActive(true);
	}

    public void EndStirring()
    {
        isStirring = false;
        spoon.SetActive(false);
    }

	// Update is called once per frame

	void Update () 
	{
        //Debug.Log(Time.time);
        if (isStartBattle)
        {
            enemyHP += enemyHPRegenPerSec * Time.deltaTime;
            if(!isStirring && PointInPool > 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            {
                StartStirring();
            }
            if (isStirring)
            {
                tempTimeCount += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.A))
                {
                    keyADownCount++;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    keyDDownCount++;
                }

                if (tempTimeCount >= detectDeltaTime)
                {
                    if (keyADownCount + keyDDownCount <= 0)
                    {
                        tempTapSpeedRatio = 0.0f;
                    }
                    else if (maxADCountPerSec * tempTimeCount <= (keyADownCount + keyDDownCount))
                    {
                        tempTapSpeedRatio = 1.0f;
                    }
                    else
                    {
                        tempTapSpeedRatio = (keyADownCount + keyDDownCount) / (maxADCountPerSec * tempTimeCount);
                    }

                    /*Debug.Log("Time past " + tempTimeCount + "sec, TempTapSpeedRatio: " + tempTapSpeedRatio + ", A down " + keyADownCount + ", D down " + keyDDownCount);*/
                    tempTimeCount = 0.0f;
                    keyADownCount = 0;
                    keyDDownCount = 0;
                    if (spoonComp != null)
                        spoonComp.setAnimationSpeedRatio(tempTapSpeedRatio);
                    enemyHP -= tempTapSpeedRatio * maxPlayerHPRegenPerSec;
                    PointInPool -= (tempTapSpeedRatio * maxPlayerHPRegenPerSec) * GetRatioMultiByStat(CurRatio);
                    if (PointInPool <= 0)
                    {
                        EndStirring();
                        soldierCount = 0;
                        tankCount = 0;
                        jetCount = 0;
                    }
                }

            }


            if (enemyHP <= 0)
            {
                if (curLevel < 5)
                {
                    EndStirring();
                    EndBattle(EEndGameType.NextLevel);
                }
                else
                {
                    EndStirring();
                    EndBattle(EEndGameType.HappyEnding);
                }

            }
            else if (enemyHP >= SideHP * 2)
            {
                EndStirring();
                endStoryAndRetry = true;
                EndBattle(EEndGameType.BadEnding);
            }
            UpdatePointPoolUI();
            if (pointRatioSB != null)
            {
                pointRatioSB.value = (SideHP * 2 - enemyHP) / (SideHP * 2);
                //Debug.Log("Set value: " + (SideHP * 2 - enemyHP) / (SideHP * 2) + "e HP: " + enemyHP);
            }
        }

	}


    //Tool functions
    private float GetRatioMultiByStat(ERatioStat Stat)
    {
        switch (Stat)
        {
            case ERatioStat.Bad:
                return multiWhenBad;
            case ERatioStat.Normal:
                return multiWhenNormal;
            case ERatioStat.Good:
                return multiWhenGood;
            case ERatioStat.Perfect:
                return multiWhenPerfect;
        }
        return 1.0f;
    }

    public bool GetIsDoingSetup()
    {
        return isDoingSetup;
    }

    public bool GetIsPlayingStory()
    {
        return isPlayingStory;
    }

    public bool GetIsStartBattle ()
    {
        return isStartBattle;
    }

    public int GetCurLevel()
    {
        return curLevel;
    }

    public EGameStat GetCurGameStat()
    {
        return curGameStat;
    }

    private void ShowLoading()
    {
        loadingView.SetActive(true);
    }

    private void HideLoading()
    {
        loadingView.SetActive(false);
    }
}
