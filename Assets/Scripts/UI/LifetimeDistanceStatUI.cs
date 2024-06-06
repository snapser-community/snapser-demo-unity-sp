using System.Collections.Generic;
using Snapser.Model;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    public class LifetimeDistanceStatUI : BaseUI
    {
        [SerializeField] private TextMeshProUGUI distanceLabel;
        
        #region Game Lifecycle Overrides
        
        protected override void Activate()
        {
            distanceLabel.text = "0000";
            if (SnapserManager.HasStatisticsEnabled)
                GetLifetimeDistanceStat();
        }

        protected override void OnSnapendEdited(Dictionary<SnapserService, bool> servicesState, Dictionary<SnapserService, bool> servicesChangedState)
        {
            if (servicesChangedState[SnapserService.statistics])
            {
                if (SnapserManager.HasStatisticsEnabled)
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
            if (SnapserManager.HasStatisticsEnabled)
                IncrementLifetimeDistance((int) lastRunScore);
        }
        
        #endregion

        #region Snapser Calls & Handlers

        private void IncrementLifetimeDistance(int score)
        {
            if (SnapserManager.Instance.HasStatisticsEnabled)
                SnapserManager.Instance.IncrementStatistic(GameConstants.LifetimeDistanceTraveledKey, score ,OnIncrementLifetimeDistanceStatCallComplete);
        }
        
        private void GetLifetimeDistanceStat()
        {
            if (SnapserManager.Instance.HasStatisticsEnabled)
                SnapserManager.Instance.GetStatistic(GameConstants.LifetimeDistanceTraveledKey, OnGetLifetimeDistanceStatCallComplete);
        }

        void OnGetLifetimeDistanceStatCallComplete(SnapserServiceResponse response)
        {
            if (response.Success && response.Data is StatisticsUserStatistic statistic)
            {
                distanceLabel.text = statistic.Value.PadLeft(GameConstants.GameStatsZeroPadding, '0');
            }
            Show(false);
        }

        void OnIncrementLifetimeDistanceStatCallComplete(SnapserServiceResponse response)
        {
            if (response.Success)
            {
                GetLifetimeDistanceStat();
            }
        }
        
        #endregion
    }
}