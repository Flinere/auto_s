using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClassLibrary2.Context;
using ClassLibrary2.Models;
using ClassLibrary2.Services;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using ClassLibrary2.Services.Implementations;
using ClassLibrary2.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace auto;

public partial class Window_instructor : Window
{
    private readonly InstructorService _instrucService;
    private readonly ScheduleService _scheduleService;
    private List<Schedule> _schedulesorg;
    private string Curent = "ScheduledDate";
    private bool sort = true;
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
            _schedulesorg = info_pers.Schedules.ToList();
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

    private void Border_Data(object? sender, TappedEventArgs e)
    {
        if (sender is Border bord &&  bord.Tag is string colms)
        {
            if (Curent == colms)
            {
                sort = !sort;
            }
            else
            {
                Curent = colms;
                sort = true;
            }
            Sorted(Curent, sort);
        }
    }

    private void Sorted(string Colms, bool sort)
    {
        if (_schedulesorg == null) return;

        var sorted = Colms switch
        {
            "datas" => sort
                ? _schedulesorg.OrderBy(s => s.ScheduledDate).ToList()
                : _schedulesorg.OrderByDescending(s => s.ScheduledDate).ToList(),

            "grup" => sort
                ? _schedulesorg.OrderBy(s => s.GroupId).ToList()
                : _schedulesorg.OrderByDescending(s => s.GroupId).ToList(),

            "cares" => sort
                ? _schedulesorg.OrderBy(s => s.Car?.LicensePlate).ToList()
                : _schedulesorg.OrderByDescending(s => s.Car?.LicensePlate).ToList(),

            "durrat" => sort
                ? _schedulesorg.OrderBy(s => s.DurationMinutes).ToList()
                : _schedulesorg.OrderByDescending(s => s.DurationMinutes).ToList(),

            "statts" => sort
                ? _schedulesorg.OrderBy(s => s.Status).ToList()
                : _schedulesorg.OrderByDescending(s => s.Status).ToList(),

            _ => _schedulesorg
        };
            Listes.ItemsSource = sorted;
    }
    
}