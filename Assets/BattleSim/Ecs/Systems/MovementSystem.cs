using System.Collections.Generic;
using BattleSim.Config;
using BattleSim.Ecs.Components;
using BattleSim.Game;
using BattleSim.Game.GroundService;
using BattleSim.Game.PlayAreaBounds;
using BattleSim.Game.Repositories;
using UnityEngine;

namespace BattleSim.Ecs.Systems
{
    public sealed class MovementSystem : IEcsSystem
    {
        private readonly CombatSettingsSO _combatSettings;
        private readonly IGroundSnap _groundSnap;
        private readonly IUnitViewRegistry _viewRegistry;
        private readonly IPathBlockChecker _pathBlockChecker;
        private readonly IPlayAreaBounds _playAreaBounds;
        private readonly List<int> _livingEntityIds = new(64);
        private readonly List<Vector3> _livingPositions = new(64);
        private readonly List<float> _livingRadii = new(64);
        private readonly List<Vector3> _separationVectors = new(64);

        public MovementSystem
        (
            CombatSettingsSO combatSettings,
            IGroundSnap groundSnap,
            IUnitViewRegistry viewRegistry,
            IPathBlockChecker pathBlockChecker,
            IPlayAreaBounds playAreaBounds
        )
        {
            _combatSettings = combatSettings;
            _groundSnap = groundSnap;
            _viewRegistry = viewRegistry;
            _pathBlockChecker = pathBlockChecker;
            _playAreaBounds = playAreaBounds;
        }

        private void ApplyGroundAndBounds(ref Vector3 position)
        {
            position = _groundSnap.GetPositionOnGround(position);
            _playAreaBounds.ClampPosition(ref position);
        }

        public void Run(EcsWorld world, float deltaTime)
        {
            var unitPool = world.GetPool<UnitComponent>();
            var positionPool = world.GetPool<PositionComponent>();
            var targetPool = world.GetPool<TargetComponent>();
            var statsPool = world.GetPool<StatsComponent>();
            var boundsPool = world.GetPool<UnitBoundsComponent>();
            var statePool = world.GetPool<UnitStateComponent>();
            var waypointPool = world.GetPool<WaypointComponent>();
            var tacticPool = world.GetPool<UnitTacticComponent>();

            _livingEntityIds.Clear();
            _livingPositions.Clear();
            _livingRadii.Clear();

            foreach (var entityId in world.GetAllEntities())
            {
                if (!unitPool.Has(entityId) || !positionPool.Has(entityId) || !statsPool.Has(entityId))
                    continue;
                if (!boundsPool.Has(entityId) || !statePool.Has(entityId))
                    continue;

                ref var positionComponent = ref positionPool.Get(entityId);
                ref var statsComponent = ref statsPool.Get(entityId);
                if (statsComponent.Hp <= 0) continue;

                ref var boundsComponent = ref boundsPool.Get(entityId);
                ref var stateComponent = ref statePool.Get(entityId);
                var myRadius = boundsComponent.Radius;

                var targetEntityId = targetPool.Has(entityId) ? targetPool.Get(entityId).TargetEntityId : -1;

                if (targetEntityId < 0 || !world.HasEntity(targetEntityId) || !positionPool.Has(targetEntityId) || !boundsPool.Has(targetEntityId))
                {
                    stateComponent.State = MovementState.Idle;
                    ApplyGroundAndBounds(ref positionComponent.Value);
                    SyncView(entityId, positionComponent.Value);
                    _livingEntityIds.Add(entityId);
                    _livingPositions.Add(positionComponent.Value);
                    _livingRadii.Add(myRadius);
                    
                    continue;
                }

                var targetPosition = positionPool.Get(targetEntityId).Value;
                var targetRadius = boundsPool.Get(targetEntityId).Radius;
                var attackRange = myRadius + targetRadius + _combatSettings.AttackGap;
                var distanceToTarget = Vector3.Distance(positionComponent.Value, targetPosition);

                if (distanceToTarget <= attackRange)
                {
                    stateComponent.State = MovementState.InRange;
                    ApplyGroundAndBounds(ref positionComponent.Value);
                    SyncView(entityId, positionComponent.Value);
                    _livingEntityIds.Add(entityId);
                    _livingPositions.Add(positionComponent.Value);
                    _livingRadii.Add(myRadius);
                    
                    continue;
                }

                var isPathBlocked = _pathBlockChecker.IsPathBlocked(world, entityId, targetEntityId);
                var tactic = tacticPool.Has(entityId) ? tacticPool.Get(entityId).Tactic : UnitTactic.Flank;
                var canFlank = tactic == UnitTactic.Flank || tactic == UnitTactic.TakeAdvantage;

                if (isPathBlocked && canFlank && !waypointPool.Has(entityId))
                {
                    var dirToTarget = targetPosition - positionComponent.Value;
                    dirToTarget.y = 0f;

                    if (dirToTarget.sqrMagnitude > _combatSettings.MinimumDistanceEpsilon * _combatSettings.MinimumDistanceEpsilon)
                    {
                        dirToTarget.Normalize();
                        var perpendicular = Vector3.Cross(dirToTarget, Vector3.up).normalized;
                        var side = (entityId % 2 == 0) ? 1f : -1f;
                        var waypointPos = positionComponent.Value + perpendicular * side * _combatSettings.FlankOffsetDistance;
                        ApplyGroundAndBounds(ref waypointPos);
                        waypointPool.Add(entityId, new WaypointComponent { Value = waypointPos });
                    }
                }

                if (isPathBlocked && !canFlank)
                {
                    stateComponent.State = MovementState.Blocked;
                    ref var targetComponent = ref targetPool.Get(entityId);
                    targetComponent.TargetEntityId = -1;
                    ApplyGroundAndBounds(ref positionComponent.Value);
                    SyncView(entityId, positionComponent.Value);
                    _livingEntityIds.Add(entityId);
                    _livingPositions.Add(positionComponent.Value);
                    _livingRadii.Add(myRadius);
                    
                    continue;
                }

                Vector3 moveTarget;

                if (waypointPool.Has(entityId))
                {
                    var waypointPos = waypointPool.Get(entityId).Value;
                    var distToWaypoint = Vector3.Distance(positionComponent.Value, waypointPos);
                    var pathClear = !_pathBlockChecker.IsPathBlocked(world, entityId, targetEntityId);

                    if (distToWaypoint <= _combatSettings.WaypointReachedRadius || pathClear)
                    {
                        waypointPool.Remove(entityId);
                        moveTarget = targetPosition;
                    }
                    else
                        moveTarget = waypointPos;
                }
                else
                    moveTarget = targetPosition;

                stateComponent.State = MovementState.MovingToTarget;
                var directionToTarget = moveTarget - positionComponent.Value;
                directionToTarget.y = 0f;

                if (directionToTarget.sqrMagnitude > _combatSettings.MinimumDistanceEpsilon * _combatSettings.MinimumDistanceEpsilon)
                    directionToTarget.Normalize();
                else
                    directionToTarget = Vector3.zero;

                positionComponent.Value += directionToTarget * (statsComponent.Speed * deltaTime);
                ApplyGroundAndBounds(ref positionComponent.Value);
                SyncView(entityId, positionComponent.Value);
                _livingEntityIds.Add(entityId);
                _livingPositions.Add(positionComponent.Value);
                _livingRadii.Add(myRadius);
            }

            SeparationPass(positionPool);
            _livingEntityIds.Clear();
            _livingPositions.Clear();
            _livingRadii.Clear();
        }

        private void SeparationPass(EcsWorld.EcsPool<PositionComponent> positionPool)
        {
            var livingCount = _livingEntityIds.Count;

            if (livingCount == 0)
                return;

            _separationVectors.Clear();
            for (var index = 0; index < livingCount; index++)
                _separationVectors.Add(Vector3.zero);

            for (var index = 0; index < livingCount; index++)
            {
                var unitPosition = _livingPositions[index];
                var unitRadius = _livingRadii[index];
                var separationVector = Vector3.zero;

                for (var otherIndex = 0; otherIndex < livingCount; otherIndex++)
                {
                    if (index == otherIndex)
                        continue;

                    var otherUnitRadius = _livingRadii[otherIndex];
                    var minimumDistance = unitRadius + otherUnitRadius;
                    var distanceBetween = Vector3.Distance(unitPosition, _livingPositions[otherIndex]);

                    if (distanceBetween < minimumDistance && distanceBetween > _combatSettings.MinimumDistanceEpsilon)
                        separationVector += (unitPosition - _livingPositions[otherIndex]).normalized * (minimumDistance - distanceBetween);
                }

                _separationVectors[index] = separationVector;
            }

            for (var index = 0; index < livingCount; index++)
            {
                var entityId = _livingEntityIds[index];
                ref var positionComponent = ref positionPool.Get(entityId);
                var separation = _separationVectors[index];
                separation.y = 0f;
                positionComponent.Value += separation;
                ApplyGroundAndBounds(ref positionComponent.Value);
                SyncView(entityId, positionComponent.Value);
            }
        }

        private void SyncView(int entityId, Vector3 position)
        {
            if (_viewRegistry.TryGetView(entityId, out var view))
                view.SetPosition(position);
        }
    }
}