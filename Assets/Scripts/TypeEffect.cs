using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeEffect : MonoBehaviour
{
    public int CPS;
    public bool isAnim;
    public GameObject EndCursor;

    TextMeshProUGUI msgText;
    AudioSource audioSource;
    string targetMessage;
    int index;
    float interval;

    void Awake()
    {
        msgText = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg)
    {
        if(isAnim)
        {
            msgText.text = targetMessage;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            targetMessage = msg;
            EffectStart();
        }
    }

    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false);
        isAnim = true;

        interval = 1.0f / CPS; // 1초/CPS = 1글자가 나오는 딜레이
        Invoke("Effecting", interval); 
    }

    void Effecting()
    {
        if (msgText.text == targetMessage)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMessage[index];

        if (targetMessage[index] != ' ' && targetMessage[index] != '.')
            audioSource.Play();

        index++;

        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isAnim = false;
        EndCursor.SetActive(true);
    }
}
