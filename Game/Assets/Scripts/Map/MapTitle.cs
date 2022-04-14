using TMPro;
using UnityEngine;

namespace Map
{
    public class MapTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetLocation(MapLocation mapLocation)
        {
            text.text = mapLocation.Name.Value;
        }
    }
}