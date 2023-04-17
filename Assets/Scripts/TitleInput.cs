using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;

public class TitleInput : FadeOutCompletionReceiver
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fadePanel.GetComponent<Fader>().FadeOut(this);
        }
    }

    public override void ProcessAfterFadeOut()
    {
        SceneManager.Instance.LoadStageSelect();
    }

    [SerializeField] private GameObject fadePanel = null;
}
