using Contracts;
using Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BackgroundServices;

public class UserTaskDeadlineNotificationService : BackgroundService
{
    private readonly ILoggerManager _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UserTaskDeadlineNotificationService(ILoggerManager logger,
                                               IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        DateTime now, nextRunTime;
        TimeSpan delay;
        while (!cancellationToken.IsCancellationRequested)
        {
            now = DateTime.UtcNow;
            nextRunTime = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(1);
            delay = nextRunTime - now;

            await Task.Delay(delay, cancellationToken);

            await SendNotificationAsync();
        }
    }

    private async Task SendNotificationAsync()
    {
        try
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var tomorrow = DateTime.UtcNow.Date.AddDays(1);
            var tasks = await repository.UserTask.GetUserTasksWithDeadlineAsync(tomorrow, trackChanges: false);
            List<EmailMetadata> emailsMetadata = new();

            foreach (var task in tasks.GroupBy(t => t.User.Email))
            {
                var userEmail = task.Key;
                var taskList = string.Join("<br />", task.Select(t => $"- {t.Title} (deadline: {t.Deadline:dd/MM/yyyy H:mm} GMT) <p>{t.Description}<p />"));

                var subject = "Deadline Notification";
                var body = $"<p>You have the following tasks with a deadline tomorrow:</p><p>{taskList}</p>";
                emailsMetadata.Add(new EmailMetadata(userEmail!, subject, body));

            }
            await emailService.SendMultiple(emailsMetadata);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the notification service: {ex.Message}");
            _logger.LogError($"Stack Trace: {ex.StackTrace}");
            await Task.Delay(TimeSpan.FromMinutes(5));
        }
    }
}
