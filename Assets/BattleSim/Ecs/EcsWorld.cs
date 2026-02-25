using System;
using System.Collections.Generic;

namespace BattleSim.Ecs
{
    public sealed class EcsWorld
    {
        private int _nextEntityId = 1;
        private readonly List<int> _entities = new();
        private readonly Dictionary<Type, object> _pools = new();

        public EcsEntity NewEntity()
        {
            var id = _nextEntityId++;
            _entities.Add(id);
            return new EcsEntity(id, this);
        }

        public void RemoveEntity(int entityId)
        {
            foreach (var pool in _pools.Values)
                ((IEcsPool)pool).Remove(entityId);
            _entities.Remove(entityId);
        }

        public bool HasEntity(int entityId) => _entities.Contains(entityId);

        public EcsPool<T> GetPool<T>() where T : struct
        {
            var t = typeof(T);

            if (!_pools.TryGetValue(t, out var box))
            {
                box = new EcsPool<T>();
                _pools[t] = box;
            }

            return (EcsPool<T>)box;
        }

        public IEnumerable<int> GetAllEntities() => _entities;

        private interface IEcsPool
        {
            void Remove(int entityId);
        }

        public sealed class EcsPool<T> : IEcsPool where T : struct
        {
            private const int InitialCapacity = 64;
            private readonly Dictionary<int, int> _entityToIndex = new Dictionary<int, int>();
            private int[] _indexToEntity = new int[InitialCapacity];
            private T[] _components = new T[InitialCapacity];
            private int _count;

            public ref T Add(int entityId, in T component)
            {
                if (_entityToIndex.TryGetValue(entityId, out int idx))
                {
                    _components[idx] = component;
                    return ref _components[idx];
                }

                if (_count >= _components.Length)
                    Grow();

                idx = _count;
                _entityToIndex[entityId] = idx;
                _indexToEntity[idx] = entityId;
                _components[idx] = component;
                _count++;
                return ref _components[idx];
            }

            private void Grow()
            {
                int newCap = _components.Length * 2;
                var newEntities = new int[newCap];
                var newComponents = new T[newCap];
                Array.Copy(_indexToEntity, newEntities, _count);
                Array.Copy(_components, newComponents, _count);
                _indexToEntity = newEntities;
                _components = newComponents;
            }

            public void Remove(int entityId)
            {
                if (!_entityToIndex.TryGetValue(entityId, out int idx)) return;

                int lastIdx = _count - 1;

                if (idx != lastIdx)
                {
                    int lastEntityId = _indexToEntity[lastIdx];
                    _components[idx] = _components[lastIdx];
                    _indexToEntity[idx] = lastEntityId;
                    _entityToIndex[lastEntityId] = idx;
                }

                _entityToIndex.Remove(entityId);
                _count--;
            }

            public bool Has(int entityId) => _entityToIndex.ContainsKey(entityId);

            public ref T Get(int entityId) => ref _components[_entityToIndex[entityId]];

            public IEnumerable<(int entityId, T component)> GetAll()
            {
                for (int i = 0; i < _count; i++)
                    yield return (_indexToEntity[i], _components[i]);
            }
        }
    }
}