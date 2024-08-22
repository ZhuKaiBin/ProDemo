namespace AdapterDesignPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }



        public interface INotificationAdapter
        {
            void Notify(User user, Message message);
        }

        public class EmailNotificationAdapter : INotificationAdapter
        {
            private readonly IEmailSender _emailSender;
            private readonly EmailSettings _settings;

            public EmailNotificationAdapter(IEmailSender emailSender, EmailSettings settings)
            {
                _emailSender = emailSender;
                _settings = settings;
            }

            public void Notify(User user, Message message)
            {
                if (!user.AllowEmailNotifications)
                    return;

                string fromEmail = _settings.DefaultFromAddress;

                _emailSender.Send(user.EmailAddress, fromEmail, message.Title, message.Details);
            }
        }
    }
}
