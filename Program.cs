
using System;

namespace UniversityManagementSystem
{
    // Requirement: Using enum for performance level[span_1](end_span)
    public enum PerformanceLevel
    {
        Failing = 0,
        Good = 1,
        VeryGood = 2,
        Excellent = 3
    }

   // Requirement: Doubly Linked List node for student data[span_2](end_span)
    public class StudentNode
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public string Province { get; set; }
        public double FirstExam { get; set; }
        public double SecondExam { get; set; }
        public double FinalResult { get; set; }
        public PerformanceLevel Rank { get; set; }

       // Pointers for Doubly Linked List[span_3](end_span)
        public StudentNode Next { get; set; }
        public StudentNode Previous { get; set; }

        public StudentNode(int id, string name, string city, double e1, double e2, PerformanceLevel level)
        {
            StudentID = id;
            FullName = name;
            Province = city;
            FirstExam = e1;
            SecondExam = e2;
          // Requirement: Calculation of average (e1+e2)/2[span_4](end_span)
            FinalResult = (e1 + e2) / 2;
            Rank = level;
        }

        public override string ToString()
        {
            return $"ID: {StudentID} | Name: {FullName} | Province: {Province} | Avg: {FinalResult} | Rank: {Rank}";
        }
    }

    public class StudentRegistry
    {
        public StudentNode Head { get; private set; }
        public StudentNode Tail { get; private set; }

       // Requirement: Add at start or end based on user choice[span_5](end_span)
        public void InsertRecord(StudentNode newNode, bool toFront)
        {
            if (Head == null)
            {
                Head = Tail = newNode;
                return;
            }

            if (toFront)
            {
                newNode.Next = Head;
                Head.Previous = newNode;
                Head = newNode;
            }
            else
            {
                Tail.Next = newNode;
                newNode.Previous = Tail;
                Tail = newNode;
            }
        }

      // Requirement: Sorting by Name (A-Z) or Score (Low-High)[span_6](end_span)
        public void Reorder(bool useName)
        {
            if (Head == null) return;
            bool isSwapped;
            do
            {
                isSwapped = false;
                StudentNode current = Head;
                while (current.Next != null)
                {
                    bool swapNeeded = useName
                        ? string.Compare(current.FullName, current.Next.FullName) > 0
                        : current.FinalResult > current.Next.FinalResult;

                    if (swapNeeded)
                    {
                        ExchangeValues(current, current.Next);
                        isSwapped = true;
                    }
                    current = current.Next;
                }
            } while (isSwapped);
        }

        private void ExchangeValues(StudentNode nodeA, StudentNode nodeB)
        {
            var tId = nodeA.StudentID; nodeA.StudentID = nodeB.StudentID; nodeB.StudentID = tId;
            var tName = nodeA.FullName; nodeA.FullName = nodeB.FullName; nodeB.FullName = tName;
            var tProv = nodeA.Province; nodeA.Province = nodeB.Province; nodeB.Province = tProv;
            var tRes = nodeA.FinalResult; nodeA.FinalResult = nodeB.FinalResult; nodeB.FinalResult = tRes;
            var tRnk = nodeA.Rank; nodeA.Rank = nodeB.Rank; nodeB.Rank = tRnk;
        }

      // Requirement: Recursive search for a specific grade[span_7](end_span)
public void SearchRecursive(StudentNode current, double target)
        {
            if (current == null) return;

            if (current.FinalResult == target)
                Console.WriteLine(">> Match Found: " + current);

            SearchRecursive(current.Next, target);
        }

       // Additional Requirement: Delete student by ID[span_8](end_span)
        public void DeleteNode(int id)
        {
            StudentNode current = Head;
            while (current != null)
            {
                if (current.StudentID == id)
                {
                    if (current.Previous != null) current.Previous.Next = current.Next;
                    else Head = current.Next;

                    if (current.Next != null) current.Next.Previous = current.Previous;
                    else Tail = current.Previous;

                    Console.WriteLine("Student record deleted.");
                    return;
                }
                current = current.Next;
            }
            Console.WriteLine("ID not found.");
        }

       // Additional Requirement: Display results > 85[span_9](end_span)
        public void DisplayHighAchievers()
        {
            StudentNode current = Head;
            Console.WriteLine("\n--- Students with Result > 85 ---");
            while (current != null)
            {
                if (current.FinalResult > 85) Console.WriteLine(current);
                current = current.Next;
            }
        }

        public void PrintAll()
        {
            StudentNode runner = Head;
            if (runner == null) Console.WriteLine("The list is empty.");
            while (runner != null)
            {
                Console.WriteLine(runner);
                runner = runner.Next;
            }
        }
    }

    class Program
    {
        static StudentRegistry masterRegistry = new StudentRegistry();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Academic Data System (Algorithms Lab) ===");
// Requirement: Initial input for 5 students[span_10](end_span)
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"\n--- Registering Student {i} ---");
                masterRegistry.InsertRecord(CollectData(), false);
            }

            bool isExit = false;
            while (!isExit)
            {
              // Requirement: User-friendly messages[span_11](end_span)
                Console.WriteLine("\nOPERATIONS MENU:");
                Console.WriteLine("1. Display All Records");
                Console.WriteLine("2. Sort by Name (A to Z)");
                Console.WriteLine("3. Sort by Result (Low to High)");
                Console.WriteLine("4. Recursive Search (by Grade)");
                Console.WriteLine("5. Add New Record");
                Console.WriteLine("6. Delete Record (by ID)");
                Console.WriteLine("7. Show Students Above 85");
                Console.WriteLine("8. Terminate Program");
                Console.Write("Action Number: ");

                switch (Console.ReadLine())
                {
                    case "1": masterRegistry.PrintAll(); break;
                    case "2": masterRegistry.Reorder(true); Console.WriteLine("Sorted by Name."); break;
                    case "3": masterRegistry.Reorder(false); Console.WriteLine("Sorted by Result."); break;
                    case "4":
                        Console.Write("Enter grade to search: ");
                        if (double.TryParse(Console.ReadLine(), out double score))
                            masterRegistry.SearchRecursive(masterRegistry.Head, score);
                        break;

                      
                        Console.Write("Position? (1: Start / 2: End): ");
                        bool start = Console.ReadLine() == "1";
                        masterRegistry.InsertRecord(CollectData(), start);
                        break;
                    case "6":
                        Console.Write("Enter ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int delId)) masterRegistry.DeleteNode(delId);
                        break;
                    case "7": masterRegistry.DisplayHighAchievers(); break;
                    case "8": isExit = true; break;
                    default: Console.WriteLine("Invalid selection."); break;
                }
            }
        }

        static StudentNode CollectData()
        {
            try
            {
                Console.Write("ID Number: "); int id = int.Parse(Console.ReadLine());
                Console.Write("Full Name: "); string name = Console.ReadLine();
                Console.Write("Province: "); string prov = Console.ReadLine();
                Console.Write("Exam 1 Score: "); double e1 = double.Parse(Console.ReadLine());
                Console.Write("Exam 2 Score: "); double e2 = double.Parse(Console.ReadLine());

              // Requirement: Enum selection[span_12](end_span)
                Console.WriteLine("Select Rank (0:Failing, 1:Good, 2:VeryGood, 3:Excellent):");
                PerformanceLevel level = (PerformanceLevel)int.Parse(Console.ReadLine());

                return new StudentNode(id, name, prov, e1, e2, level);
            }
            catch
            {
                Console.WriteLine("Input error. Creating default record.");
                return new StudentNode(0, "N/A", "N/A", 0, 0, PerformanceLevel.Failing);
            }
        }
    }
}