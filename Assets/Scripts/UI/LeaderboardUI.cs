using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Snapser.Model;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class LeaderboardUI : BaseUI
    {
        [SerializeField] private LeaderboardEntryUI leaderboardEntryUI;

        private readonly Dictionary<string, LeaderboardEntryUI> _entryUis = new Dictionary<string, LeaderboardEntryUI>();

        #region Game Lifecycle Overrides

        protected override void Activate()
        {
            if (SnapserManager.HasLeaderboardsEnabled)
                GetLeaderboard();
        }

        protected override void OnSnapendEdited(Dictionary<SnapserService, bool> servicesState, Dictionary<SnapserService, bool> servicesChangedState)
        {
            if (servicesChangedState[SnapserService.leaderboards])
            {
                if (SnapserManager.HasLeaderboardsEnabled)
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
            if (SnapserManager.HasLeaderboardsEnabled)
                SetLeaderboardScore((int) lastRunScore);
        }

        #endregion

        #region UI

        private void AddLeaderboardEntry(List<LeaderboardsUserScore> entries)
        {
            foreach (LeaderboardsUserScore entry in entries)
            {
                if (!_entryUis.Keys.Contains(entry.UserId))
                {
                    LeaderboardEntryUI entryUI = Instantiate(leaderboardEntryUI, transform);
                    _entryUis.Add(entry.UserId, entryUI);
                    entryUI.Initialize(entry.Rank.ToString(), entry.UserId, entry.Score.ToString(CultureInfo.InvariantCulture));
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
                }
            }
        }

        private void ResetLeaderboardUI()
        {
            foreach (LeaderboardEntryUI entryUI in _entryUis.Values)
            {
                DestroyImmediate(entryUI.gameObject);
            }
            _entryUis.Clear();
        }

        #endregion

        #region Snapser Calls & Handlers

        private void SetLeaderboardScore(int score)
        {
            if (SnapserManager.Instance.HasLeaderboardsEnabled)
                SnapserManager.Instance.SetLeaderboardScoreAsync(GameConstants.LeaderboardName, score, OnSetLeaderboardScoreCallComplete);
        }

        void OnSetLeaderboardScoreCallComplete(SnapserServiceResponse response)
        {
            if (response.Success)
                GetLeaderboard();
        }

        private void GetLeaderboard()
        {
            ResetLeaderboardUI();

            if (SnapserManager.Instance.HasLeaderboardsEnabled)
            {
                SnapserManager.Instance.GetLeaderboardAsync(GameConstants.LeaderboardName, GameConstants.LeaderboardTopRange, GameConstants.LeaderboardEntryCount, OnGetLeaderboardCallComplete);
            }
        }

        void OnGetLeaderboardCallComplete(SnapserServiceResponse response)
        {
            if (response.Success && response.Data is List<LeaderboardsUserScore> leaderboardScores)
            {
                AddLeaderboardEntry(leaderboardScores);
                Show(false);
            }
        }

        #endregion
    }
}