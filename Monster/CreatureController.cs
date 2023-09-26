using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureController : MonoBehaviour
{
    //====================변수 및 프로퍼티==========================
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected CreatureState _state = CreatureState.None;
    protected SpriteRenderer _sr;

    //======================능력치=================    
    [SerializeField]
    protected float _hp = 1000f;
    //=============================================
    
    public virtual float Hp 
    { 
        get 
        {
            return _hp; 
        } 
        set 
        {
            _hp = value;

            if (_hp <= 0)
                _state = CreatureState.Die;
        }  
    }
    //==============================================================
    public enum CreatureState
    {
        Idle,
        Move,
        Die,
        UseSkill,
        None,
    }

    protected virtual void Start()
    {
        Init();
    }
    public virtual void Init()
    {
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();        
    }    

    public virtual void UpdateController()
    {
        UpdateState();
    }

    //상태 패턴을 응용하여 상태에 따른 행동 로직을 관리
    public virtual void UpdateState()
    {        
        switch(_state)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Move:
                UpdateMove();
                break;
            case CreatureState.Die:
                Die();
                break;
            case CreatureState.UseSkill:
                UseSkill();
                break;
        }
    }

    public virtual void Die() { }
    public virtual void UpdateIdle() { }
    public virtual void UpdateMove() { }
    public virtual void UseSkill() { }

}
