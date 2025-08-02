using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool m_lockX = false;
    [SerializeField] private bool m_lockY = false;
    [SerializeField] private bool m_lockZ = false;

    [SerializeField] private bool m_invertX = false;
    [SerializeField] private bool m_invertY = false;
    [SerializeField] private bool m_invertZ = false;

    private void Update()
    {
        Vector3 originalRotation = transform.localEulerAngles;
        transform.LookAt(Camera.main.transform);

        if(!m_lockX)
            originalRotation.x = transform.localEulerAngles.x;
        if (!m_lockY)
            originalRotation.y = transform.localEulerAngles.y;
        if (!m_lockZ)
            originalRotation.z = transform.localEulerAngles.z;

        if (m_invertY)
            originalRotation.y = originalRotation.y + 180f;

        transform.localEulerAngles = originalRotation;
    }
}
