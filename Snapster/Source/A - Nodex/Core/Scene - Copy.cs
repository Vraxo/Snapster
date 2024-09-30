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
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine)) continue;

            if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
            {
                string content = trimmedLine[1..^1].Trim();
                string[] parts = content.Split(new[] { ' ' }, 4); // Handle up to 4 parts (with scene reference)

                string typeName = parts[0];
                string nodeName = ExtractQuotedString(parts[1]);
                string parentName = parts.Length >= 3 ? ExtractQuotedString(parts[2]) : null;

                // Check if it's a SceneReference
                if (typeName == "Scene" && parts.Length == 4)
                {
                    string scenePath = ExtractQuotedString(parts[3]);

                    // Recursively load the scene
                    Scene referencedScene = new Scene(scenePath);
                    var referencedRootNode = referencedScene.Instantiate<Node>(); // Assume the root is of type Node

                    // Set the name of the root node of the referenced scene
                    referencedRootNode.Name = nodeName;

                    if (parentName == null && firstNode)
                    {
                        instance = (T)(object)referencedRootNode; // Cast to T (since it's Node) and assign as the root
                        namedNodes[nodeName] = referencedRootNode;
                        firstNode = false;
                    }
                    else if (namedNodes.TryGetValue(parentName, out Node parentNode))
                    {
                        parentNode.AddChild(referencedRootNode, nodeName, false); // Add the referenced scene's root node as a child
                        namedNodes[nodeName] = referencedRootNode;
                    }
                    else
                    {
                        throw new Exception($"Parent node '{parentName}' not found for SceneReference.");
                    }
                }
                else
                {
                    // Normal node creation (existing behavior)
                    Type type = ResolveType(typeName);
                    obj = Activator.CreateInstance(type);

                    if (firstNode)
                    {
                        (obj as Node).Name = nodeName;
                        instance = (T)obj;
                        namedNodes[nodeName] = (Node)obj;
                        firstNode = false;
                    }
                    else
                    {
                        if (parentName == null) throw new Exception($"Node '{nodeName}' must specify a parent.");
                        if (namedNodes.TryGetValue(parentName, out Node parentNode))
                        {
                            parentNode.AddChild(obj as Node, nodeName, false);
                        }
                        else
                        {
                            throw new Exception($"Parent node '{parentName}' could not be found for node '{nodeName}'.");
                        }
                    }
                    namedNodes[nodeName] = (Node)obj;
                }
            }
            else if (trimmedLine.Contains(" = "))
            {
                int equalsIndex = trimmedLine.IndexOf(" = ");
                string fieldName = trimmedLine.Substring(0, equalsIndex).Trim();
                string value = trimmedLine.Substring(equalsIndex + 3).Trim();
                SetValue(obj, fieldName, value);
            }
        }

        foreach (Node child in (instance as Node).Children)
        {
            child.Start();
        }

        return instance;
    }

    private string ExtractQuotedString(string str)
    {
        if (str.Length >= 2 && str.StartsWith("\"") && str.EndsWith("\""))
        {
            return str[1..^1];
        }

        return str;
    }

    private Type ResolveType(string typeName)
    {
        // Get all loaded assemblies
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
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
            if (propertyInfo.PropertyType == typeof(Vector2))
            {
                propertyInfo.SetValue(obj, ParseVector2(value.ToString()));
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