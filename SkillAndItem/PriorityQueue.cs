using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : Skill
{
    int _count = 0;

    List<T> _list = new List<T>();

    public void Enqueue(T obj)
    {
        _list.Add(obj);
        _count++;

        int now = _count - 1;
        int next;

        while (now > 0) // 가중치가 낮을 수록 위로 올라오게 구현
        {
            next = (now - 1) / 2; // 부모 노드 인덱스

            if (_list[now]._nextActionTime > _list[next]._nextActionTime)
                break;

            T temp = _list[now];
            _list[now] = _list[next];
            _list[next] = temp;

            now = next;
        }
    }

    public T Dequeue()
    {
        T pop = _list[0];
        _list[0] = _list[_count - 1];
        _list.RemoveAt(_count - 1);
        _count--;

        #region 갱신
        int now = 0;
        int left;
        int right;
        while (true)
        {
            left = now * 2 + 1;
            right = now * 2 + 2;

            int next = now;

            if (left <= _count - 1 && _list[next]._nextActionTime > _list[left]._nextActionTime)
                next = left;
            if (right <= _count - 1 && _list[next]._nextActionTime > _list[right]._nextActionTime)
                next = right;

            if (now == next)
                break;

            T temp = _list[now];
            _list[now] = _list[next];
            _list[next] = temp;

            now = next;

        }
        #endregion

        return pop;
    }

    public void Remove(T element)
    {
        int idx = _list.FindIndex(x => x._nextActionTime == element._nextActionTime);
        _list[idx] = _list[_count - 1];
        _list.RemoveAt(_count - 1);
        _count--;

        #region 갱신
        int now = idx;
        int left;
        int right;
        while (true)
        {
            left = now * 2 + 1;
            right = now * 2 + 2;

            int next = now;

            if (left <= _count - 1 && _list[next]._nextActionTime > _list[left]._nextActionTime)
                next = left;
            if (right <= _count - 1 && _list[next]._nextActionTime > _list[right]._nextActionTime)
                next = right;

            if (now == next)
                break;

            T temp = _list[now];
            _list[now] = _list[next];
            _list[next] = temp;

            now = next;

        }
        #endregion
    }

    public void Remove(int idx) // 오버로딩 
    {
        _list[idx] = _list[_count - 1];
        _list.RemoveAt(_count - 1);
        _count--;

        #region 갱신
        int now = idx;
        int left;
        int right;
        while (true)
        {
            left = now * 2 + 1;
            right = now * 2 + 2;

            int next = now;

            if (left <= _count - 1 && _list[next]._nextActionTime > _list[left]._nextActionTime)
                next = left;
            if (right <= _count - 1 && _list[next]._nextActionTime > _list[right]._nextActionTime)
                next = right;

            if (now == next)
                break;

            T temp = _list[now];
            _list[now] = _list[next];
            _list[next] = temp;

            now = next;

        }
        #endregion
    }

    public int Find(T element, out int num)
    {
        int idx = _list.FindIndex(x => x._nextActionTime == element._nextActionTime);

        return num = idx;
    }

    public List<T> FindAllString(string element)
    {
        return _list.FindAll(n => n._name == element);
    }

    public T GetElement(int idx)
    {
        return _list[idx] as T;
    }

    public T Peek()
    {
        return _list[0];
    }
    public int Count()
    {
        return _count;
    }
}
