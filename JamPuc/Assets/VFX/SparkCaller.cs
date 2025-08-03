using System;
using UnityEngine;

public class SparkCaller : MonoBehaviour
{
    [SerializeField] ParticleSystem sparkles;
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
