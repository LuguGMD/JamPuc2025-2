using UnityEngine;

public class StepHandler : MonoBehaviour
{
    [SerializeField] private SFX m_stepSFX;
    [SerializeField] private float m_stepDistance;
    private Vector3 m_lastStep;
    private float m_lastStepTime;
    [SerializeField] private float m_stepCooldown;

    private void Update()
    {
        HandleStep();
    }

    private void HandleStep()
    {
        if (Time.time - m_lastStepTime >= m_stepCooldown)
        { 
            if (Vector3.Distance(m_lastStep, transform.position) >= m_stepDistance)
            {
                AudioManager.Instance.PlaySFX(m_stepSFX);

                float step = Mathf.Min(m_stepDistance, Vector3.Distance(m_lastStep, transform.position));
                m_lastStep += step * (transform.position - m_lastStep).normalized;
                m_lastStepTime = Time.time;
            }
        }
    }
}
