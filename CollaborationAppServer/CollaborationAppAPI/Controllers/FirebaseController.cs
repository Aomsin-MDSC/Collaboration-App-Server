using CollaborationAppAPI.Models;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using System.Diagnostics;
public class FirebaseController
{
    private readonly AppDbContext _context;

    public FirebaseController(AppDbContext context)
    {
        _context = context;
    }
    public async System.Threading.Tasks.Task NotificationAnnounce(int projectId,string Announce_title, string Announce_text)
    {
        try
        {
            var memberList = await _context.Members
                .Where(m => m.Project_id == projectId)
                .Include(m => m.User)
                .ToListAsync();

            var tokens = memberList
                .Where(m => !string.IsNullOrEmpty(m.User.User_token))
                .Select(m => m.User.User_token)
                .ToList();

            if (tokens.Count == 0)
            {
                Console.WriteLine("No valid tokens found.");
                return;
            }

            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Notification = new Notification
                {
                    Title = "New Announce Created!",
                    Body = $"{Announce_title} : {Announce_text}",
                },

            };
            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async System.Threading.Tasks.Task NotificationAssignment(int taskId, string task_Name, string task_detail)
{
    try
    {
            var task = await _context.Tasks
         .FirstOrDefaultAsync(t => t.Task_id == taskId);

            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.User_id == task.Task_Owner);

            if (user == null || string.IsNullOrEmpty(user.User_token))
            {
                Console.WriteLine("User not found or User_token is empty.");
                return;
            }
            var message = new Message()
            {
            Token = user.User_token,

                Notification = new Notification
            {
                Title = "You have Assignment!",
                Body = $"{task_Name} : {task_detail}",
            },
        };
        var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
    }
    catch (Exception ex)
    {
            Console.WriteLine($"Error: {ex.Message}");
        }
}

public async System.Threading.Tasks.Task NotificationComment(int taskId,string comment_text, int userId)
{
    try
    {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Task_id == taskId);

            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            if (task.Task_Owner == userId)
            {
                Console.WriteLine("No notification needed: user is the task owner.");
                return;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.User_id == task.Task_Owner);

            if (user == null || string.IsNullOrEmpty(user.User_token))
            {
                Console.WriteLine("User not found or User_token is empty.");
                return;
            }
            var commentingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.User_id == userId);

            var message = new Message()
            {
            Token = user.User_token,
                
            Notification = new Notification
            {
                Title = $"{commentingUser.User_name}",
                Body = $"Comment in your Task : {comment_text}",
            },
        };
        var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
    }
    catch (Exception ex)
    {
            Console.WriteLine($"Error: {ex.Message}");
        }
}

public async System.Threading.Tasks.Task NotificationTaskStatus(int projectId,string taskName)
{
    try
    {
            var memberList = await _context.Members
                .Where(m => m.Project_id == projectId)
                .Include(m => m.User)
                .ToListAsync();

            var tokens = memberList
                .Where(m => !string.IsNullOrEmpty(m.User.User_token))
                .Select(m => m.User.User_token)
                .ToList();

            if (tokens.Count == 0)
            {
                Console.WriteLine("No valid tokens found.");
                return;
            }

            var message = new MulticastMessage()
        {
            Tokens = tokens,
            Notification = new Notification
            {
                Title = $"{taskName}",
                Body = "Change status is done.",
            },
        };
        var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
    }
    catch (Exception ex)
    {
            Console.WriteLine($"Error: {ex.Message}");
        }
}
}