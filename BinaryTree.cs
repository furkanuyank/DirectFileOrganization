using static BT;

//table Row for Binary Tree Table
class BinaryTreeTableRow
{
    public int? Key { get; set; }

    public BinaryTreeTableRow()
    {
        Key = null;
    }
}

class BinaryTree
{
    private BinaryTreeTableRow[] HashTable { get; set; }
    private int ModValue;

    //fills all table rows as null
    public BinaryTree(int modValue)
    {
        ModValue = modValue;
        HashTable = new BinaryTreeTableRow[modValue];
        for (int i = 0; i < modValue; i++)
        {
            HashTable[i] = new BinaryTreeTableRow();
        }
    }

    //hash function
    private int Hash(int key)
    {
        return key % ModValue;
    }
    //quotient function, if key is smaller than mod value, it returns key to prevent infinite loop
    private int Quotient(int key)
    {
        if (key < ModValue)
        {
            return key;
        }
        else
        {
            return key / ModValue % ModValue;
        }
    }

    //inserts key to table
    public void Insert(int key)
    {
        int hashedIndex = Hash(key);

        //if hashed index is empty, insert key
        if (HashTable[hashedIndex].Key == null)
        {
            HashTable[hashedIndex].Key = key;
            return;
        }
        //if hashed index is not empty, find space for key
        else
        {
            //create tree that uses for insertion in binary tree method
            //get the path between root and first found empty node
            BT tree = new BT(ModValue);
            List<BTNode> path = tree.CreateTree(HashTable, key);
            path.Reverse();

            int tempMovedKey = key;
            int tempMovedIndex = path[0].Index.Value;

            //move the keys that needed to move
            int i = 0;
            while (i < path.Count - 1)
            {
                if (path[i].IsMove.Value)
                {
                    HashTable[tempMovedIndex].Key = HashTable[path[i + 1].Index.Value].Key;
                    tempMovedIndex = path[i + 1].Index.Value;
                }
                else
                {
                    tempMovedIndex = path[i].Index.Value;
                    while (path[i + 1].IsMove != null)
                    {
                        if (path[i + 1].IsMove.Value)
                        {
                            HashTable[tempMovedIndex].Key = HashTable[path[i + 1].Index.Value].Key;
                            break;
                        }
                        i++;
                    }
                }
                i++;
            }
            HashTable[tempMovedIndex].Key = tempMovedKey;
        }
    }

    //returns index of key (with Linear Quotient method)
    private int GetIndex(int key)
    {
        int hashedIndex = Hash(key);
        if (HashTable[hashedIndex].Key == null)
        {
            return -1;
        }
        else if (HashTable[hashedIndex].Key == key)
        {
            return hashedIndex;
        }
        else
        {
            int quotient = Quotient(key);
            int pointer = (hashedIndex + quotient) % ModValue;
            while (pointer != hashedIndex)
            {
                if (HashTable[pointer].Key == null)
                {
                    pointer = (pointer + quotient) % ModValue;
                }
                else
                {
                    if (HashTable[pointer].Key.Value == key)
                    {
                        return pointer;
                    }
                    pointer = (pointer + quotient) % ModValue;
                }
            }
        }
        return -1;
    }
    //searches key in table
    public void Search(int key)
    {
        int index = GetIndex(key);
        if (index != -1)
        {
            Console.WriteLine("Key {0} is found on index {1}", key, index);
            return;
        }
        Console.WriteLine("Key {0} is not found", key);
    }

    //returns number of probes for key (with Linear Quotient method)
    public int GetProb(int key)
    {
        int hashedIndex = Hash(key);
        if (HashTable[hashedIndex].Key == null)
        {
            return 0;
        }
        else if (HashTable[hashedIndex].Key == key)
        {
            return 1;
        }
        else
        {
            int count = 1;
            int quotient = Quotient(key);
            int pointer = (hashedIndex + quotient) % ModValue;
            while (pointer != hashedIndex)
            {
                if (HashTable[pointer].Key == null)
                {
                    count++;
                    pointer = (pointer + quotient) % ModValue;
                }
                else
                {
                    count++;
                    if (HashTable[pointer].Key.Value == key)
                    {
                        return count;
                    }
                    pointer = (pointer + quotient) % ModValue;
                }
            }
        }
        return 0;
    }
    //returns average number of probes for all keys
    public double AverageProbes(List<int> insertNumbers)
    {
        double totalProbes = 0;
        foreach (var item in insertNumbers)
        {
            totalProbes += GetProb(item);
        }
        return totalProbes / insertNumbers.Count;
    }

    //prints table
    public void Print()
    {
        Console.WriteLine("--Binary Tree--");
        string row = string.Format("{0,0}{1,8}", "Index", "Key");
        Console.WriteLine(row);
        for (int i = 0; i < HashTable.Length; i++)
        {
            row = string.Format("{0,3}{1,10}", i, HashTable[i].Key);
            Console.WriteLine(row);
        }
    }
}

//Binary Tree class for tree creation to insert key when collision occurs
class BT
{
    //Node class for Binary Tree
    public class BTNode
    {
        public int? Index;
        public BTNode Left, Right, Parent;
        public bool? IsMove;

        public BTNode()
        {
            Index = null;
            Left = Right = Parent = null;
            IsMove = null;
        }
    }

    public BTNode Root;
    public int ModValue;
    public BT(int modValue)
    {
        Root = null;
        ModValue = modValue;
    }
    private int Quotient(int key)
    {
        if (key<ModValue)
        {
            return key;
        }
        else
        {
            return key / ModValue % ModValue;
        }  
    }

    //inserts node to the binary tree with Breadth First approach
    //inserts according to rules of binary tree hashing method
    //returns null if the node that inserted is not empty
    //return itself if the node that inserted is empty
    private BTNode InsertNode(BinaryTreeTableRow[] table, int key)
    {
        if (Root == null)
        {
            Root = new BTNode();
            Root.Index = key % ModValue;
            if (table[key % ModValue].Key == null)
            {
                return Root;
            }
            return null;
        }

        Queue<BTNode> queue = new Queue<BTNode>();
        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            BTNode temp = queue.Dequeue();

            if (temp.Left == null)
            {
                temp.Left = new BTNode();
                temp.Left.Parent = temp;
                temp.Left.IsMove = false;

                BTNode iterator = temp.Left;
                bool isAllLeft = true;

                while (iterator.Parent.IsMove != null)
                {
                    if (iterator.Parent.IsMove == true)
                    {
                        isAllLeft = false;
                        break;
                    }
                    iterator = iterator.Parent;
                }

                int tempQuotient;
                if (isAllLeft)
                {
                    tempQuotient = Quotient(key);

                }
                else
                {
                    while (iterator.Parent.IsMove != null)
                    {

                        if (iterator.Parent.IsMove.Value)
                        {
                            iterator = iterator.Parent;
                            break;
                        }
                        iterator = iterator.Parent;

                    }
                    tempQuotient = Quotient(table[iterator.Parent.Index.Value].Key.Value);

                }
                temp.Left.Index = (tempQuotient + temp.Index) % ModValue;

                if (table[temp.Left.Index.Value].Key == null)
                {
                    return temp.Left;
                }
                break;
            }
            else
            {
                queue.Enqueue(temp.Left);
            }

            if (temp.Right == null)
            {
                temp.Right = new BTNode();
                temp.Right.Parent = temp;
                temp.Right.IsMove = true;

                int tempQuotient = Quotient(table[temp.Index.Value].Key.Value);
                temp.Right.Index = (tempQuotient + temp.Index) % ModValue;

                if (table[temp.Right.Index.Value].Key == null)
                {
                    return temp.Right;
                }
                break;
            }
            else
            {
                queue.Enqueue(temp.Right);
            }
        }
        return null;
    }
    //create tree and return the path between root and last inserted node
    public List<BTNode> CreateTree(BinaryTreeTableRow[] table, int data)
    {
        BTNode lastNode = InsertNode(table, data);
        while (lastNode == null)
        {
            lastNode = InsertNode(table, data);
        }
        List<BTNode> path = FindPath(lastNode.Index.Value);
        return path;
    }

    //finds path between root and target node
    private List<BTNode> FindPath(int target)
    {
        List<BTNode> path = new List<BTNode>();
        FindPathRecursive(Root, target, path);
        return path;
    }
    //helper function for FindPath
    private bool FindPathRecursive(BTNode current, int target, List<BTNode> path)
    {
        if (current == null)
        {
            return false;
        }
        path.Add(current);

        if (current.Index == target)
        {
            return true;
        }
        if (FindPathRecursive(current.Left, target, path) || FindPathRecursive(current.Right, target, path))
        {
            return true;
        }

        path.RemoveAt(path.Count - 1);
        return false;
    }
}






