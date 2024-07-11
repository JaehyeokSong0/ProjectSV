using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    private const float DEFAULT_BLINK_TICK = 0.7f;

    [SerializeField] private TMP_Text _text;
    [Tooltip("Can set blink tick.")]
    [SerializeField] private float _tick = 0f; // Input tick from inspector
    private Coroutine _blinkCoroutine = null;

    private void Awake()
    {
        if(_text == null )
            _text = GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
        StartBlink(_text, _tick);
    }
    private void OnDisable()
    {
        StopBlink();
    }
    public void StartBlink(TMP_Text text, float tick)
    {
        if (tick <= 0f)
            tick = DEFAULT_BLINK_TICK;

        _blinkCoroutine = StartCoroutine(C_StartBlink(text, tick));
    }
    public void StopBlink()
    {
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
    }
    private IEnumerator C_StartBlink(TMP_Text text, float tick)
    {
        WaitForSecondsRealtime tickWait = new WaitForSecondsRealtime(tick);
        while (true)
        {
            text.enabled = false;
            yield return tickWait;
            text.enabled = true;
            yield return tickWait;
        }
    }
}