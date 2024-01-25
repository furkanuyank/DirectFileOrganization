internal class Program
{
    private static void Main(string[] args)
    {
        //sets mod, packing factor and random numbers that will be inserted
        Console.Write("Kaç adet rastgele eleman eklensin: ");
        int n = Convert.ToInt32(Console.ReadLine());
        (int mod, int packingFactor) = SetModAndPP(n);
        List<int> insertNumbers = SetInsertNumbers(n);

        //create objects of methods
        BEISCH beisch = new BEISCH(mod);
        ComputedChaining computedChaining = new ComputedChaining(mod);
        BinaryTree binaryTree = new BinaryTree(mod);


        Console.WriteLine("\nMod: {0}, Packing Factor: {1}/{2} => %{3}\n", mod, n, mod, packingFactor);

        //insert numbers that generated randomly in order to all 3 methods
        Console.Write("Inserted numbers in order: ");
        foreach (var item in insertNumbers)
        {
            Console.Write(item + ", ");
            beisch.Insert(item);
            computedChaining.Insert(item);
            binaryTree.Insert(item);
        }
        Console.WriteLine("\n");


        //Requirements 2
        beisch.Print();
        Console.WriteLine();
        computedChaining.Print();
        Console.WriteLine();
        binaryTree.Print();
        Console.WriteLine();


        //Requirements 3
        string row = String.Format("{0,4}{1,9}{2,18}{3,13}", "Key", "BEISCH", "ComputeChaining", "BinaryTree");
        Console.WriteLine(row);
        foreach (var item in insertNumbers)
        {
            row = String.Format("{0,4}{1,7}{2,13}{3,15}", item, beisch.GetProb(item), computedChaining.GetProb(item), binaryTree.GetProb(item));
            Console.WriteLine(row);
        }


        //Requirements 4
        Console.WriteLine("\nBEISCH -> {0:0.000}", beisch.AverageProbes(insertNumbers));
        Console.WriteLine("Compute Chaining -> {0:0.000}", computedChaining.AverageProbes(insertNumbers));
        Console.WriteLine("Binary Tree -> {0:0.000}\n", binaryTree.AverageProbes(insertNumbers));


        //Requirements 5
        int serachNumber = 17;
        beisch.Search(serachNumber);
        computedChaining.Search(serachNumber);
        binaryTree.Search(serachNumber);



        //Generate random numbers between 0 and 3n with control of duplicate for prevent to insert same numbers
        static List<int> SetInsertNumbers(int n)
        {
            Random rnd = new Random();
            List<int> list = new List<int>();
            int num;
            int i = 0;
            while (i < n)
            {
                num = rnd.Next(0, n * 3);
                if (!list.Contains(num))
                {
                    list.Add(num);
                    i++;
                }
            }
            return list;
        }

        //Adjust the mode value with a packing factor between 65% and 95%
        static (int mod, int packingFactor) SetModAndPP(int n)
        {
            if (n == 7 || n <= 3)
                return (11, (n * 100) / 11);

            int mod = FindFirstPrime(n);
            int packingFactor = (n * 100) / mod;
            while (!(packingFactor > 65 && packingFactor < 95))
            {
                mod = FindFirstPrime(mod);
                packingFactor = (n * 100) / mod;
            }
            return (mod, packingFactor);
        }
        //Heleper function for SetNodAndPP
        static int FindFirstPrime(int start)
        {
            int prime = start + 1;
            while (!IsPrime(prime))
            {
                prime++;
            }
            return prime;
        }
        //Helper function for FindFirstPrime
        static bool IsPrime(int number)
        {
            if (number < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
    }
}


