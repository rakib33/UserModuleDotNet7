namespace UserManagementCore.Models
{
    /// <summary>
    /// This is Single DBContext for Whole application 
    /// Need to seed from startup.cs
    /// </summary>
    public class EF_Model
    {

        //-------------< Class: DB >-------------

      //  public static ApplicationDbContext dbContext = null;

        public static void Initialize_DbContext_in_Startup(IServiceProvider serviceProvider)
        {

            //--------< Initialize_DbContext_byStartup() >--------

            //*Set global dbContext. Initialized in startup.configure

            IServiceScope serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();

            //dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();


            //--------< Initialize_DbContext_in_Startup() >--------
            #region AutoDBMigration 

            #endregion
        }

        //-------------</ Class: DB >-------------
    }
}
