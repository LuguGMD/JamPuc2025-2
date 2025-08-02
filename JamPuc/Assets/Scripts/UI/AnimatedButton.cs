using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform m_rectTransform;
    [SerializeField] private float m_scaleFactor = 1.1f; // Scale factor for the button when hovered

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        // Reset the scale when the button is disabled
        m_rectTransform.DOKill();
        m_rectTransform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_rectTransform.DOKill(); // Stop any ongoing animations
        m_rectTransform.DOScale(m_scaleFactor, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_rectTransform.DOKill(); // Stop any ongoing animations
        m_rectTransform.DOScale(1f, 0.2f).SetEase(Ease.InBack);
    }
}
