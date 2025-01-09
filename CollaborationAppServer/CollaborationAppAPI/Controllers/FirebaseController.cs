using FirebaseAdmin;
using FirebaseAdmin.Messaging;
public static class FirebaseController
{
    public static async Task MakeAnnounce()
    {
        try
        {
            var message = new MulticastMessage()
            {
                Tokens = new List<string>
                {
                    "dymo4AheSEeMQb4oVW5Tj_:APA91bGO0ac8PiwvHbB3pZHsV2qDCmdECSkqdjcxXaprZagHyMFT6oRi7y0Hb7wRKn5T62S8-9l73JO2enaBCZXsx_JvD3fnMcjwRkaCUUvbdEfyDShttn8",
                    "cdiy5vJwTea1qTz5zhmrI1:APA91bEiJpoFZtlaw6-nYXaqd0RijnMdz8tcMf2N-N9pR9fuq_WBnzJx9yQixIrifBKtWteNmaXicfjtqKqH2k5txJBnfRWWzkWtPUB3JHfZZsS2sK6-4Jw"
                },
                Notification = new Notification
                {
                    Title = "Thank",
                    Body = "Phetto",
                },
            };
            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }
        catch (Exception ex)
        {
        }

    }
}

