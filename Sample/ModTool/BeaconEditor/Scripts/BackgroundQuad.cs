using UnityEngine;
using Primus.Core;

namespace Primus.Sample.ModTool.BeaconEditor
{
    ///<summary>Assumes a Quad facing upwards, a rotation of (90,0,0).</summary>
    public class BackgroundQuad : MonoBehaviour
    {
        private float PositionLeft
        {
            get => transform.position.x - (transform.localScale.x / 2.0f);
        }
        private float PositionRight
        {
            get => transform.position.x + (transform.localScale.x / 2.0f);
        }
        private float PositionBackward
        {
            get => transform.position.z - (transform.localScale.y / 2.0f);
        }
        private float PositionForward
        {
            get => transform.position.z + (transform.localScale.y / 2.0f);
        }

        ///<summary>Clamps provided position into bacground rectangle's area.</summary>
        public Vector3 Clamp(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, PositionLeft, PositionRight);
            position.z = Mathf.Clamp(position.z, PositionBackward, PositionForward);
            return position;
        }
    }
}