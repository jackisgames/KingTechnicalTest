using TMPro;
using UnityEngine;

namespace GameUI
{
    class GameplayScreen:MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_timerText;

        [SerializeField]
        private TextMeshProUGUI m_scoreText;

        public TextMeshProUGUI TimerText
        {
            get { return m_timerText; }
        }

        public TextMeshProUGUI ScoreText
        {
            get { return m_scoreText; }
        }
    }
}
