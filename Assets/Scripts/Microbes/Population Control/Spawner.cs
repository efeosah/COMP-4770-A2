using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Timers;
using Microbes.Entities;
using UnityEngine;

namespace Microbes.Population_Control
{
    // TODO for A2 (optional): Should this be integrated with MovingAgent Spawn?
    public class Spawner : ExtendedMonoBehaviour
    {
        public GameObject microbePrefab;

        // TODO for A2 (optional): Should spawn points be defined in the level?
        public Vector2[] spawnPointArray = 
        {
            new Vector2(9.0f, 9.0f),
            new Vector2(-9.0f, -9.0f),
            new Vector2(9.0f, -9.0f),
            new Vector2(-9.0f, 9.0f),
            new Vector2(4.5f, 4.5f),
            new Vector2(-4.5f, -4.5f),
            new Vector2(4.5f, -4.5f),
            new Vector2(-4.5f, 4.5f)
        };

        public float spawnPointRadius = 1;
        
        #region Regulator

        [SerializeField] float minimumTimeMs = 10f;
        [SerializeField] float maximumDelayMs;
        [SerializeField] RegulatorMode regulatorMode;
        [SerializeField] DelayDistribution delayDistribution;

        [SerializeField] AnimationCurve distributionCurve
            = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        Regulator spawnerRegulator;
        public Regulator SpawnerRegulator => spawnerRegulator;

        public float MinimumTimeMs
        {
            get => minimumTimeMs;
            set => minimumTimeMs = value;
        }

        public float MaximumDelayMs
        {
            get => maximumDelayMs;
            set => maximumDelayMs = value;
        }

        public RegulatorMode RegulatorMode
        {
            get => regulatorMode;
            set => regulatorMode = value;
        }

        public DelayDistribution DelayDistribution
        {
            get => delayDistribution;
            set => delayDistribution = value;
        }

        public AnimationCurve DistributionCurve
        {
            get => distributionCurve;
            set => distributionCurve = value;
        }

        #endregion Regulator

        //int count = 0;

        public override void Start()
        {
            base.Start();
            
            spawnerRegulator ??= new Regulator
            {
                MinimumTimeMs = minimumTimeMs,
                MaximumDelayMs = maximumDelayMs,
                Mode = regulatorMode,
                DelayDistribution = delayDistribution,
                DistributionCurve = distributionCurve
            };
        }

        public override void Update()
        {
            base.Update();
            
            if (spawnPointArray.Length == 0) { return; }

            if (SpawnerRegulator.IsReady)
            //if(count < 5 && SpawnerRegulator.IsReady)
            {
                Vector2 spawnPoint = GetEmptySpawnPoint();

                Microbe.SpawnRandomTypeAt(microbePrefab, spawnPoint);
                //count++;
            }
        }

        public Vector2 GetEmptySpawnPoint()
        {
            Vector2 spawnPoint = Vector2.zero;
            bool isSpawnPointOccupied = false;

            for (int i = 0; i < spawnPointArray.Length; i++)
            {
                isSpawnPointOccupied = false;
                int spawnPointIndex = Random.Range(0, spawnPointArray.Length);
                spawnPoint = spawnPointArray[spawnPointIndex];

                foreach (Microbe existingMicrobe in EntityManager.FindAll<Microbe>())
                {
                    if (Vector3.Distance(existingMicrobe.transform.position, new Vector3(spawnPoint.x, 0, spawnPoint.y)) <= spawnPointRadius)
                    {
                        isSpawnPointOccupied = true;
                        break;
                    }
                }

                if (!isSpawnPointOccupied) { break; }
            }

            if (isSpawnPointOccupied)
            {
                // give up this time. Maybe things will be clear next time.
                return Vector2.negativeInfinity;
            }

            return spawnPoint;
        }

        //spawn child microbe
        //add size?
        public void SpawnChild(MicrobeTypes childType, Vector2 spawnPos)
        {
            Microbe.Spawn(microbePrefab, childType, spawnPos);
        }
    }
}