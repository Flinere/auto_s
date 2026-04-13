using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassLibrary2.Context;
using ClassLibrary2.Models;
using ClassLibrary2.Services;
using ClassLibrary2.Services.Implementations;

namespace auto;

public partial class MainWindow : Window
{
    public bool but_char{get;set;} = true;
    public string Login{get;set;}
    public string Password{get;set;}
    private readonly PostgresContext _context;
    private readonly AdminService  _adminService;
    private readonly StudentService  _studentService;
    private readonly InstructorService  _instructorService;
    private readonly ScheduleService  _scheduleService;
    private readonly LessonService  _lessonService;
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        _context = new PostgresContext();
        _adminService = new AdminService(_context);
        _studentService = new StudentService(_context);
        _instructorService = new InstructorService(_context);
        _scheduleService = new ScheduleService(_context);
        _lessonService = new LessonService(_context);
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Login = Logins.Text;
        Password = Passwords.Text;
        if (Login != null && Password != null)
        {
            try
            {
                
                var admin = await _adminService.AuthenticateAsync(Login, Password);
                if (admin != null)
                {
                    var winds = new Window_Admin(admin.AdminId, _scheduleService, _studentService);
                    winds.Show();
                    this.Close();
                }
                var students = await _studentService.AuthenticateAsync(Login, Password);
                if (students != null)
                {
                    var winds = new Window_student(students.StudentId, _studentService, _lessonService);
                    winds.Show();
                    this.Close();
                }
                var instruct = await _instructorService.AuthenticateAsync(Login, Password);
                if (instruct != null)
                {
                    var winds = new Window_instructor(instruct.InstructorId, _instructorService, _scheduleService);
                    winds.Show();
                    this.Close();
                }
            }
            catch (Exception exception)
            {
                Texxer.Text = "Введите пароль и логин";
                throw;
            }
            
            
        }
        else
        {
            Texxer.Text = "Введите пароль и логин";
            return;
        }
    }


    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        but_char = !but_char;
        Passwords.PasswordChar = but_char ? '*' : '\0';
    }
}