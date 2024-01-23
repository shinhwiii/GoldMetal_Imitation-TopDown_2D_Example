using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public Animator talkPanel;
    public Animator portraitAnim;
    public Image portraitImg;
    public Sprite prePortrait;
    public TypeEffect talk;
    public Text questText;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI objName;
    public GameObject scanObject;
    public GameObject menuSet;
    public GameObject player;
    public bool isAction;
    public int talkIndex;

    void Start()
    {
        GameLoad();
        questText.text = questManager.CheckQuest();
    }

    void Update()
    {
        // 메뉴 창 
        if (Input.GetButtonDown("Cancel"))
        {
            Cancel();
        }
    }


    public void Action(GameObject scanobject)
    {
        scanObject = scanobject;
        ObjData objdata = scanObject.GetComponent<ObjData>();
        Talk(objdata.id, objdata.isNpc, objdata.objName);

        talkPanel.SetBool("isShow", isAction);
    }

    void Talk(int id, bool isNpc, string name)
    {
        int questTalkIndex = 0;
        string talkData = "";
        objName.text = name;

        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        // 대화가 끝났을 때
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            questText.text = questManager.CheckQuest(id);
            return;
        }

        if (isNpc)
        {
            objName.color = new Color(0.02963687f, 0.6981132f, 0.3131268f, 1);
            talk.SetMsg(talkData.Split(':')[0]);

            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);

            // 과거의 스프라이트와 다른 애니메이션을 한다면 
            if (prePortrait != portraitImg.sprite)
            {
                portraitAnim.SetTrigger("doEffect"); // 초상화에 애니메이션을 넣음
                prePortrait = portraitImg.sprite;
            }  
        }
        else
        {
            objName.color = new Color(0.5377358f, 0.5377358f, 0.5377358f, 1);
            talk.SetMsg(talkData);
            portraitImg.color = new Color(1, 1, 1, 0);
        }
        isAction = true;
        talkIndex++;
    }

    public void GameSave()
    {
        // 플레이어 위치, 퀘스트 진행 저장
        // PlayerPrefs : 간단한 데이터 저장 기능을 지원함
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad() // 불러오기 
    {
        if (!PlayerPrefs.HasKey("PlayerX")) // 저장한적이 없다면 그냥 반환
            return;
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();
    }

    public void GameExit() // 종료 버튼
    {
        Application.Quit();
    }

    public void Cancel()
    {
        if (menuSet.activeSelf) // 메뉴가 켜져 있다면
        {
            Resume(); // 재개
        }
        else
        {
            menuSet.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Resume() // 재개 버튼 
    {
        menuSet.SetActive(false);
        Time.timeScale = 1;
    }
}
