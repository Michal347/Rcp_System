using RCP_Sys.DAL.Interface;
using RCP_Sys.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Services
{
    public class DialogService : IDialogService
    {
        public void ShowDialog()
        {
            var dialog = new ProgramCloseDialog();
            dialog.ShowDialog();
        }
    }
}
