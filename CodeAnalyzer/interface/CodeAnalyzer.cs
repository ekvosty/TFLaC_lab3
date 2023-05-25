using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace @interface
{
    internal class CodeAnalyzer
    {
        string pattern = @"(\/\*)|(\*\/)|( )|(=)|(\%)|(\n)|(;)|(\+)|(\-)|(\/)|(\*)|(\()|(\))|(int)";
        private bool isCommented = false;
        private bool sHasDigit = false;
        private bool isFail = false;
        private AnalyzerState AnalyzerState = AnalyzerState.NEW_OPERATOR;
        private int RowCount = 1;
        private int ColumnCount = 0;
        private int startCommentColumn = 1;
        private int startCommentRow = 1;
        private int sCounter = 0;
        RichTextBox richTextBox;
        private List<string> lexems = new List<string>();
        //public CodeAnalyzer(string text, RichTextBox richTextBox)
        //{
        //    this.richTextBox = richTextBox;
        //    string[] substrings = Regex.Split(text, pattern);
        //    substrings = substrings.Where(s => s != String.Empty).ToArray<string>();
        //    foreach (string match in substrings)
        //    {
        //        ColumnCount++;
        //        if (match == "\n")
        //        {
        //            ColumnCount++;
        //            if (!isCommented)
        //            {
        //                AppendInfo("конец строки", match);
        //            }
        //            RowCount++;
        //            ColumnCount = 0;
        //            continue;
        //        }
        //        if (isCommented)
        //        {
        //            if (match == "*/")
        //            {
        //                isCommented = false;
        //                ColumnCount += match.Length - 1;
        //                AppendInfo("комментарии", "*/", startCommentColumn, ColumnCount, startCommentRow, RowCount);
        //            }
        //            else
        //            {
        //                ColumnCount += match.Length - 1;
        //            }
        //        }
        //        else
        //        {

        //            if (match == " ")
        //            {
        //                AppendInfo("разделитель", match);
        //                continue;
        //            }

        //            if (match == "/*")
        //            {
        //                startCommentColumn = ColumnCount;
        //                startCommentRow = RowCount;
        //                ColumnCount += match.Length - 1;
        //                isCommented = true;
        //                continue;
        //            }

        //            if (AnalyzerState == AnalyzerState.NEW_OPERATOR)
        //            {
        //                if (match == "int")
        //                {
        //                    AppendInfo("ключевое слово", match);
        //                    ColumnCount += match.Length - 1;
        //                    AnalyzerState = AnalyzerState.CREATE_INT;
        //                }
        //                else if (match == ";")
        //                {
        //                    AppendInfo("конец оператора", match);
        //                    AnalyzerState = AnalyzerState.NEW_OPERATOR;
        //                }
        //                else
        //                {
        //                    if (vars.Contains(match))
        //                    {
        //                        AppendInfo("идентификатор", match);
        //                        ColumnCount += match.Length - 1;
        //                        AnalyzerState = AnalyzerState.SET_VARIABLE;
        //                    }
        //                    else
        //                    {
        //                        AppendInfo("недопустимый символ", match);//Если написано z = 2, но выше не было объявлено int z = ..., то такой переменной не существует, значит она не валидна.
        //                        isFail = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            else if (AnalyzerState == AnalyzerState.CREATE_INT)
        //            {
        //                if (char.IsLetter(match[0]) && match != "int" && !vars.Contains(match))//имя переменной не может быть ключевым словом, и начинаться с числа или любого другого символа кроме буквы. Ну и создать вторую переменную с таким же именем нельзя
        //                {
        //                    AnalyzerState = AnalyzerState.SET_VARIABLE;
        //                    ColumnCount += match.Length - 1;
        //                    AppendInfo("идентификатор", match);
        //                    vars.Add(match);
        //                }
        //                else
        //                {
        //                    AppendInfo("недопустимый символ", match);
        //                    isFail = true;
        //                    break;
        //                }
        //            }
        //            else if (AnalyzerState == AnalyzerState.UPDATE_INT)
        //            {
        //                if (vars.Contains(match))
        //                {
        //                    AnalyzerState = AnalyzerState.SET_VARIABLE;
        //                    AppendInfo("идентификатор", match);
        //                    ColumnCount += match.Length - 1;
        //                }
        //                else
        //                {
        //                    AppendInfo("недопустимый символ", match);
        //                    isFail = true;
        //                    break;
        //                }
        //            }
        //            else if (AnalyzerState == AnalyzerState.SET_VARIABLE)
        //            {
        //                if (match == "=")
        //                {
        //                    AppendInfo("оператор присваивания", match);
        //                    AnalyzerState = AnalyzerState.NEED_DIGIT;
        //                }
        //                else
        //                {
        //                    AppendInfo("недопустимый символ", match);
        //                    isFail = true;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                if (AnalyzerState == AnalyzerState.NEED_DIGIT || AnalyzerState == AnalyzerState.NEED_DIGIT_FOR_DIVIDE)
        //                {
        //                    if (match == "(")
        //                    {
        //                        sCounter++;
        //                        AppendInfo("круглая скобка открытие", match);
        //                    }
        //                    else if (match == ")")
        //                    {
        //                        sCounter--;
        //                        if (sCounter < 0)
        //                        {
        //                            AppendInfo("недопустимый символ", match, "скобка не открыта");
        //                            isFail = true;
        //                            break;
        //                        }
        //                        if (!sHasDigit)
        //                        {
        //                            AppendInfo("недопустимый символ", match, "скобка пустая");
        //                            isFail = true;
        //                            break;
        //                        }
        //                        if (sCounter == 0)
        //                        {
        //                            sHasDigit = false;
        //                        }
        //                        AppendInfo("круглая скобка закрытие", match);
        //                    }
        //                    else if (vars.Contains(match))
        //                    {
        //                        AppendInfo("идентификатор", match);
        //                        ColumnCount += match.Length - 1;
        //                        sHasDigit = true;
        //                        AnalyzerState = AnalyzerState.NEED_OPERAND;
        //                    }
        //                    else if (int.TryParse(match, out _))
        //                    {
        //                        if (AnalyzerState == AnalyzerState.NEED_DIGIT_FOR_DIVIDE && match == "0")
        //                        {
        //                            AppendInfo("недопустимый символ", match, "делить на 0 нельзя");
        //                            isFail = true;
        //                            break;
        //                        }
        //                        AppendInfo("целое без знака", match);
        //                        ColumnCount += match.Length - 1;
        //                        sHasDigit = true;
        //                        AnalyzerState = AnalyzerState.NEED_OPERAND;
        //                    }
        //                    else
        //                    {
        //                        AppendInfo("недопустимый символ", match);
        //                        isFail = true;
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    if (match == "+" || match == "*" || match == "-")
        //                    {
        //                        AnalyzerState = AnalyzerState.NEED_DIGIT;
        //                        AppendInfo("арифметическая операция", match);
        //                    }
        //                    else if (match == "/" || match == "%")
        //                    {
        //                        AnalyzerState = AnalyzerState.NEED_DIGIT_FOR_DIVIDE;
        //                        AppendInfo("арифметическая операция", match);
        //                    }
        //                    else if (match == ";")
        //                    {
        //                        AppendInfo("конец оператора", match);
        //                        AnalyzerState = AnalyzerState.NEW_OPERATOR;
        //                    }
        //                    else
        //                    {
        //                        AppendInfo("недопустимый символ", match);
        //                        isFail = true;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (!isFail)
        //    {
        //        if (isCommented)
        //        {
        //            AppendInfo("недопустимый символ", "/*", startCommentColumn, ColumnCount, startCommentRow, RowCount, "не закрыт комментарий");
        //        }


        //        if (sCounter != 0)
        //        {
        //            AppendInfo("недопустимый символ", "(", "не закрыта скобка");
        //        }

        //        if (AnalyzerState != AnalyzerState.NEW_OPERATOR)
        //        {
        //            AppendInfo("недопустимый символ", substrings.Last(), "не закрыт оператор");
        //        }
        //    }

        //}

        public CodeAnalyzer(string text, RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
            string[] substrings = Regex.Split(text, pattern);
            substrings = substrings.Where(s => s != String.Empty).ToArray<string>();
            foreach (string match in substrings)
            {
                lexems.Add(match);
                ColumnCount++;
                if (match == "\n")
                {
                    ColumnCount++;
                    if (!isCommented)
                    {
                        AppendInfo("конец строки", match);
                    }
                    RowCount++;
                    ColumnCount = 0;
                    continue;
                }
                if (isCommented)
                {
                    if (match == "*/")
                    {
                        isCommented = false;
                        ColumnCount += match.Length - 1;
                        AppendInfo("комментарии", "*/", startCommentColumn, ColumnCount, startCommentRow, RowCount);
                    }
                    else
                    {
                        ColumnCount += match.Length - 1;
                    }
                }
                else
                {
                    if (match == " ")
                    {
                        AppendInfo("разделитель", match);
                        continue;
                    }

                    else if (match == "/*")
                    {
                        startCommentColumn = ColumnCount;
                        startCommentRow = RowCount;
                        ColumnCount += match.Length - 1;
                        isCommented = true;
                        continue;
                    }

                    else if (match == "int")
                    {
                        AppendInfo("ключевое слово", match);
                        ColumnCount += match.Length - 1;
                    }

                    else if (match == ";")
                    {
                        AppendInfo("конец оператора", match);
                    }
                    else if (match == "=")
                    {
                        AppendInfo("оператор присваивания", match);
                    }
                    else if (match == "(")
                    {
                        AppendInfo("круглая скобка открытие", match);
                    }
                    else if (match == ")")
                    {
                        AppendInfo("круглая скобка закрытие", match);
                    }
                    else if (match == "+" || match == "*" || match == "-" || match == "/")
                    {
                        AppendInfo("арифметическая операция", match);
                    }

                    else if (IsIdentificator(match))
                    {
                        AppendInfo("идентификатор", match);
                        ColumnCount += match.Length - 1;
                    }
                    else if (IsDigit(match))
                    {
                        AppendInfo("ЦБЗ", match);
                        ColumnCount += match.Length - 1;
                    }
                    else 
                    {
                        AppendInfo("недопустимый символ", match);
                        ColumnCount += match.Length - 1;
                    }
                }
            }
        }

        private void AppendInfo(string type, string text, int columnCountStart, int columnCountEnd, int rowCountStart, int rowCountEnd, string error = "")
        {
            string textName = text == " " ? "(пробел)" : text;
            textName = text == "*/" ? "/* */" : textName;
            textName = text == "\n" ? "\\n" : textName;
            string errorText = error == "" ? "" : "- " + error;
            richTextBox.AppendText($"{type} - {textName} - c {columnCountStart} символ {rowCountStart} строки по {columnCountEnd} символ {rowCountEnd} строки {errorText}{Environment.NewLine}");
        }

        private void AppendInfo(string type, string text, string error = "")
        {
            AppendInfo(type, text, ColumnCount, ColumnCount + text.Length - 1, RowCount, RowCount, error);
        }

        private bool IsDigit(string content) 
        {
            bool isDigit = true;
            foreach (char c in content) 
            {
                if (!char.IsDigit(c)) 
                {
                    isDigit = false;
                    break;
                }
            }
            return isDigit;
        }

        private bool IsIdentificator(string content)
        {
            bool isDigit = char.IsLetter(content[0]);
            foreach (char c in content)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    isDigit = false;
                    break;
                }
            }
            return isDigit;
        }
    }
}
