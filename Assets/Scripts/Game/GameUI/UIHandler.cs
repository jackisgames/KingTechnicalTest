using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameUI
{
    class UIHandler:MonoBehaviour
    {
        public UnityEvent OnPlayAgainButtonClicked
        {
            get { return m_gameoverScreen.PlayButton.onClick; }
        }

        [SerializeField]
        private GameplayScreen m_gameplayScreenPrefab;

        [SerializeField]
        private GameoverScreen m_gameoverScreenPrefab;

        private GameplayScreen m_gameplayScreen;

        private GameoverScreen m_gameoverScreen;

        [SerializeField]
        private TextMeshPro m_scorePopupPrefab;

        [SerializeField]
        private Transform m_scorePopUpTransform;

        private Stack<TextMeshPro> m_popupTextPool = new Stack<TextMeshPro>();

        private List<TextMeshPro> m_popupTextActive = new List<TextMeshPro>();

        private readonly StringBuilder m_stringBuilder = new StringBuilder();

        public void Init()
        {
            m_gameplayScreen = Instantiate(m_gameplayScreenPrefab, transform, false);
            m_gameoverScreen = Instantiate(m_gameoverScreenPrefab, transform, false);
        }

        public void StartGame()
        {
            m_gameplayScreen.gameObject.SetActive(true);
            m_gameoverScreen.gameObject.SetActive(false);
        }

        public void EndGame(int finalScore)
        {
            m_gameplayScreen.gameObject.SetActive(false);

            m_stringBuilder.Length = 0;
            m_stringBuilder.Append("SCORE: ");
            m_stringBuilder.Append(finalScore);

            m_gameoverScreen.ScoreText.text = m_stringBuilder.ToString();

            StartCoroutine(ShowEndGame(.3f));

        }

        IEnumerator ShowEndGame(float delay)
        {
            yield return new WaitForSeconds(delay);
            m_gameoverScreen.gameObject.SetActive(true);
        }

        public void UpdateTimer(float time)
        {
            int second = (int)time;
            int milSec = (int) (100 * (time - second));

            m_stringBuilder.Length = 0;
            if (second < 10)
            {
                m_stringBuilder.Append("0");
            }
            m_stringBuilder.Append(second);
            m_stringBuilder.Append("<sub>");
            if (milSec < 10)
            {
                m_stringBuilder.Append("0");
            }
            m_stringBuilder.Append(milSec);
            m_stringBuilder.Append("</sub>");

            m_gameplayScreen.TimerText.text = m_stringBuilder.ToString();

            //m_gameplayScreen.TimerText.text = string.Format("{0:00}<sub>{1:00}</sub>", second, (int)(100 * (time - second)));
        }

        public void UpdateScore(int totalScore)
        {
            m_gameplayScreen.ScoreText.text = totalScore.ToString();
        }

        public void UpdateScore(Vector3 position,int deltaScore,int totalScore)
        {
            TextMeshPro popupScore = m_popupTextPool.Count > 0 ? m_popupTextPool.Pop() : Instantiate(m_scorePopupPrefab, m_scorePopUpTransform);
            popupScore.transform.localPosition = position;
            popupScore.text = deltaScore.ToString();

            popupScore.alpha = 1;

            popupScore.gameObject.SetActive(true);

            m_popupTextActive.Add(popupScore);
            UpdateScore(totalScore);
        }

        private void Update()
        {
            for (int i = 0; i < m_popupTextActive.Count; i++)
            {
                TextMeshPro popupText = m_popupTextActive[i];
                float alpha = popupText.alpha;
                if (alpha <= 0)
                {
                    popupText.gameObject.SetActive(false);
                    m_popupTextActive.RemoveAt(i);
                    m_popupTextPool.Push(popupText);
                    i--;
                }
                else
                {
                    popupText.alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime);
                    popupText.transform.position = popupText.transform.position + Vector3.up * Time.deltaTime;
                }
            }
        }

    }
}
