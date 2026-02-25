using BattleSim.Ecs.Components;
using UnityEngine;

namespace BattleSim.Presentation
{
    public sealed class UnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] private Renderer _meshRenderer;
        [SerializeField] private Transform _scaleRoot;

        private Transform _cachedTransform;
        private static readonly int s_baseColor = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void SetPosition(Vector3 position)
        {
            if (_cachedTransform == null) _cachedTransform = transform;
            _cachedTransform.position = position;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetScale(float scale)
        {
            if (_scaleRoot != null) _scaleRoot.localScale = Vector3.one * scale;
        }

        public void SetColor(Color color)
        {
            if (_meshRenderer == null)
                return;

            var block = new MaterialPropertyBlock();
            block.SetColor(s_baseColor, color);
            _meshRenderer.SetPropertyBlock(block);
        }
    }
}
