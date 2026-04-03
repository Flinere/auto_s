using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassLibrary2.Context;
using ClassLibrary2.Models;
using ClassLibrary2.Services;

namespace auto;

public partial class MainWindow : Window
{
    public bool but_char{get;set;} = true;
    public string Login{get;set;}
    public string Password{get;set;}
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Login = Logins.Text;
        Password = Passwords.Text;
        if (Login != null && Password != null)
        {
            try
            {
                var bd = new PostgresContext();
                var admin = bd.Admins.FirstOrDefault(u => u.Username == Login && u.Password == Password);
                if (admin != null)
                {
                    int id_pers = admin.AdminId;
                    var winds = new Window_Admin(id_pers);
                    winds.Show();
                    this.Close();
                }
                var students = bd.Students.FirstOrDefault(u => u.Email == Login && u.Password == Password);
                if (students != null)
                {
                    int id_pers = students.StudentId;
                    var winds = new Window_student(id_pers);
                    winds.Show();
                    this.Close();
                }
                var instruct = bd.Instructors.FirstOrDefault(u => u.Email == Login && u.Password == Password);
                if (instruct != null)
                {
                    int id_pers = instruct.InstructorId;
                    var winds = new Window_instructor(id_pers);
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