using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLighting : MonoBehaviour
{
    [SerializeField] LayerMask stageLayer;
    [SerializeField] LayerMask characterLayer;
    public List<Renderer> renderers; //todo fazer os objetos entrarem na lista automaticamente

    private void OnEnable()
    {
        ActionsManager.Instance.onActorToggle += ToggleCharacterLighting;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onActorToggle -= ToggleCharacterLighting;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(StageHit(ray, out hit))
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetVector("_LightMaskPosition", hit.point);
            }
        }
    }

    private void ToggleCharacterLighting(Actor actor, bool enabled)
    {
        if(enabled)
        {
            renderers.Add(actor.GetComponent<Renderer>());
        }
        else
        {
            renderers.Remove(actor.GetComponent<Renderer>());
        }
    }

    public bool StageHit(Ray ray, out RaycastHit hit)
    {
        return Physics.Raycast(ray, out hit, 100f, stageLayer);
    }
}
