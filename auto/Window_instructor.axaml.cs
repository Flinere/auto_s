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

public partial class Window_instructor : Window
{
    private readonly InstructorService _instrucService;
    private readonly ScheduleService _scheduleService;
    public Window_instructor()
    {
        InitializeComponent();
    }
    public Window_instructor(int pers, InstructorService instrucService, ScheduleService scheduleService)
    {
        var db = new PostgresContext();
        InitializeComponent();
        _instrucService = instrucService;
        _scheduleService = scheduleService;
        Frontwins(pers);
        List_es(pers);
    }
    public async Task Frontwins(int people)
    {
        var info_pers = await _instrucService.GetByIdAsync(people);
        First_nam.Text = info_pers.FirstName;
        Login.Text = info_pers.Email;
        Last_name.Text = info_pers.LastName;
        
    }

    public async Task List_es(int people)
    {
        using var db = new PostgresContext();
        var info_pers = await db.Instructors.Include(s => s.Schedules).ThenInclude(c => c.Car).FirstOrDefaultAsync(l => l.InstructorId == people);
        if (info_pers != null)
        {
            Listes.ItemsSource = info_pers.Schedules.ToList();
        }
        else
        {
            return;
        }
        
        
    }

    private async void Doubless(object sender, RoutedEventArgs e)
    {
        if (Listes.SelectedItem is Schedule selected)
        {
            // Передаём ScheduleId вместо LessonId
            var window = new WindowInfo(selected.ScheduleId); 
            await window.ShowDialog(this);
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var winds = new MainWindow();
        winds.Show();
        this.Close();
    }
}