using UnityEngine;

public class LightController : MonoBehaviour
{
    [Header("Movement")]
    private Vector3 m_currentPosition;
    [SerializeField] private float m_moveSpeed;

    [Header("Scale")]
    private float m_lightScale;
    [SerializeField] private float m_maxLightScale;
    [SerializeField] private float m_minLightScale;
    [SerializeField] private float m_scaleSpeed;

    #region Properties

    public Vector3 currentPosition
    {
        get => m_currentPosition;
        private set => m_currentPosition = value;
    }

    public float moveSpeed
    {
        get => m_moveSpeed;
        private set => m_moveSpeed = value;
    }

    public float lightScale
    {
        get => m_lightScale;
        private set
        {
            m_lightScale = value;
        }
    }

    public float maxLightScale
    {
        get => m_maxLightScale;
        private set
        {
            m_maxLightScale = value;
        }
    }

    public float minLightScale
    {
        get => m_minLightScale;
        private set
        {
            m_minLightScale = value;
        }
    }
    #endregion

    private void Start()
    {
        m_lightScale = (m_maxLightScale + m_minLightScale) / 2f;
    }

    private void Update()
    {
        MoveToPosition();
        ScaleToSize();
    }

    private void OnEnable()
    {
        ActionsManager.Instance.onLightSizeChange += UpdateLightScale;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onLightSizeChange -= UpdateLightScale;
    }

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

    private void MoveToPosition()
    {
        m_currentPosition = GetMouseWorldPosition();
        transform.position = Vector3.Lerp(transform.position, m_currentPosition, Time.deltaTime * m_moveSpeed);
    }

    private void ScaleToSize()
    {
        //DEBUG
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_lightScale += Time.deltaTime * m_scaleSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            m_lightScale -= Time.deltaTime * m_scaleSpeed;
        }

        m_lightScale = Mathf.Clamp(m_lightScale, m_minLightScale, m_maxLightScale);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * m_lightScale, Time.deltaTime * m_scaleSpeed);
    }

    private void UpdateLightScale(float scalePercent)
    {
        m_lightScale = Mathf.Lerp(m_minLightScale, m_maxLightScale, scalePercent);
    }
}
