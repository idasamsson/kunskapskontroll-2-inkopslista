// Holds the items and takes care of loading and saving them.
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.Marshalling;

class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;

    private int budgetLimit;

    public ShoppingList(string path, int budgetLimit)
    {
        this.path = path;
        this.budgetLimit = budgetLimit;
    }

    public void Add(Item item)
    {
        if (budgetLimit < Total() + item.Price)
        {
            throw new InvalidOperationException ("Varans pris spräcker budgettaket.");
        }
        
        items.Add(item);
             
    }

    // Removes the item the user sees as number 1, 2, 3 ...
    public void RemoveAt(int number)
    {
        if(number > items.Count || number < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Du måste skriva en siffra som finns i listan.");
        }

        items.RemoveAt(number - 1);
    }

    // Adds up the price of every item on the list.
    public int Total()
    {
        int sum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // Looks up an item by its name. Returns null if there is no such item.
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            if (String.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }
        }
        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total()} kr");
    }

    // Writes one item per line, as "price;name".
    public void Save()
    {
        List<string> lines = new List<string>();

        foreach (Item item in items)
        {
            lines.Add($"{item.Price};{item.Name}");
        }

        try
        {
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
            Console.WriteLine("Listan är sparad.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Listan kunde inte sparas, du har inte behörighet.");
        }
        catch (IOException)
        {
            Console.WriteLine("Listan kunde inte sparas.");
        }
    }

    // Reads the file back into the list.
    public void Load()
    {
        string[] lines;
        
        try
        {
            lines = File.ReadAllLines(path);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Det finns ingen lista än, lägg till en vara för att påbörja listan.");
            return;
        }

        foreach (string line in lines)
        {
            string[] parts = line.Split(';');
            
                if (parts.Length != 2)
                {
                    continue;
                }
                if (!int.TryParse(parts[0], out int price))
                {
                    Console.WriteLine("En rad hoppades över då priset är ogiltigt.");
                    continue;
                }
            
            items.Add(new Item(parts[1], price));
        }
    }
}
