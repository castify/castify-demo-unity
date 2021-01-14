using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CastifyDemo {
  public class UIController : MonoBehaviour {
    virtual protected void Awake()
    {
      var fields = GetType().GetFields(
        BindingFlags.DeclaredOnly | 
        BindingFlags.Public       | 
        BindingFlags.NonPublic    | 
        BindingFlags.Instance
      );
      foreach (var field in fields) {
        var a = field.GetCustomAttributes(typeof(ScenePathAttribute), false);
        if (a == null || a.Length == 0) {
          continue;
        }
        var e = a[0] as ScenePathAttribute;
        var obj = ResolvePathExpression(e.name, gameObject);
        if (obj == null) {
          Debug.LogError("Could not find a GameObject named '"+ e.name +"'.");
          continue;
        }
        var component = obj.GetComponent(field.FieldType);
        if (component == null) {
          Debug.LogError("The GameObject named '"+ e.name +"' has no component attached to it, which is of '"+ field.FieldType.Name +"'.");
          continue;
        }
        field.SetValue(this, component);
      }
    }
    static GameObject ResolvePathExpression(string path, GameObject root)
    {
      return path.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries).Aggregate(root, (acc, e) => FindFirstMatchedByPrefix(e, acc));
    }

    static GameObject FindFirstMatchedByPrefix(string path, GameObject root)
    {
      if (path.StartsWith("/")) {
        return GameObject.Find(path);
      }
      if (root == null) {
        return null;
      }
      var descendant = root.transform.Find(path);
      if (descendant != null) {
        return descendant.gameObject;
      }
      for (int i = 0; i < root.transform.childCount; ++i) {
        var obj = FindFirstMatchedByPrefix(path, root.transform.GetChild(i).gameObject);
        if (obj != null) {
          return obj;
        }
      }
      return null;
    }
  }
}
