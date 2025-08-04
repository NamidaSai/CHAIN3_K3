using UnityEngine;

namespace Prototyping
{
    public class SpawnTest : MonoBehaviour
    {
        public void MoveTo(Transform target)
        {
            transform.position = target.position;
        }
    }
}
