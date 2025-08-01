using UnityEngine;

public class LightController : MonoBehaviour
{
    private Vector3 m_currentPosition;
    [SerializeField] private float m_speed;

    private Vector3 GetMouseWorldPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = m_currentPosition;
        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo))
        {
            float lastHeight = pos.y;
            pos = hitInfo.point;
            pos.y = lastHeight; // Maintain the current height
        }

        return pos;
    }

    private void Update()
    {
        m_currentPosition = GetMouseWorldPosition();

        transform.position = Vector3.Lerp(transform.position, m_currentPosition, Time.deltaTime * m_speed);
    }
}
