
//table Row for Computed Chaining Table
class ComputedChainingTableRow
{
    public int? Key { get; set; }
    public int? nof { get; set; }

    public ComputedChainingTableRow()
    {
        Key = null;
        nof = null;
    }
}

class ComputedChaining
{
    private ComputedChainingTableRow[] HashTable { get; set; }
    private int ModValue;

    //fills all table rows as null
    public ComputedChaining(int modValue)
    {
        ModValue = modValue;
        HashTable = new ComputedChainingTableRow[modValue];
        for (int i = 0; i < modValue; i++)
        {
            HashTable[i] = new ComputedChainingTableRow();
        }
    }

    //hash function
    private int Hash(int key)
    {
        return key % ModValue;
    }
    //quotient function, if quotient is 0, it returns 1 to prevent infinite loop
    private int Quotient(int key)
    {
        int quotient = key / ModValue % ModValue == 0 ? 1 : key / ModValue % ModValue;
        return quotient;
    }

    //finds space for key
    private void FindSpace(int key)
    {
        int hashedIndex = Hash(key);

        //find first key that nof column is null
        int pointer = hashedIndex;
        while (HashTable[pointer].nof != null)
        {
            int tempNof = HashTable[pointer].nof.Value;
            int moveSize = tempNof * Quotient(HashTable[pointer].Key.Value);

            pointer = (pointer + moveSize) % ModValue;
        }

        //find space for key then write nof of keys that found above
        int count = 1;
        int offset = Quotient(HashTable[pointer].Key.Value);
        int iterator = pointer;
        iterator = (iterator + offset) % ModValue;
        while (iterator != pointer)
        {
            if (HashTable[iterator].Key == null)
            {
                HashTable[iterator].Key = key;
                break;
            }
            else
            {
                count++;
                iterator = (iterator + offset) % ModValue;
            }
        }
        HashTable[pointer].nof = count;
    }
    //inserts key to table
    public void Insert(int key)
    {
        int hashedIndex = Hash(key);

        //if table row is null, insert key
        if (HashTable[hashedIndex].Key == null)
        {
            HashTable[hashedIndex].Key = key;
            return;
        }
        //if collision occurs
        else
        {
            //if the key at the location of the collison is its own home adress
            if (Hash(HashTable[hashedIndex].Key.Value) == hashedIndex)
            {
                FindSpace(key);
            }
            //if the key at the location of the collison is not its own home adress
            else
            {
                //get chain of key that place in the collison occurs
                List<int> keyChain = new List<int>();
                List<int> indexChain = new List<int>();

                int pointer = Hash(HashTable[hashedIndex].Key.Value);
                int tempNof;
                int moveSize;
                while (HashTable[pointer].nof != null)
                {
                    keyChain.Add(HashTable[pointer].Key.Value);
                    indexChain.Add(pointer);
                    tempNof = HashTable[pointer].nof.Value;
                    moveSize = tempNof * Quotient(HashTable[pointer].Key.Value);
                    pointer = (pointer + moveSize) % ModValue;
                }
                keyChain.Add(HashTable[pointer].Key.Value);
                indexChain.Add(pointer);

                //find the breakpoint of the chain
                int chainBreakpoint = indexChain.IndexOf(hashedIndex);
                //replace the key that came by parameter with the key that caused the collision
                HashTable[hashedIndex] = new ComputedChainingTableRow() { Key = key, nof = null };
                //delete the nof value of the key just before the breakpoint 
                HashTable[indexChain[chainBreakpoint - 1]].nof = null;

                //delete all rows in the chain after the breakpoint
                for (int i = chainBreakpoint + 1; i < indexChain.Count; i++)
                {
                    HashTable[indexChain[i]] = new ComputedChainingTableRow();
                }
                //find new space for all keys in the chain just deleted
                for (int i = chainBreakpoint; i < keyChain.Count; i++)
                {
                    FindSpace(keyChain[i]);
                }
            }
        }
    }

    //returns index of key
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
            while (HashTable[hashedIndex].nof != null)
            {

                int tempNof = HashTable[hashedIndex].nof.Value;
                int moveSize = tempNof * Quotient(HashTable[hashedIndex].Key.Value);

                hashedIndex = (hashedIndex + moveSize) % ModValue;
                if (HashTable[hashedIndex].Key == key)
                {
                    return hashedIndex;
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

    //returns number of probes for key
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
            while (HashTable[hashedIndex].nof != null)
            {
                int tempNof = HashTable[hashedIndex].nof.Value;
                int moveSize = tempNof * Quotient(HashTable[hashedIndex].Key.Value);

                hashedIndex = (hashedIndex + moveSize) % ModValue;
                count++;
                if (HashTable[hashedIndex].Key == key)
                {
                    return count;
                }
            }
        }
        return 0;
    }
    //returns average number of probes for all keys inserted
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
        Console.WriteLine("--Computed Chaining--");
        string row = string.Format("{0,0}{1,8}{2,8}", "Index", "Key", "nof");
        Console.WriteLine(row);
        for (int i = 0; i < HashTable.Length; i++)
        {
            row = string.Format("{0,3}{1,10}{2,7}", i, HashTable[i].Key, HashTable[i].nof);
            Console.WriteLine(row);
        }
    }
}
