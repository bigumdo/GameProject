using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class SpanHelper
{
    public static bool Contains<T>(this Span<T> arr, T value)
        where T : struct, IEquatable<T>
    {
        for (int i = 0; i < arr.Length; ++i)
        {
            if (arr[i].Equals(value))
            {
                return true;
            }
        }

        return false;
    }
}

/// [todo]
///  - Wave High lighter
///  - Wave Image Setting function
///  - Setting UI
///  - Resource value linking
///  - background movement effect (with shader)
///  - wave&boss warning ui


public class UIManager : MonoBehaviour
{
    [Header("Hp Gauge")]
    [SerializeField] private Transform _hpGaugeTrm;
    [SerializeField] private float _hpGaugeChangeTime;
    [SerializeField] private Ease _hpGaugeChangeEase = Ease.Linear;

    // Hp Gauge가 급격하게 깎였을 때, gauge가 잠시 두꺼워지는 이펙트를 위한 변수들
    [Space]
    // Hp Gauge 이펙트를 발동했을 때, 더해지는 굵기값.
    [SerializeField] private float _hpGaugeEffectAdditionThickness = 20;

    // Hp Gauge가 이펙트를 발동하기 위해 필요한 차이값.
    [SerializeField] private float _hpGaugeEffectRequiredDiffValue = 0.1f;
    [SerializeField] private Ease _hpGaugeEffectEase = Ease.Linear;

    private Vector2 _hpGaugeGettenEffectSize;
    private Vector2 _hpGaugeStartSize;
    private float _previousHpGaugePercent = 1;
    private Image _hpGaugeImg;
    private Tween _hpGaugeChangeTween;
    private Sequence _hpGaugeEffectSeq;


    [Header("Aircraft Position Visual")]
    [SerializeField] private Transform _aircraftPositionVisualParent;
    [SerializeField] private float _aircraftPositionFadeTime;
    [SerializeField] private Ease _aircraftPositionVisualEase = Ease.Linear;
    private Image[] _aircraftPositionVisualArr = null;
    private Sequence _aircraftPositionFadeSeq = null;


    private void Awake()
    {
        _hpGaugeImg = _hpGaugeTrm.GetComponent<Image>();
        _hpGaugeStartSize = _hpGaugeImg.rectTransform.sizeDelta;
        _hpGaugeGettenEffectSize = _hpGaugeStartSize + _hpGaugeEffectAdditionThickness * Vector2.up;

        _aircraftPositionVisualArr = new Image[_aircraftPositionVisualParent.childCount];

        for (int i = 0; i < _aircraftPositionVisualArr.Length; ++i)
        {
            _aircraftPositionVisualArr[i]
                = _aircraftPositionVisualParent.GetChild(i).GetComponent<Image>();
        }
    }

    public void SetHpPercent(float percent)
    {
        if (_hpGaugeChangeTween != null && _hpGaugeChangeTween.active)
            _hpGaugeChangeTween.Kill();

        _hpGaugeChangeTween = DOTween.Sequence();

        float diffValue = _previousHpGaugePercent - percent;
        if (_hpGaugeEffectRequiredDiffValue >= diffValue)
        {
            if (_hpGaugeEffectSeq != null && _hpGaugeEffectSeq.active)
                _hpGaugeEffectSeq.Kill();

            _hpGaugeEffectSeq.Join(_hpGaugeImg.rectTransform
                .DOSizeDelta(_hpGaugeGettenEffectSize, _hpGaugeChangeTime / 2)
                    .SetEase(_hpGaugeEffectEase))

                             .Join(_hpGaugeImg.rectTransform
                .DOSizeDelta(_hpGaugeStartSize, _hpGaugeChangeTime / 2)
                    .SetEase(_hpGaugeEffectEase));
        }

        _hpGaugeChangeTween = _hpGaugeImg.DOFillAmount(percent, _hpGaugeChangeTime)
            .SetEase(_hpGaugeChangeEase);

        _previousHpGaugePercent = percent;
    }

    public void EnableAirCraftPositionVisual(Span<int> exceptIdxArr)
    {
        if (_aircraftPositionFadeSeq != null && _aircraftPositionFadeSeq.active)
            _aircraftPositionFadeSeq.Kill();

        _aircraftPositionFadeSeq = DOTween.Sequence();
        for (int i = 0; i < _aircraftPositionVisualArr.Length; ++i)
        {
            if (exceptIdxArr != null && exceptIdxArr.Contains(i))
            {
                _aircraftPositionFadeSeq
                    .Join(_aircraftPositionVisualArr[i].DOFade(1, _aircraftPositionFadeTime)
                            .SetEase(_aircraftPositionVisualEase));
            }
        }
    }

    public void DisableAirCraftPositionVisual()
    {
        if (_aircraftPositionFadeSeq != null && _aircraftPositionFadeSeq.active)
            _aircraftPositionFadeSeq.Kill();

        _aircraftPositionFadeSeq = DOTween.Sequence();
        foreach (Image img in _aircraftPositionVisualArr)
        {
            _aircraftPositionFadeSeq
                .Join(img.DOFade(0, _aircraftPositionFadeTime)
                            .SetEase(_aircraftPositionVisualEase));
        }
    }


}
