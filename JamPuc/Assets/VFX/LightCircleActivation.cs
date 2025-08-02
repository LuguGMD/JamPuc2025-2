using System.Collections;
using System.Drawing;
using UnityEngine;

public class LightCircleActivation : MonoBehaviour
{
    [SerializeField] float circleTargetValue;
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetFloat("_CirclePosition", 0);
    }

    public void Activation(float t)
    {
        float size = Mathf.Lerp(1f, circleTargetValue, t);
        rend.material.SetFloat("_CirclePosition", size);
    }

    public void StopActivation()
    {
        rend.material.SetFloat("_CirclePosition", 0f);
    }
}
