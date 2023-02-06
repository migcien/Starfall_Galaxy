using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarfallGalaxy.utils
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform player;
        public Vector3 defaultDistanceToTarget;
        public float distanceDamp = 1f;
        public Vector3 velocity;

        // Update is called once per frame
        private void LateUpdate()
        {
            if (player != null)
            {
                MoveCamera();
            }
            else
            {
                player = GameObject.FindWithTag("Player").transform;
                MoveCamera();
            }
        }

        void MoveCamera()
        {
            if (player != null)
            {
                Vector3 wantedPosition = player.position + (player.rotation * defaultDistanceToTarget);
                Vector3 updatedPosition = Vector3.SmoothDamp(transform.position, wantedPosition, ref velocity, distanceDamp);
                transform.position = updatedPosition;
                transform.LookAt(player, player.up);

            }
        }
    }
}