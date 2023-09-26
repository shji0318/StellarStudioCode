using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager 
{
    PriorityQueue<Skill> _skillQueue = new PriorityQueue<Skill>();
    List<Skill> _registerSkill = new List<Skill>();
    Queue<ItemInfo> _registerSkillInfo = new Queue<ItemInfo>(); 
    Skill _nextSkill = null;

    public PriorityQueue<Skill> SkillQueue { get { return _skillQueue; } }    
    public Queue<ItemInfo> RegisterSkillInfo { get { return _registerSkillInfo; } }
    public Transform SkillRoot;

    public SkillManager()
    {
        if(SkillRoot == null)
        {
            GameObject root = GameObject.Find(string.Format(Util.objectFormat, "SKill"));

            if(root == null)
            {
                root = new GameObject() { name = string.Format(Util.objectFormat, "Skill") };
                root.transform.position = Vector3.zero;
            }
            SkillRoot = root.transform;
        }
    }

    public Skill UpdateSkill()
    {        
        return ActionSkill();
    }

    public Skill ActionSkill()
    {
        if (SceneManagerEx.Scene.NowSceneState != Define.SceneState.BattleScene)
            return null;

        if (_skillQueue.Count() == 0)
            return null;

        if (_nextSkill == null)
            _nextSkill = _skillQueue.Peek();

        if (_nextSkill != _skillQueue.Peek())
            _nextSkill = _skillQueue.Peek();

        if (DataManager.Data.NowStageData._playTime > _nextSkill._nextActionTime)
            return SkillQueue.Dequeue();

        return null;
    }

    public void SetSkill(string name, bool image = true)
    {
        GameObject skill = Resources.Load<GameObject>($"Prefabs/Skill/ActiveSkill/{name}");
        GameObject go = GameObject.Instantiate(skill);
        go.name = go.name.Replace("(Clone)", "");
        go.transform.SetParent(SkillRoot);
        _registerSkill.Add(go.GetComponent<Skill>());

        if(image)
            UIManager.UI.UIBattle.UpdateRegisterSkillImage();

        IBuffSkill buffSkill = go.GetComponent<IBuffSkill>();

        if (buffSkill == null)
            return;

        buffSkill.ImageObjectReceiver(UIManager.UI.UIBattle.GetBuffImageObject());
    }

    public void ChangeSkillDamage(string name, float damage)
    {
        foreach(Skill s in _registerSkill.FindAll(n => n._name == name))
            s.ChangeDamage(damage);
    }

    public void ChangeSkillCount(string name, float count)
    {
        foreach (Skill s in _registerSkill.FindAll(n => n._name == name))
            s.ChangeCount(count);
    }

    public void ChangeSkillDelay(string name, float delay)
    {
        foreach(Skill s in _registerSkill.FindAll(n => n._name == name))
            s.ChangeDelay(delay);
    }

    public void ChangeSkillHoldingTime (string name, float holdingTime)
    {
        foreach (Skill s in _registerSkill.FindAll(n => n._name == name))
            s.ChangeHoldingTime(holdingTime);
    }

    public void ChangeSkillScale (string name, float scale)
    {
        foreach (Skill s in _registerSkill.FindAll(n => n._name == name))
            s.ChangeScale(scale);
    }

    public void ChangeSkillSpeed (string name, float speed)
    {
        foreach (Skill s in _registerSkill.FindAll(n => n._name == name))
            s.ChangeSpeed(speed);
    }

    public Skill FindSkill(string name)
    {
        foreach (Skill s in _registerSkill.FindAll(n => n._name == name))
            return s;

        return null;
    }

    public T FindSkill<T>(string name) where T : Skill
    {
        foreach (Skill s in _registerSkill.FindAll(n => n._name == name))
            return s as T;

        return null;
    }
}
