using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{    
    public class TimePattern
    {
        public float _time;
        public Action _pattern;

        public TimePattern(float time, Action pattern)
        {
            _time = time;
            _pattern = pattern;
        }
    }

    [SerializeField]
    List<GameObject> _monsterList = new List<GameObject>();
    List<GameObject> _bigMonsterList = new List<GameObject>();
    List<GameObject> _stageMonsterList = new List<GameObject>();
    GameObject _bossMonster;
    List<Action> _patternList = new List<Action>();
    Queue<TimePattern> _timePatternQueue = new Queue<TimePattern>();
    float _timer;
    float _stage = 0;    
        
    float _confinerX;
    float _confinerY;
    float _norm;

    bool _doPattern = false;
    bool _isBoss = false;
    
    public void Awake()
    {
        Bind();        
        _bossMonster = Resources.Load<GameObject>("Prefabs/Creature/Monster/BossGangzi");
        AddStageMonster((int)MonsterType.MonsterTypes.Bineul);
        AddStageMonster((int)MonsterType.MonsterTypes.Arnyang);
    }

    public void Start()
    {
        _confinerX = MapManager.Map.CameraHorizontal;
        _confinerY = MapManager.Map.CameraVertical;
        _norm = (float)Math.Sqrt(Math.Pow(_confinerX, 2) + Math.Pow(_confinerY, 2)) + 0.2f;        
        _patternList.Add(SpawnPatternCircleBineul);
        _patternList.Add(SpawnPatternCircleArnyang);
        InitTimePattern();
    }

    public void Update()
    {
        if (SceneManagerEx.Scene.NowSceneState != Define.SceneState.BattleScene)
            return; 

        if (PlayerController.Player.DoSkill == true)
            return;

        _timer += Time.deltaTime;
        if(!_isBoss)
            UpdateSpawn();

        UpdateRandomPattern();
        UpdateTimePattern();
    }

    void Bind ()
    {
        string[] names = typeof(MonsterType.MonsterTypes).GetEnumNames();

        for(int i = 0; i < names.Length; i++)
        {
            GameObject monster = null;
            monster = Resources.Load<GameObject>($"Prefabs/Creature/Monster/{names[i]}");

            if(monster != null)
                _monsterList.Add(monster);
        }

        names = typeof(MonsterType.BigMonsterTypes).GetEnumNames();

        for(int i = 0; i< names.Length; i++)
        {
            GameObject monster = null;
            monster = Resources.Load<GameObject>($"Prefabs/Creature/Monster/{names[i]}");

            if (monster != null)
                _bigMonsterList.Add(monster);
        }
    }

    private void InitTimePattern()
    {        
        _timePatternQueue.Enqueue(new TimePattern(60, TimePatternMonsterSpawn));
        _timePatternQueue.Enqueue(new TimePattern(60, TimePatternBossMonster));
        _timePatternQueue.Enqueue(new TimePattern(60, TimePatternRushMaro));
        _timePatternQueue.Enqueue(new TimePattern(70, TimePatternRushMaro));
        _timePatternQueue.Enqueue(new TimePattern(80, TimePatternRushMaro));

        _timePatternQueue.Enqueue(new TimePattern(120, TimePatternMonsterSpawn));

        _timePatternQueue.Enqueue(new TimePattern(180, TimePatternMonsterSpawn));

        _timePatternQueue.Enqueue(new TimePattern(180, TimePatternRushMaro));
        _timePatternQueue.Enqueue(new TimePattern(190, TimePatternRushMaro));
        _timePatternQueue.Enqueue(new TimePattern(200, TimePatternRushMaro));

        _timePatternQueue.Enqueue(new TimePattern(240, TimePatternMonsterSpawn));

        _timePatternQueue.Enqueue(new TimePattern(300, TimePatternBigMonster));

        _timePatternQueue.Enqueue(new TimePattern(360, TimePatternMonsterSpawn));

        _timePatternQueue.Enqueue(new TimePattern(360, TimePatternRushMaro));
        _timePatternQueue.Enqueue(new TimePattern(370, TimePatternRushMaro));
        _timePatternQueue.Enqueue(new TimePattern(380, TimePatternRushMaro));

        _timePatternQueue.Enqueue(new TimePattern(450, TimePatternBigMonster));

        _timePatternQueue.Enqueue(new TimePattern(600, TimePatternBossMonster));
    }

    private void UpdateSpawn()
    {
         if((int)_timer == 10)
         {
            _stage++;
            _timer = 0;

            MonsterSpawn();

            if (_stage == 18)
            {
                RefreshStageMonster();
                AddStageMonster((int)MonsterType.MonsterTypes.Bboongdang);
                AddStageMonster((int)MonsterType.MonsterTypes.Haedoong);
            }

            if(_stage == 30)
            {
                RefreshStageMonster();
                AddStageMonster((int)MonsterType.MonsterTypes.Maro);
                AddStageMonster((int)MonsterType.MonsterTypes.Pienna);
            }

            if(_stage == 42)
            {
                AddStageMonster((int)MonsterType.MonsterTypes.Bineul);
                AddStageMonster((int)MonsterType.MonsterTypes.Arnyang);
                AddStageMonster((int)MonsterType.MonsterTypes.Bboongdang);
                AddStageMonster((int)MonsterType.MonsterTypes.Haedoong);
            }
         }
    }

    public void MonsterSpawn()
    {
        for (int i = 0; i < 10 + (int)(_stage / 6); i++)
        {
            Vector3 randomVector = new Vector3(0, 0, 0);

            int degree = UnityEngine.Random.Range(-180, 181);
            float sin = Mathf.Sin(degree);
            float cos = Mathf.Cos(degree);
            randomVector = PlayerController.Player.transform.position + new Vector3(_norm * cos, _norm * sin, 0);

            GameObject monster = RandomMonster();
            monster.transform.position = randomVector;
            monster.transform.parent = transform;
        }
    }

    private void UpdateRandomPattern()
    {
        if (DataManager.Data.NowStageData._playTime % 45f > 1 && _doPattern == true)
            _doPattern = false;
        if(DataManager.Data.NowStageData._playTime % 45f < 1 && _doPattern == false)
        {
            _doPattern = true;
            int rand = UnityEngine.Random.Range(0, _patternList.Count);

            _patternList[rand].Invoke();
        }
    }

    private void UpdateTimePattern()
    {
        if (_timePatternQueue.Count == 0)
            return;

        if (_timePatternQueue.Peek()._time > DataManager.Data.NowStageData._playTime)
            return;

        _timePatternQueue.Dequeue()._pattern.Invoke();
    }

    GameObject RandomMonster ()
    {        
        int monsterCount = UnityEngine.Random.Range(0,_stageMonsterList.Count);

        GameObject monster = Util.Instantiate(_stageMonsterList[monsterCount]);

        return monster;
    }

    public void AddStageMonster(int index)
    {
        _stageMonsterList.Add(_monsterList[index]);
    }

    public void RefreshStageMonster()
    {
        _stageMonsterList.Clear();
    }
    //---------------------------------SpawnerPattern---------------------------------------

    public void SpawnPatternCircleBineul()
    {        
        Vector3 nowPosition = PlayerController.Player.transform.position;
        // 벡터의 크기를 알 때, 임의의 벡터를 구할 때 벡터의 좌표계는 (norm*CosΘ,norm*SinΘ)
        // 위 공식을 통해 현재 위치에 원형으로 몬스터를 스폰
        for (int i = 0; i < 20; i++)
        {
            float degree = (180 - (18 * i));
            float sin = Mathf.Sin(degree);
            float cos = Mathf.Cos(degree);

            GameObject monster = Util.Instantiate(_monsterList[(int)MonsterType.MonsterTypes.Bineul]);
            Vector3 vec = nowPosition + new Vector3(_norm * cos, _norm * sin, 0);

            vec.z = 0;

            monster.transform.position = vec;
            monster.transform.parent = transform;
        }
    }

    public void SpawnPatternCircleArnyang()
    {        
        Vector3 nowPosition = PlayerController.Player.transform.position;
        
        for (int i = 0; i < 10; i++)
        {
            float degree = (180 - (36 * i));
            float sin = Mathf.Sin(degree);
            float cos = Mathf.Cos(degree);

            GameObject monster = Util.Instantiate(_monsterList[(int)MonsterType.MonsterTypes.Arnyang]);
            Vector3 vec = nowPosition + new Vector3(_norm * cos, _norm * sin, 0);            

            vec.z = 0;

            monster.transform.position = vec;
            monster.transform.parent = transform;
        }
    }

    //----------------TimePattern-------------------------------------
    public void TimePatternRushMaro()
    {        
        int degree = UnityEngine.Random.Range(0, 5);

        float sin = Mathf.Sin(degree * 45);
        float cos = Mathf.Cos(degree * 45);

        Vector3 vec = PlayerController.Player.transform.position + new Vector3(_norm * cos, _norm * sin, 0);
        GameObject rushMaro = Resources.Load<GameObject>(($"Prefabs/Creature/Monster/RushMaro"));
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomVector = new Vector3(0, 0, 0);
            float norm = UnityEngine.Random.Range(0f, 0.5f);

            degree = UnityEngine.Random.Range(-180, 181);
            sin = Mathf.Sin(degree);
            cos = Mathf.Cos(degree);
            randomVector = vec + new Vector3(norm * cos, norm * sin, 0);

            GameObject monster = Util.Instantiate(rushMaro);
            monster.transform.position = randomVector;
            monster.transform.parent = transform;
            monster.GetComponent<RushMaro>().Dir = (PlayerController.Player.transform.position - monster.transform.position).normalized;
        }
    }

    public void TimePatternBigMonster()
    {        
        Vector3 nowPosition = PlayerController.Player.transform.position;
        // 벡터의 크기를 알 때, 임의의 벡터를 구할 때 벡터의 좌표계는 (norm*CosΘ,norm*SinΘ)
        // 위 공식을 통해 현재 위치에 원형으로 몬스터를 스폰
        for (int i = 0; i < 2; i++)
        {
            float degree = (180 - (18 * i));
            float sin = Mathf.Sin(degree);
            float cos = Mathf.Cos(degree);

            GameObject monster = Util.Instantiate(_bigMonsterList[UnityEngine.Random.Range(0, _bigMonsterList.Count)]);
            Vector3 vec = nowPosition + new Vector3(_norm * cos, _norm * sin, 0);

            vec.z = 0;

            monster.transform.position = vec;
            monster.transform.parent = transform;
        }
    }

    public void TimePatternMonsterSpawn()
    {
        for(int i = 0; i < 4; i ++)
        {
            MonsterSpawn();
        }
    }
    public void TimePatternBossMonster()
    {
        SoundManager.Sound.PlayBGM(Resources.Load<AudioClip>("Sound/BGM/BossBGM"));        
        Vector3 nowPosition = PlayerController.Player.transform.position;
        // 벡터의 크기를 알 때, 임의의 벡터를 구할 때 벡터의 좌표계는 (norm*CosΘ,norm*SinΘ)
        // 위 공식을 통해 현재 위치에 원형으로 몬스터를 스폰
        float degree = 0f;
        float sin = Mathf.Sin(degree);
        float cos = Mathf.Cos(degree);

        GameObject monster = Util.Instantiate(_bossMonster);
        Vector3 vec = nowPosition + new Vector3(_norm * cos, _norm * sin, 0);

        vec.z = 0;

        monster.transform.position = vec;
        monster.transform.parent = transform;
        _isBoss = true;
    }
}
