using System;
using UnityEngine;

public class ActionsManager : Singleton<ActionsManager>
{
    public Action<float> onLightSizeChange;
    public Action<Actor, float> onLightActor;

    public Action onActionStart;
    public Action onActionEnd;
    public Action<ActionScriptable> onActionStateChange;
    public Action<Actor, bool> onActorToggle;

    public Action<ReactionType> onReactionTrigger; 
}
