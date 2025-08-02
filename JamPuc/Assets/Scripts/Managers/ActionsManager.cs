using System;
using UnityEngine;

public class ActionsManager : Singleton<ActionsManager>
{
    public Action<float> onLightSizeChange;
    public Action<ActionScriptable> onActionStateChange;
}
