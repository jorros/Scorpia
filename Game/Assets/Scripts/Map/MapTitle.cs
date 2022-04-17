using Actors;
using TMPro;
using UnityEngine;

namespace Map
{
    public class MapTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetLocation(Location location)
        {
            text.text = location.Name.Value.Value;
        }
    }
}