using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fischer.CosmosDb.Solution.PolarisCdbLibraries;
using Fischer.CosmosDb.Solution.PolarisCdbLibraries.Objects;

namespace PolarisCdbTestHarness
{
    public partial class btnFindAccount : Form
    {
        private PolarisAccountHolder accountHolder;

        public btnFindAccount()
        {
            InitializeComponent();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            PolarisCdbDataLibrary library = new PolarisCdbDataLibrary();

            accountHolder = new PolarisAccountHolder();
            accountHolder.id = Guid.NewGuid();
            accountHolder.AccountGuid = Guid.NewGuid();
            accountHolder.AccountHolder = txtAccountHolder.Text.ToUpper();
            accountHolder.AccountType = cboAccountType.SelectedItem.ToString().ToUpper();

            library.AddAccountHolderToCosmosDb(accountHolder);

            ClearAccountHolderFields();

            MessageBox.Show("Done");
        }

        private void ClearAccountHolderFields()
        {
            txtAccountHolder.Text = "";
            txtAccountGuid.Text = "";
            cboAccountType.SelectedValue = -1;
        }

    }
}
