using System.Collections;
using System.Drawing;
using UnityEngine;

public class LightCircleActivation : MonoBehaviour
{
    [SerializeField] float circleTargetValue;
    Renderer rend;
    [SerializeField] private LightController lightController;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetFloat("_CirclePosition", 0);
    }

    private void Update()
    {
        circleTargetValue = lightController.selectionPercentage;

        if(circleTargetValue == 0)
        {
            StopActivation();
        }
        else
        {
            Activation(circleTargetValue);
        }
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
