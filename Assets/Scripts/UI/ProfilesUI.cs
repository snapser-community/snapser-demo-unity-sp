using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class ProfilesUI : BaseUI
    {
        [SerializeField] private CanvasGroup addTagSectionCanvasGroup, labelCanvasGroup;
        [SerializeField] private TextMeshProUGUI gamerTagLabel;
        [SerializeField] private TMP_InputField enterGamerTagField;
        [SerializeField] private Button confirmTagButton;

        private static bool HasCreatedProfile => !PlayerPrefs.GetString(GameConstants.Gamertag).IsEmptyOrNull();

        #region Game Object Lifecycle Overrides
        
        protected override void OnDestroy()
        {
            enterGamerTagField.onValueChanged.RemoveAllListeners();
        }
        
        #endregion
        
        #region Game Lifecycle Overrides
        
        protected override void Activate()
        {
            InitializeUI();
        }

        protected override void OnGameStarted()
        {
            if (!HasCreatedProfile)
                Hide();
        }       
        
        protected override void OnSnapendEdited(Dictionary<SnapserService, bool> servicesState, Dictionary<SnapserService, bool> servicesChangedState)
        {
            if (servicesChangedState[SnapserService.profiles]) 
            {
                if (SnapserManager.HasProfilesEnabled)
                    Activate();
                else
                    Hide();
            }
        }

        protected override void OnGameRunEnded(float lastRunScore)
        {
            if (SnapserManager.HasProfilesEnabled)
                Show();
        }
        
        #endregion
        
        #region UI

        void InitializeUI()
        {
            if (SnapserManager.HasProfilesEnabled)
            {
                Show();
                if (!HasCreatedProfile)
                {
                    addTagSectionCanvasGroup.Show();
                    labelCanvasGroup.Hide();
                    enterGamerTagField.text = String.Empty;
                    confirmTagButton.gameObject.SetActive(false);
                    enterGamerTagField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(); });
                }
                else
                {
                    addTagSectionCanvasGroup.Hide();
                    labelCanvasGroup.Show();
                    gamerTagLabel.text = PlayerPrefs.GetString(GameConstants.Gamertag);

                    SnapserManager.GetUserProfileAsync(OnGetUserProfileCallComplete);
                }
            }
        }
        
        void OnInputFieldValueChanged()
        {
            confirmTagButton.gameObject.SetActive(ValidateGamertag());
        }

        public void OnConfirmTagButtonPressed()
        {
            gamerTagLabel.text = enterGamerTagField.text;
            SnapserManager.UpsertProfileAsync(enterGamerTagField.text, OnUpsertProfileCallComplete);
            addTagSectionCanvasGroup.Hide();
            labelCanvasGroup.Show();
        }

        public void OnEditTagButtonPressed()
        {
            enterGamerTagField.text = gamerTagLabel.text;
            addTagSectionCanvasGroup.Show();
            labelCanvasGroup.Hide();
        }
        
        #endregion
        
        #region Snapser Calls & Handlers
        
        void OnUpsertProfileCallComplete(SnapserServiceResponse response)
        {
            if (response.Success)
            {
                PlayerPrefs.SetString(GameConstants.Gamertag, enterGamerTagField.text);
            }
            else
            {
                PlayerPrefs.DeleteKey(GameConstants.Gamertag);
            }
            
            Show();
        }

        void OnGetUserProfileCallComplete(SnapserServiceResponse response)
        {
            if (response.Success)
            {
                if (HasCreatedProfile && response.Data is SnapserProfile profile) 
                {
                    if (PlayerPrefs.GetString(GameConstants.Gamertag) != profile.gamertag)
                    {
                        gamerTagLabel.text = enterGamerTagField.text;
                        PlayerPrefs.SetString(GameConstants.Gamertag, profile.gamertag);
                    }
                }
            }
            else
            {
                if (SnapserManager.HasProfilesEnabled)
                    Show();
            }
        }
        
        #endregion
        
        #region Input Validation

        bool ValidateGamertag()
        {
            string gamertag = enterGamerTagField.text;
            return !gamertag.IsEmptyOrNull() && gamertag.Length < GameConstants.GamertagCharacterLimit;
        }
        
        #endregion
    }
}