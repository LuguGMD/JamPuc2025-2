using System.Collections.Generic;
using UnityEngine;

public class Iluminacao : MonoBehaviour
{
    [SerializeField] LayerMask stageLayer;
    [SerializeField] LayerMask characterLayer;
    public List<Renderer> renderers; //todo fazer os objetos entrarem na lista automaticamente

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && StageHit(ray, out hit))
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetVector("_LightMaskPosition", hit.point);
            }
        }
    }

    public bool StageHit(Ray ray, out RaycastHit hit)
    {
        return Physics.Raycast(ray, out hit, 100f, stageLayer);
    }
}
