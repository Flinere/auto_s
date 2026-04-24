using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using ClassLibrary2.Context;
using ClassLibrary2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace auto;

public partial class window_Createstud : Window
{
    public window_Createstud()
    {
        InitializeComponent();
        LoadGroupsAsync();
    }
    
    private async Task LoadGroupsAsync()
    {
        try
        {
            using var db = new PostgresContext();
            var groups = await db.Groups.OrderBy(g => g.GroupName).ToListAsync(); 
            Grope.ItemsSource = groups;
        }
        catch (Exception ex)
        {
          return;
        }
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var firstName = Firstname.Text;
        var lastName = Firstname.Text;

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            return;
        }

        try
        {
            using var db = new PostgresContext();

            var newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = Birthdates.SelectedDate.HasValue
                    ? DateOnly.FromDateTime(Birthdates.SelectedDate.Value.DateTime)
                    : null,
                Phone = Phone.Text,
                Email = Email.Text,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                Groups = new List<Group>()
            };

            if (Grope.SelectedItems?.Count > 0)
            {
                var selectedIds = Grope.SelectedItems.OfType<Group>().Select(g => g.GroupId).ToList();
                var groupsToAdd = await db.Groups.Where(g => selectedIds.Contains(g.GroupId)).ToListAsync();
                newStudent.Groups = groupsToAdd;
            }

            await db.Students.AddAsync(newStudent);
            await db.SaveChangesAsync();

            this.Close(true);
        }
        catch
        {
            return;
        }
    
}

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}