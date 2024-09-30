

//[MainScene]
//[ImageDisplayer "ImageDisplayer"]
//Name = "ImageDisplayer"


//using System.Reflection;
//
//namespace Snapster;
//
//public class Scene
//{
//    private string path;
//    private Dictionary<string, Node> nodesByName; // Dictionary to store nodes by name
//
//    public Scene(string path)
//    {
//        this.path = path;
//        nodesByName = new Dictionary<string, Node>(); // Initialize the dictionary
//    }
//
//    public T Instantiate<T>() where T : new()
//    {
//        T instance = new();
//        string[] fileLines = File.ReadAllLines(path);
//        object obj = null;
//        bool firstNode = true;
//
//        foreach (string line in fileLines)
//        {
//            // Check for type and name declaration
//            if (line.StartsWith('[') && line.EndsWith(']'))
//            {
//                string trimmedLine = line.Trim('[', ']');
//                string[] parts = trimmedLine.Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);
//
//                // The first part is the type name
//                string typeName = parts[0].Trim();
//                Type type = ResolveType(typeName);
//
//                if (type == null)
//                {
//                    throw new Exception($"Type '{typeName}' could not be found.");
//                }
//
//                obj = Activator.CreateInstance(type);
//
//                // If there's a second part, it's the name
//                if (parts.Length > 1)
//                {
//                    string nodeName = parts[1].Trim();
//                    // Assuming all nodes have a Name property, you can set it here
//                    SetValue(obj, "Name", nodeName);
//                }
//
//                if (firstNode)
//                {
//                    instance = (T)obj;
//                    firstNode = false;
//                }
//                else
//                {
//                    (instance as Node)?.AddChild(obj as Node, false);
//                }
//            }
//            // Handle other lines for properties
//            else if (line.Length > 0)
//            {
//                string[] tokens = line.Split(" = ");
//                string fieldName = tokens[0];
//                string value = tokens[1];
//
//                SetValue(obj, fieldName, value);
//            }
//        }
//
//        foreach (Node child in (instance as Node).Children)
//        {
//            child.Start();
//        }
//
//        return instance;
//    }
//
//    private Type ResolveType(string typeName)
//    {
//        // Get all loaded assemblies
//        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
//
//        foreach (var assembly in assemblies)
//        {
//            // Try to find the type in the current assembly
//            var type = assembly.GetType(typeName, false, true);
//            if (type != null)
//            {
//                return type;
//            }
//
//            // Additionally, you could try to find the type with a default namespace if applicable
//            var defaultNamespace = assembly.GetName().Name; // Get the default namespace
//            var namespacedTypeName = defaultNamespace + "." + typeName;
//            type = assembly.GetType(namespacedTypeName, false, true);
//            if (type != null)
//            {
//                return type;
//            }
//        }
//
//        // If no type found, return null
//        return null;
//    }
//
//    private static void SetValue(object obj, string name, object value)
//    {
//        string[] pathSegments = name.Split('/');
//        Type type = obj.GetType();
//        PropertyInfo propertyInfo = null;
//
//        for (int i = 0; i < pathSegments.Length; i++)
//        {
//            string segment = pathSegments[i];
//            propertyInfo = type.GetProperty(segment, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//
//            if (propertyInfo == null)
//            {
//                throw new Exception($"Property '{segment}' not found on type '{type.Name}'.");
//            }
//
//            if (i < pathSegments.Length - 1)
//            {
//                obj = propertyInfo.GetValue(obj);
//                type = obj.GetType();
//            }
//        }
//
//        if (propertyInfo != null && propertyInfo.CanWrite)
//        {
//            // Handle Vector2 property
//            if (propertyInfo.PropertyType == typeof(Vector2))
//            {
//                string[] tokens = value.ToString().Split(' ');
//
//                float x = float.Parse(tokens[0]);
//                float y = float.Parse(tokens[1]);
//
//                propertyInfo.SetValue(obj, new Vector2(x, y));
//                return;
//            }
//
//            // Handle Enum property
//            if (propertyInfo.PropertyType.IsEnum)
//            {
//                var enumValue = Enum.Parse(propertyInfo.PropertyType, value.ToString());
//                propertyInfo.SetValue(obj, enumValue);
//                return;
//            }
//
//            // Handle other property types
//            if (propertyInfo.PropertyType == typeof(int))
//            {
//                propertyInfo.SetValue(obj, int.Parse(value.ToString()));
//                return;
//            }
//
//            if (propertyInfo.PropertyType == typeof(float))
//            {
//                propertyInfo.SetValue(obj, float.Parse(value.ToString()));
//                return;
//            }
//
//            if (propertyInfo.PropertyType == typeof(double))
//            {
//                propertyInfo.SetValue(obj, double.Parse(value.ToString()));
//                return;
//            }
//
//            if (propertyInfo.PropertyType == typeof(bool))
//            {
//                propertyInfo.SetValue(obj, bool.Parse(value.ToString()));
//                return;
//            }
//
//            if (propertyInfo.PropertyType == typeof(string))
//            {
//                string stringValue = value.ToString();
//
//                if (stringValue.Length >= 2 && stringValue.StartsWith("\"") && stringValue.EndsWith("\""))
//                {
//                    stringValue = stringValue[1..^1];
//                }
//
//                propertyInfo.SetValue(obj, stringValue);
//                return;
//            }
//
//            propertyInfo.SetValue(obj, value);
//        }
//    }
//}