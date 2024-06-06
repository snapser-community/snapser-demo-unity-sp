using System.Globalization;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    public class DistanceUI : BaseUI
    {
        [SerializeField] private TextMeshProUGUI distanceLabel;

        private Player _player;
        private bool _updateLabel = false;

        #region Game Object Lifecycle

        protected override void Start()
        {
            base.Start();
            _player = Player.Instance;
        }

        private void Update()
        {
            if (_updateLabel)
                distanceLabel.text = ((int) _player.DistanceTraveled).ToString(CultureInfo.InvariantCulture).PadLeft(GameConstants.GameStatsZeroPadding, '0');
        }
        
        #endregion
        
        #region Game Lifecycle Overrides

        protected override void Activate()
        {
            distanceLabel.text = "0000";
            Show(false);
        }

        protected override void OnGameStarted()
        {
            _updateLabel = true;
        }

        protected override void OnGameRunEnded(float lastRunScore)
        {
            _updateLabel = false;
        }

        #endregion
    }
}