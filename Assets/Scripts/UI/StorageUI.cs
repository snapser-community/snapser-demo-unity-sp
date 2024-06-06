using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class StorageUI : BaseUI
    {
        [SerializeField] private Button redButton, blueButton;

        private Player _player;

        #region Game Lifecycle Overrides

        protected override void Activate()
        {
            if (SnapserManager.HasStorageEnabled)
            {
                string savedColor = _player.GetSavedColor;
                if (savedColor.IsEmptyOrNull() || savedColor == GameConstants.PlayerColorBlue)
                {
                    blueButton.Select();
                    OnBlueButtonPressed();
                }
                else if (savedColor == GameConstants.PlayerColorRed)
                {
                    redButton.Select();
                    OnRedButtonPressed();
                }
            
                Show();
            }
        }
        
        protected override void OnAuthenticationSuccessful()
        {
            _player = Player.Instance;
            base.OnAuthenticationSuccessful();
        }

        protected override void OnSnapendEdited(Dictionary<SnapserService, bool> servicesState, Dictionary<SnapserService, bool> servicesChangedState)
        {
            if (servicesChangedState[SnapserService.storage])
            {
                if (SnapserManager.HasStorageEnabled)
                    Activate();
                else
                    Hide();
            }
        }
        
        protected override void OnGameStarted()
        {
            Hide();
        }

        protected override void OnGameRunEnded(float lastRunScore)
        {
            if (SnapserManager.HasStorageEnabled)
                Show();
        }
        
        #endregion
        
        #region UI
        
        public void OnBlueButtonPressed()
        {
            SnapserManager.Instance.ReplaceStorageBlob(GameConstants.StorageColorKey, GameConstants.PlayerColorBlue, OnReplaceStorageBlobCallComplete);
        }
        
        public void OnRedButtonPressed()
        {
            SnapserManager.Instance.ReplaceStorageBlob(GameConstants.StorageColorKey, GameConstants.PlayerColorRed, OnReplaceStorageBlobCallComplete);
        }

        void UpdateColor(string colorValue)
        {
            bool valueIsBlue = colorValue == GameConstants.PlayerColorBlue;
            _player.SetPlayerColor(valueIsBlue);
            redButton.interactable = valueIsBlue;
            blueButton.interactable = !valueIsBlue;
            
            Show();
        }
        
        #endregion
        
        #region Snapser Calls & Handlers

        void OnReplaceStorageBlobCallComplete(SnapserServiceResponse response)
        {
            if (response.Success && response.Data is string colorValue)
            {
                UpdateColor(colorValue);
            }
            else
            {
                Hide();
                SnapserManager.Instance.GetStorageBlob(GameConstants.StorageColorKey, OnGetStorageBlobCallComplete);
            }
        }

        void OnGetStorageBlobCallComplete(SnapserServiceResponse response)
        {
            if (response.Success && response.Data is string colorValue)
            {
                UpdateColor(colorValue);
            }
            else
            {
                Hide();
            }
        }
        
        #endregion
    }
}