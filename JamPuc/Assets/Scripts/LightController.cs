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

    public enum Control
    {
        None,
        Mouse,
        Actor,
    }

    [Header("Actor Interaction")]
    [SerializeField] private float m_actorConnectDistance = 2f;
    [SerializeField] private float m_actorDisconnectDistance = 2f;
    private Control m_currentControl = Control.Mouse;
    private bool m_canChangeState = true;

    private Actor m_selectedActor;
    private float m_selectionStartTime;
    [SerializeField] private float m_selectionDuration = 0.5f;
    [SerializeField] private float m_skipSelectionSpeed = 2f;

    private float m_selectionPercentage;

    private bool m_isFocused = false;

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

    public float selectionPercentage
    {
        get => m_selectionPercentage;
        private set
        {
            m_selectionPercentage = value;
        }
    }

    #endregion

    private void Start()
    {
        transform.localScale = Vector3.one * m_lightScale;
    }

    private void Update()
    {
        MoveToPosition();
        ScaleToSize();

        switch(m_currentControl)
        {
            case Control.Mouse:
                CheckConnectionFromActor();
                break;
            case Control.Actor:
                CheckDisconnectionFromActor();
                break;
        }

        ActivateSelectionAction();
    }

    private void OnEnable()
    {
        ActionsManager.Instance.onLightSizeChange += UpdateLightScale;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onLightSizeChange -= UpdateLightScale;
    }

    #region Control

    private Vector3 GetMouseWorldPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = m_currentPosition;
        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Stage")))
        {
            float lastHeight = pos.y;
            pos = hitInfo.point;
            pos.y = lastHeight; // Maintain the current height
        }

        return pos;
    }

    private void MoveToPosition()
    {
        switch(m_currentControl)
        {
            case Control.Mouse:
                m_currentPosition = GetMouseWorldPosition();
                break;
            case Control.Actor:
                if (m_selectedActor != null)
                {
                    float lastHeight = m_currentPosition.y;
                    m_currentPosition = m_selectedActor.transform.position;
                    m_currentPosition.y = lastHeight;
                }
                break;
        }
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

    #endregion

    #region Connection

    private float CheckActorDistance()
    {
        if (m_selectedActor == null)
            return 10000;

        Vector3 mousePos = GetMouseWorldPosition();

        Vector2 actorPosVec2 = new Vector2(m_selectedActor.transform.position.x, m_selectedActor.transform.position.z);
        Vector2 mousePosVec2 = new Vector2(mousePos.x, mousePos.z);

        return Vector2.Distance(actorPosVec2, mousePosVec2);
    }

    private void CheckDisconnectionFromActor()
    {
        if (m_selectedActor == null)
            return;
        
        if (CheckActorDistance() >= m_actorDisconnectDistance || ActorManager.Instance.isTimelinePlaying)
        {
            m_isFocused = false;
            ChangeState(Control.Mouse);
        }
    }

    private void CheckConnectionFromActor()
    {
        if (m_selectedActor == null)
            return;

        if(m_selectedActor.hasAction)
            if (CheckActorDistance() <= m_actorConnectDistance && !ActorManager.Instance.isTimelinePlaying)
            {
                m_isFocused = true;
                ChangeState(Control.Actor);
            }
    }

    private void ChangeState(Control state)
    {
        if(m_canChangeState)
            m_currentControl = state;
    }

    private void ActivateSelectionAction()
    {
        m_selectionPercentage = 0;

        if (m_selectedActor == null)
            return;
        if (!m_selectedActor.hasAction)
            return;
        if ((!m_isFocused))
            return;
        if(!ActorManager.Instance.canPlayAction)
            return;

        bool doSpeedUp = Input.GetMouseButton(0);

        if(doSpeedUp)
        {
            m_selectionStartTime -= Time.deltaTime * m_skipSelectionSpeed;
        }

        m_selectionPercentage = (Time.time - m_selectionStartTime) / m_selectionDuration;
        m_selectionPercentage = Mathf.Clamp01(m_selectionPercentage);

        if (Time.time - m_selectionStartTime >= m_selectionDuration)
        {
            m_selectedActor.PlayAction();
            m_selectedActor = null;
        }
    }

    private void ChangeSelectedActor(Actor actor)
    {
        if(actor != m_selectedActor)
        {
            m_isFocused = false;
            m_selectionStartTime = Time.time;
            m_selectedActor = actor;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (m_selectedActor == null)
            {
                Actor newActor = other.GetComponent<Actor>();
                ChangeSelectedActor(newActor);
            }
            else if(ActorManager.Instance.isTimelinePlaying && m_selectedActor.doNeedLighting)
            {
                ActionsManager.Instance.onLightActor?.Invoke(m_selectedActor, CheckActorDistance());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_selectedActor != null)
            if (other.CompareTag("Player"))
            {
                m_selectedActor = null;
            }
    }

    #endregion

}
