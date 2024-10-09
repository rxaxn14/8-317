using System;
using System.Windows.Forms;
using WindowsFormsApp.CalculadoraService;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        CalculadoraSoapClient cliente;

        public Form1()
        {
            InitializeComponent();
            cliente = new CalculadoraSoapClient();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            string expresion = txtExpresion.Text;
            int resultado = 0;

            if (rbtnInfijo.Checked)
            {
                resultado = cliente.EvaluarInfija(expresion);
            }
            else if (rbtnPrefijo.Checked)
            {
                resultado = cliente.EvaluarPrefija(expresion);
            }

            lblResultado.Text = "Resultado: " + resultado.ToString();
        }
    }
}
