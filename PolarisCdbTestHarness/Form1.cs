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
        private PolarisTransaction accountTransaction;
        private List<PolarisAccountHolder> accountHolderList;
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

        private void btbAddTransaction_Click(object sender, EventArgs e)
        {
            PolarisCdbDataLibrary library = new PolarisCdbDataLibrary();

            accountTransaction = new PolarisTransaction();
            accountTransaction.TransactionGuid = Guid.NewGuid();//Guid.Parse(txtTransAccountGuid.Text);
            accountTransaction.AccountGuid = Guid.Parse(txtTransAccountGuid.Text);
            accountTransaction.TransactionDateTime = DateTime.Now;
            accountTransaction.BeginningBalance = decimal.Parse(txtBeginningBalance.Text);
            accountTransaction.TransactionDateTime = DateTime.Now;
            accountTransaction.TransactionAmount = decimal.Parse(txtTransactionAmount.Text);
            accountTransaction.EndingBalance = decimal.Parse(txtEndingBalance.Text);
            accountTransaction.TransactionType = cboTransactionType.SelectedItem.ToString();
            accountTransaction.Memo = txtTransMemo.Text.ToUpper();

            //Add the transaction
            library.AddTransactionToCosmosDb(accountTransaction);

            ClearAccountTransactionFields();

            MessageBox.Show("Done");

        }


        private void btnFindAccount_Load(object sender, EventArgs e)
        {
            GetAccountHldrs();
            GetAccountTransactions();
        }

        #region Custom Methods

        private async Task GetAccountHldrs()
        {
            PolarisCdbDataLibrary library = new PolarisCdbDataLibrary();
            List<PolarisAccountHolder> theList = await library.GetAllAccountHolders();
            polarisAccountDataGridView.DataSource = theList;
            polarisAccountDataGridView.Columns["id"].Visible = false;
            polarisAccountDataGridView.Columns["AccountGuid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            polarisAccountDataGridView.RowHeadersVisible = false;
        }

        private async Task GetAccountTransactions()
        {
            PolarisCdbDataLibrary library = new PolarisCdbDataLibrary();
            List<PolarisTransaction> theList = await library.GetAllTransactions();
            polarisTransactionDataGridView.DataSource = theList;
            polarisTransactionDataGridView.Columns["id"].Visible = false;
            polarisTransactionDataGridView.Columns["TransactionGuid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            polarisTransactionDataGridView.RowHeadersVisible = false;
        }

        private void ClearAccountHolderFields()
        {
            txtAccountHolder.Text = "";
            txtAccountGuid.Text = "";
            cboAccountType.SelectedValue = -1;
        }

        private void ClearAccountTransactionFields()
        {
            txtTransactionGuid.Text = "";
            txtTransAccountGuid.Text = "";
            txtTransactionDateTime.Text = "";
            txtBeginningBalance.Text = "";
            txtTransactionAmount.Text = "";
            txtEndingBalance.Text = "";
            txtTransMemo.Text = "";
            cboTransactionType.Text = "";
            cboTransactionType.SelectedValue = -1;
        }

        #endregion

    }
}
