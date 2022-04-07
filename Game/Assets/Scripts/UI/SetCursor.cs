using UnityEngine;

namespace UI
{
    public class SetCursor : MonoBehaviour
    {
        public Texture2D cursor;

        private void Start()
        {
            Cursor.SetCursor(cursor, new Vector2(3, 3), CursorMode.Auto);
        }
    }
}

