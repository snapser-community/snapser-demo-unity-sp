using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using Obstacles;
using UnityEngine;
using Random = System.Random;

namespace Obstacles
{
    public class ObstacleManager : MonoBehaviour
    {
        [SerializeField] private List<ObstacleCourse> obstacleCourseList;
        [SerializeField] private int additionalPreloadedObstacleCourses = 2;
        [SerializeField] private float obstacleCourseDestroyWaitPeriod = 5f;
        
        private ObstacleCourse _preloadedObstacleCourse, _latestObstacleCourse;
        private readonly List<ObstacleCourse> _generatedCourses = new List<ObstacleCourse>();
        private CoroutineHandle _destroyObstacleCoroutineHandle;
        
        // Singleton instantiation
        private static ObstacleManager _instance;
        public static ObstacleManager Instance
        {
            get
            {
                if (_instance == null) 
                    _instance = GameObject.FindObjectOfType<ObstacleManager>();
                return _instance;
            }
        }
        
        private void Awake()
        {
            ObstacleCourse[] preloadedObstacles = GetComponentsInChildren<ObstacleCourse>();

            if (preloadedObstacles.Length is > 1 or 0)
                Debug.LogError(
                    "There can only be one preloaded obstacle course. This is to make sure we position the generated obstacle courses correctly");
            else
            {
                _preloadedObstacleCourse = preloadedObstacles.FirstOrDefault();
                if (_preloadedObstacleCourse != null)
                {
                    _latestObstacleCourse = _preloadedObstacleCourse;
                    _latestObstacleCourse.OnCourseCompleted += OnObstacleCompleted;
                }
            }
        }

        private void OnDestroy()
        {
            _generatedCourses.ForEach(c=>c.OnCourseCompleted -= OnObstacleCompleted);
            Timing.KillCoroutines(_destroyObstacleCoroutineHandle);
        }

        public void InitializeLevel()
        {
            _generatedCourses.ForEach(Destroy);
            _generatedCourses.Clear();
            
            for (int i = 0; i < additionalPreloadedObstacleCourses; i++)
            {
                GenerateObstacle();
            }
        }

        void GenerateObstacle()
        {
            Random randomGenerator = new Random();
            ObstacleCourse newObstacleCourse = Instantiate(obstacleCourseList[randomGenerator.Next(0, obstacleCourseList.Count - 1)],
                _latestObstacleCourse.ObstacleEndTransform.position, Quaternion.identity, gameObject.transform);
            _latestObstacleCourse = newObstacleCourse;
            newObstacleCourse.OnCourseCompleted += OnObstacleCompleted;
            _generatedCourses.Add(newObstacleCourse);
        }

        void OnObstacleCompleted(ObstacleCourse oc)
        {
            oc.OnCourseCompleted -= OnObstacleCompleted;
            GenerateObstacle();

            _destroyObstacleCoroutineHandle = Timing.RunCoroutine(DestroyObstacle(oc));
        }

        IEnumerator<float> DestroyObstacle(ObstacleCourse oc)
        {
            yield return Timing.WaitForSeconds(obstacleCourseDestroyWaitPeriod);
            
            DestroyImmediate(oc);
            Timing.KillCoroutines(_destroyObstacleCoroutineHandle);
        }
    }
}