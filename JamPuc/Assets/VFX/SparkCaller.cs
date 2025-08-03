using System;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class SparkCaller : MonoBehaviour
{
    private Actor actor;
    [SerializeField] ParticleSystem sparkles;

    private void Awake()
    {
        actor = GetComponent<Actor>();
        if (actor == null)
        {
            Debug.LogError("SparkCaller requires an Actor component on the same GameObject.");
        }
    }

    private void OnEnable()
    {
        ActionsManager.Instance.onActorNeedLightToggle += ToggleSpark;
    }
    private void OnDisable()
    {
        ActionsManager.Instance.onActorNeedLightToggle -= ToggleSpark;
    }

    private void ToggleSpark(Actor actor, bool toogle)
    {
        if (actor == this)
        {
            if (sparkles != null)
            {
                if (toogle == true)
                {
                    sparkles.Play();
                }
                else
                {
                    sparkles.Stop();
                }
            }
            else
            {
                Debug.LogWarning("No sparkles vfx in " + gameObject.name);
            }
        }
    }

    
}
