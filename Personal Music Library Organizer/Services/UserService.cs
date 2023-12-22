﻿using GrooveHub.Interfaces;
using GrooveHub.Models;

namespace GrooveHub.Services;

internal class UserService : IUserService
{
    private IUserService userService;
    private ILibraryService libraryService;

    private List<User> users;
    public UserService(UserService userService, LibraryService libraryService)
    {
        this.userService = userService;
        this.libraryService = libraryService;

        this.users = new List<User>();
    }
    public User Create(User user)
    {
        int index = users.LastIndexOf(user);
        user.Id = users[index].Id+1;
        users.Add(user);
        return user;
    }
    public User Update(User user)
    {
        User found = null;
        foreach (User item in users)
        {
            if (item.Id == user.Id)
            {
                item.FirstName = user.FirstName;
                item.LastName = user.LastName;
                found = item;
                break;
            }
        }
        return found;
    }
    public bool Delete(int id)
    {
        bool found = false;
        foreach (User user in users)
        {
            if (user.Id == id)
            {
                found = true;
                users.Remove(user);
                break;
            }
        }
        return found;
    }
    public User Get(int id)
    {
        User found = null;
        foreach (User user in users)
        {
            if (user.Id == id)
            {
                found = user;
                break;
            }
        }
        return found;
    }
    public List<User> GetAll()
    {
        return users;
    }
    public (bool foundUser, bool foundLibrary) AddLibrary(int userId, int libraryId)
    {
        bool foundUser = false;
        bool foundLibrary = false;
        foreach (var user in users)
        {
            if (user.Id == userId)
            {
                foundUser = true;
                Library library = libraryService.GetLibrary(libraryId);
                if (library != null)
                {
                    foundLibrary = true;
                    user.SavedLibraries.Add(library);
                }
                break;
            }
        }

        return (foundUser, foundLibrary);

    }
    public (bool foundUser, bool foundLibrary) AddLibraryByGenre(int userID, string genre)
    {
        bool foundUser = false;
        bool foundLibrary = false;
        foreach (var user in users)
        {
            if (user.Id == userID)
            {
                foundUser = true;
                Library library = libraryService.GetLibraryByMusicGenre(genre);
                if (library != null)
                {
                    foundLibrary = true;
                    user.SavedLibraries.Add(library);
                }
                break;
            }
        }

        return (foundUser, foundLibrary);
    }
    public (bool foundUser, bool foundLibrary) RemoveUserLibrary(int userId, int libraryId)
    {
        bool foundUser = false;
        bool foundLibrary = false;
        foreach (var user in users)
        {
            if (user.Id == userId)
            {
                foundUser = true;
                foreach (Library library in user.SavedLibraries)
                {
                    if (library != null)
                    {
                        foundLibrary = true;
                        user.SavedLibraries.Remove(library);
                        break;
                    }
                }
            }
        }

        return (foundUser, foundLibrary);
    }
}