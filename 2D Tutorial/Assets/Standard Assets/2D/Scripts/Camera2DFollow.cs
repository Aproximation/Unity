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
        public float yPosRestriction = -1;  //means when to stop follow the player if he falls down


        private float m_OffsetZ;
        private Vector3 m_TargetLastPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        private string playerTag = "Player";
        private float nextTimeToSearch = 0;

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
            if (target == null)
            {
                FindPlayer();
                return;
            }

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

            smoothedAheadTargetPos = new Vector3(smoothedAheadTargetPos.x, Mathf.Clamp(smoothedAheadTargetPos.y, yPosRestriction, Mathf.Infinity), smoothedAheadTargetPos.z);

            transform.position = smoothedAheadTargetPos;

            m_TargetLastPosition = target.position;
        }

        private void FindPlayer()
        {
            if (nextTimeToSearch <= Time.time)
            {
                var searchResult = GameObject.FindGameObjectWithTag(playerTag);
                if (searchResult != null)
                {
                    target = searchResult.transform;
                }
                nextTimeToSearch = Time.time + 0.5f;
            }
        }
    }
}
