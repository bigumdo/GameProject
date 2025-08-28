using System;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using System.Collections;


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
///  - 전체적인 쉐이더 작업 및 디버깅, 그리고 에셋 적용

namespace Kbh
{
    public class UIManager : MonoBehaviour
    {

        [Serializable]
        public struct ResourceBundle
        {
            public Image icon;
            public TextMeshProUGUI textMesh;
        }


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


        [Header("Wave")]
        [SerializeField] private CanvasGroup _wavePanel;
        [SerializeField] private Transform _waveImageParent;
        [SerializeField] private Transform _waveHighlightTrm;
        [SerializeField] private float _wavePanelFadeTime;
        [SerializeField] private float _wavePanelShowTime;
        private Image[] _waveImgList;
        private Sequence _wavePanelShowSeq;



        [Header("Setting UI Button")]
        [SerializeField] private Button _settingUIBtn;
        [SerializeField] private SettingUI _settingUIPanel;

        [Header("Status UI")]
        [SerializeField] private StatusUI _statusUIPanel;


        [Header("Background Visual")]
        [SerializeField] private Image _backgroundVisualImg;
        private Material _backgroundVisualMat;


        [Header("Resource Link")]
        [SerializeField] private ResourceBundle _goldResource;
        [SerializeField] private ResourceBundle _rockResource;
        [SerializeField] private ResourceBundle _hpResource;


        [Header("Aircraft Position Visual")]
        [SerializeField] private Transform _aircraftPositionVisualParent;
        [SerializeField] private float _aircraftPositionFadeTime;
        [SerializeField] private Ease _aircraftPositionVisualEase = Ease.Linear;
        private Image[] _aircraftPositionVisualArr = null;
        private Sequence _aircraftPositionFadeSeq = null;
        public IEnumerable GetAircraftPositionTrms() => _aircraftPositionVisualParent;
        public int GetAircraftPositionCount() => _aircraftPositionVisualParent.childCount;


        [Header("Wave boss Warning UI")]
        [SerializeField] private Image _bossWarningUI;
        [SerializeField] private float _bossWarningUIFadeTime;
        [SerializeField] private Ease _bossWarningUIFadeEase = Ease.Linear;
        private Sequence _bossWarningSeq;


        public void Init()
        {
            _hpGaugeImg = _hpGaugeTrm.GetComponent<Image>();
            _hpGaugeStartSize = _hpGaugeImg.rectTransform.sizeDelta;
            _hpGaugeGettenEffectSize = _hpGaugeStartSize + _hpGaugeEffectAdditionThickness * Vector2.up;

            _settingUIBtn.onClick.AddListener(HandleSettingUIBtnClick);


            _waveImgList = new Image[_waveImageParent.childCount];
            for (int i = 0; i < _waveImgList.Length; ++i)
            {
                _waveImgList[i] = _waveImageParent.GetChild(i).GetComponent<Image>();
            }



            _aircraftPositionVisualArr = new Image[_aircraftPositionVisualParent.childCount];
            for (int i = 0; i < _aircraftPositionVisualArr.Length; ++i)
            {
                _aircraftPositionVisualArr[i]
                    = _aircraftPositionVisualParent.GetChild(i).GetComponent<Image>();
            }
        }

        private void Start()
        {
            _backgroundVisualMat = _backgroundVisualImg.material;
        }

        public void Update()
        {
            // background UI Image scroll
        }

        #region SUB_UIPANEL

        private void HandleSettingUIBtnClick()
        {
            _settingUIPanel.Open();
        }

        public void OpenStatusPanel(/* 여기에 aircraft 정보를 넘겨줘야 한다. */)
        {
            
        }
        #endregion


        #region RESOURCE_UPDATE
        public void SetGold(int value, int maxValue)
        {
            _goldResource.textMesh.SetText($"{value}/{maxValue}");
            // 여기서 text shader effect를 넣어주는 로직을 호출하도록 구성하기 
        }

        public void SetRock(int value, int maxValue)
        {
            _rockResource.textMesh.SetText($"{value}/{maxValue}");
            // 여기서 text shader effect를 넣어주는 로직을 호출하도록 구성하기 
        }

        public void SetHp(int value, int maxValue)
        {
            _hpResource.textMesh.SetText($"{value}/{maxValue}");
            SetHpPercent((float)value / maxValue);
            // 여기서 text shader effect를 넣어주는 로직을 호출하도록 구성하기 
        }

        private void SetHpPercent(float percent)
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
        #endregion

        #region WAVE
        public void SetWaveImage(Span<Sprite> imageList)
        {
            for (int i = 0; i < imageList.Length; ++i)
                if (i < _waveImgList.Length)
                    _waveImgList[i].sprite = imageList[i];
        }

        public void SetWaveHighLightIdx(int idx, bool isBoss = false)
        {
            if (_wavePanelShowSeq != null && _wavePanelShowSeq.IsActive())
                _wavePanelShowSeq.Kill();

            _wavePanelShowSeq = DOTween.Sequence()
                .Append(_wavePanel.DOFade(1, _wavePanelFadeTime / 2))
                .AppendCallback(() =>
                {
                    if (isBoss)
                        GiveBossWarningEffect();
                })
                .AppendInterval(_wavePanelShowTime)
                .Append(_wavePanel.DOFade(0, _wavePanelFadeTime / 2));


            idx = Mathf.Clamp(idx, 0, _waveImgList.Length - 1);
            _waveHighlightTrm.position = _waveImgList[idx].transform.position;
        }

        #endregion

        #region AIRCRAFT_POSITION_VISUAL
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
        #endregion

        private void GiveBossWarningEffect()
        {
            if (_bossWarningSeq != null && _bossWarningSeq.IsActive())
                _bossWarningSeq.Kill();

            _bossWarningSeq = DOTween.Sequence()
                .Append(_bossWarningUI.DOFade(1, _bossWarningUIFadeTime).SetEase(_bossWarningUIFadeEase))
                .Append(_bossWarningUI.DOFade(0, _bossWarningUIFadeTime).SetEase(_bossWarningUIFadeEase));
        }


    }
    
}