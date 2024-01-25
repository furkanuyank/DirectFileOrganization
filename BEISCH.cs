
//table Row for BEISCH Table
class BEISCHTableRow
{
    public int? Record { get; set; }
    public int? Link { get; set; }

    public BEISCHTableRow()
    {
        Record = null;
        Link = null;
    }
}

class BEISCH
{
    private bool bottom = true;
    private BEISCHTableRow[] HashTable { get; set; }
    private int ModValue;

    //fills all table rows as null
    public BEISCH(int modValue)
    {
        ModValue = modValue;
        HashTable = new BEISCHTableRow[modValue];
        for (int i = 0; i < modValue; i++)
        {
            HashTable[i] = new BEISCHTableRow();
        }
    }

    //hash function
    private int Hash(int key)
    {
        return key % ModValue;
    }

    //inserts key to table
    public void Insert(int key)
    {
        int hashedIndex = Hash(key);
        int iterator = 0;
        //if table row is null, insert key
        if (HashTable[hashedIndex].Record == null)
        {
            HashTable[hashedIndex].Record = key;
            return;
        }
        //if table row is not null, insert key to bottom or top
        else if (HashTable[hashedIndex].Record != null)
        {
            //if bottom is true, insert key to bottom
            if (bottom)
            {
                bottom = false;
                iterator = HashTable.Length - 1;
                while (iterator >= 0)
                {
                    if (HashTable[iterator].Record == null)
                    {
                        HashTable[iterator].Record = key;
                        break;
                    }
                    else
                    {
                        iterator--;
                    }
                }

            }
            //if bottom is false, insert key to top
            else
            {
                bottom = true;
                iterator = 0;
                while (iterator <= HashTable.Length - 1)
                {
                    if (HashTable[iterator].Record == null)
                    {
                        HashTable[iterator].Record = key;
                        break;
                    }
                    else
                    {
                        iterator++;
                    }
                }
            }

            //set link for previous key
            int tempHashedIndex = hashedIndex;
            //if link is null, set link
            if (HashTable[tempHashedIndex].Link == null)
            {
                HashTable[tempHashedIndex].Link = iterator;
            }
            else
            {
                //if link is not null and link is equal to hashedIndex, set link
                if (Hash(HashTable[tempHashedIndex].Record.Value) == hashedIndex)
                {
                    int temp1 = HashTable[tempHashedIndex].Link.Value;
                    HashTable[iterator].Link = temp1;
                    HashTable[tempHashedIndex].Link = iterator;
                }
                //if link is not null and it is not chain of inserted key ,find last link and set link
                else
                {
                    while (HashTable[tempHashedIndex].Link != null)
                    {
                        tempHashedIndex = HashTable[tempHashedIndex].Link.Value;
                    }
                    HashTable[tempHashedIndex].Link = iterator;
                }
            }
        }
    }

    //returns index of key if it is exist in table
    private int GetIndex(int key)
    {
        int hashedIndex = Hash(key);
        if (HashTable[hashedIndex].Record == null)
        {
            return -1;
        }
        else if (HashTable[hashedIndex].Record == key)
        {
            return hashedIndex;
        }
        else
        {
            while (HashTable[hashedIndex].Link != null)
            {
                hashedIndex = HashTable[hashedIndex].Link.Value;
                if (HashTable[hashedIndex].Record == key)
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
        if (HashTable[hashedIndex].Record == null)
        {
            return 0;
        }
        else if (HashTable[hashedIndex].Record == key)
        {
            return 1;
        }
        else
        {
            int count = 1;
            while (HashTable[hashedIndex].Link != null)
            {
                hashedIndex = HashTable[hashedIndex].Link.Value;
                count++;
                if (HashTable[hashedIndex].Record == key)
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
        Console.WriteLine("{0,16}", "--BEISCH--");
        string row = string.Format("{0,0}{1,10}{2,7}", "Index", "Record", "Link");
        Console.WriteLine(row);
        for (int i = 0; i < HashTable.Length; i++)
        {
            row = string.Format("{0,3}{1,10}{2,7}", i, HashTable[i].Record, HashTable[i].Link);
            Console.WriteLine(row);
        }
    }
}
