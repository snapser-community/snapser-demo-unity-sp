using System.ComponentModel.DataAnnotations;
using UnityEngine;
using Utilities;

namespace UI
{
    public class StartMenuUI : BaseUI
    {
        [SerializeField, Required] private CanvasGroup loginSectionCanvasGroup;
        #region Game Lifecycle Overrides

        protected override void OnGameStarted()
        {
            Hide();
        }

        protected override void OnGameRunEnded(float lastRunScore)
        {
            Show();
        }

        #endregion

        #region UI

        public void OnLoginButtonPressed()
        {
            loginSectionCanvasGroup.Hide();
            SnapserManager.AuthenticationAnonLoginAsync(OnAuthenticationCallCompleted);
        }
        
        #endregion

        #region Snapser Calls & Handlers

        private void OnAuthenticationCallCompleted(SnapserServiceResponse response)
        {
            if (response.Success)
            {
               Debug.Log("Anonymous Login Success!");
            }
            else
            {
                loginSectionCanvasGroup.Show();
            }
        }

        #endregion
    }
}