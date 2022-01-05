using Microsoft.EntityFrameworkCore;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly DbContext _context; 
        public Form1(DbContext context)
        {
            _context = context;
            InitializeComponent();
        }
    }
}