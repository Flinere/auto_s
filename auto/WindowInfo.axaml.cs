using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ClassLibrary2.Context;
using ClassLibrary2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace auto;

public partial class WindowInfo : Window
{
    private readonly int _scheduleId;

    public WindowInfo(int scheduleId)
    {
        _scheduleId = scheduleId;
        InitializeComponent();
        LoadData();
    }
    
    private async void LoadData()
    {
        try
        {
            using var db = new PostgresContext();
            
            var schedule = await db.Schedules
                .Include(s => s.Group).ThenInclude(g => g.Students)
                .Include(s => s.Car)
                .Include(s => s.Instructor)
                .FirstOrDefaultAsync(s => s.ScheduleId == _scheduleId);

            if (schedule == null)
            {
                return;
            }

            var carInfo = schedule.Car?.LicensePlate ?? "Не назначен";
            var groupInfo = schedule.Group?.GroupName ?? "Индивидуальное";
            var instructorInfo = schedule.Instructor?.FirstName ?? "Инструктор";

            var students = schedule.Group?.Students?.ToList() ?? new List<Student>();
            LbStudents.ItemsSource = students;
        }
        catch (Exception ex)
        {
            return;
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}