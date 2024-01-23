using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // 대화형 데이터는 Dictionary를 사용, 문장을 쓰기 때문에 배열을 사용
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitrArr;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        // Talk Data
        talkData.Add(1000, new string[] { "안녕?:0",
                                          "이 곳에 처음 왔구나?:1",
                                          "한번 둘러보도록 해.:0"});
        talkData.Add(2000, new string[] { "여어.:1",
                                          "이 호수는 정말 아릅답지?:0",
                                          "사실 이 호수에는 무언가의 비밀이 숨겨져있다고 해.:1"});
        talkData.Add(100, new string[] { "평범한 나무상자이다." });
        talkData.Add(200, new string[] { "누군가 사용했던 흔적이 있는 책상이다." });

        // Quest Data
        talkData.Add(10 + 1000, new string[] { "어서 와.:0",
                                               "이 마을에 놀라운 전설이 있다는데:1",
                                               "왼쪽 호수 쪽에 있는 루도가 알려줄거야.:0"});
        talkData.Add(11 + 1000, new string[] { "아직도 안 갔어?:1",
                                               "왼쪽 호수 쪽에 있는 루도가 알려줄거야.:0"});
        talkData.Add(11 + 2000, new string[] { "여어.:1",
                                               "이 호수의 전설을 들으러 온거야?:0",
                                               "그럼 일 좀 하나 해주면 좋겠는데...:1",
                                               "내 집 근처에 떨어진 동전 좀 주어줬으면 해.:1"});

        talkData.Add(20 + 1000, new string[] { "루도의 동전?:1",
                                               "돈을 흘리고 다니면 못쓰지!:3",
                                               "나중에 루도에게 한마디 해야겠어.:3"});
        talkData.Add(20 + 2000, new string[] { "찾으면 꼭 가져다줘.:1"});
        talkData.Add(20 + 5000, new string[] { "근처에서 동전을 찾았다." });
        talkData.Add(21 + 2000, new string[] { "엇, 찾아줘서 고마워.:2" });

        // Portrait Data
        portraitData.Add(1000 + 0, portraitrArr[0]);
        portraitData.Add(1000 + 1, portraitrArr[1]);
        portraitData.Add(1000 + 2, portraitrArr[2]);
        portraitData.Add(1000 + 3, portraitrArr[3]);
        portraitData.Add(2000 + 0, portraitrArr[4]);
        portraitData.Add(2000 + 1, portraitrArr[5]);
        portraitData.Add(2000 + 2, portraitrArr[6]);
        portraitData.Add(2000 + 3, portraitrArr[7]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id)) // Dictionary에 key가 존재하는지 검사
        {
            if(!talkData.ContainsKey(id - id % 10)) // 1021 - 1 = 1020으로 되돌아가 검사함
                return GetTalk(id - id % 100, talkIndex); // 퀘스트 맨 처음 대사가 없을 때, 기본 대사를 가지고 옴
            else
                return GetTalk(id - id % 10, talkIndex);
        }
    
        if (talkIndex == talkData[id].Length)
            return null;
        else    
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }

}
