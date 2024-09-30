using System.Reflection;

namespace Snapster;

public class Scene
{
    private string path;

    public Scene(string path)
    {
        this.path = path;
    }

    public T Instantiate<T>() where T : new()
    {
        T instance = new();

        string[] fileLines = File.ReadAllLines(path);

        object obj = null;

        bool firstNode = true;

        foreach (string line in fileLines)
        {
            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                string typeName = line.Substring(1, line.Length - 2).Trim();
                Type type = Type.GetType("Snapster." + typeName);
                obj = Activator.CreateInstance(type);

                if (firstNode)
                {
                    instance = (T?)obj;
                    firstNode = false;
                }
                else
                {
                    (instance as Node)?.AddChild(obj as Node, false);
                }
            }
            else if (line.Length > 0)
            {
                string[] tokens = line.Split(" = ");
                string fieldName = tokens[0];
                string value = tokens[1];

                SetValue(obj, fieldName, value);
            }
        }

        foreach (Node child in (instance as Node).Children)
        {
            child.Start();
        }

        return instance;
    }

    private static void SetValue(object obj, string name, object value)
    {
        string[] pathSegments = name.Split('/');
        Type type = obj.GetType();
        PropertyInfo propertyInfo = null;

        for (int i = 0; i < pathSegments.Length; i++)
        {
            string segment = pathSegments[i];
            propertyInfo = type.GetProperty(segment, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new Exception($"Property '{segment}' not found on type '{type.Name}'.");
            }

            if (i < pathSegments.Length - 1)
            {
                obj = propertyInfo.GetValue(obj);
                type = obj.GetType();
            }
        }

        if (propertyInfo != null && propertyInfo.CanWrite)
        {
            if (propertyInfo.PropertyType == typeof(Vector2))
            {
                string[] tokens = value.ToString().Split(' ');

                float x = float.Parse(tokens[0]);
                float y = float.Parse(tokens[1]);

                propertyInfo.SetValue(obj, new Vector2(x, y));
                return;
            }

            if (propertyInfo.PropertyType.IsEnum)
            {
                var enumValue = Enum.Parse(propertyInfo.PropertyType, value.ToString());
                propertyInfo.SetValue(obj, enumValue);
                return;
            }

            if (propertyInfo.PropertyType == typeof(int))
            {
                propertyInfo.SetValue(obj, int.Parse(value.ToString()));
                return;
            }

            if (propertyInfo.PropertyType == typeof(float))
            {
                propertyInfo.SetValue(obj, float.Parse(value.ToString()));
                return;
            }

            if (propertyInfo.PropertyType == typeof(double))
            {
                propertyInfo.SetValue(obj, double.Parse(value.ToString()));
                return;
            }

            if (propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(obj, bool.Parse(value.ToString()));
                return;
            }

            propertyInfo.SetValue(obj, value);
        }
    }
}