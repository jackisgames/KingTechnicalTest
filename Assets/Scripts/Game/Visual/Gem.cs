using Core;
using Core.GameEvent;
using UnityEngine;

namespace Visual
{
    class Gem:MonoBehaviour,IPiece
    {
        private const float DisperseSpeed = 1f / Constants.PieceMatchDuration;

        private static readonly int GlowPropertyId = Shader.PropertyToID("_GlowAmount");

        private static readonly int HintPropertyId = Shader.PropertyToID("_HintAmount");

        private static readonly int DispersePropertyId = Shader.PropertyToID("_DisperseAmount");

        private static readonly int SelectedPropertyId = Shader.PropertyToID("_Selected");

        private readonly PieceDisposedEvent m_pieceDisposedEvent = new PieceDisposedEvent();

        private float m_moveSpeed = 5;

        private float m_hintDelay;

        private float m_hintTimer;

        private float m_selectedTimer = 0;

        //private Material m_material;

        private Vector3 m_targetPosition;

        private bool m_isDisperse;

        private float m_disperseAmount;

        private MaterialPropertyBlock m_propertyBlock;

        private Renderer m_renderer;

        private void Update()
        {

            if (m_isDisperse)
            {
                m_disperseAmount = Mathf.MoveTowards(m_disperseAmount, 1, Time.deltaTime * DisperseSpeed);
                //m_material.SetFloat(DispersePropertyId, m_disperseAmount);

                m_propertyBlock.SetFloat(DispersePropertyId, m_disperseAmount);
                if (m_disperseAmount >= 1)
                {
                    Dispose();
                }
                else
                {
                    m_renderer.SetPropertyBlock(m_propertyBlock);
                }
                return;
            }
            bool propertyBlockUpdated = false;


            if (m_hintTimer > 0)
            {
                if (m_hintDelay <= 0)
                {
                    m_hintTimer -= Time.deltaTime;
                    if (m_hintTimer <= 0)
                    {
                        m_propertyBlock.SetFloat(GlowPropertyId, 0);
                    }
                    else
                    {
                        m_propertyBlock.SetFloat(GlowPropertyId, Mathf.PingPong(m_hintTimer * 2f, 1));
                    }

                    propertyBlockUpdated = true;
                    
                }
                else
                {
                    m_hintDelay -= Time.deltaTime;
                }
            }

            if (m_selectedTimer > 0)
            {
                m_selectedTimer -= Time.deltaTime;

                if (m_selectedTimer <= 0)
                {
                    m_propertyBlock.SetFloat(SelectedPropertyId, 0);
                    propertyBlockUpdated = true;
                }
            }

            if (propertyBlockUpdated)
            {
                m_renderer.SetPropertyBlock(m_propertyBlock);
            }
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_targetPosition, m_moveSpeed * Time.deltaTime);
        }

        public void Create()
        {
            m_renderer = GetComponent<Renderer>();
            m_propertyBlock = new MaterialPropertyBlock();
            m_renderer.GetPropertyBlock(m_propertyBlock);
            //m_material = GetComponent<Renderer>().material;
        }

        public void Init(int x, int y)
        {

            m_propertyBlock.SetFloat(DispersePropertyId, 0);
            m_propertyBlock.SetFloat(SelectedPropertyId, 0);
            m_renderer.SetPropertyBlock(m_propertyBlock);

            m_hintTimer = 0;
            m_selectedTimer = 0;
            m_disperseAmount = 0;
            m_isDisperse = false;

            m_targetPosition = transform.localPosition = GetPosition(x, y);

            gameObject.SetActive(true);
        }

        public void Hint(float delay,float amount)
        {
            m_propertyBlock.SetFloat(HintPropertyId, amount);
            m_hintDelay = delay;
            m_hintTimer = 1;
        }

        public void Selected()
        {
            m_propertyBlock.SetFloat(SelectedPropertyId, 1);
            m_renderer.SetPropertyBlock(m_propertyBlock);
            m_selectedTimer = .1f;
        }

        public void Dispose()
        {
            gameObject.SetActive(false);
            m_pieceDisposedEvent.Invoke(this);
        }

        public void StartDisappear()
        {
            m_isDisperse = true;
        }

        public void Move(Vector2Int destination)
        {
            m_targetPosition = GetPosition(destination.x, destination.y);
            float distance = Vector3.Distance(transform.localPosition, m_targetPosition);

            m_moveSpeed = Mathf.Max(5, distance / Constants.PieceFallDuration);
        }
        public Transform Transform
        {
            get { return transform; }
        }

        public Vector3 GetPosition(int x, int y)
        {
            return new Vector3(x * Constants.GridSize, y * Constants.GridSize);
        }

        public int PoolId { get; set; }


        public PieceDisposedEvent OnDisposed
        {
            get { return m_pieceDisposedEvent; }
        }
    }
}
