namespace Contacts.Persistence
{
    public class ContactsInitializer
    {
        public static void Initialize(ContactsDbContext context)
        {
            var initializer = new ContactsInitializer();
            initializer.SeedEverything(context);
        }

        public void SeedEverything(ContactsDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
