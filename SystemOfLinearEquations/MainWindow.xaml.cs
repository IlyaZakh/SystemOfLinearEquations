using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfMath;
namespace SystemOfLinearEquations
{
    public partial class MainWindow : Window
    {
        public TStringGrid StringGrid1 = new TStringGrid();
        public TStringGrid StringGrid2 = new TStringGrid();
        public TStringGrid StringGrid3 = new TStringGrid();
        Details detWindow = new Details();
        public static RoutedCommand ClearMatrix = new RoutedCommand();
        public MainWindow() //Инициализация при старте программы
        {
            InitializeComponent();
            AGrid.Children.Add(StringGrid1.Cells[0, 0]);
            BGrid.Children.Add(StringGrid2.Cells[0, 0]);
            StringGrid1.Cells[0, 0].TextChanged += StringGrid_Change;
            StringGrid1.Cells[0, 0].LostFocus += StringGrid_LostFocus;
            StringGrid1.Cells[0, 0].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x90, 0xFF, 0x81));
            StringGrid1.Cells[0, 0].TabIndex = 0;
            StringGrid2.Cells[0, 0].TextChanged += StringGrid_Change;
            StringGrid2.Cells[0, 0].LostFocus += StringGrid_LostFocus;
            StringGrid2.Cells[0, 0].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x90, 0xFF, 0x81));
            StringGrid2.Cells[0, 0].TabIndex = 1;
            StringGrid2.Cells[0, 0].Margin = new Thickness(StringGrid2.Cells[0, 0].Margin.Left + 25, StringGrid2.Cells[0, 0].Margin.Top, 0, 0);
            StringGrid3.SetMinRow(0);
            StringGrid3.DelRow();
            ClearMatrix.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control | ModifierKeys.Shift));
            CommandBindings.Add(new CommandBinding(ClearMatrix, ClearCommand));
        }
        public static int DS_Count(string s) //Decimal Separator Count
        {
            string substr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString();
            int count = (s.Length - s.Replace(substr, "").Length) / substr.Length;
            return count;
        }
        public static int Minus_Count(string s) //кол-во минусов
        {
            string substr = "-";
            int count = (s.Length - s.Replace(substr, "").Length) / substr.Length;
            return count;
        }
        public class TStringGrid //Класс сетки из TextBox
        {
            private uint RowCount;
            private uint ColCount;
            private uint MinRow;
            private uint MinCol;

            public TextBox[,] Cells;

            public TStringGrid() //конструктор
            {
                RowCount = 1;
                ColCount = 1;
                MinRow = 1;
                MinCol = 1;
                Cells = new TextBox[RowCount, ColCount];
                Cells[0, 0] = new TextBox();
                Cells[0, 0].PreviewTextInput += TStringGrid_PreviewTextInput;
                Cells[0, 0].PreviewKeyDown += TStringGrid_PreviewKeyDown;
                Cells[0, 0].TextAlignment = TextAlignment.Center;
                Cells[0, 0].HorizontalContentAlignment = HorizontalAlignment.Center;
                Cells[0, 0].VerticalContentAlignment = VerticalAlignment.Center;
                Cells[0, 0].Width = 50;
                Cells[0, 0].Height = 25;
                Cells[0, 0].HorizontalAlignment = HorizontalAlignment.Left;
                Cells[0, 0].VerticalAlignment = VerticalAlignment.Top;
                Cells[0, 0].Margin = new Thickness(0, 0, 0, 0);
                Cells[0, 0].ToolTip = "[1, 1]";
            }

            public void AddRow() //Добавление строки
            {
                RowCount++;
                TextBox[,] newCells = new TextBox[RowCount, ColCount];
                for (int i = 0; i < RowCount - 1; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        newCells[i, j] = Cells[i, j];
                    }
                }
                for (int j = 0; j < ColCount; j++)
                {
                    newCells[RowCount - 1, j] = new TextBox();
                    newCells[RowCount - 1, j].PreviewTextInput += TStringGrid_PreviewTextInput;
                    newCells[RowCount - 1, j].PreviewKeyDown += TStringGrid_PreviewKeyDown;
                    newCells[RowCount - 1, j].TextAlignment = TextAlignment.Center;
                    newCells[RowCount - 1, j].HorizontalContentAlignment = HorizontalAlignment.Center;
                    newCells[RowCount - 1, j].VerticalContentAlignment = VerticalAlignment.Center;
                    newCells[RowCount - 1, j].Width = 50;
                    newCells[RowCount - 1, j].Height = 25;
                    newCells[RowCount - 1, j].HorizontalAlignment = HorizontalAlignment.Left;
                    newCells[RowCount - 1, j].VerticalAlignment = VerticalAlignment.Top;
                    newCells[RowCount - 1, j].Margin = new Thickness(50 * j, 25 * (RowCount - 1), 0, 0);
                    newCells[RowCount - 1, j].ToolTip = "[" + RowCount.ToString() + ", " + (j + 1).ToString() + "]";
                }
                Cells = newCells;
            }
            public void AddCol() //Добавление столбца
            {
                ColCount++;
                TextBox[,] newCells = new TextBox[RowCount, ColCount];
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount - 1; j++)
                    {
                        newCells[i, j] = Cells[i, j];
                    }
                }
                for (int i = 0; i < RowCount; i++)
                {
                    newCells[i, ColCount - 1] = new TextBox();
                    newCells[i, ColCount - 1].PreviewTextInput += TStringGrid_PreviewTextInput;
                    newCells[i, ColCount - 1].PreviewKeyDown += TStringGrid_PreviewKeyDown;
                    newCells[i, ColCount - 1].TextAlignment = TextAlignment.Center;
                    newCells[i, ColCount - 1].HorizontalContentAlignment = HorizontalAlignment.Center;
                    newCells[i, ColCount - 1].VerticalContentAlignment = VerticalAlignment.Center;
                    newCells[i, ColCount - 1].Width = 50;
                    newCells[i, ColCount - 1].Height = 25;
                    newCells[i, ColCount - 1].HorizontalAlignment = HorizontalAlignment.Left;
                    newCells[i, ColCount - 1].VerticalAlignment = VerticalAlignment.Top;
                    newCells[i, ColCount - 1].Margin = new Thickness(50 * (ColCount - 1), 25 * i, 0, 0);
                    newCells[i, ColCount - 1].ToolTip = "[" + (i + 1).ToString() + ", " + ColCount.ToString() + "]";
                }
                Cells = newCells;
            }
            public void DelRow() //Удаление строки
            {
                if (RowCount > MinRow)
                {
                    RowCount--;
                    TextBox[,] newCells = new TextBox[RowCount, ColCount];
                    for (int i = 0; i < RowCount; i++)
                    {
                        for (int j = 0; j < ColCount; j++)
                        {
                            newCells[i, j] = Cells[i, j];
                        }
                    }
                    for (int j = 0; j < ColCount; j++)
                    {
                        Cells[RowCount, j] = null;
                    }
                    Cells = newCells;
                    newCells = null;
                }
            }
            public void DelCol() //Удаление столбца
            {
                if (ColCount > MinCol)
                {
                    ColCount--;
                    TextBox[,] newCells = new TextBox[RowCount, ColCount];
                    for (int i = 0; i < RowCount; i++)
                    {
                        for (int j = 0; j < ColCount; j++)
                        {
                            newCells[i, j] = Cells[i, j];
                        }
                    }
                    for (int i = 0; i < RowCount; i++)
                    {
                        Cells[i, ColCount] = null;
                    }
                    Cells = newCells;
                    newCells = null;
                }
            }
            public void SetMinRow(uint count) //Установка минимального кол-ва строк
            {
                MinRow = count;
            }
            public void SetMinCol(uint count)//Установка минимального кол-ва столбцов
            {
                MinCol = count;
            }
            public uint GetRowCount() //Получение текущего кол-ва строк
            {
                return RowCount;
            }
            public uint GetColCount() //Получение текущего кол-ва столбцов
            {
                return ColCount;
            }
            public uint GetMinRow() //Получение минимального кол-ва строк
            {
                return MinRow;
            }
            public uint GetMinCol() //Получение минимального кол-ва столбцов
            {
                return MinCol;
            }
            private void TStringGrid_PreviewTextInput(object sender, TextCompositionEventArgs e) //Начальная защита от "дурака"
            {
                if (e.Text != "")
                {
                    if (!(Minus_Count(((TextBox)sender).Text) < 1 && ((TextBox)sender).CaretIndex == 0 && e.Text == "-")) //запрет ввода больше одного минуса и минуса в середине числа
                        if (!(((TextBox)sender).CaretIndex == 0 && Minus_Count(((TextBox)sender).Text) > 0)) //запрет ввода числа перед минусом
                        {
                            e.Handled = !(Char.IsDigit(e.Text, 0) || ((e.Text == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString()) && (DS_Count(((TextBox)sender).Text) < 1))); //запрет ввода любых символов кроме чисел и одного десятичного разделителя
                            if (e.Text == "." && DS_Count(((TextBox)sender).Text) < 1) //замена символа "." на "," и проверка что десятичный разделитель входит в строку один раз
                            {
                                var eventArgs = new TextCompositionEventArgs(Keyboard.PrimaryDevice,
                                    new TextComposition(InputManager.Current, Keyboard.FocusedElement, ","));
                                eventArgs.RoutedEvent = TextInputEvent;

                                InputManager.Current.ProcessInput(eventArgs);
                            }
                        }
                        else
                            e.Handled = true;
                }
            }
            private void TStringGrid_PreviewKeyDown(object sender, KeyEventArgs e)
            {
                e.Handled = e.Key == Key.Space; //отдельно запрет ввода "Space" т.к. PreviewTextInput не обрабатывает нажатие клавиш <Ctrl>, <Shift>, <Backspace>, <Space>, клавиш управления курсором, функциональных клавиш и т.п.
            }
        }
        public void SetTabIndex() //Устанавливаем очередность Tab индекса
        {
            if (StringGrid1.GetRowCount() > StringGrid1.GetMinRow())
            {
                for (int i = 0; i < StringGrid1.GetRowCount() - 1; i++)
                {
                    for (int j = 0; j < StringGrid1.GetColCount() - 1; j++)
                    {
                        StringGrid1.Cells[i, j].TabIndex = (int)(i * (StringGrid1.GetColCount() - 1) + j);
                        StringGrid1.Cells[StringGrid1.GetRowCount() - 1, j].TabIndex = (int)((StringGrid1.GetRowCount() - 1) * (StringGrid1.GetColCount() - 1) + (StringGrid1.GetRowCount() - 1 + j));
                    }
                    StringGrid1.Cells[i, StringGrid1.GetColCount() - 1].TabIndex = (int)((StringGrid1.GetRowCount() - 1) * (StringGrid1.GetColCount() - 1) + i);
                    StringGrid2.Cells[i, 0].TabIndex = (int)(StringGrid1.GetRowCount() * StringGrid1.GetColCount() + i);
                }
                StringGrid1.Cells[StringGrid1.GetRowCount() - 1, StringGrid1.GetColCount() - 1].TabIndex = (int)(StringGrid1.GetRowCount() * StringGrid1.GetColCount() - 1);
                StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0].TabIndex = (int)(StringGrid1.GetRowCount() * StringGrid1.GetColCount() + StringGrid2.GetRowCount() - 1);
            }
            else
            {
                StringGrid1.Cells[0, 0].TabIndex = 0;
                StringGrid2.Cells[0, 0].TabIndex = 1;
            }
        }
        public void ProtectionFromFool() //Защита от "дурака"
        {
            for (int i = 0; i < StringGrid1.GetRowCount() - 1; i++)
            {
                for (int j = 0; j < StringGrid1.GetColCount() - 1; j++)
                {
                    //if (!StringGrid1.Cells[i, j].IsFocused)
                    if (StringGrid1.Cells[i, j].Text != "")
                    {
                        /*if (StringGrid1.Cells[i, j].Text == "") //замена пустых полей на 0
                        {
                            StringGrid1.Cells[i, j].Text = "0";
                        } */

                        if (StringGrid1.Cells[i, j].Text.Last() == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] || StringGrid1.Cells[i, j].Text.Last() == '-') //Если значение в конце текстового поля равно десятичному разделителю или "-" то дописываем 0 в конце
                        {
                            StringGrid1.Cells[i, j].Text += "0";
                        }
                        if (StringGrid1.Cells[i, j].Text.First() == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) //Если значение в начале текстового поля равно десятичному разделителю то дописываем 0 в начале
                        {
                            StringGrid1.Cells[i, j].Text = "0" + StringGrid1.Cells[i, j].Text;
                        }
                        if (StringGrid1.Cells[i, j].Text.First() == '-' && StringGrid1.Cells[i, j].Text.Length > 1 && StringGrid1.Cells[i, j].Text[1] == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) //Обрабатываем ситуацию "-,0123..." добавлением "0" после "-"
                        {
                            StringGrid1.Cells[i, j].Text = StringGrid1.Cells[i, j].Text.Insert(1, "0");
                        }
                    }
                }
                //if (!StringGrid2.Cells[i, 0].IsFocused)
                if (StringGrid2.Cells[i, 0].Text != "")
                {
                    /*if (StringGrid2.Cells[i, 0].Text == "") //замена пустых полей на 0
                    {
                        StringGrid2.Cells[i, 0].Text = "0";
                    }*/
                    if (StringGrid2.Cells[i, 0].Text.Last() == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] || StringGrid2.Cells[i, 0].Text.Last() == '-') //Если значение в конце текстового поля равно десятичному разделителю или "-" то дописываем 0 в конце
                    {
                        StringGrid2.Cells[i, 0].Text += "0";
                    }
                    if (StringGrid2.Cells[i, 0].Text.First() == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) //Если значение в начале текстового поля равно десятичному разделителю то дописываем 0 в начале
                    {
                        StringGrid2.Cells[i, 0].Text = "0" + StringGrid2.Cells[i, 0].Text;
                    }
                    if (StringGrid2.Cells[i, 0].Text.First() == '-' && StringGrid2.Cells[i, 0].Text.Length > 1 && StringGrid2.Cells[i, 0].Text[1] == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) //Обрабатываем ситуацию "-,0123..." добавлением "0" после "-"
                    {
                        StringGrid2.Cells[i, 0].Text = StringGrid2.Cells[i, 0].Text.Insert(1, "0");
                    }
                }
            }
        }
        public void CallRequiredProcedure(bool detail)
        {
            if (!detail)
            {
                label3.Visibility = Visibility.Hidden;
                for (int i = 0; i < StringGrid3.GetRowCount(); i++)
                    StringGrid3.Cells[i, 0].Text = "";
            }
            if (StringGrid1.GetRowCount() > 1)
            {
                //перевод из StreingGrid в матрицу и вектор и вызов необходимой функции в зависимости от того какая выбрана в выпадающем списке
                double[,] A = new double[StringGrid1.GetRowCount() - 1, StringGrid1.GetColCount() - 1];
                double[] b = new double[StringGrid2.GetRowCount() - 1];
                for (int i = 0; i < StringGrid1.GetRowCount() - 1; i++)
                {
                    for (int j = 0; j < StringGrid1.GetColCount() - 1; j++)
                    {
                        try
                        {
                            A[i, j] = Convert.ToDouble(StringGrid1.Cells[i, j].Text);
                        }
                        catch
                        {
                            string S = StringGrid1.Cells[i, j].Text;
                            if (S == "") //замена пустых полей на 0
                            {
                                S = "0";
                            }
                            if (S.Last() == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] || S.Last() == '-') //Если значение в конце текстового поля равно десятичному разделителю или "-" то дописываем 0 в конце
                            {
                                S += "0";
                            }
                            if (S.First() == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) //Если значение в начале текстового поля равно десятичному разделителю то дописываем 0 в начале
                            {
                                S = "0" + S;
                            }
                            if (S.First() == '-' && S.Length > 1 && S[1] == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) //Обрабатываем ситуацию "-,0123..." добавлением "0" после "-"
                            {
                                S = S.Insert(1, "0");
                            }
                            try
                            {
                                A[i, j] = Convert.ToDouble(S);
                            }
                            catch
                            {
                                A[i, j] = 0;
                            }
                        }
                    }
                }
                for (int i = 0; i < StringGrid2.GetRowCount() - 1; i++)
                {
                    try
                    {
                        b[i] = Convert.ToDouble(StringGrid2.Cells[i, 0].Text);
                    }
                    catch
                    {
                        string S = StringGrid2.Cells[i, 0].Text;
                        if (S == "") //замена пустых полей на 0
                        {
                            S = "0";
                        }
                        if (S.Last() == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] || S.Last() == '-') //Если значение в конце текстового поля равно десятичному разделителю или "-" то дописываем 0 в конце
                        {
                            S += "0";
                        }
                        if (S.First() == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) //Если значение в начале текстового поля равно десятичному разделителю то дописываем 0 в начале
                        {
                            S = "0" + S;
                        }
                        if (S.First() == '-' && S.Length > 1 && S[1] == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) //Обрабатываем ситуацию "-,0123..." добавлением "0" после "-"
                        {
                            S = S.Insert(1, "0");
                        }
                        try
                        {
                            b[i] = Convert.ToDouble(S);
                        }
                        catch
                        {
                            b[i] = 0;
                        }
                    }
                }
                if (detail)
                {
                    switch (comboBox.SelectedIndex)
                    {
                        case 0:
                            detGaussianElimination(A, b);
                            break;
                        case 1:
                            detGaussJordanElimination(A, b);
                            break;
                        case 2:
                            detLU_decomposition(A, b);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (comboBox.SelectedIndex)
                    {
                        case 0:
                            GaussianElimination(A, b);
                            break;
                        case 1:
                            GaussJordanElimination(A, b);
                            break;
                        case 2:
                            LU_decomposition(A, b);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                label3.Content = "Введите данные";
                label3.Visibility = Visibility.Visible;
                Paragraph warn = new Paragraph();
                detWindow.richTextBox.Document.Blocks.Add(warn);
                warn.Foreground = Brushes.Red;
                warn.FontWeight = FontWeights.Bold;
                warn.Inlines.Add("Введите матрицу A и вектор b");
            }
        }
        public bool detGaussianElimination(double[,] A, double[] b) //Генерация документа содержащего подробное решение СЛАУ методом Гаусса для окна "Подробно"
        {
            double[,] Ab = (double[,])A.Clone();
            double[] bb = (double[])b.Clone();
            #region Вывод исходной матрицы и вектора
            Paragraph Given = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Given);
            Given.Inlines.Add("Дана система " + b.Length.ToString() + "-го порядка");
            Given.Inlines.Add(new LineBreak());
            Given.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"\left(" + LaTeX_matrix(A) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(b) + @"\right)", 20.0);
            #endregion

            #region Проверяем вырожденость матрицы A
            Paragraph Determ = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Determ);
            Determ.Inlines.Add("Проверим матрицу A на вырожденность:");
            Determ.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"det(A) = \left|" + LaTeX_matrix(A) + @"\right|=" + LaTeX_Determinant(A), 20.0);
            Determ.Inlines.Add(new LineBreak());
            if (Determinant(A) != 0)
            {
                Determ.Inlines.Add("det(A) = " + Determinant(A).ToString() + " ≠ 0 => матрица A - невырожденная, а значит система совместна");
            }
            else
            {
                Determ.Inlines.Add("det(A) = 0 => матрица A - вырожденная, а значит система не совместна");
                return false;
            }
            #endregion

            #region Проверяем нули по диагонали
            bool iizero = false;
            for (int i = 0; i < b.Length - 1; i++)
                if (A[i, i] == 0)
                    iizero = true;
            #endregion

            #region Если есть нули по диагонали кроме элемента A[n,n] меняем строки местами
            if (iizero)
            {
                Paragraph Repair = new Paragraph();
                detWindow.richTextBox.Document.Blocks.Add(Repair);
                Repair.Inlines.Add("Необходимо поменять строки матрицы так, чтобы по диагонали не было нулей (кроме элемента A[n, n]):");
                Repair.Inlines.Add(new LineBreak());
                for (int i = 0; i < b.Length - 1; i++)
                {
                    if (A[i, i] == 0)
                    {
                        int j = i + 1;
                        while (j < b.Length && A[i, i] == 0)
                        {
                            if (A[j, i] != 0 && A[i, j] != 0)
                            {
                                string[,] swapMrx = new string[A.GetLength(0), A.GetLength(1)];
                                for (int x = 0; x < A.GetLength(0); x++)
                                    for (int y = 0; y < A.GetLength(1); y++)
                                        swapMrx[x, y] = A[x, y].ToString();
                                swapMrx[i, 0] = "[" + swapMrx[i, 0];
                                swapMrx[i, swapMrx.GetLength(1) - 1] += "]";
                                swapMrx[j, 0] = "[" + swapMrx[j, 0];
                                swapMrx[j, swapMrx.GetLength(1) - 1] += "]";
                                string[] swapVec = new string[b.Length];
                                for (int k = 0; k < b.Length; k++)
                                    swapVec[k] = b[k].ToString();
                                swapVec[i] = "[" + swapVec[i] + "]";
                                swapVec[j] = "[" + swapVec[j] + "]";
                                string SwapSOLE = @"\left(" + LaTeX_matrix(swapMrx) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(swapVec) + @"\right)\Rightarrow";
                                double temp;
                                for (int k = 0; k < b.Length; k++)
                                {
                                    temp = A[i, k];
                                    A[i, k] = A[j, k];
                                    A[j, k] = temp;
                                }
                                temp = b[i];
                                b[i] = b[j];
                                b[j] = temp;
                                SwapSOLE += @"\left(" + LaTeX_matrix(A) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(b) + @"\right)";
                                LaTeX_toDOC(SwapSOLE, 20.0);
                                Repair.Inlines.Add(new LineBreak());
                            }
                            j++;
                        }
                    }
                }
                for (int i = 0; i < b.Length - 1; i++)
                    if (A[i, i] == 0)
                    {
                        Repair.Inlines.Add(new LineBreak());
                        Repair.Inlines.Add("Не удалось заменить " + (i + 1).ToString() + "-ую  строку");
                        Repair.Inlines.Add(new LineBreak());
                        LaTeX_toDOC(@"det(A) = \left|" + LaTeX_matrix(A) + @"\right|=" + LaTeX_Determinant(A), 20.0);
                        Repair.Inlines.Add(new LineBreak());
                        Repair.Inlines.Add("det(A) = 0 => матрица A - вырожденная, а значит система не совместна");
                        Repair.Inlines.Add(new LineBreak());
                        return false;
                    }
            }
            #endregion

            #region Прямой ход
            Paragraph Direct = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Direct);
            Direct.Inlines.Add("Прямой ход алгоритма");
            Direct.Inlines.Add(new LineBreak());
            Direct.Inlines.Add("Приведём систему к трапецевидной форме:");
            Direct.Inlines.Add(new LineBreak());
            for (int i = 0; i < b.Length - 1; i++)
            {
                string SDirect;
                for (int j = i + 1; j < b.Length; j++)
                {
                    if (i == 0 && j == 1)
                    {
                        SDirect = @"\left(";
                    }
                    else
                    {
                        SDirect = @"=\left(";
                    }
                    string[,] swapMrx = new string[A.GetLength(0), A.GetLength(1)];
                    for (int x = 0; x < A.GetLength(0); x++)
                        for (int y = 0; y < A.GetLength(1); y++)
                            swapMrx[x, y] = A[x, y].ToString();
                    swapMrx[i, 0] = "[" + swapMrx[i, 0];
                    swapMrx[j, 0] = "[" + swapMrx[j, 0];
                    string[] swapVec = new string[b.Length];
                    for (int k = 0; k < b.Length; k++)
                        swapVec[k] = b[k].ToString();
                    swapVec[i] += "]";
                    swapVec[j] += "]";
                    string[] MulMatrix = new string[b.Length];
                    for (int k = 0; k < b.Length; k++)
                        MulMatrix[k] = @"\matrix{\\}";
                    MulMatrix[i] = @"*(" + A[j, i] + @"/" + A[i, i] + @")";
                    MulMatrix[j] = @"\leftarrow^\rfloor^-";
                    SDirect += LaTeX_matrix(swapMrx) + LaTeX_vector(@"\mid", b.Length, false) + LaTeX_vector(swapVec) + @"\right)" + LaTeX_vector(MulMatrix) + @"=";
                    double d = A[j, i] / A[i, i];

                    A[j, i] = 0;
                    for (int k = i + 1; k < b.Length; k++)
                    {
                        A[j, k] -= d * A[i, k];
                    }
                    b[j] -= d * b[i];
                    SDirect += @"\left(" + LaTeX_matrix(A) + LaTeX_vector(@"\mid", b.Length, false) + LaTeX_vector(b) + @"\right)";
                    if (i != b.Length - 2 || j != b.Length - 1)
                    {
                        SDirect += @"=";
                    }
                    LaTeX_toDOC(SDirect, 20.0);
                    Direct.Inlines.Add(new LineBreak());
                }
            }
            #endregion

            #region Обратный ход
            Paragraph Reverse = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Reverse);
            Reverse.Inlines.Add("Обратный ход");
            Reverse.Inlines.Add(new LineBreak());
            Reverse.Inlines.Add("Решим систему:");
            Reverse.Inlines.Add(new LineBreak());
            string SgetX = @"\cases{";
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    if (A[i, j] != 0)
                    {
                        if (A[i, j] != 1)
                        {
                            SgetX += A[i, j].ToString() + @" \cdot x_{" + (j + 1).ToString() + @"}+";
                        }
                        else
                        {
                            SgetX += @" x_{" + (j + 1).ToString() + @"}+";
                        }
                    }
                }
                SgetX = SgetX.Remove(SgetX.Length - 1);
                SgetX += @"=" + b[i].ToString() + @"\\";
            }
            SgetX = SgetX.Remove(SgetX.Length - 2);
            SgetX += @"} \Rightarrow ";
            LaTeX_toDOC(SgetX, 20.0);
            Reverse.Inlines.Add(new LineBreak());

            double[] X = new double[b.Length];
            for (int i = b.Length - 1; i >= 0; i--)
            {
                double S = 0;
                for (int j = i + 1; j < b.Length; j++)
                {
                    S += A[i, j] * X[j];
                }
                X[i] = (b[i] - S) / A[i, i];
            }
            for (int i = b.Length - 1; i >= 0; i--)
            {
                SgetX = @"\Rightarrow \cases{";
                for (int k = 0; k < i; k++)
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        if (A[k, j] != 0)
                        {
                            if (A[k, j] != 1)
                            {
                                SgetX += A[k, j].ToString() + @" \cdot x_{" + (j + 1).ToString() + @"}+";
                            }
                            else
                            {
                                SgetX += @" x_{" + (j + 1).ToString() + @"}+";
                            }
                        }
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @"=" + b[k].ToString() + @"\\";
                }
                if (i != b.Length - 1)
                {
                    SgetX += @"x_{" + (i + 1).ToString() + @"} = \frac{" + b[i].ToString() + @" - (";
                    for (int j = i + 1; j < b.Length; j++)
                    {
                        SgetX += A[i, j].ToString() + @" \cdot " + X[j] + "+";
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @")}{" + A[i, i].ToString() + @"}\\";
                    for (int j = i + 1; j < b.Length; j++)
                    {
                        SgetX += @"x_{" + (j + 1).ToString() + @"} = " + X[j].ToString() + @"\\";
                    }
                }
                else
                {
                    SgetX += @"x_{" + (i + 1).ToString() + @"} = \frac{" + b[i].ToString() + "}{" + A[i, i].ToString() + @"}\\";
                }

                SgetX = SgetX.Remove(SgetX.Length - 2);
                SgetX += @"}\Rightarrow \cases{";
                for (int k = 0; k < i; k++)
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        if (A[k, j] != 0)
                        {
                            if (A[k, j] != 1)
                            {
                                SgetX += A[k, j].ToString() + @" \cdot x_{" + (j + 1).ToString() + @"}+";
                            }
                            else
                            {
                                SgetX += @" x_{" + (j + 1).ToString() + @"}+";
                            }
                        }
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @"=" + b[k].ToString() + @"\\";
                }
                for (int j = i; j < b.Length; j++)
                {
                    SgetX += @"x_{" + (j + 1).ToString() + @"} = " + X[j].ToString() + @"\\";
                }
                SgetX = SgetX.Remove(SgetX.Length - 2);

                if (i != 0)
                {
                    SgetX += @"}\Rightarrow";
                }
                else
                {
                    SgetX += @"}";
                }
                LaTeX_toDOC(SgetX, 20.0);
                Reverse.Inlines.Add(new LineBreak());
            }
            #endregion

            #region Ответ x
            Paragraph Res = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Res);
            Res.Inlines.Add("Отсюда получаем:");
            Res.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"x^{*}=\left(" + LaTeX_vector("x", b.Length, true) + @"\right) = \left(" + LaTeX_vector(X) + @"\right)", 20.0);
            #endregion

            A = Ab;
            b = bb;

            #region Вычисление вектора-невязки
            Paragraph nVec = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(nVec);
            nVec.Inlines.Add("Вычислим вектор невязки:");
            nVec.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"r=Ax^{*}-b \Rightarrow \left(" + LaTeX_vector("r", b.Length, true) + @"\right) = \left(" + LaTeX_matrix(A) + @"\right) \left(" + LaTeX_vector(X) + @"\right) - \left(" + LaTeX_vector(b) + @"\right) =", 20.0);
            nVec.Inlines.Add(new LineBreak());
            string[] SnVec = new string[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                SnVec[i] = @"";
                for (int j = 0; j < b.Length; j++)
                {
                    if (A[i, j] < 0)
                    {
                        SnVec[i] += @"(" + A[i, j].ToString() + @")";
                    }
                    else
                    {
                        SnVec[i] += A[i, j].ToString();
                    }
                    SnVec[i] += @" \cdot ";
                    if (X[j] < 0)
                    {
                        SnVec[i] += @"(" + X[j].ToString() + @")";
                    }
                    else
                    {
                        SnVec[i] += X[j].ToString();
                    }
                    if (j != b.Length - 1)
                    {
                        SnVec[i] += @" + ";
                    }
                }
                if (i != b.Length - 1)
                {
                    SnVec[i] += @"\\";
                }
            }
            LaTeX_toDOC(@"=\left(" + LaTeX_vector(SnVec) + @"\right) - \left(" + LaTeX_vector(b) + @"\right)=", 20.0);
            nVec.Inlines.Add(new LineBreak());
            double[] Ax = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    Ax[i] += A[i, j] * X[j];
                }
            }
            double[] Axb = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                Axb[i] = Ax[i] - b[i];
            }
            LaTeX_toDOC(@"=\left(" + LaTeX_vector(Ax) + @" \right) - \left(" + LaTeX_vector(b) + @"\right) = \left(" + LaTeX_vector(Axb) + @"\right)", 20.0);
            nVec.Inlines.Add(new LineBreak());
            #endregion

            return true;
        }
        public bool detGaussJordanElimination(double[,] A, double[] b) //Генерация документа содержащего подробное решение СЛАУ методом Гаусса-Жордана для окна "Подробно"
        {
            double[,] Ab = (double[,])A.Clone();
            double[] bb = (double[])b.Clone();

            #region Вывод исходной матрицы и вектора
            Paragraph Given = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Given);
            Given.Inlines.Add("Дана система " + b.Length.ToString() + "-го порядка");
            Given.Inlines.Add(new LineBreak());
            Given.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"\left(" + LaTeX_matrix(A) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(b) + @"\right)", 20.0);
            #endregion

            #region Проверяем вырожденость матрицы A
            Paragraph Determ = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Determ);
            Determ.Inlines.Add("Проверим матрицу A на вырожденность:");
            Determ.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"det(A) = \left|" + LaTeX_matrix(A) + @"\right|=" + LaTeX_Determinant(A), 20.0);
            Determ.Inlines.Add(new LineBreak());
            if (Determinant(A) != 0)
            {
                Determ.Inlines.Add("det(A) = " + Determinant(A).ToString() + " ≠ 0 => матрица A - невырожденная, а значит система совместна");
            }
            else
            {
                Determ.Inlines.Add("det(A) = 0 => матрица A - вырожденная, а значит система не совместна");
                return false;
            }
            #endregion

            #region Проверяем нули по диагонали
            bool iizero = false;
            for (int i = 0; i < b.Length - 1; i++)
                if (A[i, i] == 0)
                    iizero = true;
            #endregion

            #region Если есть нули по диагонали кроме элемента A[n,n] меняем строки местами
            if (iizero)
            {
                Paragraph Repair = new Paragraph();
                detWindow.richTextBox.Document.Blocks.Add(Repair);
                Repair.Inlines.Add("Необходимо поменять строки матрицы так, чтобы по диагонали не было нулей (кроме элемента A[n, n]):");
                Repair.Inlines.Add(new LineBreak());
                for (int i = 0; i < b.Length - 1; i++)
                {
                    if (A[i, i] == 0)
                    {
                        int j = i + 1;
                        while (j < b.Length && A[i, i] == 0)
                        {
                            if (A[j, i] != 0 && A[i, j] != 0)
                            {
                                string[,] swapMrx = new string[A.GetLength(0), A.GetLength(1)];
                                for (int x = 0; x < A.GetLength(0); x++)
                                    for (int y = 0; y < A.GetLength(1); y++)
                                        swapMrx[x, y] = A[x, y].ToString();
                                swapMrx[i, 0] = "[" + swapMrx[i, 0];
                                swapMrx[i, swapMrx.GetLength(1) - 1] += "]";
                                swapMrx[j, 0] = "[" + swapMrx[j, 0];
                                swapMrx[j, swapMrx.GetLength(1) - 1] += "]";
                                string[] swapVec = new string[b.Length];
                                for (int k = 0; k < b.Length; k++)
                                    swapVec[k] = b[k].ToString();
                                swapVec[i] = "[" + swapVec[i] + "]";
                                swapVec[j] = "[" + swapVec[j] + "]";
                                string SwapSOLE = @"\left(" + LaTeX_matrix(swapMrx) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(swapVec) + @"\right)\Rightarrow";
                                double temp;
                                for (int k = 0; k < b.Length; k++)
                                {
                                    temp = A[i, k];
                                    A[i, k] = A[j, k];
                                    A[j, k] = temp;
                                }
                                temp = b[i];
                                b[i] = b[j];
                                b[j] = temp;
                                SwapSOLE += @"\left(" + LaTeX_matrix(A) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(b) + @"\right)";
                                LaTeX_toDOC(SwapSOLE, 20.0);
                                Repair.Inlines.Add(new LineBreak());
                            }
                            j++;
                        }
                    }
                }
                for (int i = 0; i < b.Length - 1; i++)
                    if (A[i, i] == 0)
                    {
                        Repair.Inlines.Add(new LineBreak());
                        Repair.Inlines.Add("Не удалось заменить " + (i + 1).ToString() + "-ую  строку");
                        Repair.Inlines.Add(new LineBreak());
                        LaTeX_toDOC(@"det(A) = \left|" + LaTeX_matrix(A) + @"\right|=" + LaTeX_Determinant(A), 20.0);
                        Repair.Inlines.Add(new LineBreak());
                        Repair.Inlines.Add("det(A) = 0 => матрица A - вырожденная, а значит система не совместна");
                        Repair.Inlines.Add(new LineBreak());
                        return false;
                    }
            }
            #endregion

            #region Прямой ход
            Paragraph Direct = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Direct);
            Direct.Inlines.Add("Прямой ход алгоритма:");
            Direct.Inlines.Add(new LineBreak());
            for (int i = 0; i < b.Length; i++)
            {
                string SDirect;
                if (i == 0)
                {
                    SDirect = @"\left(";
                }
                else
                {
                    SDirect = @"=\left(";
                }
                string[,] swapMrx0 = new string[A.GetLength(0), A.GetLength(1)];
                for (int x = 0; x < A.GetLength(0); x++)
                    for (int y = 0; y < A.GetLength(1); y++)
                        swapMrx0[x, y] = A[x, y].ToString();
                swapMrx0[i, i] = "[" + swapMrx0[i, i];
                string[] swapVec0 = new string[b.Length];
                for (int k = 0; k < b.Length; k++)
                    swapVec0[k] = b[k].ToString();
                swapVec0[i] += "]";
                string[] MulMatrix0 = new string[b.Length];
                for (int k = 0; k < b.Length; k++)
                    MulMatrix0[k] = @"\matrix{\\}";
                MulMatrix0[i] = @"/(" + A[i, i] + @")";
                SDirect += LaTeX_matrix(swapMrx0) + LaTeX_vector(@"\mid", b.Length, false) + LaTeX_vector(swapVec0) + @"\right)" + LaTeX_vector(MulMatrix0) + @"=";
                double c = A[i, i];
                b[i] /= c;
                for (int j = i; j < b.Length; j++)
                    A[i, j] /= c;
                SDirect += @"\left(" + LaTeX_matrix(A) + LaTeX_vector(@"\mid", b.Length, false) + LaTeX_vector(b) + @"\right)";
                if (i != b.Length - 1)
                {
                    SDirect += @"=";
                }
                LaTeX_toDOC(SDirect, 20.0);
                Direct.Inlines.Add(new LineBreak());
                if (i != b.Length - 1)
                {
                    for (int j = i + 1; j < b.Length; j++)
                    {
                        SDirect = @"=\left(";
                        string[,] swapMrx = new string[A.GetLength(0), A.GetLength(1)];
                        for (int x = 0; x < A.GetLength(0); x++)
                            for (int y = 0; y < A.GetLength(1); y++)
                                swapMrx[x, y] = A[x, y].ToString();
                        swapMrx[i, 0] = "[" + swapMrx[i, 0];
                        swapMrx[j, 0] = "[" + swapMrx[j, 0];
                        string[] swapVec = new string[b.Length];
                        for (int k = 0; k < b.Length; k++)
                            swapVec[k] = b[k].ToString();
                        swapVec[i] += "]";
                        swapVec[j] += "]";
                        string[] MulMatrix = new string[b.Length];
                        for (int k = 0; k < b.Length; k++)
                            MulMatrix[k] = @"\matrix{\\}";
                        MulMatrix[i] = @"*(" + A[j, i] + @")";
                        MulMatrix[j] = @"\leftarrow^\rfloor^-";
                        SDirect += LaTeX_matrix(swapMrx) + LaTeX_vector(@"\mid", b.Length, false) + LaTeX_vector(swapVec) + @"\right)" + LaTeX_vector(MulMatrix) + @"=";
                        double d = A[j, i];

                        A[j, i] = 0;
                        for (int k = i + 1; k < b.Length; k++)
                        {
                            A[j, k] -= d * A[i, k];
                        }
                        b[j] -= d * b[i];
                        SDirect += @"\left(" + LaTeX_matrix(A) + LaTeX_vector(@"\mid", b.Length, false) + LaTeX_vector(b) + @"\right)=";
                        LaTeX_toDOC(SDirect, 20.0);
                        Direct.Inlines.Add(new LineBreak());
                    }
                }
            }
            #endregion

            #region Обратный ход
            Paragraph Reverse = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Reverse);
            Reverse.Inlines.Add("Обратный ход алгоритма:");
            Reverse.Inlines.Add(new LineBreak());
            for (int i = b.Length - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    string SDirect = @"=\left(";
                    string[,] swapMrx = new string[A.GetLength(0), A.GetLength(1)];
                    for (int x = 0; x < A.GetLength(0); x++)
                        for (int y = 0; y < A.GetLength(1); y++)
                            swapMrx[x, y] = A[x, y].ToString();
                    swapMrx[i, 0] = "[" + swapMrx[i, 0];
                    swapMrx[j, 0] = "[" + swapMrx[j, 0];
                    string[] swapVec = new string[b.Length];
                    for (int k = 0; k < b.Length; k++)
                        swapVec[k] = b[k].ToString();
                    swapVec[i] += "]";
                    swapVec[j] += "]";
                    string[] MulMatrix = new string[b.Length];
                    for (int k = 0; k < b.Length; k++)
                        MulMatrix[k] = @"\matrix{\\}";
                    MulMatrix[i] = @"*(" + A[j, i] + @")";
                    MulMatrix[j] = @"\leftarrow_\rceil_-";
                    SDirect += LaTeX_matrix(swapMrx) + LaTeX_vector(@"\mid", b.Length, false) + LaTeX_vector(swapVec) + @"\right)" + LaTeX_vector(MulMatrix) + @"=";
                    double d = A[j, i];

                    A[j, i] = 0;
                    for (int k = i + 1; k < b.Length; k++)
                    {
                        A[j, k] -= d * A[i, k];
                    }
                    b[j] -= d * b[i];
                    SDirect += @"\left(" + LaTeX_matrix(A) + LaTeX_vector(@"\mid", b.Length, false) + LaTeX_vector(b) + @"\right)";
                    if (i != 1 || j != 0)
                    {
                        SDirect += @"=";
                    }
                    LaTeX_toDOC(SDirect, 20.0);
                    Reverse.Inlines.Add(new LineBreak());
                }
            }
            #endregion

            double[] X = (double[])b.Clone();

            #region Ответ x
            Paragraph Res = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Res);
            Res.Inlines.Add("Отсюда получаем:");
            Res.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"x^{*}=\left(" + LaTeX_vector("x", b.Length, true) + @"\right) = \left(" + LaTeX_vector(X) + @"\right)", 20.0);
            #endregion

            A = Ab;
            b = bb;

            #region Вычисление вектора-невязки
            Paragraph nVec = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(nVec);
            nVec.Inlines.Add("Вычислим вектор невязки:");
            nVec.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"r=Ax^{*}-b \Rightarrow \left(" + LaTeX_vector("r", b.Length, true) + @"\right) = \left(" + LaTeX_matrix(A) + @"\right) \left(" + LaTeX_vector(X) + @"\right) - \left(" + LaTeX_vector(b) + @"\right) =", 20.0);
            nVec.Inlines.Add(new LineBreak());
            string[] SnVec = new string[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                SnVec[i] = @"";
                for (int j = 0; j < b.Length; j++)
                {
                    if (A[i, j] < 0)
                    {
                        SnVec[i] += @"(" + A[i, j].ToString() + @")";
                    }
                    else
                    {
                        SnVec[i] += A[i, j].ToString();
                    }
                    SnVec[i] += @" \cdot ";
                    if (X[j] < 0)
                    {
                        SnVec[i] += @"(" + X[j].ToString() + @")";
                    }
                    else
                    {
                        SnVec[i] += X[j].ToString();
                    }
                    if (j != b.Length - 1)
                    {
                        SnVec[i] += @" + ";
                    }
                }
                if (i != b.Length - 1)
                {
                    SnVec[i] += @"\\";
                }
            }
            LaTeX_toDOC(@"=\left(" + LaTeX_vector(SnVec) + @"\right) - \left(" + LaTeX_vector(b) + @"\right)=", 20.0);
            nVec.Inlines.Add(new LineBreak());
            double[] Ax = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    Ax[i] += A[i, j] * X[j];
                }
            }
            double[] Axb = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                Axb[i] = Ax[i] - b[i];
            }
            LaTeX_toDOC(@"=\left(" + LaTeX_vector(Ax) + @" \right) - \left(" + LaTeX_vector(b) + @"\right) = \left(" + LaTeX_vector(Axb) + @"\right)", 20.0);
            nVec.Inlines.Add(new LineBreak());
            #endregion

            return true;
        }
        public string LaTeX_matrix(double[,] A) //Генерация LaTeX матрицы из двумерного вещественного массива
        {
            string S = @"\matrix{";
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    S += A[i, j].ToString() + @"&";
                }
                S = S.Remove(S.Length - 1);
                S += @"\\";
            }
            S = S.Remove(S.Length - 2);
            S += @"}";
            return S;
        }
        public string LaTeX_vector(double[] b) //Генерация LaTeX вектора из одномерного вещественного массива
        {
            string S = @"\matrix{";
            for (int i = 0; i < b.Length; i++)
            {
                S += b[i].ToString() + @"\\";
            }
            S = S.Remove(S.Length - 2);
            S += @"}";
            return S;
        }
        public string LaTeX_vector(string[] b) //Генерация LaTeX вектора из одномерного массива строк
        {
            string S = @"\matrix{";
            for (int i = 0; i < b.Length; i++)
            {
                S += b[i] + @"\\";
            }
            S = S.Remove(S.Length - 2);
            S += @"}";
            return S;
        }
        public string LaTeX_vector(string Sym, int count, bool Index) // Генерация LaTeX вектора из символа(строки) Sym в кол-ве count, Index true - с нижним индексом
        {
            string S = @"\matrix{";
            for (int i = 0; i < count; i++)
            {
                if (Index)
                {
                    S += Sym + "_{" + (i + 1).ToString() + @"}\\";
                }
                else
                {
                    S += Sym + @"\\";
                }
            }
            S = S.Remove(S.Length - 2);
            S += @"}";
            return S;
        }
        public string LaTeX_matrix(string[,] A) //Генерация LaTeX матрицы из двумерного массива строк
        {
            string S = @"\matrix{";
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    S += A[i, j] + @"&";
                }
                S = S.Remove(S.Length - 1);
                S += @"\\";
            }
            S = S.Remove(S.Length - 2);
            S += @"}";
            return S;
        }
        public string LaTeX_Determinant(double[,] A) //Генерация LaTeX для расчета определителся матрицы
        {
            if (A.GetLength(0) <= 2)
            {
                if (A.GetLength(0) == 2)
                {
                    if (A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0] >= 0)
                        return (A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0]).ToString();
                    else
                        return @"(" + (A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0]).ToString() + @")";
                }
                else
                {
                    if (A[0, 0] >= 0)
                        return (A[0, 0]).ToString();
                    else
                        return @"(" + (A[0, 0].ToString()) + @")";
                }
            }
            else
            {
                string det = @"";
                double[,] B = new double[A.GetLength(0) - 1, A.GetLength(1) - 1];
                for (int k = 0; k < A.GetLength(0); k++)
                {
                    for (int i = 1; i < A.GetLength(0); i++)
                        for (int j = 0; j < B.GetLength(1); j++)
                        {
                            if (j < k)
                            {
                                B[i - 1, j] = A[i, j];
                            }
                            else
                            {
                                B[i - 1, j] = A[i, j + 1];
                            }
                        }
                    if (A[0, k] >= 0)
                    {
                        det += A[0, k].ToString() + @" \cdot (-1)^{1+" + (k + 1).ToString() + @"} \cdot \left|" + LaTeX_matrix(B) + @"\right| + ";
                    }
                    else
                    {
                        det += @"(" + A[0, k].ToString() + @") \cdot (-1)^{1+" + (k + 1).ToString() + @"} \cdot \left|" + LaTeX_matrix(B) + @"\right| + ";
                    }
                }
                det = det.Remove(det.Length - 3);
                return det + @"=" + Determinant(A).ToString();
            }
        }
        public bool detLU_decomposition(double[,] A, double[] b) //Генерация документа содержащего подробное решение СЛАУ методом LU-разложение для окна "Подробно"
        {
            #region Вывод исходной матрицы и вектора
            Paragraph Given = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Given);
            Given.Inlines.Add("Дана система " + b.Length.ToString() + "-го порядка");
            Given.Inlines.Add(new LineBreak());
            Given.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"\left(" + LaTeX_matrix(A) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(b) + @"\right)", 20.0);
            #endregion

            #region Проверяем нули по диагонали
            bool iizero = false;
            for (int i = 0; i < b.Length - 1; i++)
                if (A[i, i] == 0)
                    iizero = true;
            #endregion

            #region Если есть нули по диагонали кроме элемента A[n,n] меняем строки местами
            if (iizero)
            {
                Paragraph Repair = new Paragraph();
                detWindow.richTextBox.Document.Blocks.Add(Repair);
                Repair.Inlines.Add("Необходимо поменять строки матрицы так, чтобы по диагонали не было нулей (кроме элемента A[n, n]):");
                Repair.Inlines.Add(new LineBreak());
                for (int i = 0; i < b.Length - 1; i++)
                {
                    if (A[i, i] == 0)
                    {
                        int j = i + 1;
                        while (j < b.Length && A[i, i] == 0)
                        {
                            if (A[j, i] != 0 && A[i, j] != 0)
                            {
                                string[,] swapMrx = new string[A.GetLength(0), A.GetLength(1)];
                                for (int x = 0; x < A.GetLength(0); x++)
                                    for (int y = 0; y < A.GetLength(1); y++)
                                        swapMrx[x, y] = A[x, y].ToString();
                                swapMrx[i, 0] = "[" + swapMrx[i, 0];
                                swapMrx[i, swapMrx.GetLength(1) - 1] += "]";
                                swapMrx[j, 0] = "[" + swapMrx[j, 0];
                                swapMrx[j, swapMrx.GetLength(1) - 1] += "]";
                                string[] swapVec = new string[b.Length];
                                for (int k = 0; k < b.Length; k++)
                                    swapVec[k] = b[k].ToString();
                                swapVec[i] = "[" + swapVec[i] + "]";
                                swapVec[j] = "[" + swapVec[j] + "]";
                                string SwapSOLE = @"\left(" + LaTeX_matrix(swapMrx) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(swapVec) + @"\right)\Rightarrow";
                                double temp;
                                for (int k = 0; k < b.Length; k++)
                                {
                                    temp = A[i, k];
                                    A[i, k] = A[j, k];
                                    A[j, k] = temp;
                                }
                                temp = b[i];
                                b[i] = b[j];
                                b[j] = temp;
                                SwapSOLE += @"\left(" + LaTeX_matrix(A) + @"\right)\left(" + LaTeX_vector("x", b.Length, true) + @"\right)=\left(" + LaTeX_vector(b) + @"\right)";
                                LaTeX_toDOC(SwapSOLE, 20.0);
                                Repair.Inlines.Add(new LineBreak());
                            }
                            j++;
                        }
                    }
                }
                for (int i = 0; i < b.Length - 1; i++)
                    if (A[i, i] == 0)
                    {
                        Repair.Inlines.Add(new LineBreak());
                        Repair.Inlines.Add("Не удалось заменить " + (i + 1).ToString() + "-ую  строку");
                        Repair.Inlines.Add(new LineBreak());
                        LaTeX_toDOC(@"det(A) = \left|" + LaTeX_matrix(A) + @"\right|=" + LaTeX_Determinant(A), 20.0);
                        Repair.Inlines.Add(new LineBreak());
                        Repair.Inlines.Add("det(A) = 0 => матрица A - вырожденная, а значит система не совместна");
                        Repair.Inlines.Add(new LineBreak());
                        return false;
                    }
            }
            #endregion

            #region Проверяем ведущие (угловые) главные миноры на вырожденность
            Paragraph LeadingPrincipal = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(LeadingPrincipal);
            LeadingPrincipal.Inlines.Add("Проверим все ведущие (угловые) главные миноры на вырожденность:");
            LeadingPrincipal.Inlines.Add(new LineBreak());
            for (int k = 0; k < A.GetLength(0); k++)
            {
                string LeadingPrincipalS = @"\Delta_{" + (k + 1).ToString() + @"} = ";
                double[,] B = new double[k + 1, k + 1];
                for (int i = 0; i <= k; i++)
                {
                    for (int j = 0; j <= k; j++)
                    {
                        B[i, j] = A[i, j];
                    }
                }
                LeadingPrincipalS += @"\left|" + LaTeX_matrix(B) + @"\right|=" + LaTeX_Determinant(B);
                if (Determinant(B) == 0)
                {
                    LeadingPrincipalS += @" \Rightarrow";
                    LaTeX_toDOC(LeadingPrincipalS, 20.0);
                    LeadingPrincipal.Inlines.Add("не существует LU-разложения");
                    LeadingPrincipal.Inlines.Add(new LineBreak());
                    return false;
                }
                LeadingPrincipalS += @"\neq 0";
                LaTeX_toDOC(LeadingPrincipalS, 20.0);
                LeadingPrincipal.Inlines.Add(new LineBreak());
            }
            LeadingPrincipal.Inlines.Add("Все ведущие главные миноры матрицы A не равны 0, следовательно ∃L,U такие что A=L∙U");
            #endregion

            #region Находим матрицы L и U
            double[,] ll = new double[b.Length, b.Length];
            double[,] uu = new double[b.Length, b.Length];
            Paragraph LandUmatrix = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(LandUmatrix);
            LandUmatrix.Inlines.Add("Найдем нижнюю треугольную матрицу L и верхнюю треугольную матрицу U:");
            LandUmatrix.Inlines.Add(new LineBreak());
            string LandUgen = @"";
            for (int k = 0; k < b.Length; k++)
            {
                uu[0, k] = A[0, k];
                LandUgen += @"u_{1, " + (k + 1).ToString() + @"}=a_{1, " + (k + 1).ToString() + @"}=" + uu[0, k].ToString() + @"\\";
            }
            LandUgen = LandUgen.Remove(LandUgen.Length - 2);
            LaTeX_toDOC(LandUgen, 20.0);
            LandUmatrix.Inlines.Add(new LineBreak());
            LandUgen = @"l_{1,1} = 1\\";
            ll[0, 0] = 1.0;
            for (int k = 1; k < b.Length; k++)
            {
                ll[k, 0] = A[k, 0] / uu[0, 0];
                LandUgen += @"l_{" + (k + 1).ToString() + @", 1}=\frac{a_{" + (k + 1).ToString() + @", 1}}{u_{1, 1}}=" + ll[k, 0].ToString() + @"\\";
            }
            LandUgen = LandUgen.Remove(LandUgen.Length - 2);
            LaTeX_toDOC(LandUgen, 20.0);
            LandUmatrix.Inlines.Add(new LineBreak());
            for (int i = 1; i < b.Length; i++)
            {
                double S = 0;
                for (int k = 0; k < i; k++)
                {
                    S += ll[i, k] * uu[k, i];
                }
                uu[i, i] = A[i, i] - S;
                LandUgen = @"u_{" + (i + 1).ToString() + @", " + (i + 1).ToString() + @"} = a_{" + (i + 1).ToString() + @", " + (i + 1).ToString() + @"}- \sum_{i = 1}^{" + i.ToString() + @"} {l_{" + (i + 1).ToString() + @", i} u_{i, " + (i + 1).ToString() + @"}} = " + A[i, i].ToString() + @"-" + S.ToString() + "=" + uu[i, i].ToString();
                LaTeX_toDOC(LandUgen, 20.0);
                LandUmatrix.Inlines.Add(new LineBreak());
                for (int j = i + 1; j < b.Length; j++)
                {
                    S = 0;
                    for (int k = 0; k < i; k++)
                    {
                        S += ll[i, k] * uu[k, j];
                    }
                    uu[i, j] = A[i, j] - S;
                    LandUgen = @"u_{" + (i + 1).ToString() + @", " + (j + 1).ToString() + @"} = a_{" + (i + 1).ToString() + @", " + (j + 1).ToString() + @"}- \sum_{i = 1}^{" + i.ToString() + @"} {l_{" + (i + 1).ToString() + @", i} u_{i, " + (j + 1).ToString() + @"}} = " + A[i, j].ToString() + @"-" + S.ToString() + "=" + uu[i, j].ToString();
                    LaTeX_toDOC(LandUgen, 20.0);
                    LandUmatrix.Inlines.Add(new LineBreak());
                }
                for (int j = i; j < b.Length; j++)
                {
                    S = 0;
                    for (int k = 0; k < i; k++)
                    {
                        S += ll[j, k] * uu[k, i];
                    }
                    ll[j, i] = (A[j, i] - S) / uu[i, i];
                    LandUgen = @"l_{" + (j + 1).ToString() + @", " + (i + 1).ToString() + @"} = \frac{a_{" + (j + 1).ToString() + @", " + (i + 1).ToString() + @"}- \sum_{i = 1}^{" + i.ToString() + @"} {l_{" + (j + 1).ToString() + @", i} u_{i, " + (i + 1).ToString() + @"}}}{u_{" + (i + 1).ToString() + @", " + (i + 1).ToString() + @"}} = \frac{" + A[j, i].ToString() + @"-" + S.ToString() + "}{" + uu[i, i] + @"}=" + ll[j, i].ToString();
                    LaTeX_toDOC(LandUgen, 20.0);
                    LandUmatrix.Inlines.Add(new LineBreak());
                }
            }
            LandUgen = @"L = \left(" + LaTeX_matrix(ll) + @"\right)";
            LaTeX_toDOC(LandUgen, 20.0);
            LandUmatrix.Inlines.Add(new LineBreak());
            LandUgen = @"U = \left(" + LaTeX_matrix(uu) + @"\right)";
            LaTeX_toDOC(LandUgen, 20.0);
            LandUmatrix.Inlines.Add(new LineBreak());
            LandUmatrix.Inlines.Add(new LineBreak());
            LandUmatrix.Inlines.Add("Проверим:");
            LandUmatrix.Inlines.Add(new LineBreak());
            LandUgen = @"L \cdot U = \left(" + LaTeX_matrix(ll) + @" \right) \cdot \left(" + LaTeX_matrix(uu) + @"\right) = ";
            LaTeX_toDOC(LandUgen, 20.0);
            LandUmatrix.Inlines.Add(new LineBreak());
            LandUgen = @" = \left(";
            string[] SLmulU = new string[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                SLmulU[i] = @"";
                for (int j = 0; j < b.Length; j++)
                {
                    for (int k = 0; k < b.Length; k++)
                    {
                        SLmulU[i] += ll[i, k].ToString() + @" \cdot " + uu[k, j].ToString() + @"+";
                    }
                    SLmulU[i] = SLmulU[i].Remove(SLmulU[i].Length - 1);
                    SLmulU[i] += " & ";
                }
                SLmulU[i] = SLmulU[i].Remove(SLmulU[i].Length - 3);
            }
            LandUgen += LaTeX_vector(SLmulU) + @"\right) = ";
            LaTeX_toDOC(LandUgen, 12.0);
            LandUmatrix.Inlines.Add(new LineBreak());
            LandUgen = @" = \left(" + LaTeX_matrix(A) + @"\right)";
            LaTeX_toDOC(LandUgen, 20.0);
            LandUmatrix.Inlines.Add(new LineBreak());
            LandUmatrix.Inlines.Add("Матрицы L и U найдены верно");
            #endregion

            #region Решаем получившуюся СЛАУ
            Paragraph GetX = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(GetX);
            GetX.Inlines.Add("Решим систему:");
            GetX.Inlines.Add(new LineBreak());
            string SgetX = @"\cases{Ly=b\\Ux=y} \Rightarrow \cases{";
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (ll[i, j] != 0)
                    {
                        if (ll[i, j] != 1)
                        {
                            SgetX += ll[i, j].ToString() + @" \cdot y_{" + (j + 1).ToString() + @"}+";
                        }
                        else
                        {
                            SgetX += @" y_{" + (j + 1).ToString() + @"}+";
                        }
                    }
                }
                SgetX = SgetX.Remove(SgetX.Length - 1);
                SgetX += @"=" + b[i].ToString() + @"\\";
            }
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    if (uu[i, j] != 0)
                    {
                        if (uu[i, j] != 1)
                        {
                            SgetX += uu[i, j].ToString() + @" \cdot x_{" + (j + 1).ToString() + @"}+";
                        }
                        else
                        {
                            SgetX += @" x_{" + (j + 1).ToString() + @"}+";
                        }
                    }
                }
                SgetX = SgetX.Remove(SgetX.Length - 1);
                SgetX += @"=y_{" + (i + 1).ToString() + @"}\\";
            }
            SgetX = SgetX.Remove(SgetX.Length - 2);
            SgetX += @"} \Rightarrow ";
            LaTeX_toDOC(SgetX, 20.0);
            GetX.Inlines.Add(new LineBreak());
            //находим y
            double[] Y = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                double S = 0;
                for (int j = 0; j < i; j++)
                {
                    S += ll[i, j] * Y[j];
                }
                Y[i] = (b[i] - S) / ll[i, i];
            }
            for (int i = 1; i < b.Length; i++)
            {
                SgetX = @"\Rightarrow \cases{";
                for (int j = 0; j < i; j++)
                {
                    SgetX += @"y_{" + (j + 1).ToString() + @"} = " + Y[j].ToString() + @"\\";
                }
                SgetX += @"y_{" + (i + 1).ToString() + @"} =" + b[i].ToString() + @" - (";
                for (int j = 0; j < i; j++)
                {
                    SgetX += ll[i, j].ToString() + @" \cdot " + Y[j] + "+";
                }
                SgetX = SgetX.Remove(SgetX.Length - 1);
                SgetX += @")\\";
                for (int k = i + 1; k < b.Length; k++)
                {
                    for (int j = 0; j <= k; j++)
                    {
                        if (ll[k, j] != 0)
                        {
                            if (ll[k, j] != 1)
                            {
                                SgetX += ll[k, j].ToString() + @" \cdot y_{" + (j + 1).ToString() + @"}+";
                            }
                            else
                            {
                                SgetX += @" y_{" + (j + 1).ToString() + @"}+";
                            }
                        }
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @"=" + b[k].ToString() + @"\\";
                }
                for (int k = 0; k < b.Length; k++)
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        if (uu[k, j] != 0)
                        {
                            if (uu[k, j] != 1)
                            {
                                SgetX += uu[k, j].ToString() + @" \cdot x_{" + (j + 1).ToString() + @"}+";
                            }
                            else
                            {
                                SgetX += @" x_{" + (j + 1).ToString() + @"}+";
                            }
                        }
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @"=y_{" + (k + 1).ToString() + @"}\\";
                }
                SgetX = SgetX.Remove(SgetX.Length - 2);
                SgetX += @"} \Rightarrow \cases{";
                for (int j = 0; j <= i; j++)
                {
                    SgetX += @"y_{" + (j + 1).ToString() + @"} = " + Y[j].ToString() + @"\\";
                }
                for (int k = i + 1; k < b.Length; k++)
                {
                    for (int j = 0; j <= k; j++)
                    {
                        if (ll[k, j] != 0)
                        {
                            if (ll[k, j] != 1)
                            {
                                SgetX += ll[k, j].ToString() + @" \cdot y_{" + (j + 1).ToString() + @"}+";
                            }
                            else
                            {
                                SgetX += @" y_{" + (j + 1).ToString() + @"}+";
                            }
                        }
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @"=" + b[k].ToString() + @"\\";
                }
                for (int k = 0; k < b.Length; k++)
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        if (uu[k, j] != 0)
                        {
                            if (uu[k, j] != 1)
                            {
                                SgetX += uu[k, j].ToString() + @" \cdot x_{" + (j + 1).ToString() + @"}+";
                            }
                            else
                            {
                                SgetX += @" x_{" + (j + 1).ToString() + @"}+";
                            }
                        }
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    if (i == b.Length - 1)
                    {
                        SgetX += @"=" + Y[k].ToString() + @"\\";
                    }
                    else
                    {
                        SgetX += @"=y_{" + (k + 1).ToString() + @"}\\";
                    }
                }
                SgetX = SgetX.Remove(SgetX.Length - 2);
                SgetX += @"}\Rightarrow ";
                LaTeX_toDOC(SgetX, 20.0);
                GetX.Inlines.Add(new LineBreak());
            }
            //находим x
            double[] X = new double[b.Length];
            for (int i = b.Length - 1; i >= 0; i--)
            {
                double S = 0;
                for (int j = i + 1; j < b.Length; j++)
                {
                    S += uu[i, j] * X[j];
                }
                X[i] = (Y[i] - S) / uu[i, i];
            }
            for (int i = b.Length - 1; i >= 0; i--)
            {
                SgetX = @"\Rightarrow \cases{";
                for (int j = 0; j < b.Length; j++)
                {
                    SgetX += @"y_{" + (j + 1).ToString() + @"} = " + Y[j].ToString() + @"\\";
                }
                for (int k = 0; k < i; k++)
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        if (uu[k, j] != 0)
                        {
                            if (uu[k, j] != 1)
                            {
                                SgetX += uu[k, j].ToString() + @" \cdot x_{" + (j + 1).ToString() + @"}+";
                            }
                            else
                            {
                                SgetX += @" x_{" + (j + 1).ToString() + @"}+";
                            }
                        }
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @"=" + Y[k].ToString() + @"\\";
                }
                if (i != b.Length - 1)
                {
                    SgetX += @"x_{" + (i + 1).ToString() + @"} = \frac{" + Y[i].ToString() + @" - (";
                    for (int j = i + 1; j < b.Length; j++)
                    {
                        SgetX += uu[i, j].ToString() + @" \cdot " + X[j] + "+";
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @")}{" + uu[i, i].ToString() + @"}\\";
                    for (int j = i + 1; j < b.Length; j++)
                    {
                        SgetX += @"x_{" + (j + 1).ToString() + @"} = " + X[j].ToString() + @"\\";
                    }
                }
                else
                {
                    SgetX += @"x_{" + (i + 1).ToString() + @"} = \frac{" + Y[i].ToString() + "}{" + uu[i, i].ToString() + @"}\\";
                }

                SgetX = SgetX.Remove(SgetX.Length - 2);
                SgetX += @"}\Rightarrow \cases{";

                for (int j = 0; j < b.Length; j++)
                {
                    SgetX += @"y_{" + (j + 1).ToString() + @"} = " + Y[j].ToString() + @"\\";
                }
                for (int k = 0; k < i; k++)
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        if (uu[k, j] != 0)
                        {
                            if (uu[k, j] != 1)
                            {
                                SgetX += uu[k, j].ToString() + @" \cdot x_{" + (j + 1).ToString() + @"}+";
                            }
                            else
                            {
                                SgetX += @" x_{" + (j + 1).ToString() + @"}+";
                            }
                        }
                    }
                    SgetX = SgetX.Remove(SgetX.Length - 1);
                    SgetX += @"=" + Y[k].ToString() + @"\\";
                }

                for (int j = i; j < b.Length; j++)
                {
                    SgetX += @"x_{" + (j + 1).ToString() + @"} = " + X[j].ToString() + @"\\";
                }


                SgetX = SgetX.Remove(SgetX.Length - 2);
                if (i != 0)
                {
                    SgetX += @"}\Rightarrow";
                }
                else
                {
                    SgetX += @"}";
                }
                LaTeX_toDOC(SgetX, 20.0);
                GetX.Inlines.Add(new LineBreak());
            }
            #endregion

            #region Ответ x
            Paragraph Res = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(Res);
            Res.Inlines.Add("Отсюда получаем:");
            Res.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"x^{*}=\left(" + LaTeX_vector("x", b.Length, true) + @"\right) = \left(" + LaTeX_vector(X) + @"\right)", 20.0);
            Res.Inlines.Add(new LineBreak());
            #endregion

            #region Вычисление вектора-невязки
            Paragraph nVec = new Paragraph();
            detWindow.richTextBox.Document.Blocks.Add(nVec);
            nVec.Inlines.Add("Вычислим вектор невязки:");
            nVec.Inlines.Add(new LineBreak());
            LaTeX_toDOC(@"r=Ax^{*}-b \Rightarrow \left(" + LaTeX_vector("r", b.Length, true) + @"\right) = \left(" + LaTeX_matrix(A) + @"\right) \left(" + LaTeX_vector(X) + @"\right) - \left(" + LaTeX_vector(b) + @"\right) =", 20.0);
            nVec.Inlines.Add(new LineBreak());
            string[] SnVec = new string[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                SnVec[i] = @"";
                for (int j = 0; j < b.Length; j++)
                {
                    if (A[i, j] < 0)
                    {
                        SnVec[i] += @"(" + A[i, j].ToString() + @")";
                    }
                    else
                    {
                        SnVec[i] += A[i, j].ToString();
                    }
                    SnVec[i] += @" \cdot ";
                    if (X[j] < 0)
                    {
                        SnVec[i] += @"(" + X[j].ToString() + @")";
                    }
                    else
                    {
                        SnVec[i] += X[j].ToString();
                    }
                    if (j != b.Length - 1)
                    {
                        SnVec[i] += @" + ";
                    }
                }
                if (i != b.Length - 1)
                {
                    SnVec[i] += @"\\";
                }
            }
            LaTeX_toDOC(@"=\left(" + LaTeX_vector(SnVec) + @"\right) - \left(" + LaTeX_vector(b) + @"\right)=", 20.0);
            nVec.Inlines.Add(new LineBreak());
            double[] Ax = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    Ax[i] += A[i, j] * X[j];
                }
            }
            double[] Axb = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                Axb[i] = Ax[i] - b[i];
            }
            LaTeX_toDOC(@"=\left(" + LaTeX_vector(Ax) + @" \right) - \left(" + LaTeX_vector(b) + @"\right) = \left(" + LaTeX_vector(Axb) + @"\right)", 20.0);
            nVec.Inlines.Add(new LineBreak());
            #endregion

            return true;
        }
        public void GaussianElimination(double[,] A, double[] b) //метод Гаусса
        {
            if (Determinant(A) != 0) //проверяем вырожденость матрицы A
            {
                RepairMatrix(ref A, ref b); //избавляемся от нулей по диагонали
                double[] x = new double[b.Length];
                for (int i = 0; i < b.Length - 1; i++)
                {
                    for (int j = i + 1; j < b.Length; j++)
                    {
                        double d = A[j, i] / A[i, i];
                        A[j, i] = 0;
                        for (int k = i + 1; k < b.Length; k++)
                        {
                            A[j, k] -= d * A[i, k];
                        }
                        b[j] -= d * b[i];
                    }
                }
                if (checkRankAndN(A, b)) //Проверка чтобы ранг матрицы r = n числу неизвестных (возможно не обязательно)
                {
                    x[b.Length - 1] = b[b.Length - 1] / A[b.Length - 1, b.Length - 1];
                    for (int i = b.Length - 2; i >= 0; i--)
                    {
                        double S = 0;
                        for (int j = i + 1; j < b.Length; j++)
                        {
                            S += A[i, j] * x[j];
                        }
                        x[i] = (b[i] - S) / A[i, i];
                    }
                    for (int i = 0; i < b.Length; i++)
                    {
                        StringGrid3.Cells[i, 0].Text = x[i].ToString();
                    }
                }
                else
                {
                    label3.Content = "r < n => СЛУ - неопределенная (см. Подробно)";
                    label3.Visibility = Visibility.Visible;
                }
            }
            else
            {
                label3.Content = "det(A) = 0 => матрица A - вырожденная (см. Подробно)";
                label3.Visibility = Visibility.Visible;
            }
        }
        public void GaussJordanElimination(double[,] A, double[] b) //Метод Гаусса-Жордана
        {
            if (Determinant(A) != 0) //проверяем вырожденость матрицы A
            {
                RepairMatrix(ref A, ref b); //избавляемся от нулей по диагонали
                for (int i = 0; i < b.Length; i++)
                {
                    double d = A[i, i];
                    b[i] /= d;
                    for (int j = i; j < b.Length; j++)
                    {
                        A[i, j] /= d;
                    }
                    for (int k = 0; k < b.Length; k++)
                        if (k != i)
                        {
                            double S = A[k, i];
                            for (int j = i; j < b.Length; j++)
                                A[k, j] -= S * A[i, j];
                            b[k] -= S * b[i];
                        }
                }
                for (int i = 0; i < b.Length; i++)
                {
                    StringGrid3.Cells[i, 0].Text = b[i].ToString();
                }
            }
            else
            {
                label3.Content = "det(A) = 0 => матрица A - вырожденная (см. Подробно)";
                label3.Visibility = Visibility.Visible;
            }
        }
        public void LU_decomposition(double[,] A, double[] b) //LU-декомпозиция
        {
            RepairMatrix(ref A, ref b); //избавляемся от нулей по диагонали (если возможно)
            if (checkLeadingPrincipal(A)) //проверяем ведущие главные миноры на вырожденность
            {
                double[,] ll = new double[b.Length, b.Length];
                double[,] uu = new double[b.Length, b.Length];
                uu[0, 0] = A[0, 0];
                ll[0, 0] = 1.0;
                for (int k = 1; k < b.Length; k++)
                {
                    uu[0, k] = A[0, k];
                    ll[k, 0] = A[k, 0] / uu[0, 0];
                }
                for (int i = 1; i < b.Length; i++)
                {
                    double S = 0;
                    for (int k = 0; k < i; k++)
                    {
                        S += ll[i, k] * uu[k, i];
                    }
                    uu[i, i] = A[i, i] - S;
                    for (int j = i; j < b.Length; j++)
                    {
                        S = 0;
                        for (int k = 0; k < i; k++)
                        {
                            S += ll[i, k] * uu[k, j];
                        }
                        uu[i, j] = A[i, j] - S;
                    }
                    for (int j = i; j < b.Length; j++)
                    {
                        S = 0;
                        for (int k = 0; k < i; k++)
                        {
                            S += ll[j, k] * uu[k, i];
                        }
                        ll[j, i] = (A[j, i] - S) / uu[i, i];
                    }
                }
                double[] y = new double[b.Length];
                for (int i = 0; i < b.Length; i++)
                {
                    double S = 0;
                    for (int j = 0; j < i; j++)
                    {
                        S += ll[i, j] * y[j];
                    }
                    y[i] = (b[i] - S) / ll[i, i];
                }
                double[] x = new double[b.Length];
                for (int i = b.Length - 1; i >= 0; i--)
                {
                    double S = 0;
                    for (int j = i + 1; j < b.Length; j++)
                    {
                        S += uu[i, j] * x[j];
                    }
                    x[i] = (y[i] - S) / uu[i, i];
                }
                for (int i = 0; i < b.Length; i++)
                {
                    StringGrid3.Cells[i, 0].Text = x[i].ToString();
                }
            }
            else
            {
                label3.Content = "Один из угловых миноров матрицы A - вырожденный (см. Подробно)";
                label3.Visibility = Visibility.Visible;
            }
        }
        public void RepairMatrix(ref double[,] A, ref double[] b) //Избавление он нулей по диагонали
        {
            for (int i = 0; i < b.Length - 1; i++)
            {
                if (A[i, i] == 0)
                {
                    int j = i + 1;
                    while (j < b.Length && A[i, i] == 0)
                    {
                        if (A[j, i] != 0 && A[i, j] != 0)
                        {
                            double temp;
                            for (int k = 0; k < b.Length; k++)
                            {
                                temp = A[i, k];
                                A[i, k] = A[j, k];
                                A[j, k] = temp;
                            }
                            temp = b[i];
                            b[i] = b[j];
                            b[j] = temp;
                        }
                        j++;
                    }
                }
            }
        }
        public double Determinant(double[,] A) //Определитель матрицы
        {
            if (A.GetLength(0) <= 2)
            {
                if (A.GetLength(0) == 2)
                    return A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0];
                else
                    return A[0, 0];
            }
            else
            {
                double det = 0;
                double[,] B = new double[A.GetLength(0) - 1, A.GetLength(1) - 1];
                for (int k = 0; k < A.GetLength(0); k++)
                {
                    for (int i = 1; i < A.GetLength(0); i++)
                        for (int j = 0; j < B.GetLength(1); j++)
                        {
                            if (j < k)
                            {
                                B[i - 1, j] = A[i, j];
                            }
                            else
                            {
                                B[i - 1, j] = A[i, j + 1];
                            }
                        }
                    det += A[0, k] * Math.Pow(-1, k) * Determinant(B);
                }
                return det;
            }
        }
        public bool checkLeadingPrincipal(double[,] A) //проверка невырожденности всех ведущих (угловых) главных миноров
        {
            for (int k = 0; k < A.GetLength(0); k++)
            {
                double[,] B = new double[k + 1, k + 1];
                for (int i = 0; i <= k; i++)
                {
                    for (int j = 0; j <= k; j++)
                    {
                        B[i, j] = A[i, j];
                    }
                }
                if (Determinant(B) == 0)
                    return false;
            }
            return true;
        }
        public bool checkRankAndN(double[,] A, double[] b) //Проверка чтобы ранг матрицы r = n числу неизвестных
        {
            for (int i = 0; i < b.Length; i++) //проверяем на наличие нулевых строк
            {
                bool flag = true;
                for (int j = 0; j < b.Length; j++)
                {
                    if (A[i, j] != 0)
                        flag = false;
                }
                if (flag && b[i] == 0)
                    return false;
            }
            for (int i = 0; i < b.Length - 1; i++) //проверяем на наличие кратных строк
            {
                for (int j = i + 1; j < b.Length; j++)
                {
                    bool flag = true;
                    double s = A[i, 0] / A[j, 0];
                    for (int k = 1; k < b.Length; k++)
                    {
                        if (A[i, k] / A[j, k] != s)
                            flag = false;
                    }
                    if (flag && b[i] / b[j] == s)
                        return false;
                }
            }
            return true;
        }
        public void LaTeX_toDOC(string LaTeX, double TextSize) //Генерация изображения из LaTeX, избавление от прозрачности и последующая вставка в документ на окне "Подробно"
        {
            TexFormulaParser Parser = new TexFormulaParser();
            TexFormula Formula = Parser.Parse(LaTeX);
            Formula.SetBackground(new SolidColorBrush(Color.FromRgb(255, 255, 255)));
            Formula.SetForeground(new SolidColorBrush(Color.FromRgb(0, 0, 0)));
            TexRenderer Renderer = Formula.GetRenderer(TexStyle.Display, TextSize, "Times New Roman");
            BitmapSource bitmapSource = Renderer.RenderToBitmap(0.0, 0.0);//, 300.0);
            byte[] img = BitmapSourceToByteArray(bitmapSource);
            for (int i = 54; i < img.Length; i++)
            {
                if ((i - 53) % 4 == 0)
                {
                    if (img[i] == img[i - 1] && img[i - 1] == img[i - 2] && img[i - 2] == img[i - 3])
                    {
                        img[i] = 0xFF;
                        img[i - 1] = 0xFF;
                        img[i - 2] = 0xFF;
                        img[i - 3] = 0xFF;
                    }
                }
            }
            BitmapSource newBMPsource = ByteArrayToBitmapSource(img);
            try
            {
                IDataObject origData = Clipboard.GetDataObject();
                Clipboard.SetImage(newBMPsource);
                detWindow.richTextBox.CaretPosition = detWindow.richTextBox.CaretPosition.DocumentEnd;
                detWindow.richTextBox.Paste();
                Clipboard.SetDataObject(origData);
            }
            catch
            {
                Clipboard.SetImage(newBMPsource);
                detWindow.richTextBox.CaretPosition = detWindow.richTextBox.CaretPosition.DocumentEnd;
                detWindow.richTextBox.Paste();
            }
        }
        public static byte[] BitmapSourceToByteArray(BitmapSource bitmapSource) //Bitmap в массив байтов
        {
            byte[] data;
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
        public static BitmapSource ByteArrayToBitmapSource(byte[] data) //Массив байтов в Bitmap
        {
            BitmapSource result;
            using (MemoryStream ms = new MemoryStream(data))
            {
                BmpBitmapDecoder decoder = new BmpBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                result = decoder.Frames[0];
            }
            return result;
        }
        private void StringGrid_Change(object sender, TextChangedEventArgs e) //Динамическое изменение матриц и пересчет при их изменении
        {
            bool x1 = false;
            bool y1 = false;
            bool x2 = false;
            bool y2 = false;
            for (int k = 0; k < StringGrid1.GetRowCount(); k++) //Если последние ячейки не пустые, то добавляем ещё строки и столбец
            {
                if (StringGrid1.Cells[StringGrid1.GetColCount() - 1, k].Text != "" || StringGrid1.Cells[k, StringGrid1.GetRowCount() - 1].Text != "" || StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0].Text != "")
                {
                    StringGrid1.AddRow();
                    for (int j = 0; j < StringGrid1.GetColCount(); j++)
                    {
                        AGrid.Children.Add(StringGrid1.Cells[StringGrid1.GetRowCount() - 1, j]);
                        StringGrid1.Cells[StringGrid1.GetRowCount() - 1, j].TextChanged += StringGrid_Change;
                        StringGrid1.Cells[StringGrid1.GetRowCount() - 1, j].LostFocus += StringGrid_LostFocus;
                        StringGrid1.Cells[StringGrid1.GetRowCount() - 1, j].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x90, 0xFF, 0x81));
                        StringGrid1.Cells[StringGrid1.GetRowCount() - 2, j].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                    }
                    StringGrid1.AddCol();
                    for (int i = 0; i < StringGrid1.GetRowCount(); i++)
                    {
                        AGrid.Children.Add(StringGrid1.Cells[i, StringGrid1.GetColCount() - 1]);
                        StringGrid1.Cells[i, StringGrid1.GetColCount() - 1].TextChanged += StringGrid_Change;
                        StringGrid1.Cells[i, StringGrid1.GetColCount() - 1].LostFocus += StringGrid_LostFocus;
                        StringGrid1.Cells[i, StringGrid1.GetColCount() - 1].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x90, 0xFF, 0x81));
                        if (i != StringGrid1.GetRowCount() - 1)
                        {
                            StringGrid1.Cells[i, StringGrid1.GetColCount() - 2].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                        }
                    }
                    StringGrid2.AddRow();
                    BGrid.Children.Add(StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0]);
                    StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x90, 0xFF, 0x81));
                    StringGrid2.Cells[StringGrid2.GetRowCount() - 2, 0].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                    StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0].TextChanged += StringGrid_Change;
                    StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0].LostFocus += StringGrid_LostFocus;
                    StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0].Margin = new Thickness(StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0].Margin.Left + 25, StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0].Margin.Top, 0, 0);
                    StringGrid3.AddRow();
                    XGrid.Children.Add(StringGrid3.Cells[StringGrid3.GetRowCount() - 1, 0]);
                    StringGrid3.Cells[StringGrid3.GetRowCount() - 1, 0].IsReadOnly = true;
                    StringGrid3.Cells[StringGrid3.GetRowCount() - 1, 0].IsTabStop = false;
                    StringGrid3.Cells[StringGrid3.GetRowCount() - 1, 0].Margin = new Thickness(StringGrid3.Cells[StringGrid3.GetRowCount() - 1, 0].Margin.Left + 25, StringGrid3.Cells[StringGrid3.GetRowCount() - 1, 0].Margin.Top, 0, 0);
                    SetTabIndex();
                }
            }
            for (int i = 0; i < StringGrid1.GetRowCount() - 1; i++) //все ячейки последнего столбца - пустые
            {
                if (StringGrid1.Cells[i, StringGrid1.GetRowCount() - 2].Text == "" && StringGrid2.Cells[StringGrid2.GetRowCount() - 2, 0].Text == "")
                {
                    x1 = true;
                }
                else
                {
                    x2 = true;
                }
            }
            for (int j = 0; j < StringGrid1.GetColCount() - 1; j++) //все ячейки последней строки - пустые
            {
                if (StringGrid1.Cells[StringGrid1.GetRowCount() - 2, j].Text == "" && StringGrid2.Cells[StringGrid2.GetRowCount() - 2, 0].Text == "")
                {
                    y1 = true;
                }
                else
                {
                    y2 = true;
                }
            }
            if (x1 && !x2 && y1 && !y2) //Если последняя строка и последний столбец матрицы A и последняя строка вектора B пустые, то удаляем одну строку и один стобец в матрице А и по одной строке в B и X
            {
                if (StringGrid1.GetRowCount() > StringGrid1.GetMinRow())
                    for (int j = 0; j < StringGrid1.GetColCount(); j++)
                    {
                        AGrid.Children.Remove(StringGrid1.Cells[StringGrid1.GetRowCount() - 1, j]);
                        StringGrid1.Cells[StringGrid1.GetRowCount() - 2, j].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x90, 0xFF, 0x81));
                    }
                StringGrid1.DelRow();
                if (StringGrid1.GetColCount() > StringGrid1.GetMinCol())
                    for (int i = 0; i < StringGrid1.GetRowCount(); i++)
                    {
                        AGrid.Children.Remove(StringGrid1.Cells[i, StringGrid1.GetColCount() - 1]);
                        StringGrid1.Cells[i, StringGrid1.GetColCount() - 2].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x90, 0xFF, 0x81));
                    }
                StringGrid1.DelCol();
                if (StringGrid2.GetRowCount() > StringGrid2.GetMinRow())
                {
                    BGrid.Children.Remove(StringGrid2.Cells[StringGrid2.GetRowCount() - 1, 0]);
                    StringGrid2.Cells[StringGrid2.GetRowCount() - 2, 0].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x90, 0xFF, 0x81));
                }
                StringGrid2.DelRow();
                if (StringGrid3.GetRowCount() > StringGrid3.GetMinRow())
                    XGrid.Children.Remove(StringGrid3.Cells[StringGrid3.GetRowCount() - 1, 0]);
                StringGrid3.DelRow();
                SetTabIndex(); //Устанавливаем очередность Tab индекса
                StringGrid_Change(sender, e); //на случай если нужно удалить несколько строк/столбцов подряд
            }
            CallRequiredProcedure(false); //Пересчитываем
        }
        private void button_Click(object sender, RoutedEventArgs e) //Открываем окно "Подробно"
        {
            detWindow.Title = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
            detWindow.richTextBox.Document.Blocks.Clear();
            detWindow.Width = Width - 40.0;
            detWindow.Height = Height - 40.0;
            detWindow.Left = Left + 20.0;
            detWindow.Top = Top + 20.0;
            CallRequiredProcedure(true);
            detWindow.Show();
            detWindow.Activate();
        }
        private void StringGrid_LostFocus(object sender, RoutedEventArgs e) //Обрабатываем значения и пересчитываем
        {
            ProtectionFromFool();
            CallRequiredProcedure(false);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            detWindow.closing = true;
            detWindow.Close();
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //Пересчитываем при изменении метода решения СЛАУ
        {
            CallRequiredProcedure(false);
        }
        public void Clear()
        {
            for (int i = 0; i < StringGrid1.GetRowCount(); i++)
            {
                for (int j = 0; j < StringGrid1.GetColCount(); j++)
                {
                    StringGrid1.Cells[i, j].Text = "";
                }
                StringGrid2.Cells[i, 0].Text = "";
            }
        }
        private void ClearCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Clear();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
    }
}