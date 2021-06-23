using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class L_DiagManager : MonoBehaviour {

    public static L_DiagManager instance = null;
    public string storyFileName = "StoryText.json";
    public GameObject DiagWin;
	public GameObject DiagColonel;
	public GameObject DiagRadio;
	public GameObject BEBG;
	public GameObject GEBG;
	public GameObject HEBG;
    public Text DiagName;
    public Text DiagText;

    private bool isReady;
    private L_DiagDataArray dataArray;
/*    private Dictionary<string, L_DiagItem[]> dataDict;*/
    private L_DiagItem[] story1Data;
    private L_DiagItem[] story2Data;
    private L_DiagItem[] story3Data;
    private L_DiagItem[] story4Data;
    private L_DiagItem[] story5Data;
    private L_DiagItem[] ending1Data;
    private L_DiagItem[] ending2Data;
    private L_DiagItem[] ending3Data;

    private int curReadingIndex = 0;
    private L_DiagItem[] curReadingData;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        LoadTextFromFile(storyFileName);
	}
	
    public void LoadTextFromFile (string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath))
        {
            string dataJson = File.ReadAllText(filePath);
            dataArray = JsonUtility.FromJson<L_DiagDataArray>(dataJson);
            //Debug.Log(dataArray.items[0].value[0].text);
            for(int i = 0; i < dataArray.items.Length; i++)
            {
/*                dataDict.Add(dataArray.items[i].key, dataArray.items[i].value);*/
/*                Debug.Log(dataDict);*/
                switch (dataArray.items[i].key)
                {
                    case "story1":
                        story1Data = dataArray.items[i].value;
                        break;
                    case "story2":
                        story2Data = dataArray.items[i].value;
                        break;
                    case "story3":
                        story3Data = dataArray.items[i].value;
                        break;
                    case "story4":
                        story4Data = dataArray.items[i].value;
                        break;
                    case "story5":
                        story5Data = dataArray.items[i].value;
                        break;
                    case "ending1":
                        ending1Data = dataArray.items[i].value;
                        break;
                    case "ending2":
                        ending2Data = dataArray.items[i].value;
                        break;
                    case "ending3":
                        ending3Data = dataArray.items[i].value;
                        break;
                    default:
                        break;
                }
            }
/*            Debug.Log(story1Data[0].speaker);*/
        }
        else
        {
            Debug.Log("File can not be found!");
        }
        isReady = true;
        PlayStory("story1");
    }

	private void PlayEndingPrepare()
	{
	}

    public void PlayStory (string chapterName)
    {
        switch (chapterName)
        {
		case "story1":
			    DiagColonel.SetActive (true);
			    DiagRadio.SetActive (true);
                HEBG.SetActive(false);
                BEBG.SetActive(false);
                GEBG.SetActive(false);
                curReadingData = story1Data;
                break;
            case "story2":
                DiagColonel.SetActive(true);
                DiagRadio.SetActive(true);
                HEBG.SetActive(false);
                BEBG.SetActive(false);
                GEBG.SetActive(false);
                curReadingData = story2Data;
                break;
            case "story3":
                DiagColonel.SetActive(true);
                DiagRadio.SetActive(true);
                HEBG.SetActive(false);
                BEBG.SetActive(false);
                GEBG.SetActive(false);
                curReadingData = story3Data;
                break;
            case "story4":
                DiagColonel.SetActive(true);
                DiagRadio.SetActive(true);
                HEBG.SetActive(false);
                BEBG.SetActive(false);
                GEBG.SetActive(false);
                curReadingData = story4Data;
                break;
            case "story5":
                DiagColonel.SetActive(true);
                DiagRadio.SetActive(true);
                HEBG.SetActive(false);
                BEBG.SetActive(false);
                GEBG.SetActive(false);
                curReadingData = story5Data;
                break;
            case "ending1":
                DiagColonel.SetActive(false);
                DiagRadio.SetActive(false);
                HEBG.SetActive(false);
                BEBG.SetActive(true);
                GEBG.SetActive(false);
                curReadingData = ending1Data;
                break;
            case "ending2":
                DiagColonel.SetActive(false);
                DiagRadio.SetActive(false);
                HEBG.SetActive(false);
                BEBG.SetActive(false);
                GEBG.SetActive(true);
                curReadingData = ending2Data;
                break;
            case "ending3":
                DiagColonel.SetActive(false);
                DiagRadio.SetActive(false);
                HEBG.SetActive(true);
                BEBG.SetActive(false);
                GEBG.SetActive(false);
                curReadingData = ending3Data;
                break;
            default:
                break;
        }
        curReadingIndex = 0;
        DiagWin.SetActive(true);
        if (DiagName != null && DiagText != null)
        {
            DiagName.text = curReadingData[curReadingIndex].speaker;
            DiagText.text = curReadingData[curReadingIndex].text;
        }
    }

    public void ClickSkip()
    {
        curReadingIndex++;
        if (curReadingIndex > curReadingData.Length - 1)
        {
            EndStory();
            return;
        }
        if (DiagName != null && DiagText != null)
        {
            DiagName.text = curReadingData[curReadingIndex].speaker;
            DiagText.text = curReadingData[curReadingIndex].text;
        }
    }

    public void EndStory()
    {
        DiagWin.SetActive(false);

        if (L_GameManager.instance.GetCurGameStat() == EGameStat.Ending)
        {
            L_GameManager.instance.ResetGame();
            return;
        }
        L_GameManager.instance.PlayStoryEnd();
    }
		
	public bool GetIsReady()
	{
		return isReady;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
