using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ClassLibrary2.Context;
using System.Threading.Tasks;
using ClassLibrary2.Services.Implementations;
using Avalonia.Threading;
using Microsoft.EntityFrameworkCore;

namespace auto;

public partial class Window_CreateLesson : Window
{
    public DateTime LessonTime{get;set;}
    
    public Window_CreateLesson()
    {
        InitializeComponent();
        GroupLoad();
    }

    public async Task GroupLoad()
    {
        using var bd = new PostgresContext();
        var group = bd.Groups.ToListAsync();
        Group.ItemsSource = await group;
        var car =   bd.Cars.ToListAsync();
        Cars.ItemsSource = await car;
        var Instructors =  bd.Instructors.ToListAsync();
        Instructor.ItemsSource = await Instructors;

    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    
    {
        var selectedGroup = Group.SelectedItem as ClassLibrary2.Models.Group;
        var selectedCar = Cars.SelectedItem as ClassLibrary2.Models.Car;
        var selectedInstructor = Instructor.SelectedItem as ClassLibrary2.Models.Instructor;
        var selectedStatus = Status.SelectedItem as ComboBoxItem;
        
        if (selectedGroup == null || selectedCar == null || selectedInstructor == null ||
            !Date.SelectedDate.HasValue || !Time.SelectedTime.HasValue)
        {
            return;
        }
        try
        {
            using var bd = new PostgresContext();
            
            var studentsInGroup = await bd.Students.Where(s => s.Groups.Any(g => g.GroupId == selectedGroup.GroupId)).ToListAsync();
            
            
            var lessonDate = Date.SelectedDate.Value.Date.Add(Time.SelectedTime.Value);
            var duration = (int)(Dura.Value ?? 60);
            var status = selectedStatus?.Content?.ToString() ?? "scheduled";
            var schedule = new ClassLibrary2.Models.Schedule
            {
                InstructorId = selectedInstructor.InstructorId,
                CarId = selectedCar.CarId,
                GroupId = selectedGroup.GroupId,
                ScheduledDate = lessonDate,
                DurationMinutes = duration,
                Status = status
            };
        
            await bd.Schedules.AddAsync(schedule);
            await bd.SaveChangesAsync();
            
            var lessons = studentsInGroup.Select(student => new ClassLibrary2.Models.Lesson
            {
                StudentId = student.StudentId,        
                InstructorId = selectedInstructor.InstructorId,
                CarId = selectedCar.CarId,
                GroupId = selectedGroup.GroupId,
                LessonDate = lessonDate,
                DurationMinutes = duration,
                Status = status
            }).ToList();

            await bd.Lessons.AddRangeAsync(lessons);
            await bd.SaveChangesAsync();
            this.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("pizda");
           return;
        }
    }

    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}