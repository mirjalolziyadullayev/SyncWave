﻿using Spectre.Console;
using SyncWave.Models;
using SyncWave.Services;

namespace SyncWave.ConsoleUI.SubMenu;

public class UserMenu
{
    private UserService userService;
    private LibraryService libraryService;
    public UserMenu(UserService userService, LibraryService libraryService)
    {
        this.userService = userService;
        this.libraryService = libraryService;
    }
    public void Display()
    {
        bool loop = true;
        while (loop)
        {
            var choise = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Sync[green]Wave[/][grey] / [/]Users")
                    .PageSize(8)
                    .AddChoices(new[] {
                        "Create user",
                        "Update user",
                        "Delete user",
                        "Get user",
                        "Get all users",
                        "Add Musiclibrary to user",
                        "Remove MusicLibrary from user\n",
                        "[red]Go back[/]"}));
            switch (choise)
            {
                case "Create user":
                    Console.Clear();

                    var Cfirstname = AnsiConsole.Ask<string>("Enter your [green]Firstname[/]:");
                    var Clastname = AnsiConsole.Ask<string>("Enter your [green]Lastname[/]:");

                    User user = new User();
                    user.FirstName = Cfirstname;
                    user.LastName = Clastname;

                    User createdUser = userService.Create(user);

                    var table = new Table();

                    table.AddColumn("Created User");

                    table.AddRow($"[green]UserID[/]: {createdUser.Id}");
                    table.AddRow($"[green]User's Firstname[/]: {createdUser.FirstName}");
                    table.AddRow($"[green]User's Firstname[/]: {createdUser.LastName}");

                    AnsiConsole.Write(table);

                    break;
                case "Update user":
                    Console.Clear();

                    var Uid = AnsiConsole.Ask<int>("Enter your [green]ID[/]:");
                    var Ufirstname = AnsiConsole.Ask<string>("Enter your [green]Firstname[/]:");
                    var Ulastname = AnsiConsole.Ask<string>("Enter your [green]Lastname[/]:");

                    User Updateuser = new User();
                    Updateuser.Id = Uid;
                    Updateuser.FirstName = Ufirstname;
                    Updateuser.LastName = Ulastname;

                    User updatedUser = userService.Update(Updateuser);

                    if (updatedUser != null)
                    {
                        var table1 = new Table();

                        table1.AddColumn("Updated User");

                        table1.AddRow($"[green]UserID[/]: {updatedUser.Id}");
                        table1.AddRow($"[green]User's Firstname[/]: {updatedUser.FirstName}");
                        table1.AddRow($"[green]User's Firstname[/]: {updatedUser.LastName}");

                        AnsiConsole.Write(table1);
                    }
                    else
                    {
                        var tablee = new Table();
                        tablee.AddColumn("Updated User");
                        tablee.AddRow($"[green]User with ID[/]: {Uid} not found");
                        AnsiConsole.Write(tablee);

                        Console.WriteLine("Press any key to try again...");
                        Console.ReadLine();
                        continue;
                    }

                    break;
                case "Delete user":
                    Console.Clear();

                    var Did = AnsiConsole.Ask<int>("Enter your [green]ID[/]:");

                    bool deletedUser = userService.Delete(Did);
                    if (deletedUser != false)
                    {
                        var table2 = new Table();
                        table2.AddColumn("Deleted User");
                        table2.AddRow($"[green]UserID[/]: {Did}");
                        AnsiConsole.Write(table2);
                    }
                    else
                    {
                        var table3 = new Table();
                        table3.AddColumn("Deleted User");
                        table3.AddRow($"[green]User with ID[/]: {Did} not found");
                        AnsiConsole.Write(table3);

                        Console.WriteLine("Press any key to try again...");
                        Console.ReadLine();
                        continue;
                    }

                    break;
                case "Get user":
                    Console.Clear();

                    var Gid = AnsiConsole.Ask<int>("Enter your [green]ID[/]:");

                    User gottenUser = userService.Get(Gid);
                    if (gottenUser != null)
                    {
                        var table4 = new Table();
                        table4.AddColumn("Found User");
                        table4.AddRow($"[green]UserID[/]: {gottenUser.Id}");
                        table4.AddRow($"[green]User's Firstname[/]: {gottenUser.FirstName}");
                        table4.AddRow($"[green]User's Firstname[/]: {gottenUser.LastName}");

                        var innerTable = new Table();
                        innerTable.AddColumn("[green]Saved Libraries[/]");

                        if (gottenUser.SavedLibraries != null)
                        {
                            foreach (Library library in gottenUser.SavedLibraries)
                            {
                                innerTable.AddRow("------------------------------------");
                                innerTable.AddRow($"Library ID: {library.Id}");
                                innerTable.AddRow($"Library Name: {library.Name}");
                                innerTable.AddRow($"Library Genre: {library.Genre}");
                                innerTable.AddRow("------------------------------------");
                            }
                        }
                        else
                        {
                            innerTable.AddRow("Empty");
                        }
                        AnsiConsole.Write(table4);
                        AnsiConsole.Write(innerTable);

                    }
                    else
                    {
                        var table5 = new Table();
                        table5.AddColumn("Found User");
                        table5.AddRow($"[green]User with ID[/]: {Gid} not found");
                        AnsiConsole.Write(table5);

                        Console.WriteLine("Press any key to try again...");
                        Console.ReadLine();
                        continue;
                    }
                    break;
                case "Get all users":
                    Console.Clear();
                    List<User> getallusers = userService.GetAll();
                    if (getallusers.Count != 0)
                    {
                        foreach (User item in getallusers)
                        {
                            var table6 = new Table();
                            table6.AddColumn("Found User");
                            table6.AddRow($"[green]UserID[/]: {item.Id}");
                            table6.AddRow($"[green]User's Firstname[/]: {item.FirstName}");
                            table6.AddRow($"[green]User's Firstname[/]: {item.LastName}");
                            AnsiConsole.Write(table6);
                        }
                    }
                    else
                    {
                        var table7 = new Table();
                        table7.AddColumn("Found User");
                        table7.AddRow($"[red]User's List is empty[/]");
                        AnsiConsole.Write(table7);
                    }

                    break;
                case "Add Musiclibrary to user":
                    Console.Clear();

                    var Aid = AnsiConsole.Ask<int>("Enter your [green]ID[/]:");
                    var ALibID = AnsiConsole.Ask<int>("Enter Library [green]ID[/]:");

                    (bool foundUser, bool foundLibrary) = userService.AddLibrary(Aid, ALibID);

                    if (foundLibrary == false)
                    {
                        var table8 = new Table();
                        table8.AddColumn("Found Library");
                        table8.AddRow($"[green]Library with ID[/]: {ALibID} not found");
                        AnsiConsole.Write(table8);

                        Console.WriteLine("Press any key to try again...");
                        Console.ReadLine();
                        continue;
                    }
                    if (foundUser == false)
                    {
                        var table9 = new Table();
                        table9.AddColumn("Found User");
                        table9.AddRow($"[green]User with ID[/]: {Aid} not found");
                        AnsiConsole.Write(table9);

                        Console.WriteLine("Press any key to try again...");
                        Console.ReadLine();
                        continue;
                    }

                    var table10 = new Table();
                    table10.AddColumn("Adding MusicLibrary to uesr");
                    table10.AddRow($"[green]Library with ID[/]: {ALibID} added to [green]User with ID[/]: {Aid}");
                    AnsiConsole.Write(table10);

                    break;
                case "Remove MusicLibrary from user\n":
                    Console.Clear();

                    var Rid = AnsiConsole.Ask<int>("Enter your [green]ID[/]:");
                    var RLibID = AnsiConsole.Ask<int>("Enter Library [green]ID[/]:");

                    (bool RfoundUser, bool RfoundLibrary) = userService.RemoveUserLibrary(Rid, RLibID);

                    if (RfoundLibrary == false)
                    {
                        var table11 = new Table();
                        table11.AddColumn("Found Library");
                        table11.AddRow($"[green]Library with ID[/]: {RLibID} not found");
                        AnsiConsole.Write(table11);

                        Console.WriteLine("Press any key to try again...");
                        Console.ReadLine();
                        continue;
                    }
                    if (RfoundUser == false)
                    {
                        var table11 = new Table();
                        table11.AddColumn("Found User");
                        table11.AddRow($"[green]User with ID[/]: {Rid} not found");
                        AnsiConsole.Write(table11);

                        Console.WriteLine("Press any key to try again...");
                        Console.ReadLine();
                        continue;
                    }
                    var table12 = new Table();
                    table12.AddColumn("Removing MusicLibrary to uesr");
                    table12.AddRow($"[green]Library with ID[/]: {RLibID} removed from [green]User with ID[/]: {Rid}");
                    AnsiConsole.Write(table12);

                    break;
                case "[red]Go back[/]":
                    Console.Clear();

                    return;
            }
        }
    }
}
