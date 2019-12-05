using UnityEngine;

namespace InterfaceScripting {
    public class CursorSetter : MonoBehaviour
    {
        private Texture2D _texture;
        private Vector2 _hotspot = Vector2.one;
        private CursorMode _mode = CursorMode.Auto;

        public void SetTexture(Texture2D texture) => _texture = texture;
        public void SetHotspotX(float x) => _hotspot.x = x;
        public void SetHotspotY(float y) => _hotspot.y = y;
        public void SetCursorMode(CursorMode mode) => _mode = mode;

        public void SetCursor() => Cursor.SetCursor(_texture, _hotspot, _mode);
    }
}
