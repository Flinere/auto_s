using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClassLibrary2.Context;
using ClassLibrary2.Models;
using ClassLibrary2.Services;
using System.Threading.Tasks;
using ClassLibrary2.Services.Implementations;
using ClassLibrary2.Services.interfaces;

namespace auto;

public partial class Window_student : Window
{
    private readonly StudentService  _studentService;
    
    public Window_student()
    {
        InitializeComponent();
    }

    public Window_student(int pers)
    {
        var db = new PostgresContext();
        _studentService = new StudentService(db);
        InitializeComponent();
        Frontwins(pers);
        List_es(pers);
        
    }

    public void Frontwins(int people)
    {
        var db = new PostgresContext();
        var info_pers = db.Students.FirstOrDefault(s => s.StudentId == people);
        First_nam.Text = info_pers.FirstName;
        Login.Text = info_pers.Email;
        Last_name.Text = info_pers.LastName;
        
    }

    public async Task List_es(int people)
    {
        var studens = await _studentService.GetByIdAsync(people);
        Listes.ItemsSource = new List<Student>(){studens};
    }
}