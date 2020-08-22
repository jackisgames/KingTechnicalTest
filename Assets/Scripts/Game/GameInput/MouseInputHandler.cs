using Core;
using Core.GameEvent;
using Core.State;
using UnityEngine;
using Visual;

namespace GameInput
{
    [System.Serializable]
    class MouseInputHandler:AGameInput
    {
        [SerializeField]
        private Camera m_camera;

        [SerializeField]
        private Transform m_cornerTransform;

        private Plane m_inputPlane;

        private Vector2Int m_dragOriginPosition;

        private Vector2Int m_clickOriginPosition;

        private bool m_firstPieceClicked;

        public override void Init(BoardState boardState, APieceFactory pieceFactory)
        {
            m_inputPlane = new Plane(m_cornerTransform.forward, m_cornerTransform.position);
        }

        public override void StartGame()
        {
            
        }

        public override void Begin()
        {
            m_firstPieceClicked = false;
            OnStateBegin.Invoke(this);
        }

        public override void Tick()
        {
            Ray inputRay = m_camera.ScreenPointToRay(Input.mousePosition);

            float penetration;
            if (m_inputPlane.Raycast(inputRay, out penetration))
            {
                //get world input position
                Vector3 inputPosition = m_inputPlane.ClosestPointOnPlane(inputRay.GetPoint(penetration));

#if UNITY_EDITOR
                Debug.DrawLine(inputRay.origin, inputPosition);
#endif
                //convert to 'board' space
                inputPosition -= m_cornerTransform.position;

                Vector2Int boardInputPosition = new Vector2Int(Mathf.RoundToInt(inputPosition.x / Constants.GridSize), Mathf.RoundToInt(inputPosition.y / Constants.GridSize));

                OnHoverEvent.Invoke(boardInputPosition);

                if (m_firstPieceClicked)
                {
                    OnHoverEvent.Invoke(m_clickOriginPosition);
                }

                //handle mouse drag
                if (Input.GetMouseButtonDown(0))
                {
                    m_dragOriginPosition = boardInputPosition;
                    OnOriginSelectedEvent.Invoke(m_dragOriginPosition);
                }
                else if(Input.GetMouseButton(0))
                {
                    Vector2Int delta = boardInputPosition - m_dragOriginPosition;
                    if (Mathf.Abs(delta.x) + Mathf.Abs(delta.y) == 1)
                    {
                        //end input state
                        OnStateEnd.Invoke(this);

                        OnSwapEvent.Invoke(new PieceMoveData(-1,m_dragOriginPosition, boardInputPosition));
                        return;
                    }
                }

                //handle click
                if (Input.GetMouseButtonUp(0))
                {
                    if (m_firstPieceClicked)
                    {
                        Vector2Int delta = boardInputPosition - m_clickOriginPosition;
                        if (Mathf.Abs(delta.x) + Mathf.Abs(delta.y) == 1)
                        {
                            OnSwapEvent.Invoke(new PieceMoveData(-1,m_clickOriginPosition, boardInputPosition));

                            //end input state
                            OnStateEnd.Invoke(this);
                            return;
                        }
                    }

                    m_firstPieceClicked = true;
                    m_clickOriginPosition = boardInputPosition;
                }
            }
        }
    }
}
