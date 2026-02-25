namespace BattleSim.Ecs
{
    public readonly struct EcsEntity
    {
        public readonly int Id;
        private readonly EcsWorld _world;

        internal EcsEntity(int id, EcsWorld world)
        {
            Id = id;
            _world = world;
        }

        public EcsWorld.EcsPool<T> Pool<T>() where T : struct => _world.GetPool<T>();
        public bool Has<T>() where T : struct => _world.GetPool<T>().Has(Id);
        public ref T Get<T>() where T : struct => ref _world.GetPool<T>().Get(Id);
        public void Add<T>(in T c) where T : struct => _world.GetPool<T>().Add(Id, c);
    }
}