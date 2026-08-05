using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 normalScale;
    private bool isHovering;

    [SerializeField] private float hoverMultiplier = 1.05f;
    [SerializeField] private float smoothSpeed = 12f;

    private void Start()
    {
        normalScale = transform.localScale;
    }

    private void Update()
    {
        Vector3 target = isHovering
            ? normalScale * hoverMultiplier
            : normalScale;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            target,
            Time.unscaledDeltaTime * smoothSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}