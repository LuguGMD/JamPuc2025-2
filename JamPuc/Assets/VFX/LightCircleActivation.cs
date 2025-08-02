using System.Collections;
using UnityEngine;

public class LightCircleActivation : MonoBehaviour
{
    [SerializeField] float circleTargetValue;
    [SerializeField] float duration; //TODO constante de ativacao de cena
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetFloat("_CirclePosition", 0);
    }

    public void StartActivation()
    {
        StopAllCoroutines();
        rend.material.SetFloat("_CirclePosition", 0);
        StartCoroutine(Activation());
    }

    public IEnumerator Activation()
    {
        rend.material.SetFloat("_CirclePosition", 1);
        float timer = 0;
        float t;
        while (timer < duration)
        {
            t = Mathf.Lerp(1f, circleTargetValue, timer/duration);
            rend.material.SetFloat("_CirclePosition", t);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
