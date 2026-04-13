using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClassLibrary2.Context;
using ClassLibrary2.Models;
using ClassLibrary2.Services;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using ClassLibrary2.Services.Implementations;
using ClassLibrary2.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace auto;

public partial class Window_student : Window
{
    private readonly StudentService  _studentService;
    private readonly LessonService _lessonService;
    private readonly int pers;
    
    public Window_student()
    {
        InitializeComponent();
    }

    public Window_student(int pers, StudentService studentService, LessonService lessonService)
    {
       
        InitializeComponent();
        _studentService = studentService;
        _lessonService = lessonService;
        Frontwins(pers);
        List_es(pers);
        
    }

    public async Task Frontwins(int people)
    {
        var info_pers = await _studentService.GetByIdAsync(people);
        First_nam.Text = info_pers.FirstName;
        Login.Text = info_pers.Email;
        Last_name.Text = info_pers.LastName;
        Grups.Text = info_pers.Groups.FirstOrDefault().GroupName;

    }

    public async Task List_es(int people)
    {
        using var db = new PostgresContext();
        var info_pers = await db.Students.Include(s => s.Lessons).ThenInclude(l => l.Instructor).FirstOrDefaultAsync(l => l.StudentId == people);
        if (info_pers != null)
        {
            Listes.ItemsSource = info_pers.Lessons.ToList();
        }
        else
        {
            return;
        }
        
        
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var winds = new MainWindow();
        winds.Show();
        this.Close();
    }
}