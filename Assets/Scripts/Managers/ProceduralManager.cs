using System.Collections.Generic;
using Modules;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{ 
    public class ProceduralManager : MonoBehaviour
    {
        /*
        * Actual Modules
        */
        
        public enum World
        {
            Normal,
            Alien
        }

        [SerializeField] 
        private Transform levelStart;

        [SerializeField]
        private Transform natToAlienTransition;
        
        [SerializeField]
        private Transform alienToNatTransition;
        
        [SerializeField]
        private Transform blankNormal;
        
        [SerializeField]
        private Transform blankAlien;
    
        [SerializeField]
        Transform[] tutLeaf;
    
        [SerializeField]
        Transform[] tutBranch;
    
        [SerializeField]
        Transform[] tutBees;
    
        [SerializeField]
        Transform[] tutFrog;
        
        [SerializeField]
        Transform[] normalEzLow;
    
        [SerializeField]
        Transform[] normalEzHigh;
    
        [SerializeField]
        Transform[] normalMedLow;
    
        [SerializeField]
        Transform[] normalMedHigh;
    
        [SerializeField]
        Transform[] normalHardLow;
    
        [SerializeField]
        Transform[] normalHardHigh;
        
        [SerializeField]
        Transform[] alienEzLow;
    
        [SerializeField]
        Transform[] alienEzHigh;
    
        [SerializeField]
        Transform[] alienMedLow;
    
        [SerializeField]
        Transform[] alienMedHigh;
    
        [SerializeField]
        Transform[] alienHardLow;
    
        [SerializeField]
        Transform[] alienHardHigh;
    
        /*
        * Where to start the next module from
        */
        private Vector3 _lastEndPos;

        /*
        * Controls proc gen variables
         */
        //Stores number of current alien or normal levels passed
        private int _levelsPassed = 1;
        // Stores number of level modules passed by the pc
        private int _totalLevels = 1;
        // Stores what world the pc is currently in
        public World currentWorld;
        // Stores max number of level modules in normal world
        public int normalMaxLevels = 10;
        // Stores max number of level modules in alien world
        public int alienMaxLevels = 10;
        // List containing all the probabilities
        public float[] probList;
        // List which stores the current list of modules to be spawned from
        Transform[] _chosenList;
        // List which stores the modules spawned
        private List<GameObject> _modules = new List<GameObject>();

        private void Awake()
        {
            _lastEndPos = levelStart.GetComponentInChildren<EndPoint>().transform.position;
            if (levelStart.gameObject != null)
                _modules.Add(levelStart.gameObject);
        }

        private void UpdateProb()
        {
            switch (_totalLevels)
            {
                case 1:
                    ZeroProbList();
                    probList[1] = 1f;
                    break;
                case 2:
                    ZeroProbList();
                    probList[2] = 1f;
                    break;
                case 3:
                    ZeroProbList();
                    probList[3] = 1f;
                    break;
                case 4:
                    ZeroProbList();
                    probList[4] = 1f;
                    break;
                case 5:
                    ZeroProbList();
                    probList[4] = 0.8f;
                    probList[5] = 0.2f;
                    break;
                case 6:
                    ZeroProbList();
                    probList[4] = 0.6f;
                    probList[5] = 0.4f;
                    break;
                case 7:
                    ZeroProbList();
                    probList[5] = 0.6f;
                    probList[6] = 0.4f;
                    break;
                case 8:
                    ZeroProbList();
                    probList[5] = 0.2f;
                    probList[6] = 0.4f;
                    probList[7] = 0.4f;
                    break;
                case 9:
                    ZeroProbList();
                    probList[6] = 0.2f;
                    probList[7] = 0.4f;
                    probList[8] = 0.4f;
                    break;
                default:
                    ZeroProbList();
                    probList[6] = 0.25f;
                    probList[7] = 0.25f;
                    probList[8] = 0.25f;
                    probList[9] = 0.25f;
                    break;
            }
        }

        public void UseProcGen()
        {
            if (_levelsPassed == normalMaxLevels || _levelsPassed == alienMaxLevels)
            {
                Transition();
                _levelsPassed = 0;
            }
            
            else
            {
                float pickedValue = Random.value;
                float cumulativeProbability = 0f;
                for (int i = 0; i < probList.Length; i++)
                {
                    cumulativeProbability += probList[i];
                    if (pickedValue <= cumulativeProbability)
                    {
                        ChosenLevel(i);
                        _levelsPassed++;
                        _totalLevels++;
                        UpdateProb();
                        break;
                    }
                }
            }
        }

        private void ChosenLevel(int chosenListIndex)
        {
            switch (chosenListIndex)
            {
                case 1:
                    _chosenList = tutBranch;
                    break;
                case 2:
                    _chosenList = tutBees;
                    break;
                case 3:
                    _chosenList = tutFrog;
                    break;
                case 4:
                    _chosenList = currentWorld == World.Normal ? normalEzLow : alienEzLow;
                    break;
                case 5:
                    _chosenList = currentWorld == World.Normal ? normalEzHigh : alienEzHigh;
                    break;
                case 6:
                    _chosenList = currentWorld == World.Normal ? normalMedLow : alienMedLow;
                    break;
                case 7:
                    _chosenList = currentWorld == World.Normal ? normalMedHigh : alienMedHigh;
                    break;
                case 8:
                    _chosenList = currentWorld == World.Normal ? normalHardLow : alienHardLow;
                    break;
                case 9:
                    _chosenList = currentWorld == World.Normal ? normalHardHigh : alienHardHigh;
                    break;
            }
            int index = Random.Range(0, _chosenList.Length);
            Transform chosenLevelPart = _chosenList[index];
            Transform lastLevelPartTransform = SpawnLevelPart(_lastEndPos,chosenLevelPart);
            _lastEndPos = lastLevelPartTransform.GetComponentInChildren<EndPoint>().transform.position;
        }

        private Transform SpawnLevelPart(Vector3 spawnPosition, Transform chosenLevelPart)
        {
            Transform levelPartTransform = Instantiate(chosenLevelPart, spawnPosition, Quaternion.identity);
            _modules.Add(levelPartTransform.gameObject);
            if (_modules.Count > 5)
            {
                Destroy(_modules[_totalLevels - 5]);
            }
            return levelPartTransform;
        }

        private void Transition()
        {
            Transform blankLevel1Transform = SpawnLevelPart(_lastEndPos,currentWorld == World.Normal ? blankNormal: blankAlien);
            _lastEndPos = blankLevel1Transform.GetComponentInChildren<EndPoint>().transform.position;
            Transform transitionLevelTransform = SpawnLevelPart(_lastEndPos,currentWorld == World.Normal ? natToAlienTransition: alienToNatTransition);
            _lastEndPos = transitionLevelTransform.GetComponentInChildren<EndPoint>().transform.position;
            Transform blankLevel2Transform = SpawnLevelPart(_lastEndPos,currentWorld == World.Normal ? blankAlien: blankNormal);
            _lastEndPos = blankLevel2Transform.GetComponentInChildren<EndPoint>().transform.position;
        }

        public void SwapWorlds()
        {
            switch (currentWorld)
            {
                case World.Normal:
                    currentWorld = World.Alien;
                    break;
                case World.Alien:
                    currentWorld = World.Normal;
                    break;
            }
        }

        void ZeroProbList()
        {
            for (int i = 0; i < probList.Length; i++) 
            {
                probList[i] = 0f;
            }
        }
    }
}