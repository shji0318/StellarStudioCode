using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected List<SkillEffective> _list = new List<SkillEffective>();    
    
    public float _nextActionTime;
    public float _coolTime;
    public float _delay;
    public AudioClip _sfxSound;
    public float _damage;
    public string _name;
    public float _count;
    public float _holdingTime;
    public float _scale;

    public void Awake()
    {
        Init();
    }

    public virtual void OnEnable()
    {
        StartCoroutine(Action());
    }

    public virtual void OnDisable()
    {
        StopAllCoroutines();

        if (PlayerController.Player == null)
            return;

        _nextActionTime = DataManager.Data.NowStageData._playTime + _coolTime;
        PlayerController.Player.Skill.SkillQueue.Enqueue(this);
    }

    public abstract void Init();
    public abstract IEnumerator Action();
    
    public void SetNextActionTime(float time)
    {
        _nextActionTime = time;
    }

    public void ChangeDamage(float damage)
    {
        _damage *= (float)(1+(damage*0.01));

        foreach (SkillEffective s in _list)
            s.Damage = _damage;
    }

    public void ChangeCount (float count)
    {
        _count += count;
    }

    public void ChangeDelay (float delay)
    {
        _delay *= (100 - delay) / 100;

        foreach (SkillEffective s in _list)
            s.Delay = _delay;
    }

    public void ChangeHoldingTime(float holdingTime)
    {
        _holdingTime *= (1+(100 - holdingTime) / 100);        
    }

    public void ChangeScale(float scale)
    {
        _scale += (scale / 100);

        foreach (SkillEffective s in _list)
            s.Scale = _scale;
    }

    public void ChangeSpeed(float speed)
    {
        foreach (SkillEffective s in _list)
            s.Speed = speed;
    }

}
