using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureController : MonoBehaviour
{
    //====================���� �� ������Ƽ==========================
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected CreatureState _state = CreatureState.None;
    protected SpriteRenderer _sr;

    //======================�ɷ�ġ=================    
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

    //���� ������ �����Ͽ� ���¿� ���� �ൿ ������ ����
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
