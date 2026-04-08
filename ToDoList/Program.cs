using ToDoList.Enums;
using ToDoList.Models;
var toDoList1 = new ToDoList.Models.ToDoList();
while(true)
{
    Console.WriteLine("\nWhat do you want to do?");
    Console.WriteLine("1. Add item in the List");
    Console.WriteLine("2. Remove item from the List");
    Console.WriteLine("3. Mark item as Completed");
    Console.WriteLine("4. Display all items in List");
    Console.WriteLine("5. Exit");
    var choice = Console.ReadLine();
    switch(choice)
    {
        case "1":
        {
      
        }
    }
}
void AddItemInList()
{
    Console.WriteLine("Enter title:");
    var title = Console.ReadLine();
    Console.WriteLine("Enter description (Optional, you can just press enter to continue):");
    var description = Console.ReadLine();
    Console.WriteLine("Enter enter date (format DD/MM/YYYY/hh:mm:ss):");
    var targetDayTime = DateTime.Parse(Console.ReadLine());
    Console.WriteLine("What type of item would you like to add?");
    Console.WriteLine("1. Call");
    Console.WriteLine("2. Meeting");
    Console.WriteLine("3. Day of Birth");
    Console.WriteLine("4. Task");
    var type = Console.ReadLine();
    switch (type)
    {
        case "1":
        {
            Console.WriteLine("Enter invited for the Call person(s)(Separate multiple persons by comma"); 
            var invitedPersons = Console.ReadLine().Split().ToList();
            toDoList1.AddItem(new Call());
        }
    }
/*var toDoList = new ToDoList.Models.ToDoList();
toDoList.AddItem(new Call(
    targetDayTime: new DateTime(2026, 04, 25, 12, 00,00),
    itemType: ToDoItemTypes.Call,
    description: "Weekly sync meeting",
    title: "Team Call",
    invitedPerson: new List<string>{"John Doe", "Jane Doe"},
    isCompleted: false
));
toDoList.AddItem(new Call(
    targetDayTime: new DateTime(2026, 04, 25, 11, 00,00),
    itemType: ToDoItemTypes.Call,
    description: "Weekly sync meeting",
    title: "Team Call Again",
    invitedPerson: new List<string>{"John Doe", "Jane Doe"},
    isCompleted: false
));
//toDoList.PrintAllItems();
Console.WriteLine("Before completing");
toDoList.PrintOnlyCompletedItems();
toDoList.CompleteItem("Team Call");
Console.WriteLine("After completing");
toDoList.PrintOnlyCompletedItems();

toDoList.PrintItemsByDateRange(new DateTime(2026, 3, 10), new DateTime(2026, 12, 20));
toDoList.PrintItemsBySpecificDate(new DateTime(2026, 4, 25));
/*toDoList.RemoveItem("Team Call");
//foreach (var entry in toDoList._items)
//{
  //  Console.WriteLine(entry);
}*/