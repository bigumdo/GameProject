using DG.Tweening;
using UnityEngine;

namespace Kbh
{
    public class SettingUI : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _backPanel;
        [SerializeField] private RectTransform _panel;
        [Space]
        [SerializeField] private float _transitionTime;
        [SerializeField] private Ease _transitionEase = Ease.Linear;
        private Tween _transitionTween;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            _panel.transform.localScale = Vector3.zero;
            SetCanvasGroupState(false);
        }

        public void Open()
        {
            if (_transitionTween != null && _transitionTween.IsActive())
                _transitionTween.Kill();

            _transitionTween = _panel.DOScale(1, _transitionTime)
                .SetUpdate(true)
                .SetEase(_transitionEase);

            SetCanvasGroupState(true);
        }

        public void Close()
        {
            if (_transitionTween != null && _transitionTween.IsActive())
                _transitionTween.Kill();

            _transitionTween = _panel.DOScale(0, _transitionTime)
                .SetUpdate(true)
                .SetEase(_transitionEase)
                .OnComplete(() => SetCanvasGroupState(false));
        }

        public void SetCanvasGroupState(bool isActive)
        {
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
            _canvasGroup.alpha = isActive ? 1 : 0;

            _backPanel.gameObject.SetActive(isActive);
            Time.timeScale = isActive ? 0 : 1;
        }
    }
}
