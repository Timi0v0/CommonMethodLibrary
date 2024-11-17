using FormFilter.Test.Filters;
using FormFilter.Test.IServices;
using FormFilter.Test.Services;

namespace FormFilter.Test
{
    [CtmFormResourceFilter]
    public partial class Form1 : Form
    {    
        public IUserService UserService { get; set; }
        public Form1(IUserService userService)
        {
            UserService = userService;
            InitializeComponent();
        }
    }
}
