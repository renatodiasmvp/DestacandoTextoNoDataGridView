using DestaqueTextoGridView.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DestaqueTextoGridView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            dataGridView1.DataSource = retornaDataTable();
        }

        // Evento de digitar na caixa de texto
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // Cria e preenche uma DataTable
            DataTable dados = retornaDataTable();
            
            // Se o conteúdo da caixa de texto não for nulo ou vazio
            if (!String.IsNullOrWhiteSpace(textBox1.Text.Trim()))
            {
                // Retorna os dados conforme a busca pela string digitada
                DataTable dvResultadoBusca = BuscaStringDigitada(textBox1.Text.Trim(), dados);

                // O DataGridView recebe os dados da busca
                dataGridView1.DataSource = dvResultadoBusca;
            }
            else
            {
                //  o DataGridView recebe os dados gerados pelo método retoranDataTable
                dataGridView1.DataSource = dados;
            }
        }

        public DataTable retornaDataTable()
        {
            // Cria um DataTable com 4 colunas  
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Nome", typeof(string));
            table.Columns.Add("Sexo", typeof(string));
            table.Columns.Add("Telefone", typeof(string));
            // Adicionamos 3 linhas de dados ao DataTable  
            table.Rows.Add(1, "Renato", "Masculino", "2222-3333");
            table.Rows.Add(2, "Vânia", "Feminino", "3333-4444");
            table.Rows.Add(3, "João", "Masculino", "4444-5555");
            // retorna o DataTable preenchido
            return table;
        }

        private DataTable BuscaStringDigitada(String textoProcurar, DataTable dt)
        {
            // Cria um DataView com o DatTable "dt" passado como parametro
            DataView dvResultadoBusca = new DataView(dt);
            // Faz a busca através de um filtro no campo Nome  
            dvResultadoBusca.RowFilter = "Nome Like '%" + textoProcurar + "%'";

            // retorna o DataView com os resultados da busca
            return dvResultadoBusca.ToTable();
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && dataGridView1.Columns[e.ColumnIndex].Name != "Id")
            {
                // Verifica se algo foi digitado na caixa de texto  
                if (!String.IsNullOrWhiteSpace(textBox1.Text.Trim()))
                {
                    String gridCellValue = e.FormattedValue.ToString();
                    // verifica o índice da string de busca na célula do DataGridView 
                    int startIndexInCellValue = gridCellValue.ToLower().IndexOf(textBox1.Text.Trim().ToLower());
                    // Se o texto existe na célula então o valor de startIndexInCellValue será maior ou igual a zero 
                    if (startIndexInCellValue >= 0)
                    {
                        e.Handled = true;
                        e.PaintBackground(e.CellBounds, true);
                        // o retângulo em destaque  
                        Rectangle hl_rect = new Rectangle();
                        hl_rect.Y = e.CellBounds.Y + 2;
                        hl_rect.Height = e.CellBounds.Height - 5;
                        //obtem o tamanho do texto antes de procurar a string  
                        String sBeforeSearchword = gridCellValue.Substring(0, startIndexInCellValue);
                        //tamanho da string procurada na célula  
                        String sSearchWord = gridCellValue.Substring(startIndexInCellValue, textBox1.Text.Trim().Length);
                        Size s1 = TextRenderer.MeasureText(e.Graphics, sBeforeSearchword, e.CellStyle.Font, e.CellBounds.Size);
                        Size s2 = TextRenderer.MeasureText(e.Graphics, sSearchWord, e.CellStyle.Font, e.CellBounds.Size);
                        if (s1.Width > 5)
                        {
                            hl_rect.X = e.CellBounds.X + s1.Width - 5;
                            hl_rect.Width = s2.Width - 6;
                        }
                        else
                        {
                            hl_rect.X = e.CellBounds.X + 2;
                            hl_rect.Width = s2.Width - 6;
                        }
                        // cor para destacar o texto na célula  
                        SolidBrush hl_brush;
                        hl_brush = new SolidBrush(Color.Yellow);
                        // pinta o fundo da célula para destacar a string procurada 
                        e.Graphics.FillRectangle(hl_brush, hl_rect);
                        hl_brush.Dispose();
                        e.PaintContent(e.CellBounds);
                    }
                }
            }
        }
    }
}
