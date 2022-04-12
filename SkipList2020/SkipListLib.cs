using System;
using System.Collections;
using System.Collections.Generic;

namespace SkipList2020
{
    public class SkipList<TKey,TValue>:
        IEnumerable<KeyValuePair<TKey,TValue>> 
        where TKey :IComparable<TKey>
    {
        Node<TKey, TValue>[] _head;
        readonly double _probability;
        readonly int _maxLevel;
        int _curLevel;
        Random _rd;
        public int Count { get; private set; }
        public SkipList(int maxLevel = 10, double p= 0.5)
        {
            _maxLevel = maxLevel;
            _probability = p;
            _head = new Node<TKey, TValue>[_maxLevel];
            //for (int i = 0; i < maxLevel; i++)
            //{
            //    _head[i] = new Node<TKey, TValue>();
            //    if (i == 0) continue;
            //    _head[i - 1].Up = _head[i];
            //    _head[i].Down = _head[i - 1];
            //}

            _curLevel = 0;
            _rd = new Random(DateTime.Now.Millisecond);
        }

        public void Add(TKey key, TValue value)
        {
            if (Count == 0)
            {
                _head[0] = new Node<TKey, TValue>(key, value);
                Count++;
                _curLevel++;
                return;
            }
            var prevNode = new Node<TKey, TValue>[_maxLevel];
            var currentNode = _head[_curLevel - 1];
            if (currentNode.Key.CompareTo(key) > 0)
            {
                for (int i = 0; i < _curLevel; i++)
                {
                    var node = new Node<TKey, TValue>(key, value) { Right = _head[i] };
                    if (i != 0)
                    {
                        node.Down = _head[i - 1];
                        _head[i - 1].Up = node;
                    }
                    _head[i] = node;
                }
                Count++;
                return;
            }
            for (int i = _curLevel - 1; i >= 0; i--)
            {
                while (currentNode.Right != null &&
                    currentNode.Right.Key.CompareTo(key) < 0)
                {
                    currentNode = currentNode.Right;
                }
                prevNode[i] = currentNode;
                if (currentNode.Down == null)
                    break;
                currentNode = currentNode.Down;
            }
            int level = 1;
            while (_rd.NextDouble() < _probability && level < _maxLevel - 1)
            {
                level++;
            }
            while (_curLevel < level)
            {
                _curLevel++;
                _head[_curLevel - 1] = new Node<TKey, TValue>() { Key = _head[_curLevel - 2].Key, Value = _head[_curLevel - 2].Value, Down = _head[_curLevel - 2] };
                _head[_curLevel - 2].Up = _head[_curLevel - 1];
                prevNode[_curLevel - 1] = _head[_curLevel - 1];
            }
            for (int i = 0; i < level; i++)
            {
                var node = new Node<TKey, TValue>(key, value) {Right = prevNode[i].Right};
                prevNode[i].Right = node;
                if (i == 0) continue;
                node.Down = prevNode[i - 1].Right;
                prevNode[i - 1].Right.Up = node;
            }
            Count++;
        }

        public bool Contains(TKey key)
        {
            var currentNode = _head[_curLevel - 1];
            for (int i = _curLevel - 1; i >= 0; i--)
            {
                while (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) < 0)
                {
                    currentNode = currentNode.Right;
                }
                if (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) == 0)
                {
                    return true;
                }
                currentNode = currentNode.Down;
            }
            return false;
        }

        public void Remove(TKey key)
        {
            var currentNode = _head[_curLevel - 1];
            if (currentNode.Key.CompareTo(key) == 0)
            {
                for (int i = 0; i < _curLevel; i++)
                {
                    if (i != 0 && _head[i - 1] != null && _head[i].Right == null)
                    {
                        _head[i] = new Node<TKey, TValue>(_head[i - 1].Key, _head[i - 1].Value) { Down = _head[i - 1] };
                        _head[i - 1].Up = _head[i];
                    }
                    else
                    {
                        _head[i] = _head[i].Right;
                    }
                }
                Count--;
                if (Count == 0)
                {
                    _curLevel = 0;
                }
                return;
            }
            var prevNode = new Node<TKey, TValue>[_maxLevel];
            for (int i = _curLevel - 1; i >= 0; i--)
            {
                while (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) < 0)
                {
                    currentNode = currentNode.Right;
                }
                if (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) == 0)
                {
                    prevNode[i] = currentNode;
                }
                currentNode = currentNode.Down;
            }
            if (prevNode[0] != null)
            {
                Count--;
            }
            for (int i = 0; prevNode[i] != null; i++)
            {
                prevNode[i].Right = prevNode[i].Right.Right;
            }
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for(var node = _head[0]; node.Right!=null; node=node.Right)
            {
                yield return new KeyValuePair<TKey,TValue>(node.Key, node.Value);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
