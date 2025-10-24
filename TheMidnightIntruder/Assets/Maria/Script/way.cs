using UnityEngine;
using UnityEngine.EventSystems;


[DisallowMultipleComponent]
public class way : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum Axis { Z, Y, X }

    [Header("Motion")]
    public Axis axis = Axis.Z;
    public float maxAngle = 15f;
    public float speed = 1f;
    public float phaseOffset = 0f;

    [Header("Tempo")]
    public bool useUnscaledTime = false;

    [Header("Controle")]
    public bool startPaused = false;

    [Header("Hover Effect")]
    [Tooltip("Fator de escala quando o mouse passa por cima (1 = sem mudança)")]
    public float hoverScale = 1.1f;
    [Tooltip("Velocidade de interpolação da escala")]
    public float scaleSpeed = 6f;

    RectTransform _rect;
    Transform _t;
    float _timeAccum;
    bool _hovering;
    Vector3 _baseScale;

    void Awake()
    {
        _t = transform;
        _rect = GetComponent<RectTransform>();
        _baseScale = _t.localScale;
    }

    void OnEnable()
    {
        _timeAccum = 0f;
    }

    void Update()
    {
        if (startPaused) return;

        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        _timeAccum += dt * speed;
        float angle = Mathf.Sin(_timeAccum + phaseOffset) * maxAngle;

        Quaternion rot;
        switch (axis)
        {
            case Axis.X: rot = Quaternion.Euler(angle, 0f, 0f); break;
            case Axis.Y: rot = Quaternion.Euler(0f, angle, 0f); break;
            default: rot = Quaternion.Euler(0f, 0f, angle); break;
        }

        if (_rect != null)
            _rect.localRotation = rot;
        else
            _t.localRotation = rot;

     
        Vector3 targetScale = _hovering ? _baseScale * hoverScale : _baseScale;
        _t.localScale = Vector3.Lerp(_t.localScale, targetScale, dt * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hovering = false;
    }

    public void Play() => startPaused = false;
    public void Pause() => startPaused = true;
    public void ResetPhase(float phase = 0f) => _timeAccum = phase;
}