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
    private readonly GroupService _groupService;
    private readonly PostgresContext _context;
    private readonly AdminService _adminService;
    private readonly LessonService _lessonService;

    public Window_Admin()
    {
        InitializeComponent();
    }

    public Window_Admin(int pers, ISheduleService sheduleService,  StudentService studentService,  GroupService groupService) 
    {
        InitializeComponent();
        _sheduleService = sheduleService;
        _studentService = studentService;
        _context = new PostgresContext();
        _adminService  = new AdminService(_context);
        _lessonService = new LessonService(_context);
        
        Lises();
        Student();
        adm(pers);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var wind = new MainWindow();
        wind.Show();
        this.Close();
    }
    private async Task adm(int pers)
    {
        var admin = await _adminService.GetByIdAsync(pers);
        Admins.Text = admin.Username;
    }

    private async Task Lises()
    {

       
        var als = await _sheduleService.GetAllAsync();
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
        window.ShowDialog(this);
    }

    private async void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        if (Lisen.SelectedItem is not Schedule sh)
        {
            return;
        }

        using var db = new PostgresContext();
        var lessonsToDelete = await db.Lessons
            .Where(l => l.GroupId == sh.GroupId && l.LessonDate == sh.ScheduledDate)
            .ToListAsync();

        if (lessonsToDelete.Any())
        {
            db.Lessons.RemoveRange(lessonsToDelete);
        }
        var scheduleInDb = await db.Schedules.FindAsync(sh.ScheduleId);
            
        if (scheduleInDb != null)
        {
            db.Schedules.Remove(scheduleInDb);
        }
        
        await db.SaveChangesAsync();
        
        await Lises();

    }

    private void Button_OnClick3(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private async void Button_OnClick4(object? sender, RoutedEventArgs e)
    {
        if (Studs.SelectedItems is not ClassLibrary2.Models.Student StSel)
        {
            return;
        }
        
        using var db = new PostgresContext();
        var dels = await db.Students.Where(s => s.StudentId == StSel.StudentId).ToListAsync();

        if (dels.Any())
        {
            db.Students.RemoveRange(dels);
        }
        await db.SaveChangesAsync();
        await Student();
    }
}