using System;
using UnityEngine;

public class ActionsManager : Singleton<ActionsManager>
{
    public Action<Actor, float> onLightActor;

    public Action<Actor, bool> onActorNeedLightToggle;

    #region Action

    public Action onActionStart;
    public Action onActionEnd;
    public Action<ActionScriptable> onActionStateChange;
    public Action<Actor, bool> onActorToggle;

    #endregion

    public Action<string[]> onDialogue;
    public Action onDialogueEnd;

    public Action<ReactionType> onReactionTrigger;

    public Action onLevelEnd;
}
