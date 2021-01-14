using System;

namespace CastifyDemo
{    
  [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class ScenePathAttribute : System.Attribute {
    public readonly string name;

    public ScenePathAttribute(string name) 
    {
      this.name = name;
    }
  }
}
