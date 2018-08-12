using Assets.Scripts.Data;
using Assets.Scripts.LiplisSystem.Msg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MothionTest : MonoBehaviour {

    public Dropdown dropEmotion;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnClick()
    {
        MsgSentence sentence = CreateSentence();

        LiplisStatus.Instance.CharDataList.SetExpression(sentence);
    }

    public MsgSentence CreateSentence()
    {
        MsgSentence sentence = new MsgSentence();

        if(dropEmotion.captionText.text == "")
        {
            return sentence;
        }

        string[] buf = dropEmotion.captionText.text.Split(',');

        sentence.Emotion = int.Parse(buf[0]);
        sentence.Point = int.Parse(buf[1]);
        sentence.AllocationId = 0;

        return sentence;
    }

}
