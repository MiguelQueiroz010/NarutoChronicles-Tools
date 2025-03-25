using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NUC_Raw_Tools.Forms_e_Controles
{
    public partial class CustomRTB : UserControl
    {
        private RichTextBox rtbEditor;
        private RichTextBox rtbLineNumbers;
        private TableLayoutPanel tableLayoutPanel;
        private Bitmap buttonSprites;
        private Dictionary<string, Rectangle> buttonMap;
        public RichTextBox Editor => rtbEditor; // Expor o RichTextBox principal


        public CustomRTB(Bitmap sprites)
        {
            buttonSprites = sprites;
            buttonMap = new Dictionary<string, Rectangle>
        {
            { "/l1", new Rectangle(0, 0, 24, 16) },
            { "/r1", new Rectangle(24, 0, 24, 16) },
            { "/r2", new Rectangle(72, 0, 24, 16) },
            { "/l2", new Rectangle(48, 0, 24, 16) },
            { "/2", new Rectangle(48, 16, 16, 16) },
            { "/4", new Rectangle(32, 16, 16, 16) },
            { "/6", new Rectangle(16, 16, 16, 16) },
            { "/8", new Rectangle(0, 16, 16, 16) }
        };

            this.Size = new Size(600, 400);
            this.BorderStyle = BorderStyle.FixedSingle;

            // Configuração do layout
            tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));  // Numeração
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // Editor
            this.Controls.Add(tableLayoutPanel);

            // RichTextBox para números das linhas
            rtbLineNumbers = new RichTextBox
            {
                Font = new Font("Consolas", 10),
                Width = 32,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.None,
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };
            tableLayoutPanel.Controls.Add(rtbLineNumbers, 0, 0);

            // RichTextBox para edição
            rtbEditor = new RichTextBox
            {
                Font = new Font("Consolas", 10),
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };
            rtbEditor.VScroll += RtbEditor_Changed;
            rtbEditor.TextChanged += RtbEditor_Changed;
            rtbEditor.SelectionChanged += RtbEditor_Changed;
            rtbEditor.Paint += RtbEditor_Changed;
            tableLayoutPanel.Controls.Add(rtbEditor, 1, 0);

            AtualizarNumeroLinhas();
        }

        private void AtualizarNumeroLinhas()
        {
            int primeiroIndice = rtbEditor.GetCharIndexFromPosition(new Point(0, 0));
            int primeiraLinha = rtbEditor.GetLineFromCharIndex(primeiroIndice);

            int ultimoIndice = rtbEditor.GetCharIndexFromPosition(new Point(0, rtbEditor.ClientSize.Height));
            int ultimaLinha = rtbEditor.GetLineFromCharIndex(ultimoIndice);

            rtbLineNumbers.Clear();
            for (int i = primeiraLinha; i <= ultimaLinha + 1; i++)
            {
                rtbLineNumbers.AppendText((i + 1) + Environment.NewLine);
            }
        }
        private void RtbEditor_Changed(object sender, EventArgs e)
        {
            AtualizarNumeroLinhas();
            string text = rtbEditor.Text;
            Graphics g = Editor.CreateGraphics();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            MatchCollection matches = Regex.Matches(text, @"\/\w+");
            foreach (Match match in matches)
            {
                string foundText = match.Value;
                if (buttonMap.ContainsKey(foundText))
                {
                    int charIndex = match.Index;
                    Point position = rtbEditor.GetPositionFromCharIndex(charIndex);
                    Rectangle buttonRect = buttonMap[foundText];

                    Bitmap buttonImage = buttonSprites.Clone(buttonRect, System.Drawing.Imaging.PixelFormat.DontCare);
                    g.DrawImage(buttonImage, position);
                    
                }
            }
            
        }
    }
}
