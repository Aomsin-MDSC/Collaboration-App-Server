using FirebaseAdmin;
using FirebaseAdmin.Messaging;
public static class FirebaseController
{
    public static async Task NotificationAnnounce()
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
                    Title = "New Announce Created!",
                    Body = "AnnounceName : ?????????????????????????????????????????????????????????????????????????????????????????????????????",
                },
            };
            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }
        catch (Exception ex)
        {
        }
    }

    public static async Task NotificationAssignment()
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
        }
    }

    public static async Task NotificationComment()
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
        }
    }

    public static async Task NotificationTaskStatus()
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
        }
    }
}

