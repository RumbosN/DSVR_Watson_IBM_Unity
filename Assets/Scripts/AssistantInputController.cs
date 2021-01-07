using System;
using System.Collections;
using System.Collections.Generic;
using IBM.Cloud.SDK.Utilities;
using UnityEngine;

public class AssistantInputController : InputFieldController
{
    private WatsonAssistant _watsonAssistant;
    
    private void Awake()
    {
        _watsonAssistant = WatsonAssistant.instance;
    }

    protected override void SendMessage(string text)
    {
        Runnable.Run(_watsonAssistant.ProcessChat(text));
    }
}
