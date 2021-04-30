// Code taken from:
// https://forum.unity.com/threads/c-properties-instead-of-public-fields.246839/#post-1633120

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Primus.Core.Editor
{

    public class SerializeProperty : PropertyAttribute { }


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerializeProperty))]
    public class SerializablePropertyAttributeDrawer : PropertyDrawer
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var prevValue = GetInstance(property).ToArray();


            EditorGUI.BeginChangeCheck();


            EditorGUI.PropertyField(position, property, label, property.hasChildren);


            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();


                var instanceValues = GetInstance(property).ToArray();
                var instances = GetInstance(property, 1).ToArray();
                for (var i = 0; i < instances.Length; i++)
                {
                    var instanceVal = instanceValues[i];
                    var instance = instances[i];


                    if (instance == null)
                    {
                        Debug.Log("Instance not found");
                        return;
                    }


                    var field = GetField(instance.GetType(), property.name);


                    field.SetValue(instance, prevValue[i]);


                    var p = GetProperty(instance.GetType(), property.name.TrimStart('_'));


                    p.SetValue(instance, instanceVal, new object[0]);
                }
            }
        }


        static public FieldInfo GetField(Type type, string name)
        {
            var field = type.GetField(name, BINDING_FLAGS);
            if (field == null)
            {
                if (type.BaseType != typeof(object))
                    return GetField(type.BaseType, name);
            }
            return field;
        }


        static public PropertyInfo GetProperty(Type type, string name)
        {
            var field = type.GetProperty(name, BINDING_FLAGS);
            if (field == null)
            {
                if (type.BaseType != typeof(object))
                    return GetProperty(type.BaseType, name);
            }
            return field;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, property.hasChildren);
        }


        public IEnumerable<object> GetInstance(SerializedProperty property, int ignore = 0)
        {
            foreach (var targetObject in property.serializedObject.targetObjects)
            {
                var obj = targetObject;


                var path = property.propertyPath;


                path = path.Replace(".Array.data", "");
                var split = path.Split('.');
                var stack = split;

                // Check this
                if (stack.Length == 1 || ignore == 1)
                {
                    yield return obj;
                    continue;
                }


                object v = obj;
                try
                {
                    var i = stack.Length;
                    foreach (var name in stack)
                    {
                        if (i-- < ignore)
                            continue;


                        if (name.Contains("["))
                        {
                            var n = name.Split('[', ']');
                            v = getField(v, n[0], int.Parse(n[1]));
                        }
                        else
                            v = getField(v, name);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                yield return v;
            }
        }


        static private object getField(object obj, string field, int index = -1)
        {
            try
            {
                return index == -1 ? GetField(obj.GetType(), field).GetValue(obj) : (GetField(obj.GetType(), field).GetValue(obj) as IList)[index];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
#endif