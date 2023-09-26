using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : CreatureController
{
    // Singleton :: 해당 게임의 플레이어는 전투 씬에서 유일성을 보장
    // 진행하는 라운드 마다 캐릭터가 변경될 수 있기 때문에 dontdestroy는 등록하지 않으며 Refresh함수를 통해 초기화
    static PlayerController _instance; 
    public static PlayerController Player { get { return _instance; } }
    //============================================================================
    float _exp;
    [SerializeField]
    int _level=1;
    [SerializeField]
    float _maxEXP;

    [SerializeField]
    int _critical;
    [SerializeField]
    protected float _maxHp = 1000f;
    public float MaxHp
    {
        get
        {
            return _maxHp;
        }
        set
        {
            _maxHp = value;
        }
    }
    [SerializeField]
    protected float _attack = 3f;
    [SerializeField]
    protected float _speed = 1.0f;

    float _hitDelay;

    bool _died = false;

    Transform _hpBar;

    SkillManager _skill;

    GameObject _playerSkill;

    bool _doSkill = false;

    float _skillCoolTime;
    float _nextUseSkill;
    //변수
    CameraController _mainCamera;
    Rigidbody2D _rigid;

    AudioClip _playerHitSound;

    //==========================PassiveSkill==================================
    bool _jeagandoongi = false;
    bool _choopChoop = false;
    bool _hinachopathy = false;
    bool _stellar = false;

    Skill _skillJeagandoongi = null;
    Skill _skillChoopchoop = null;
    Skill _skillHinachopathy = null;
    Skill _skillStellar = null;

    public bool Jeagandoongi { get { return _jeagandoongi; } set { _jeagandoongi = value; } }
    public bool ChoopChoop { get { return _choopChoop; } set { _choopChoop = value; } }
    public bool Hinacopathy { get { return _hinachopathy; } set { _hinachopathy = value; } }
    public bool Stellar { get { return _stellar; } set { _stellar = value; } }
    public Skill SkillJeagandoongi { get { return _skillJeagandoongi; } set { _skillJeagandoongi = value; } }
    public Skill SkillChoopChoop { get { return _skillChoopchoop; } set { _skillChoopchoop = value; } }
    public Skill SkillHinachopathy { get { return _skillHinachopathy; } set { _skillHinachopathy = value; } }
    public Skill SkillStellar { get { return _skillStellar; } set { _skillStellar = value; } }
    
    //========================================================================
    public bool DoSkill { get { return _doSkill; } set { _doSkill = value; } }
    

    public SkillManager Skill { get { return _skill; } }

    public override float Hp
    {
        get
        {
            return _hp;
        }
        set
        {            
            //체력이 증가됐을 때
            if(value > _hp)
            {
                if (value > _maxHp)
                    value = _maxHp;                
            }
            else // 체력이 감소했을 때
            {
                if (DoSkill)
                    return;

                if (_hitDelay < 0.5f)
                    return;

                if (Jeagandoongi)
                {
                    int rand = UnityEngine.Random.Range(0, 100);

                    if (rand < (SkillJeagandoongi._count * 10))
                    {
                        Util.SetActivePlayOnEnable(SkillJeagandoongi.gameObject);
                        return;
                    }
                }

                SoundManager.Sound.PlaySFX(_playerHitSound);
                UIManager.UI.UIBattle.HitEffectShakeUI();
            }
            

            _hp = value;
            UpdateHp();

            if (_hp <= 0)
                _state = CreatureState.Die;
        }
    }
    public float Attack { get { return _attack; } set { _attack = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public int Critical { get { return _critical; } set { _critical = value; } }
    public CreatureState State { get { return _state; } set { _state = value; } }
    public bool Died { get { return _died; } }
    
    public float EXP { 
        get 
        { 
            return _exp; 
        }
        set
        {
            float changeValue;
            if (value >= _maxEXP)
            {
                _exp = value - _maxEXP;
                changeValue = value - _maxEXP;
                _level++;
                _maxEXP = 100 + (20 * _level);
                // 레벨업 상자
                UIManager.UI.LoadPopupUI("PopUI_LevelUp");
            }
            else
            {
                _exp = value;
                changeValue = value;
            }

            UIManager.UI.UIBattle.ChangeEXPFill(changeValue,_maxEXP);
        }
    }

    public SpriteRenderer SRenderer { get { return _sr; } }

    public float SkillCoolTime { get { return _skillCoolTime; } }
    public float NextUseSkill { get { return _nextUseSkill; } set { _nextUseSkill = value; } }


    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        _level = 1;
        _maxEXP = 200 + (10 * _level);
        _mainCamera = Camera.main.GetComponent<CameraController>();
        _rigid = GetComponent<Rigidbody2D>();
        _hpBar = gameObject.Find<Transform>("Hp_bar", true);
        _skill = new SkillManager();
        _playerHitSound = Resources.Load<AudioClip>("Sound/SFX/Battle/PlayerHitSound");
    }

    private void OnDisable()
    {
        Refresh();
    }

    protected override void Start()
    {
        base.Start();
        if (SceneManagerEx.Scene.CharacterState == Define.CharacterState.Kanna)
        {            
            _playerSkill = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Skill/PlayerSkill/Kanna/PlayerSkill_Kanna"));
            _playerSkill.name = _playerSkill.name.Replace("(Clone)", "");
            _playerSkill.transform.SetParent(UIManager.UI.PopupRoot,false);            
            _playerSkill.SetActive(false);
            _skillCoolTime = 90f;
        }
        else if (SceneManagerEx.Scene.CharacterState == Define.CharacterState.Yuni)
        {
            _playerSkill = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Skill/PlayerSkill/Yuni/PlayerSkill_Yuni"));
            _playerSkill.name = _playerSkill.name.Replace("(Clone)", "");
            _playerSkill.transform.SetParent(transform, false);
            _playerSkill.SetActive(false);            
            _skillCoolTime = 60f;
        }
    }
    void Update()
    {
        if (DoSkill == true)
            return;

        _hitDelay += Time.deltaTime;
        UpdateController();
        UpdateSkillController();
    }

    public override void Init()
    {   
        base.Init();
        _hp *= DataManager.Data.NowData._hp;
        _attack *= DataManager.Data.NowData._attack;
        _speed *= DataManager.Data.NowData._speed;        
    }

    public override void UpdateController()
    {
        base.UpdateController();
        GetInput();
    }

    public override void UpdateIdle()
    {
        base.UpdateIdle();
        _animator.Play("Idle");
    }

    public override void UpdateMove()
    {
        base.UpdateMove();
        _animator.Play("Move");
    }

    public override void Die()
    {        
        UIManager.UI.LoadPopupUI("PopUI_EndGame");
        _state = CreatureState.None;
        _died = true;
        base.Die();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        _animator.Play("UseSkill");
    }


    public void GetInput()
    {
        if (Died == true)
            return;

        Vector2 moveDir = new Vector2();

        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector2.left;
            _sr.flipX = true;
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector2.right;
            _sr.flipX = false;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (DataManager.Data.NowStageData._playTime < _nextUseSkill)
                return;

            DoSkill = true;
            State = CreatureState.UseSkill;
            _playerSkill.SetActive(true);
            return;
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            if (UIManager.UI.Count() > 0)
                return;

            if (DoSkill)
                return;

            UIManager.UI.LoadPopupUI("PopUI_ESC");            
        }    

        if (moveDir == Vector2.zero)
            _state = CreatureState.Idle;
        else
            _state = CreatureState.Move;

        _rigid.position += moveDir.normalized * _speed * Time.deltaTime;        
    }

    public void UpdateHp()
    {        
        float value = Hp / MaxHp * 0.15f;

        if (value <= 0)
            _hpBar.localScale = new Vector3(0, _hpBar.localScale.y, _hpBar.localScale.z);
        else
            _hpBar.localScale = new Vector3(value, _hpBar.localScale.y, _hpBar.localScale.z);

        _hitDelay = 0f;
    }

    public void Refresh()
    {
        _instance = null;
    }

    public void UpdateSkillController()
    {
        if (Skill == null)
            return;

        Skill nowPlaySkill = Skill.UpdateSkill();

        if (nowPlaySkill == null)
            return;

        nowPlaySkill.gameObject.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == 1 << 13)
        {
            collision.GetComponent<GetDropItem>()._getItem = true;            
        }
    }
}
