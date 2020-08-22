using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    class GameoverScreen:MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_scoreText;

        [SerializeField]
        private Button m_playButton;

        public TextMeshProUGUI ScoreText
        {
            get { return m_scoreText; }
        }

        public Button PlayButton
        {
            get { return m_playButton; }
        }
    }
}
