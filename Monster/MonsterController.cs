using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MonsterController : CreatureController
{
    protected PlayerController _player;    
    protected Rigidbody2D _rigid;
    protected bool _died = false;

    
    public MonsterStatus _monsterStatusSO;

    protected MonsterStatus Status { get { return _monsterStatusSO; } }
    
    public override float Hp 
    { 
        get => 
            base.Hp;
        set
        {
            if (_died)
                return;
            int randCiritical = Random.Range(0, 101);

            float damage = (int)(_hp - value);

            if(randCiritical < PlayerController.Player.Critical)
            {
                damage *= 1.5f;
                FloatingDamageText(damage, transform.position,true);
                
                if (PlayerController.Player.Stellar && Random.Range(0,100) <26)
                    StartCoroutine(PlayerController.Player.SkillStellar.Action());
            }
            else
                FloatingDamageText(damage, transform.position,false);

            _hp = value;

            if (_hp <= 0)            
                _state = CreatureState.Die;                
            
        }
    }

    public virtual void OnEnable()
    {
        _died = false;
        _hp = Status.MaxHp * (1 + (Mathf.Floor(DataManager.Data.NowStageData._playTime / 60f) * 0.1f));
    }

    public override void Init()
    {
        base.Init();
        _rigid = GetComponent<Rigidbody2D>();
        _player = PlayerController.Player;
    }
    public virtual void Update()
    {
        if (_died == true)
            return; 

        UpdateController();
    }

    public override void UpdateController()
    {
        if (PlayerController.Player.DoSkill == true)
            return;

        base.UpdateController();
        UpdateDirection();
        UpdateReach();
    }
    public override void UpdateMove()
    {
        base.UpdateMove();        
        if (PlayerController.Player.DoSkill == true)
            return;

        Vector2 dir = (_player.transform.position - transform.position).normalized;

        _rigid.position += dir * Time.deltaTime * Status.Speed;
        
    }
    public virtual void UpdateReach()
    {
        if (_state == CreatureState.Die)
            return;

        if (PlayerController.Player.DoSkill == true)
            return;

        float distance = (_player.transform.position - transform.position).magnitude;
        
        //무한맵이기 때문에 일정 범위를 넘어가면 몬스터를 워프 시킴
        if(distance > 20f)
        {
            WarpMonster();
            return;
        }

        if (distance > Status.Reach)
            _state = CreatureState.Move;
        else
            _state = CreatureState.Idle;
    }

    public virtual void UpdateDirection()
    {
        if (PlayerController.Player.DoSkill == true)
            return;

        if (_player.transform.position.x > transform.position.x)
            _sr.flipX = false;
        else
            _sr.flipX = true;
    }

    public override void Die()
    {
        _died = true;
        DieEffective();
        DropItem();
        DataManager.Data.UpTookOutCount(Status.MonsterType);        
        _state = CreatureState.None;
        _hp = 99999f;
        _animator.Play("Die");       
    }

    public void DieEffective()
    {
        if (PlayerController.Player.ChoopChoop)
        {
            int rand = UnityEngine.Random.Range(0, 100);

            if (PlayerController.Player.SkillChoopChoop._count * 10 > rand)
                StartCoroutine(PlayerController.Player.SkillChoopChoop.Action());
        }

        if (PlayerController.Player.Hinacopathy)
        {
            int rand = UnityEngine.Random.Range(0, 100);

            if (PlayerController.Player.SkillHinachopathy._count * 10 > rand)
                Util.SetActivePlayOnEnable(PlayerController.Player.SkillHinachopathy.gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (PlayerController.Player.DoSkill == true)
            return;

        if (_died)
            return;

        if(1 << other.gameObject.layer == 1 << 6)
            _player.Hp -= Status.Attack * (1 + (Mathf.Floor(DataManager.Data.NowStageData._playTime / 120f) * 0.1f));
    }

    public void FloatingDamageText(float attack, Vector3 vec, bool critical)
    {
        EffectManager.Effect.PopFloatingDamage(attack, vec,critical);
    }

    public void DropItem()
    {
        float rand = Random.Range(0f, 100f);

        if(rand < 95)
        {
            GameObject go = Util.Instantiate(ItemManager.Item.GetItem("DropCoin"));
            go.transform.position = transform.position;
        }
        else if(rand <95.5)
        {
            GameObject go = Util.Instantiate(ItemManager.Item.GetItem("DropCoinMagnetic"));
            go.transform.position = transform.position;
        }
        else if (rand < 96)
        {
            GameObject go = Util.Instantiate(ItemManager.Item.GetItem("DropLoveLetter"));
            go.transform.position = transform.position;
        }
        else if (rand < 97)
        {
            GameObject go = Util.Instantiate(ItemManager.Item.GetItem("DropHP"));
            go.transform.position = transform.position;
        }
    }

    public void DestroyGameObject()
    {
        Util.Destroy(gameObject);
    }

    public void WarpMonster()
    {
        Vector3 randomVector = new Vector3();
        float norm = MapManager.Map.NowMapY;

        int degree = UnityEngine.Random.Range(-180, 181);
        float sin = Mathf.Sin(degree);
        float cos = Mathf.Cos(degree);
        randomVector = PlayerController.Player.transform.position + new Vector3(norm * cos, norm * sin, 0);

        transform.position = randomVector;
    }
}
