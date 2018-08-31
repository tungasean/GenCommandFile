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

namespace GenCommandFile
{
    public partial class GenBroker : Form
    {
        public GenBroker()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtUrlInput.Text = @"C:\TungData\Quant-Edge\techcombank\tcbfxgold_release\20180731_goliveu4\src";
            txtUrlBroker.Text = @"/var/web/TerminalGUIWeb";
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                txtUrlBroker.Text = txtUrlBroker.Text.Replace("\\", "/");
                var date = DateTime.Now;
                string datenow = date.Year.ToString();
                datenow = datenow +  (date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString());
                datenow = datenow + (date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString());

                //Neu chua nhap het url thi khong thuc hien
                if (string.IsNullOrEmpty(txtUrlInput.Text) || string.IsNullOrEmpty(txtUrlBroker.Text)) return;

                //Lay thong duong dan den cac thu muc code update
                Dictionary<string, string> DicPathForm = new Dictionary<string, string>();
                Dictionary<string, string> DicPathLang = new Dictionary<string, string>();
                Dictionary<string, string> DicPathVendor = new Dictionary<string, string>();
                Dictionary<string, string> DicPathValidate = new Dictionary<string, string>();
                Dictionary<string, string> DicPathJoiner = new Dictionary<string, string>();

                if (System.IO.Directory.Exists(txtUrlInput.Text + @"\js\controllers\form"))
                {
                    DirectoryInfo dirForm = new DirectoryInfo(txtUrlInput.Text + @"\js\controllers\form");
                    FileInfo[] filesForm = dirForm.GetFiles("*.js");
                    foreach (FileInfo f in filesForm)
                    {
                        DicPathForm[f.Name] = f.Directory.ToString();
                    }
                }
                    

                if(System.IO.Directory.Exists(txtUrlInput.Text + @"\js\language"))
                {
                    DirectoryInfo dirLang = new DirectoryInfo(txtUrlInput.Text + @"\js\language");
                    FileInfo[] filesLang = dirLang.GetFiles("*.js");
                    foreach (FileInfo f in filesLang)
                    {
                        DicPathLang[f.Name] = f.Directory.ToString();
                    }
                }

                if (System.IO.Directory.Exists(txtUrlInput.Text + @"\js\vendor\quantedge"))
                {
                    DirectoryInfo dirVendor = new DirectoryInfo(txtUrlInput.Text + @"\js\vendor\quantedge");
                    FileInfo[] filesVendor = dirVendor.GetFiles("*.js");
                    foreach (FileInfo f in filesVendor)
                    {
                        DicPathVendor[f.Name] = f.Directory.ToString();
                    }
                }

                if (System.IO.Directory.Exists(txtUrlInput.Text + @"\js\vendor\validation\0.1"))
                {
                    DirectoryInfo dirValidate = new DirectoryInfo(txtUrlInput.Text + @"\js\vendor\validation\0.1");
                    FileInfo[] filesValidate = dirValidate.GetFiles("*.js");
                    foreach (FileInfo f in filesValidate)
                    {
                        DicPathValidate[f.Name] = f.Directory.ToString();
                    }
                }

                if (System.IO.Directory.Exists(txtUrlInput.Text + @"\js\joiner"))
                {
                    DirectoryInfo dirJoiner = new DirectoryInfo(txtUrlInput.Text + @"\js\joiner");
                    FileInfo[] filesJoiner = dirJoiner.GetFiles("*.js");
                    foreach (FileInfo f in filesJoiner)
                    {
                        DicPathJoiner[f.Name] = f.Directory.ToString();
                    }
                }
                    

                #region Tao File Install
                String filepath = @"C:\QuantEdge\Webclient_Instal.txt";// đường dẫn của file muốn tạo
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                System.IO.StreamWriter ghiFile = new System.IO.StreamWriter(filepath);



                #region Backup
                ghiFile.WriteLine("#Bakup file");
                //Doi voi form
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/controllers/form/");
                foreach (var path in DicPathForm)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFile.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + ".js");
                }

                //Doi voi Language
                ghiFile.WriteLine("");
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/language");
                foreach (var path in DicPathLang)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFile.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + ".js");
                }

                //Doi voi Vendor
                ghiFile.WriteLine("");
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/quantedge");
                foreach (var path in DicPathVendor)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFile.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + ".js");
                }

                //Doi voi validate
                ghiFile.WriteLine("");
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/validation/0.1");
                foreach (var path in DicPathValidate)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFile.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + ".js");
                }

                //Doi voi joiner
                ghiFile.WriteLine("");
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/joiner");
                foreach (var path in DicPathJoiner)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFile.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + ".js");
                }
                #endregion

                #region Copy File
                ghiFile.WriteLine("");
                ghiFile.WriteLine("#Copy file");
                //Doi voi form
                ghiFile.WriteLine("sudo cp /home/su_user/VFG20180504/js/controllers/form/*.js " + txtUrlBroker.Text + "/js/controllers/form/");

                //Doi voi Language
                ghiFile.WriteLine("sudo cp /home/su_user/VFG20180504/js/language/*.js " + txtUrlBroker.Text + "/js/language/");

                //Doi voi Vendor
                ghiFile.WriteLine("sudo cp /home/su_user/VFG20180504/js/vendor/quantedge/*.js " + txtUrlBroker.Text + "/js/vendor/quantedge/");
                
                //Doi voi Validate
                ghiFile.WriteLine("sudo cp /home/su_user/VFG20180504/js/vendor/validation/0.1/*.js " + txtUrlBroker.Text + "/js/vendor/validation/0.1/");
                
                //Doi voi Joiner
                ghiFile.WriteLine("sudo cp /home/su_user/VFG20180504/js/joiner/*.js " + txtUrlBroker.Text + "/js/joiner/");
                #endregion


                #region Cap quyen
                ghiFile.WriteLine("");
                ghiFile.WriteLine("#Cap quyen file");
                //Doi voi form
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/controllers/form/");
                foreach (var path in DicPathForm)
                {
                    ghiFile.WriteLine("sudo chmod 755 " + path.Key);
                }

                //Doi voi Language
                ghiFile.WriteLine("");
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/language");
                foreach (var path in DicPathLang)
                {
                    ghiFile.WriteLine("sudo chmod 755 " + path.Key);
                }

                //Doi voi Vendor
                ghiFile.WriteLine("");
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/quantedge");
                foreach (var path in DicPathVendor)
                {
                    ghiFile.WriteLine("sudo chmod 755 " + path.Key);
                }

                //Doi voi Validate
                ghiFile.WriteLine("");
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/validation/0.1");
                foreach (var path in DicPathValidate)
                {
                    ghiFile.WriteLine("sudo chmod 755 " + path.Key);
                }

                //Doi voi Joiner
                ghiFile.WriteLine("");
                ghiFile.WriteLine("cd " + txtUrlBroker.Text + "/js/joiner");
                foreach (var path in DicPathJoiner)
                {
                    ghiFile.WriteLine("sudo chmod 755 " + path.Key);
                }
                #endregion


                ghiFile.Close();
                ghiFile.Dispose();
                #endregion

                #region Roll back
                String filepathBack = @"C:\QuantEdge\Webclient_rollback.txt";// đường dẫn của file muốn tạo
                if (File.Exists(filepathBack))
                {
                    File.Delete(filepathBack);
                }
                System.IO.StreamWriter ghiFileBack = new System.IO.StreamWriter(filepathBack);



                #region Backup
                ghiFileBack.WriteLine("#Bakup file");
                //Doi voi form
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/controllers/form/");
                foreach (var path in DicPathForm)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + "_E" + ".js");
                }

                //Doi voi Language
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/language");
                foreach (var path in DicPathLang)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + "_E" + ".js");
                }

                //Doi voi Vendor
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/quantedge");
                foreach (var path in DicPathVendor)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + "_E" + ".js");
                }

                //Doi voi Validate
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/validation/0.1");
                foreach (var path in DicPathValidate)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + "_E" + ".js");
                }

                //Doi voi Joiner
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/joiner");
                foreach (var path in DicPathJoiner)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + path.Key + " " + fileName + "_" + datenow + "_E" + ".js");
                }
                #endregion

                #region Revert lai ban dau
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("#Revert file");
                //Doi voi form
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/controllers/form/");
                foreach (var path in DicPathForm)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + fileName + "_" + datenow + ".js " + path.Key);
                }

                //Doi voi Language
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/language");
                foreach (var path in DicPathLang)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + fileName + "_" + datenow + ".js " + path.Key);
                }

                //Doi voi Vendor
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/quantedge");
                foreach (var path in DicPathVendor)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + fileName + "_" + datenow + ".js " + path.Key);
                }

                //Doi voi Validate
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/validation/0.1");
                foreach (var path in DicPathValidate)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + fileName + "_" + datenow + ".js " + path.Key);
                }

                //Doi voi Joiner
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/joiner");
                foreach (var path in DicPathJoiner)
                {
                    var fileName = path.Key.Replace(".js", "");
                    ghiFileBack.WriteLine("sudo mv " + fileName + "_" + datenow + ".js " + path.Key);
                }
                #endregion


                #region Cap quyen
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("#Cap quyen file");
                //Doi voi form
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/controllers/form/");
                foreach (var path in DicPathForm)
                {
                    ghiFileBack.WriteLine("sudo chmod 755 " + path.Key);
                }

                //Doi voi Language
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/language");
                foreach (var path in DicPathLang)
                {
                    ghiFileBack.WriteLine("sudo chmod 755 " + path.Key);
                }

                //Doi voi Vendor
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/quantedge");
                foreach (var path in DicPathVendor)
                {
                    ghiFileBack.WriteLine("sudo chmod 755 " + path.Key);
                }

                //Doi voi Validate
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/vendor/validation/0.1");
                foreach (var path in DicPathValidate)
                {
                    ghiFileBack.WriteLine("sudo chmod 755 " + path.Key);
                }

                //Doi voi Joiner
                ghiFileBack.WriteLine("");
                ghiFileBack.WriteLine("cd " + txtUrlBroker.Text + "/js/joiner");
                foreach (var path in DicPathJoiner)
                {
                    ghiFileBack.WriteLine("sudo chmod 755 " + path.Key);
                }
                #endregion


                ghiFileBack.Close();
                ghiFileBack.Dispose();
                #endregion


                MessageBox.Show("", "Success! Go To C:\\QuantEdge\\", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show("", exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
