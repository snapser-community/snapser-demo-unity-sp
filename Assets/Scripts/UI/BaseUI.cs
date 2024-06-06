using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        protected SnapserManager SnapserManager;

        #region Game Object Lifecycle
        
        protected virtual void Start()
        {
            SnapserManager = SnapserManager.Instance;

            SnapserManager.OnSnapendCreated += OnSnapendCreated;
            SnapserManager.OnSnapendEdited += OnSnapendEdited;
            SnapserManager.OnAuthenticationSuccessful += OnAuthenticationSuccessful;
            
            GameManager.OnGameStarted += OnGameStarted;
            GameManager.OnGameRunEnded += OnGameRunEnded;
            GameManager.OnGameReset += OnGameReset;
        }

        protected virtual void OnDestroy()
        {
            if (SnapserManager != null)
            {
                SnapserManager.OnSnapendCreated -= OnSnapendCreated;
                SnapserManager.OnSnapendEdited -= OnSnapendEdited;
                SnapserManager.OnAuthenticationSuccessful -= OnAuthenticationSuccessful;
            }

            GameManager.OnGameStarted -= OnGameStarted;
            GameManager.OnGameRunEnded -= OnGameRunEnded;
            GameManager.OnGameReset += OnGameReset;
        }
        
        
        #endregion

        #region Game Lifecycle
        
        protected virtual void OnSnapendCreated(bool success)
        {
            
        }
        
        protected virtual void OnSnapendEdited(Dictionary<SnapserService, bool> servicesState, Dictionary<SnapserService, bool> servicesChangedState)
        {
            
        }

        protected virtual void OnAuthenticationSuccessful()
        {
            Activate();
        }

        protected virtual void OnGameStarted()
        {
            
        }
        
        protected virtual void OnGameRunEnded(float lastRunScore)
        {
            
        }
        
        protected virtual void OnGameReset()
        {
            Hide();
        }
        
        #endregion

        #region UI 
        
        protected virtual void Show(bool makeInteractable = true)
        {
            canvasGroup.Show(makeInteractable);
        }

        protected virtual void Hide()
        {
            canvasGroup.Hide();
        }

        protected virtual void Activate()
        {
            
        }
        
        #endregion
    }
}