using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = 0.5f;    //opoznienie ruchu kamery
        public float lookAheadFactor = 10;   //offset kamery w osi x wzgledem kierunku w ktorym patrzy cel
        public float lookAheadReturnSpeed = 0.5f;   // szybkosc powrotu kamery z offsetu do widoku z wycentrowanym celem
        public float lookAheadMoveThreshold = 0.15f; // minimalna odleglosc jaka musi przebyc cel aby offset kamery zostal odwrocony wzgledem osi x

        private float m_OffsetZ;
        private Vector3 m_TargetLastPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            m_TargetLastPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_TargetLastPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 smoothedAheadTargetPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, smoothTime);

            transform.position = smoothedAheadTargetPos;

            m_TargetLastPosition = target.position;
        }
    }
}
