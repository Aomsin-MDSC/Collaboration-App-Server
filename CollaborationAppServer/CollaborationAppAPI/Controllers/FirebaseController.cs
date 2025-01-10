using CollaborationAppAPI.Models;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
public class FirebaseController
{
    private readonly AppDbContext _context;

    public FirebaseController(AppDbContext context)
    {
        _context = context;
    }
    public async System.Threading.Tasks.Task NotificationAnnounce(int projectId)
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
                    Body = "AnnounceName : ?????????????????????????????????????????????????????????????????????????????????????????????????????",
                },

            };
            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async System.Threading.Tasks.Task NotificationAssignment()
{
    try
    {
        var message = new MulticastMessage()
        {
            Tokens = new List<string>
                {
                    "dymo4AheSEeMQb4oVW5Tj_:APA91bGO0ac8PiwvHbB3pZHsV2qDCmdECSkqdjcxXaprZagHyMFT6oRi7y0Hb7wRKn5T62S8-9l73JO2enaBCZXsx_JvD3fnMcjwRkaCUUvbdEfyDShttn8",
                    "cdiy5vJwTea1qTz5zhmrI1:APA91bEiJpoFZtlaw6-nYXaqd0RijnMdz8tcMf2N-N9pR9fuq_WBnzJx9yQixIrifBKtWteNmaXicfjtqKqH2k5txJBnfRWWzkWtPUB3JHfZZsS2sK6-4Jw",
                    //api
                },
            Notification = new Notification
            {
                Title = "You have Assignment!",
                Body = "TaskName : ?????????????????????????????????????????????????????????????????????????????????????????????????????",
            },
        };
        var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
    }
    catch (Exception ex)
    {
            Console.WriteLine($"Error: {ex.Message}");
        }
}

public async System.Threading.Tasks.Task NotificationComment()
{
    try
    {
        var message = new MulticastMessage()
        {
            Tokens = new List<string>
                {
                    "dymo4AheSEeMQb4oVW5Tj_:APA91bGO0ac8PiwvHbB3pZHsV2qDCmdECSkqdjcxXaprZagHyMFT6oRi7y0Hb7wRKn5T62S8-9l73JO2enaBCZXsx_JvD3fnMcjwRkaCUUvbdEfyDShttn8",
                    "cdiy5vJwTea1qTz5zhmrI1:APA91bEiJpoFZtlaw6-nYXaqd0RijnMdz8tcMf2N-N9pR9fuq_WBnzJx9yQixIrifBKtWteNmaXicfjtqKqH2k5txJBnfRWWzkWtPUB3JHfZZsS2sK6-4Jw",
                    //api
                },
            Notification = new Notification
            {
                Title = "{Username}",
                Body = "Comment in your Task : ?????????????????????????????????????????????????????",
            },
        };
        var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
    }
    catch (Exception ex)
    {
            Console.WriteLine($"Error: {ex.Message}");
        }
}

public async System.Threading.Tasks.Task NotificationTaskStatus()
{
    try
    {
        var message = new MulticastMessage()
        {
            Tokens = new List<string>
                {
                    "dymo4AheSEeMQb4oVW5Tj_:APA91bGO0ac8PiwvHbB3pZHsV2qDCmdECSkqdjcxXaprZagHyMFT6oRi7y0Hb7wRKn5T62S8-9l73JO2enaBCZXsx_JvD3fnMcjwRkaCUUvbdEfyDShttn8",
                    "cdiy5vJwTea1qTz5zhmrI1:APA91bEiJpoFZtlaw6-nYXaqd0RijnMdz8tcMf2N-N9pR9fuq_WBnzJx9yQixIrifBKtWteNmaXicfjtqKqH2k5txJBnfRWWzkWtPUB3JHfZZsS2sK6-4Jw",
                    //api
                },
            Notification = new Notification
            {
                Title = "{TaskName}",
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