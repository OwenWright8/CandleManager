using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Linq;
using Spectre.Console;
using System.Data;
using Rule = Spectre.Console.Rule;

public class CandleScent
{
    public string Name { get; set; }
    public int LabelCount { get; set; }
    public int JarCount { get; set; }  // poured jars

    public CandleScent(string name)
    {
        Name = name;
        LabelCount = 0;
        JarCount = 0;
    }
}

public class CandleFragerence
{
    public string Name { get; set; }
    public double FragerenceWeight { get; set; }  // in lbs

    public CandleFragerence(string name)
    {
        Name = name;
        FragerenceWeight = 0;
    }
}

public class Supplies
{
    public double SoyWaxWeight { get; set; }  // in lbs
    public int JarCount { get; set; }
    public int JarLidCount { get; set; }
    public int WickCount { get; set; }
    public int WickStickerCount { get; set; }
    public int WarningLabelCount { get; set; }

    public Supplies()
    {
        SoyWaxWeight = 0;
        JarCount = 0;
        JarLidCount = 0;
        WickCount = 0;
        WickStickerCount = 0;
        WarningLabelCount = 0;
    }
}

public class InventoryData
{
    public List<CandleScent> Scents { get; set; }
    public List<CandleFragerence> Fragerences { get; set; }
    public Supplies Supplies { get; set; }
}

class Candles
{
    static List<CandleScent> scents = new List<CandleScent>
    {
        new CandleScent("Birthday Cake"),
        new CandleScent("Blueberry Swirl"),
        new CandleScent("Cafe Mocha"),
        new CandleScent("Cinnamon"),
        new CandleScent("Cinnamon Buns"),
        new CandleScent("Cinnamon Vanilla"),
        new CandleScent("Clean Cotton"),
        new CandleScent("Cookies for Santa"),
        new CandleScent("Creamy Vanilla"),
        new CandleScent("Cucumber Melon"),
        new CandleScent("Georgia Peach"),
        new CandleScent("Harvest"),
        new CandleScent("Hawaiian Paradise"),
        new CandleScent("Lavender"),
        new CandleScent("Lilac"),
        new CandleScent("Lovespell"),
        new CandleScent("Mistletoe"),
        new CandleScent("Pumpkin Spice"),
        new CandleScent("Spiced Cranberry"),
        new CandleScent("Vanilla Pumpkin Pie")
    };

    static List<CandleFragerence> fragerences = new()
    {
        new CandleFragerence("Birthday Cake"),
        new CandleFragerence("Blueberry Cobbler"),
        new CandleFragerence("Coffee"),
        new CandleFragerence("Chocolate"),
        new CandleFragerence("Caramel"),
        new CandleFragerence("Cinnamon"),
        new CandleFragerence("Cinnamon Buns"),
        new CandleFragerence("Clean Cotton"),
        new CandleFragerence("Santas Farts"),
        new CandleFragerence("Vanilla Bean"),
        new CandleFragerence("Cucumber Melon"),
        new CandleFragerence("Georgia Peach"),
        new CandleFragerence("Harvest"),
        new CandleFragerence("Pina Colada"),
        new CandleFragerence("Lavender"),
        new CandleFragerence("Lilac"),
        new CandleFragerence("Lovespell"),
        new CandleFragerence("Mistletoe"),
        new CandleFragerence("Pumpkin Spice"),
        new CandleFragerence("Spiced Cranberry"),
        new CandleFragerence("Vanilla Pumpkin Pie")
    };

    static Supplies supplies = new Supplies();

    static void Main()
    {
        static void SaveDataToFile()
        {
            string filePath = "inventory.json";
            try
            {
                InventoryData dataToSave = new InventoryData { Scents = scents, Fragerences = fragerences, Supplies = supplies };
                string jsonData = JsonSerializer.Serialize(dataToSave);
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("Data saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }


        static void LoadDataFromFile()
        {
            string filePath = "inventory.json";
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    InventoryData loadedData = JsonSerializer.Deserialize<InventoryData>(jsonData);
                    scents = loadedData.Scents;
                    fragerences = loadedData.Fragerences;
                    supplies = loadedData.Supplies;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        LoadDataFromFile();

        AnsiConsole.Write(
        new FigletText("Candle Manager")
        .Centered()
        .Color(Spectre.Console.Color.Yellow));

        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[springgreen2]Choose an option:[/]")
                    .AddChoices(new[] {
                            "Update Stock",
                            "Check Stock",
                            "Ordered Table",
                            "Fundraiser",
                            "Exit"
                    }));

            switch (choice)
            {
                case "Update Stock":
                    UpdateStock();
                    Console.Clear();
                    break;
                case "Check Stock":
                    CheckStock();
                    Console.Clear();
                    break;
                case "Ordered Table":
                    CheckOrderList();
                    Console.Clear();
                    break;
                case "Fundraiser":
                    Fundraiser(); //Work in progress
                    Console.Clear();
                    break;
                case "Exit":
                    return;
            }
        }
    }

    static void UpdateStock()
    {
        var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[springgreen2]Choose an option:[/]")
                    .AddChoices(new[] {
                            "Update Candle Stock",
                            "Update Fragerence Stock",
                            "Update Supply Stock",
                            "Back"
                    }));

        switch (choice)
        {
            case "Update Candle Stock":
                UpdateScentStock();
                Console.Clear();
                break;

            case "Update Fragerence Stock":
                UpdateFragerenceStock();
                Console.Clear();
                break;

            case "Update Supply Stock":
                UpdateSupplyStock();
                break;

            case "Back":
                Back();
                break;
        }
    }

    static void Back()
    {
        Console.Clear();
        Main();
    }

    static void UpdateScentStock()
    {
        bool continueUpdating;
        do
        {
            var choices = scents.Select((scent, index) => $"{index + 1}. {scent.Name}").ToList();

            var scentStock = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[Yellow]Choose a scent[/]")
                    .PageSize(100)
                    .AddChoices(choices)
            );

            static void SaveDataToFile()
            {
                string filePath = "inventory.json";
                try
                {
                    InventoryData dataToSave = new InventoryData { Scents = scents, Supplies = supplies };
                    string jsonData = JsonSerializer.Serialize(dataToSave);
                    File.WriteAllText(filePath, jsonData);
                    Console.WriteLine("Data saved successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving data: {ex.Message}");
                }
            }

            int choiceIndex = int.Parse(scentStock.Split('.')[0]) - 1;

            if (choiceIndex >= 0 && choiceIndex < scents.Count)
            {
                CandleScent selectedScent = scents[choiceIndex];

                Console.Clear();

                var rule = new Spectre.Console.Rule($"[bold darkgoldenrod]Current candle amount of {selectedScent.Name}: {selectedScent.JarCount} jars[/]");
                AnsiConsole.Write(rule);

                int addedCandles = AnsiConsole.Ask<int>("Enter added candles:");

                selectedScent.JarCount += addedCandles;

                Console.Clear();

                var ruleTwo = new Rule($"[bold darkgoldenrod]Current label amount of {selectedScent.Name}: {selectedScent.LabelCount} labels[/]");
                AnsiConsole.Write(ruleTwo);

                int addedLabels = AnsiConsole.Ask<int>("Enter added labels:");

                selectedScent.LabelCount += addedLabels;

                Console.Clear();

                var scentsTable = new Table();
                scentsTable.Border(TableBorder.Rounded);
                scentsTable.Centered();

                SaveDataToFile();

                Console.Clear();

                AnsiConsole.Live(scentsTable)
                .Start(ctx =>
                {
                    scentsTable.Title("Updated Candle Count");

                    scentsTable.AddColumn("Name");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    scentsTable.AddColumn("Poured Jars");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    scentsTable.AddColumn("Label Count");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    var labelCountColor = GetLabelCountColor(selectedScent.LabelCount);

                    scentsTable.AddRow(
                        selectedScent.Name,
                        selectedScent.JarCount.ToString(),
                        $"[{labelCountColor}]{selectedScent.LabelCount}[/]"
                    );

                    static string GetLabelCountColor(int labelCount)
                    {
                        if (labelCount <= 108) return "red";
                        if (labelCount <= 200) return "yellow";
                        return "green";
                    }
                });
            }
            else
            {
                AnsiConsole.WriteLine("Invalid choice.");
            }

            continueUpdating = AnsiConsole.Confirm("Do you want to enter another?");
            if (continueUpdating)
            {
                Console.Clear();
            }
        } while (continueUpdating);
        Console.Clear();
        Main();
    }

    static void UpdateFragerenceStock()
    {
        bool continueUpdating;

        do
        {
            var choices = fragerences.Select((fragerence, index) => $"{index + 1}. {fragerence.Name}").ToList();

            var fragerenceStock = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[Yellow]Choose a fragerence[/]")
                    .PageSize(100)
                    .AddChoices(choices)
            );

            static void SaveDataToFile()
            {
                string filePath = "inventory.json";
                try
                {
                    InventoryData dataToSave = new InventoryData { Scents = scents, Fragerences = fragerences, Supplies = supplies };
                    string jsonData = JsonSerializer.Serialize(dataToSave);
                    File.WriteAllText(filePath, jsonData);
                    Console.WriteLine("Data saved successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving data: {ex.Message}");
                }
            }

            int choiceIndex = int.Parse(fragerenceStock.Split('.')[0]) - 1;

            if (choiceIndex >= 0 && choiceIndex < scents.Count)
            {
                CandleFragerence selectedFragerence = fragerences[choiceIndex];

                Console.Clear();

                var rule = new Rule($"[bold darkgoldenrod]Current fragerence amount of {selectedFragerence.Name}: {selectedFragerence.FragerenceWeight} lbs[/]");
                AnsiConsole.Write(rule);

                int addedWeight = AnsiConsole.Ask<int>("Enter added weight:");

                selectedFragerence.FragerenceWeight += addedWeight;

                Console.Clear();

                var fragerencesTable = new Table();
                fragerencesTable.Border(TableBorder.Rounded);
                fragerencesTable.Centered();

                SaveDataToFile();

                Console.Clear();

                AnsiConsole.Live(fragerencesTable)
                .Start(ctx =>
                {
                    fragerencesTable.Title("Updated Fragerence Weight");

                    fragerencesTable.AddColumn("Name");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    fragerencesTable.AddColumn("Fragerence Weight");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    var scentWeightColor = GetScentWeightColor(selectedFragerence.FragerenceWeight);

                    fragerencesTable.AddRow(
                        selectedFragerence.Name,
                        $"[{scentWeightColor}]{selectedFragerence.FragerenceWeight}[/]"
                    );

                    static string GetScentWeightColor(double scentWeight)
                    {
                        if (scentWeight <= 10) return "red";
                        if (scentWeight <= 15) return "yellow";
                        return "green";
                    }
                });
            }
            else
            {
                AnsiConsole.WriteLine("Invalid choice.");
            }

            continueUpdating = AnsiConsole.Confirm("Do you want to enter another?");
            if (continueUpdating)
            {
                Console.Clear();
            }
        } while (continueUpdating);
        Console.Clear();
        Main();
    }

    static void UpdateSupplyStock()
    {
        bool continueUpdating;

        static void SaveDataToFile()
        {
            string filePath = "inventory.json";
            try
            {
                InventoryData dataToSave = new InventoryData { Scents = scents, Supplies = supplies };
                string jsonData = JsonSerializer.Serialize(dataToSave);
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("Data saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        do
        {
            var choices = new List<string>
        {
            "1. Soy Wax",
            "2. Jars",
            "3. Jar Lids",
            "4. Wicks",
            "5. Wick Stickers",
            "6. Warning Labels"
        };

            var selectedSupply = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[Yellow]Which supply would you like to update?[/]")
                    .PageSize(10)
                    .AddChoices(choices)
            );

            int choiceIndex = int.Parse(selectedSupply.Split('.')[0]) - 1;

            Console.Clear();

            int addedCount = 0;
            double addedWeight = 0;

            switch (choiceIndex)
            {
                case 0:
                    var ruleWax = new Rule($"[bold darkgoldenrod]Current weight of Soy Wax: {supplies.SoyWaxWeight} boxes[/]");
                    AnsiConsole.Write(ruleWax);
                    AnsiConsole.WriteLine("Enter added boxes:");
                    if (double.TryParse(Console.ReadLine(), out addedWeight))
                    {
                        supplies.SoyWaxWeight += addedWeight;
                        SaveDataToFile();
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Invalid amount entered.");
                    }
                    break;
                case 1:
                    var ruleJars = new Rule($"[bold darkgoldenrod]Current count of Jars: {supplies.JarCount}[/]");
                    AnsiConsole.Write(ruleJars);
                    AnsiConsole.WriteLine("Enter added count:");
                    if (int.TryParse(Console.ReadLine(), out addedCount))
                    {
                        supplies.JarCount += addedCount;
                        SaveDataToFile();
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Invalid count entered.");
                    }
                    break;
                case 2:
                    var ruleLids = new Rule($"[bold darkgoldenrod]Current count of Jar Lids: {supplies.JarLidCount} boxes[/]");
                    AnsiConsole.Write(ruleLids);
                    AnsiConsole.WriteLine("Enter added boxes:");
                    if (int.TryParse(Console.ReadLine(), out addedCount))
                    {
                        supplies.JarLidCount += addedCount;
                        SaveDataToFile();
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Invalid count entered.");
                    }
                    break;
                case 3:
                    var ruleWicks = new Rule($"[bold darkgoldenrod]Current count of Wicks: {supplies.WickCount}[/]");
                    AnsiConsole.Write(ruleWicks);
                    AnsiConsole.WriteLine("Enter added count:");
                    if (int.TryParse(Console.ReadLine(), out addedCount))
                    {
                        supplies.WickCount += addedCount;
                        SaveDataToFile();
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Invalid count entered.");
                    }
                    break;
                case 4:
                    var ruleStickers = new Rule($"[bold darkgoldenrod]Current count of Wick Stickers: {supplies.WickStickerCount}[/]");
                    AnsiConsole.Write(ruleStickers);
                    AnsiConsole.WriteLine("Enter added count:");
                    if (int.TryParse(Console.ReadLine(), out addedCount))
                    {
                        supplies.WickStickerCount += addedCount;
                        SaveDataToFile();
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Invalid count entered.");
                    }
                    break;
                case 5:
                    var ruleLabels = new Rule($"[bold darkgoldenrod]Current count of Warning Labels: {supplies.WarningLabelCount}[/]");
                    AnsiConsole.Write(ruleLabels);
                    AnsiConsole.WriteLine("Enter added count:");
                    if (int.TryParse(Console.ReadLine(), out addedCount))
                    {
                        supplies.WarningLabelCount += addedCount;
                        SaveDataToFile();
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Invalid count entered.");
                    }
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid choice. Returning to main menu.");
                    break;
            }

            Console.Clear();

            var suppliesTable = new Table();
            suppliesTable.Border(TableBorder.Rounded);
            suppliesTable.Centered();

            AnsiConsole.Live(suppliesTable)
                .Start(ctx =>
                {
                    suppliesTable.AddColumn("Supply");
                    suppliesTable.AddColumn("Count");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    var waxColor = GetWaxColor(supplies.SoyWaxWeight);
                    suppliesTable.AddRow("Soy Wax (Boxes)", $"[{waxColor}]{supplies.SoyWaxWeight}[/]");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    var jarColor = GetJarColor(supplies.JarCount);
                    suppliesTable.AddRow("Jars", $"[{jarColor}]{supplies.JarCount}[/]");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    var lidColor = GetLidColor(supplies.JarLidCount);
                    suppliesTable.AddRow("Jar Lids", $"[{lidColor}]{supplies.JarLidCount}[/]");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    var wickColor = GetWickColor(supplies.WickCount);
                    suppliesTable.AddRow("Wicks", $"[{wickColor}]{supplies.WickCount}[/]");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    var wickStickerColor = GetWickStickerColor(supplies.WickStickerCount);
                    suppliesTable.AddRow("Wick Stickers", $"[{wickStickerColor}]{supplies.WickStickerCount}[/]");
                    ctx.Refresh();
                    Thread.Sleep(50);

                    var warningLabelColor = GetWarningLabelColor(supplies.WarningLabelCount);
                    suppliesTable.AddRow("Warning Labels", $"[{warningLabelColor}]{supplies.WarningLabelCount}[/]");
                    ctx.Refresh();
                    Thread.Sleep(50);
                });

            static string GetLidColor(int jarLidCount)
            {
                return jarLidCount >= 2 ? "green" : "red";
            }

            static string GetWickStickerColor(int wickStickerCount)
            {
                if (wickStickerCount <= 200) return "red";
                if (wickStickerCount <= 300) return "yellow";
                return "green";
            }

            static string GetWaxColor(double soyWaxWeight)
            {
                if (soyWaxWeight <= 8) return "red";
                if (soyWaxWeight <= 12) return "yellow";
                return "green";
            }

            static string GetJarColor(int jarCount)
            {
                if (jarCount <= 32) return "red";
                if (jarCount <= 48) return "yellow";
                return "green";
            }

            static string GetWickColor(int wickCount)
            {
                if (wickCount <= 150) return "red";
                if (wickCount <= 300) return "yellow";
                return "green";
            }

            static string GetWarningLabelColor(int warningLabelCount)
            {
                if (warningLabelCount <= 150) return "red";
                if (warningLabelCount <= 200) return "yellow";
                return "green";
            }
            continueUpdating = AnsiConsole.Confirm("Do you want to enter another?");
            if (continueUpdating)
            {
                Console.Clear();
            }
        } while (continueUpdating);
        Console.Clear();
        Main();
    }

    public static void CheckStock()
    {

        var table = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Would you like to view candle scents or other stock?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see options)[/]")
                .AddChoices(new[] {
                    "Candle Stock",
                    "Fragerence Stock",
                    "Supply Stock",
                    "Back"
                }));

        switch (table)
        {
            case "Candle Stock":
                CandleScents();
                Console.Clear();
                break;

            case "Fragerence Stock":
                CandleFragerences();
                Console.Clear();
                break;

            case "Supply Stock":
                SupplyStock();
                Console.Clear();
                break;

            case "Back":
                Back();
                break;
        }

        static void CandleScents()
        {
            bool continueUpdating;
            do
            {
                var scentsTable = new Table();
                scentsTable.Border(TableBorder.Rounded);
                scentsTable.Centered();

                AnsiConsole.Live(scentsTable)
                    .Start(ctx =>
                    {
                        scentsTable.AddColumn("Scent");
                        scentsTable.AddColumn("Poured Jars");
                        scentsTable.AddColumn("Label Count");

                        foreach (var scent in scents)
                        {
                            var labelCountColor = GetLabelCountColor(scent.LabelCount);


                            scentsTable.AddRow(
                        scent.Name,
                        scent.JarCount.ToString(),
                        $"[{labelCountColor}]{scent.LabelCount}[/]"
                    );
                            ctx.Refresh();
                            Thread.Sleep(25);
                        }
                    });

                static string GetLabelCountColor(int labelCount)
                {
                    if (labelCount <= 108) return "red";
                    if (labelCount <= 200) return "yellow";
                    return "green";
                }
                continueUpdating = AnsiConsole.Confirm("Ready to go back to the menu?");
                if (continueUpdating)
                {
                    Console.Clear();
                    Main();
                }
            } while (continueUpdating);
            Console.Clear();
            Main();
        }

        static void CandleFragerences()
        {
            bool continueUpdating;
            do
            {
                var fragerencesTable = new Table();
                fragerencesTable.Border(TableBorder.Rounded);
                fragerencesTable.Centered();

                AnsiConsole.Live(fragerencesTable)
                    .Start(ctx =>
                    {
                        fragerencesTable.AddColumn("Name");
                        fragerencesTable.AddColumn("Fragerence Weight (lbs)");

                        foreach (var fragerence in fragerences)
                        {
                            var fragerenceWeightColor = GetFragerenceWeightColor(fragerence.FragerenceWeight);

                            fragerencesTable.AddRow(
                            fragerence.Name,
                            $"[{fragerenceWeightColor}]{fragerence.FragerenceWeight}[/]"
                    );
                            ctx.Refresh();
                            Thread.Sleep(25);
                        }
                    });

                static string GetFragerenceWeightColor(double fragerenceWeight)
                {
                    if (fragerenceWeight <= 10) return "red";
                    if (fragerenceWeight <= 15) return "yellow";
                    return "green";
                }

                continueUpdating = AnsiConsole.Confirm("Ready to go back to the menu?");
                if (continueUpdating)
                {
                    Console.Clear();
                    Main();
                }
            } while (continueUpdating);
            Console.Clear();
            Main();
        }

        static void SupplyStock()
        {
            bool continueUpdating;
            do
            {
                var suppliesTable = new Table();
                suppliesTable.Border(TableBorder.Rounded);
                suppliesTable.Centered();

                AnsiConsole.Live(suppliesTable)
                    .Start(ctx =>
                    {
                        suppliesTable.AddColumn("Supply");
                        suppliesTable.AddColumn("Count");
                        ctx.Refresh();
                        Thread.Sleep(50);

                        var waxColor = GetWaxColor(supplies.SoyWaxWeight);
                        suppliesTable.AddRow("Soy Wax (Boxes)", $"[{waxColor}]{supplies.SoyWaxWeight}[/]");
                        ctx.Refresh();
                        Thread.Sleep(50);

                        var jarColor = GetJarColor(supplies.JarCount);
                        suppliesTable.AddRow("Jars", $"[{jarColor}]{supplies.JarCount}[/]");
                        ctx.Refresh();
                        Thread.Sleep(50);

                        var lidColor = GetLidColor(supplies.JarLidCount);
                        suppliesTable.AddRow("Jar Lids", $"[{lidColor}]{supplies.JarLidCount}[/]");
                        ctx.Refresh();
                        Thread.Sleep(50);

                        var wickColor = GetWickColor(supplies.WickCount);
                        suppliesTable.AddRow("Wicks", $"[{wickColor}]{supplies.WickCount}[/]");
                        ctx.Refresh();
                        Thread.Sleep(50);

                        var wickStickerColor = GetWickStickerColor(supplies.WickStickerCount);
                        suppliesTable.AddRow("Wick Stickers", $"[{wickStickerColor}]{supplies.WickStickerCount}[/]");
                        ctx.Refresh();
                        Thread.Sleep(50);

                        var warningLabelColor = GetWarningLabelColor(supplies.WarningLabelCount);
                        suppliesTable.AddRow("Warning Labels", $"[{warningLabelColor}]{supplies.WarningLabelCount}[/]");
                        ctx.Refresh();
                        Thread.Sleep(50);
                    });


                static string GetLidColor(int jarLidCount)
                {
                    return jarLidCount >= 2 ? "green" : "red";
                }

                static string GetWickStickerColor(int wickStickerCount)
                {
                    if (wickStickerCount <= 200) return "red";
                    if (wickStickerCount <= 300) return "yellow";
                    return "green";
                }

                static string GetWaxColor(double soyWaxWeight)
                {
                    if (soyWaxWeight <= 8) return "red";
                    if (soyWaxWeight <= 12) return "yellow";
                    return "green";
                }

                static string GetJarColor(int jarCount)
                {
                    if (jarCount <= 32) return "red";
                    if (jarCount <= 48) return "yellow";
                    return "green";
                }

                static string GetWickColor(int wickCount)
                {
                    if (wickCount <= 150) return "red";
                    if (wickCount <= 300) return "yellow";
                    return "green";
                }

                static string GetWarningLabelColor(int warningLabelCount)
                {
                    if (warningLabelCount <= 150) return "red";
                    if (warningLabelCount <= 200) return "yellow";
                    return "green";
                }
                continueUpdating = AnsiConsole.Confirm("Ready to go back to the menu?");
                if (continueUpdating)
                {
                    Console.Clear();
                    Main();
                }
            } while (continueUpdating);
            Console.Clear();
            Main();
        }
    }

    public static void CheckOrderList()
    {
        var scentsTable = new Table();
        scentsTable.Border(TableBorder.Rounded);
        scentsTable.Centered();
        scentsTable.AddColumn(new TableColumn("Candles").Centered());
        scentsTable.AddColumn(new TableColumn("Labels").Centered());
        scentsTable.AddColumn(new TableColumn("Fragerences").Centered());

        var orderedByJarCount = scents.OrderBy(scent => scent.JarCount).ToList();
        var orderedByLabelCount = scents.OrderBy(scent => scent.LabelCount).ToList();
        var orderedByFragerenceWeight = fragerences.OrderBy(fragerences => fragerences.FragerenceWeight).ToList();

        int maxRows = Math.Max(orderedByJarCount.Count, Math.Max(orderedByLabelCount.Count, orderedByFragerenceWeight.Count));

        AnsiConsole.Live(scentsTable)
            .Start(ctx =>
            {
                for (int i = 0; i < maxRows; i++)
                {
                    var jarCountCell = i < orderedByJarCount.Count ? $"{orderedByJarCount[i].Name}: [{GetJarCountColor(orderedByJarCount[i].JarCount)}]{orderedByJarCount[i].JarCount}[/]" : string.Empty;
                    var labelCountCell = i < orderedByLabelCount.Count ? $"{orderedByLabelCount[i].Name}: [{GetLabelCountColor(orderedByLabelCount[i].LabelCount)}]{orderedByLabelCount[i].LabelCount}[/]" : string.Empty;
                    var fragerenceWeightCell = i < orderedByFragerenceWeight.Count ? $"{orderedByFragerenceWeight[i].Name}: [{GetFragerenceWeightColor(orderedByFragerenceWeight[i].FragerenceWeight)}]{orderedByFragerenceWeight[i].FragerenceWeight}[/]" : string.Empty;

                    scentsTable.AddRow(jarCountCell, labelCountCell, fragerenceWeightCell);
                    ctx.Refresh();
                    Thread.Sleep(50);
                }
            });
        AnsiConsole.WriteLine("Press Enter to go back to the menu...");
        while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter)
        {
            //Do nothing, just wait for Enter key
        }
        Console.Clear();
        Main();
    }

    static string GetJarCountColor(int jarCount)
    {
        if (jarCount <= 10) return "red";
        if (jarCount <= 20) return "yellow";
        return "green";
    }

    static string GetLabelCountColor(int labelCount)
    {
        if (labelCount <= 108) return "red";
        if (labelCount <= 200) return "yellow";
        return "green";
    }

    static string GetFragerenceWeightColor(double fragerenceWeight)
    {
        if (fragerenceWeight <= 10) return "red";
        if (fragerenceWeight <= 15) return "yellow";
        return "green";
    }

    static void Fundraiser()
    {
        var table = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an option.")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see options)[/]")
                .AddChoices(new[] {
                    "Add New Fundraiser",
                    "View Current Fundraisers",
                    "View Past Fundraisers",
                    "Back"
                }));

        switch (table)
        {
            case "Add New Fundraiser":
                AddNewFundraiser();
                Console.Clear();
                break;

            case "View Current Fundraisers":
                CurrentFundraiser();
                Console.Clear();
                break;

            case "View Past Fundraisers":
                PastFundraiser();
                Console.Clear();
                break;

            case "Back":
                Back();
                break;
        }
    }

    static void AddNewFundraiser()
    {
        var fundraiserType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select the type of fundraiser:")
                .AddChoices(new[] { "Person-based Fundraiser", "Candle-based Fundraiser" }));

        Console.Clear();

        string fundraiserName = AnsiConsole.Ask<string>("Enter the name of the fundraiser:");

        DateTime fundraiserDate = AnsiConsole.Ask<DateTime>("Enter the date of the fundraiser:");

        Console.Clear();

        if (fundraiserType == "Person-based Fundraiser")
        {
            Console.Clear();
            List<(string personName, Dictionary<string, int> candles)> personFundraiserDetails = new();

            while (true)
            {
                var personRule = new Rule("[bold darkgoldenrod]Enter the person's name, or if done enter done[/]");
                AnsiConsole.Write(personRule);
                string person = AnsiConsole.Ask<string>("Enter the person's name:");
                Console.Clear();
                if (person.ToLower() == "done")
                    break;

                Dictionary<string, int> candlesReceived = new Dictionary<string, int>();

                foreach (var scent in scents)
                {
                    var scentRule = new Rule($"[bold darkgoldenrod]Enter the number of {scent.Name} candles received for the fundraiser:[/]");
                    AnsiConsole.Write(scentRule);
                    int candleCount = AnsiConsole.Ask<int>("Number:");
                    candlesReceived[scent.Name] = candleCount;
                    Console.Clear();
                }

                DisplayCandleAmountsTable(candlesReceived);

                bool amountsCorrect = AnsiConsole.Confirm("Are the amounts correct for this person?");
                if (!amountsCorrect)
                {
                    continue;
                }

                personFundraiserDetails.Add((person, candlesReceived));
                Console.Clear();
            }

            SaveFundraiserData(fundraiserName, fundraiserDate, fundraiserType, personFundraiserDetails);
        }
        else if (fundraiserType == "Candle-based Fundraiser")
        {
            Dictionary<string, int> candleFundraiserDetails = new Dictionary<string, int>();

            foreach (var scent in scents)
            {
                int candleCount = AnsiConsole.Ask<int>($"Enter the number of {scent.Name} candles received for the fundraiser:");
                candleFundraiserDetails[scent.Name] = candleCount;
            }

            // Display a table to validate the amounts
            DisplayCandleAmountsTable(candleFundraiserDetails);

            // Ask if the amounts are correct
            bool amountsCorrect = AnsiConsole.Confirm("Are the amounts correct?");
            if (!amountsCorrect)
            {
                // Redo the process for candle amounts
                AddNewFundraiser();
                return;
            }

            SaveFundraiserData(fundraiserName, fundraiserDate, fundraiserType, candleFundraiserDetails);
        }
    }

    static void DisplayCandleAmountsTable(Dictionary<string, int> candleAmounts)
    {
        var table = new Table().Border(TableBorder.Rounded)
                                .Title("Candle Amounts")
                                .AddColumn("Scent")
                                .AddColumn("Amount");

        foreach (var kvp in candleAmounts)
        {
            table.AddRow(kvp.Key, kvp.Value.ToString());
        }

        AnsiConsole.Render(table);
    }


    static void CurrentFundraiser()
    {

    }

    static void PastFundraiser()
    {

    }

    static void SaveFundraiserData(string fundraiserName, DateTime fundraiserDate, string fundraiserType, object fundraiserDetails)
    {
        string filePath = $"fundraisers/{fundraiserName}_fundraiser.json"; // Modify the path as needed

        try
        {
            var dataToSave = new
            {
                FundraiserName = fundraiserName,
                FundraiserDate = fundraiserDate,
                FundraiserType = fundraiserType,
                FundraiserDetails = fundraiserDetails
            };

            string jsonData = JsonSerializer.Serialize(dataToSave, new JsonSerializerOptions
            {
                WriteIndented = true // Makes the JSON output more readable
            });

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            File.WriteAllText(filePath, jsonData);
            Console.WriteLine("Fundraiser data saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving fundraiser data: {ex.Message}");
        }
    }
}
