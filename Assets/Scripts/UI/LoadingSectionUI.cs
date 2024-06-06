using System.Collections.Generic;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class LoadingSectionUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform imageRectTransform;
        [SerializeField] private TextMeshProUGUI loadingMessageLabel;
        [SerializeField] private float loadingTextSwitchInterval = 3f;

        bool _showAnimation = false;
        CoroutineHandle _animationHandle;
        
        private void OnDestroy()
        {
            Timing.KillCoroutines(_animationHandle);
        }

        public void StartAnimation(bool start)
        {
            _showAnimation = start;
            if (start)
                Timing.RunCoroutine(StartLoadingMessageAnimation());
        }
        
        IEnumerator<float> StartLoadingMessageAnimation()
        {
            int i = 0;
            canvasGroup.Show(false);
            RectTransform rectTransform = (RectTransform) transform;
            while (_showAnimation)
            {
                yield return Timing.WaitForSeconds(loadingTextSwitchInterval);
                loadingMessageLabel.text = GameConstants.LoadingTexts[i];
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
                i++;
                i = i == GameConstants.LoadingTexts.Length ? 0 : i;
            }
            
            canvasGroup.Hide();
            Timing.KillCoroutines(_animationHandle);
        }
    }
}