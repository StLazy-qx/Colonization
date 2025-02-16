using System.Collections;
using UnityEngine;
using TMPro;

public class ViewCoinCountOnMap : MonoBehaviour
{
    [SerializeField] private ResourceScanner _scanner;
    [SerializeField] private TMP_Text _textCoinFound;

    private Coroutine _fadeCoroutine;
    private Color _colorAlpha;
    private int _fullAlphaValue = 1;
    private int _hideAlphaValue = 0;
    private float _delay = 2f;

    private void Awake()
    {
        _colorAlpha = _textCoinFound.color;

        SetAlpha(0);
    }

    private void OnEnable()
    {
        _scanner.ResourcesCounting += OnValueCheck;
    }

    private void OnDisable()
    {
        _scanner.ResourcesCounting -= OnValueCheck;
    }

    private void OnValueCheck(int value)
    {
        _textCoinFound.text = $"{value}";

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        SetAlpha(_fullAlphaValue);
        _fadeCoroutine = StartCoroutine(FadeOutText());
    }

    private void SetAlpha(float alpha)
    {
        Color color = _colorAlpha;
        color.a = alpha;
        _textCoinFound.color = color;
    }

    private IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(_delay);

        float duration = 1f;
        float elapsed = 0f;
        Color color = _colorAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(_fullAlphaValue, _hideAlphaValue, elapsed / duration);
            _textCoinFound.color = color;

            yield return null;
        }

        SetAlpha(_hideAlphaValue);
    }
}
