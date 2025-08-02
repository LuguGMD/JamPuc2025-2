using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLighting : MonoBehaviour
{
    [SerializeField] LayerMask stageLayer;
    [SerializeField] LayerMask characterLayer;
    public List<Renderer> renderers; //todo fazer os objetos entrarem na lista automaticamente

    [SerializeField] private LightController lightController;

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
        foreach (Renderer r in renderers)
        {
            r.material.SetVector("_LightMaskPosition", lightController.transform.position + Vector3.up * 1);
            r.material.SetFloat("_LightMaskRadius", lightController.lightScale);
        }
    }

    private void ToggleCharacterLighting(Actor actor, bool enabled)
    {
        if(enabled)
        {
            renderers.Add(actor.GetComponentInChildren<Renderer>(true));
        }
        else
        {
            renderers.Remove(actor.GetComponentInChildren<Renderer>(true));
        }
    }

    public bool StageHit(Ray ray, out RaycastHit hit)
    {
        return Physics.Raycast(ray, out hit, 100f, stageLayer);
    }
}
