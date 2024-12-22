using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HexConverter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HexValue.Text = "";
            BinValue.Text = "";
            ManValue.Text = "";
            DecValue.Text = "";
        }

        private void Button_Clean_Click(object sender, RoutedEventArgs e)
        {
            HexValue.Text = "";
            BinValue.Text = "";
            ManValue.Text = "";
            DecValue.Text = "";
        }

        private void Button_Hex2Bin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BinValue.Text = Hex2Bin(HexValue.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Bin2Hex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HexValue.Text = Bin2Hex(BinValue.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Man_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DiffMan.IsChecked == false)
                    ManValue.Text = Bin2Man(BinValue.Text);
                else
                    ManValue.Text = Bin2DiffMan(BinValue.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Man2Hex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HexValue.Text = Bin2Hex(ManValue.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 16进制转换为2进制
        /// </summary>
        /// <param name="HexValue"></param>
        /// <returns></returns>
        private string Hex2Bin(string HexValue)
        {
            string returnValue = string.Empty;
            string newValue = string.Empty;
            string decValue = string.Empty;
            BigInteger decFinal = 0;
            string BinValue = string.Empty;
            DecValue.Text = "";

            for (int i = 0; i < HexValue.Length; i++)
            {
                newValue += HexValue[i];
                if ((i + 1) % 1 == 0 && (i + 1) != HexValue.Length)
                {
                    newValue += "-";
                }
            }

            string[] stringValue = newValue.Split('-');

            foreach (var str in stringValue)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    // 16进制转换成10进制
                    decValue = Convert.ToString(Convert.ToInt64(str, 16));

                    // 10进制转换成2进制
                    returnValue += Convert.ToString(Convert.ToInt64(decValue), 2).PadLeft(4, '0');
                }
            }

            // 计算10进制
            decFinal = BigInteger.Parse(HexValue, System.Globalization.NumberStyles.HexNumber);

            DecValue.Text = decFinal.ToString();

            return returnValue;
        }

        /// <summary>
        /// 2进制转换为16进制
        /// </summary>
        /// <param name="BinValue"></param>
        /// <returns></returns>
        private string Bin2Hex(string BinValue)
        {
            string returnValue = string.Empty;
            string newValue = string.Empty;
            string decValue = string.Empty;
            string HexValue = string.Empty;
            int length = BinValue.Length;

            if (BinValue.Length % 4 != 0)
            {
                length = BinValue.Length + 4 - BinValue.Length % 4;
                BinValue = BinValue.PadLeft(length, '0');
            }

            for (int i = 0; i < length; i++)
            {
                newValue += BinValue[i];
                if ((i + 1) % 4 == 0 && (i + 1) != length)
                {
                    newValue += "-";
                }
            }

            string[] stringValue = newValue.Split('-');

            foreach (var str in stringValue)
            {
                // 2进制转换成10进制
                decValue = Convert.ToString(Convert.ToInt64(str, 2));
                // 10进制转换成16进制
                returnValue += Convert.ToString(Convert.ToInt64(decValue), 16).ToUpper();
            }

            return returnValue;
        }

        /// <summary>
        /// 标准曼彻斯特解码
        /// </summary>
        /// <param name="BinValue"></param>
        /// <returns></returns>
        private string Bin2Man(string BinValue)
        {
            string returnValue = string.Empty;
            string newValue = string.Empty;
            int length = BinValue.Length;
            string manValue = string.Empty;
            string newManValue = string.Empty;

            if (BinValue.Length % 4 != 0)
            {
                length = BinValue.Length + 4 - BinValue.Length % 4;
                BinValue = BinValue.PadLeft(length, '0');
            }

            for (int i = 0; i < length; i++)
            {
                newValue += BinValue[i];
                if ((i + 1) % 4 == 0 && (i + 1) != length)
                {
                    newValue += "-";
                }
            }

            string[] stringValue = newValue.Split('-');

            // 使用802.3曼彻斯特编码还是标准曼彻斯特编码
            if (Man802.IsChecked == true)
                foreach (var str in stringValue)
                    manValue += ManChange_8023(str);
            else if (StandardMan.IsChecked == true)
                foreach (var str in stringValue)
                    manValue += ManChange(str);

            // 是否进行反序
            if (CodeReverse.IsChecked == true)
            {
                for (int i = 0; i < manValue.Length; i++)
                {
                    newManValue += manValue[i];
                    if ((i + 1) % 8 == 0 && (i + 1) != manValue.Length)
                    {
                        newManValue += "-";
                    }
                }

                string[] stringManValue = newManValue.Split('-');

                foreach (var str in stringManValue)
                {
                    returnValue += new string(str.ToCharArray().Reverse().ToArray());
                }
            }
            else
            {
                returnValue = manValue;
            }

            return returnValue;
        }

        /// <summary>
        /// 差分曼彻斯特解码
        /// </summary>
        /// <param name="BinValue"></param>
        /// <returns></returns>
        private string Bin2DiffMan(string BinValue)
        {
            string returnValue = string.Empty;
            string newValue = string.Empty;
            int length = BinValue.Length;
            string manValue = string.Empty;
            string newManValue = string.Empty;

            // 补零
            if (BinValue.Length % 4 != 0)
            {
                length = BinValue.Length + 4 - BinValue.Length % 4;
                BinValue = BinValue.PadLeft(length, '0');
            }

            char[] binValue = BinValue.ToCharArray();

            for (int i = 1; i < binValue.Length / 2; i++)
            {
                string a = binValue[i * 2].ToString();
                string b = binValue[i * 2 - 1].ToString();
                
                if (a != b)
                    manValue += '0';
                else
                    manValue += '1';
            }

            // 是否进行反序
            if (CodeReverse.IsChecked == true)
            {
                for (int i = 0; i < manValue.Length; i++)
                {
                    newManValue += manValue[i];
                    if ((i + 1) % 8 == 0 && (i + 1) != manValue.Length)
                    {
                        newManValue += "-";
                    }
                }

                string[] stringManValue = newManValue.Split('-');

                foreach (var str in stringManValue)
                {
                    returnValue += new string(str.ToCharArray().Reverse().ToArray());
                }
            }
            else
            {
                returnValue = manValue;
            }

            return returnValue;
        }

        /// <summary>
        /// 根据01->1,10->0进行decode，这是802.3的标准
        /// </summary>
        /// <param name="man"></param>
        /// <returns></returns>
        private string ManChange_8023(string man)
        {
            string returnValue = string.Empty;
            switch (man)
            {
                case "0101":
                    returnValue = "11";
                    break;
                case "0110":
                    returnValue = "10";
                    break;
                case "1010":
                    returnValue = "00";
                    break;
                case "1001":
                    returnValue = "01";
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// 根据01->0,10->1进行decode，这是G.E. Thomas的标准
        /// </summary>
        /// <param name="man"></param>
        /// <returns></returns>
        private string ManChange(string man)
        {
            string returnValue = string.Empty;
            switch (man)
            {
                case "0101":
                    returnValue = "00";
                    break;
                case "0110":
                    returnValue = "01";
                    break;
                case "1010":
                    returnValue = "11";
                    break;
                case "1001":
                    returnValue = "10";
                    break;
            }

            return returnValue;
        }
    }
}
