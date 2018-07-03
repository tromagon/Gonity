using UnityEngine;

namespace Gonity
{
    public static class TransformUtils
    {
        public static Transform RemoveAllChildren(this Transform transform)
        {
            var enumerator = transform.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GameObject.Destroy(((Transform)enumerator.Current).gameObject);
            }

            return transform;
        }
    }
}