﻿using System.Reflection;

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

        // Dictionary to hold references to nodes by their names
        var namedNodes = new Dictionary<string, Node>();

        foreach (string line in fileLines)
        {
            // Trim whitespace and check if the line is empty
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine))
                continue;

            // Check for class type declaration enclosed in brackets
            if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
            {
                // Remove brackets and split the line into components
                string content = trimmedLine[1..^1].Trim(); // Remove the brackets
                string[] parts = content.Split(new[] { ' ' }, 3); // Split into type, name, and optional parent

                if (parts.Length < 2 || parts.Length > 3)
                {
                    throw new Exception("Each node declaration must have a type and a name, and optionally a parent.");
                }

                string typeName = parts[0];         // Type
                string nodeName = ExtractQuotedString(parts[1]); // Node Name
                string parentName = parts.Length == 3 ? ExtractQuotedString(parts[2]) : null; // Parent Name (optional for first node)

                Type type = ResolveType(typeName); // Use the new method to resolve the type

                if (type == null)
                {
                    throw new Exception($"Type '{typeName}' could not be found.");
                }

                obj = Activator.CreateInstance(type);

                if (firstNode)
                {
                    // For the root node, set its name and add it to namedNodes
                    (obj as Node).Name = nodeName;
                    instance = (T)obj;
                    namedNodes[nodeName] = (Node)obj;
                    firstNode = false;
                }
                else
                {
                    // For subsequent nodes, check for parent name and add as a child
                    if (parentName == null)
                    {
                        throw new Exception($"Node '{nodeName}' must specify a parent.");
                    }

                    // Find the parent node by name
                    if (namedNodes.TryGetValue(parentName, out Node parentNode))
                    {
                        // Use the custom AddChild method with the name
                        parentNode.AddChild(obj as Node, nodeName, false);
                    }
                    else
                    {
                        throw new Exception($"Parent node '{parentName}' could not be found for node '{nodeName}'.");
                    }
                }

                // Add the node to the namedNodes dictionary
                namedNodes[nodeName] = (Node)obj;
            }
            else if (trimmedLine.Contains(" = "))
            {
                // Split only on the first occurrence of " = "
                int equalsIndex = trimmedLine.IndexOf(" = ");
                string fieldName = trimmedLine.Substring(0, equalsIndex).Trim();
                string value = trimmedLine.Substring(equalsIndex + 3).Trim();

                SetValue(obj, fieldName, value);
            }
        }

        // Start all child nodes of the root node
        foreach (Node child in (instance as Node).Children)
        {
            child.Start();
        }

        return instance;
    }

    private string ExtractQuotedString(string str)
    {
        // Check if the string is surrounded by quotes
        if (str.Length >= 2 && str.StartsWith("\"") && str.EndsWith("\""))
        {
            // Remove the quotes
            return str[1..^1];
        }

        // If it's not quoted, return as is (this shouldn't happen in the new format)
        return str;
    }

    private Type ResolveType(string typeName)
    {
        // Get all loaded assemblies
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            // Try to find the type in the current assembly
            var type = assembly.GetType(typeName, false, true);
            if (type != null)
            {
                return type;
            }

            // Try to find the type with a default namespace if applicable
            var defaultNamespace = assembly.GetName().Name; // Get the default namespace
            var namespacedTypeName = defaultNamespace + "." + typeName;
            type = assembly.GetType(namespacedTypeName, false, true);
            if (type != null)
            {
                return type;
            }
        }

        // If no type found, return null
        return null;
    }

    private void SetValue(object obj, string name, object value)
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
            // Handle Vector2 property in the format Vector2(x, y)
            if (propertyInfo.PropertyType == typeof(Vector2))
            {
                propertyInfo.SetValue(obj, ParseVector2(value.ToString()));
                return;
            }

            // Handle Enum property
            if (propertyInfo.PropertyType.IsEnum)
            {
                var enumValue = Enum.Parse(propertyInfo.PropertyType, value.ToString());
                propertyInfo.SetValue(obj, enumValue);
                return;
            }

            // Handle other property types
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

            // Reuse ExtractQuotedString for string values
            if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(obj, ExtractQuotedString(value.ToString()));
                return;
            }

            propertyInfo.SetValue(obj, value);
        }
    }

    private static Vector2 ParseVector2(string value)
    {
        string stringValue = value.Trim();

        if (stringValue.StartsWith("Vector2(") && stringValue.EndsWith(")"))
        {
            string vectorValues = stringValue.Substring(8, stringValue.Length - 9);
            string[] tokens = vectorValues.Split(',');

            if (tokens.Length == 2)
            {
                float x = float.Parse(tokens[0].Trim());
                float y = float.Parse(tokens[1].Trim());

                return new(x, y);
            }
            else
            {
                throw new Exception("Vector2 should contain exactly two numeric values.");
            }
        }
        else
        {
            throw new Exception($"Invalid Vector2 format, expected format: Vector2(x, y)");
        }
    }
}