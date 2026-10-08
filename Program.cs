using System.ComponentModel.Design;

ShoppingList list = new ShoppingList("items.txt", 100);
list.Load();

while (true)
{
    Console.WriteLine();
    list.Print();
    Console.WriteLine();
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Sök vara");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");

   if(!int.TryParse(Console.ReadLine(), out int choice) ||choice < 1 || choice > 5)
    {
        Console.WriteLine("Du måste skriva en siffra som finns i menyn.");
        continue;
    }

    if (choice == 1)
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine();
        Console.Write("Pris: ");
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("Du har skrivit in ett ogiltigt pris, varan har inte lagts till.");
            continue;
        }
        try
        {
            list.Add(new Item(name, price));
            Console.WriteLine("Varan är tillagd.");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Priset kan inte vara negativt.");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Namnet får inte vara tomt eller innehålla semikolon.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Du överskrider din budget med den här varan.");
        }
        
    }

    else if (choice == 2)
    {
        Console.Write("Nummer: ");
        if(!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Du måste skriva ett tal.");
            continue;
        }
         try
        {
            list.RemoveAt(number);  
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Siffran måste finnas i listan.");
            continue;
        }
    }

    else if (choice == 3)
    {
        list.Save();
    }
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: ");
        string wanted = Console.ReadLine().Trim();
        Item found = list.Find(wanted);

        if (found == null)
        {
            Console.WriteLine("Varan finns inte i listan.");
        }
        else
        {
            Console.WriteLine($"Hittade: {found}");
        }
    }
    else if (choice == 5)
    {
        break;
    }
}
