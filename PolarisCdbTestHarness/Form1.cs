using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            library.AddAccountHolderToCosmosDb(accountHolder, txtUriString.Text,txtKeyString.Text);

            ClearAccountHolderFields();
            GetAccountHldrs(txtUriString.Text, txtKeyString.Text);
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
            library.AddTransactionToCosmosDb(accountTransaction,txtUriString.Text,txtKeyString.Text);

            ClearAccountTransactionFields();
            GetAccountTransactions(txtUriString.Text, txtKeyString.Text);
        }


        private void btnFindAccount_Load(object sender, EventArgs e)
        {
        }

        #region Custom Methods

        private void GetConfigSections()
        {
            string configFilePath = string.Empty;
            string configText = String.Empty;

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                configFilePath = fileDialog.FileName;
                if (!string.IsNullOrEmpty(configFilePath))
                {
                    configText = File.ReadAllText(configFilePath);
                    //Get the text fields
                    int pipeOne = configText.IndexOf("|");
                    int startx1 = configText.IndexOf("CosmosURI|");
                    txtUriString.Text = configText.Substring(0,pipeOne);
                    txtKeyString.Text = configText.Substring(pipeOne + 1, configText.Length - (pipeOne + 1));

                    //Get the records
                    GetAccountHldrs(txtUriString.Text, txtKeyString.Text);
                    GetAccountTransactions(txtUriString.Text, txtKeyString.Text);
                }
            }
        }

        private async Task GetAccountHldrs(string polarisCosmosDbEndpointUri, string polarisCosmosDbPrimaryKey)
        {
            PolarisCdbDataLibrary library = new PolarisCdbDataLibrary();
            List<PolarisAccountHolder> theList = await library.GetAllAccountHolders(polarisCosmosDbEndpointUri, polarisCosmosDbPrimaryKey);
            polarisAccountDataGridView.DataSource = theList;
            polarisAccountDataGridView.Columns["id"].Visible = false;
            polarisAccountDataGridView.Columns["AccountGuid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            polarisAccountDataGridView.RowHeadersVisible = false;
        }

        private async Task GetAccountTransactions(string polarisCosmosDbEndpointUri, string polarisCosmosDbPrimaryKey)
        {
            PolarisCdbDataLibrary library = new PolarisCdbDataLibrary();
            List<PolarisTransaction> theList = await library.GetAllTransactions(polarisCosmosDbEndpointUri, polarisCosmosDbPrimaryKey);
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

        private void btnAuthConfig_Click(object sender, EventArgs e)
        {
            GetConfigSections();
        }
    }
}
