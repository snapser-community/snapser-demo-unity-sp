using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Events;

namespace Obstacles
{
    public class ObstacleCourse : MonoBehaviour
    {
        [SerializeField] Transform obstacleEndTransform;
        public Transform ObstacleEndTransform => obstacleEndTransform;

        public event UnityAction<ObstacleCourse> OnCourseCompleted; 
        private CoroutineHandle _exitCheckerHandle;

        private void Start()
        {
            _exitCheckerHandle = Timing.RunCoroutine(CheckPlayerPresence(), Segment.SlowUpdate);
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_exitCheckerHandle);
        }

        IEnumerator<float> CheckPlayerPresence()
        {
            while (ObstacleEndTransform.position.x > Player.Instance.transform.position.x)
            {
                yield return Timing.WaitForOneFrame;
            }
            
            OnCourseCompleted?.Invoke(this);
            Timing.KillCoroutines(_exitCheckerHandle);
        }
    }
}