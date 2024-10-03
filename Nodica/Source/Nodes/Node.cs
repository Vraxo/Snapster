namespace Nodica;

public class Node
{
    public string Name { get; set; } = "";
    public Node? Parent { get; set; } = null;
    public List<Node> Children { get; set; } = [];
    public bool Active { get; private set; } = true;

    public Node RootNode => App.Instance.RootNode;

    private bool started = false;

    // Public

    public virtual void Build() { }

    public virtual void Start() { }

    public virtual void Ready() { }

    public virtual void Update() { }

    public virtual void Destroy()
    {
        List<Node> childrenToDestroy = new(Children);

        foreach (Node child in childrenToDestroy)
        {
            child.Destroy();
        }

        Parent?.Children.Remove(this);
    }

    public void Process()
    {
        if (!Active)
        {
            return;
        }

        if (!started)
        {
            Ready();
            started = true;
        }

        Update();

        for (int i = 0; i < Children.Count; i++)
        {
            Children[i].Process();
        }
    }

    public void PrintChildren(string indent = "")
    {
        Console.WriteLine(indent + "-" + Name);

        foreach (var child in Children)
        {
            child.PrintChildren(indent + "-");
        }
    }

    // (De)Activation

    public virtual void Activate()
    {
        Active = true;

        foreach (Node child in Children)
        {
            child.Activate();
        }
    }

    public virtual void Deactivate()
    {
        Active = false;

        foreach (Node child in Children)
        {
            child.Deactivate();
        }
    }

    // Get special nodes

    public T? GetParent<T>() where T : Node
    {
        if (Parent != null)
        {
            return (T)Parent;
        }

        return (T)this;
    }

    // Get node from the root

    public T? GetNode<T>(string path) where T : Node
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        // Handle absolute path starting with /root
        if (path.StartsWith("/root"))
        {
            path = path.Substring("/root".Length);
            Node currentNode = App.Instance.RootNode;

            // RemoveItem leading slash for absolute paths
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            // Traverse the path
            if (!string.IsNullOrEmpty(path))
            {
                string[] nodeNames = path.Split('/');
                foreach (var name in nodeNames)
                {
                    currentNode = currentNode.GetChild(name);

                    if (currentNode == null)
                    {
                        return null;
                    }
                }
            }

            return currentNode as T;
        }
        else
        {
            // Handle relative path
            Node currentNode = this;

            string[] nodeNames = path.Split('/');
            foreach (var name in nodeNames)
            {
                if (name == "")
                {
                    return (currentNode as T);
                }

                currentNode = currentNode.GetChild(name);
                if (currentNode == null)
                {
                    return null;
                }
            }

            return currentNode as T;
        }
    }

    //public T? GetNode1000<T>(string path) where T : Node
    //{
    //    if (path == "")
    //    {
    //        return (T)App.Instance.RootNode;
    //    }
    //
    //    string[] nodeNames = path.Split('/');
    //
    //    Node currentNode = App.Instance.RootNode;
    //
    //    for (int i = 0; i < nodeNames.Length; i++)
    //    {
    //        currentNode = currentNode.GetChild(nodeNames[i]);
    //    }
    //
    //    return (T)currentNode;
    //}
    //
    //public T? GetNode<T>() where T : Node
    //{
    //    string typeName = typeof(T).Name;
    //    return GetNode1000<T>(typeName);
    //}
    //
    //public Node GetNode(string path)
    //{
    //    if (path == "")
    //    {
    //        return App.Instance.RootNode;
    //    }
    //
    //    string[] nodeNames = path.Split('/');
    //
    //    Node currentNode = Program.RootNode;
    //
    //    for (int i = 0; i < nodeNames.Length; i++)
    //    {
    //        currentNode = currentNode.GetChild(nodeNames[i]);
    //    }
    //
    //    return currentNode;
    //}

    // Get child

    public T? GetChild<T>(string name) where T : Node
    {
        foreach (Node child in Children)
        {
            if (child.Name == name)
            {
                return (T)child;
            }
        }

        return null;
    }

    public T? GetChild<T>() where T : Node
    {
        foreach (Node child in Children)
        {
            if (child.GetType() == typeof(T))
            {
                return (T)child;
            }
        }

        return null;
    }

    public Node? GetChild(string name)
    {
        foreach (Node child in Children)
        {
            if (child.Name == name)
            {
                return child;
            }
        }

        return null;
    }

    // AddItem child

    public void AddChild(Node node, string name, bool start = true)
    {
        node.Name = name;
        node.Parent = this;

        node.Build();

        if (start)
        {
            node.Start();
        }
        
        Children.Add(node);
    }

    public void AddChild(Node node, bool start = true)
    {
        node.Name = node.GetType().Name;
        node.Parent = this;
        
        node.Build();

        if (start)
        {
            node.Start();
        }
        
        Children.Add(node);
    }

    // Change scene

    public void ChangeScene(Node node)
    {
        //App.ResetView();
        App.Instance.RootNode.Destroy();
        App.Instance.RootNode = node;

        node.Name = node.GetType().Name;
    }
}