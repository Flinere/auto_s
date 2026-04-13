using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ClassLibrary2.Context;
using ClassLibrary2.Services.Implementations;
using ClassLibrary2.Services.interfaces;
using ClassLibrary2.Models;

namespace auto;

public partial class Window_Admin : Window
{
    private readonly ISheduleService _sheduleService;
    private readonly StudentService _studentService;

    public Window_Admin()
    {
        InitializeComponent();
    }

    public Window_Admin(int pers, ISheduleService sheduleService,  StudentService studentService)
    {
        InitializeComponent();
        _sheduleService = sheduleService;
        _studentService = studentService;
        Lises();
        Student();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var wind = new MainWindow();
        wind.Show();
        this.Close();
    }

    private async Task Lises()
    {

        var als = await _sheduleService.GetAllAsync();
        Console.WriteLine(als.Count);
        try
        {
            if (als != null)
            {
                Lisen.ItemsSource = als.ToList();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;

        }
    }

    private async Task Student()
    {
        try
        {
            using var db = new PostgresContext();
            var students = await db.Students.AsSplitQuery().Include(s => s.Groups).ToListAsync();
            Studs.ItemsSource = students;
            

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void Refresh(object? sender, RoutedEventArgs e)
    {
        Lises();
        Student();
    }

    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        var window = new Window_CreateLesson();
        ShowDialog(window);
    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Button_OnClick3(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Button_OnClick4(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}