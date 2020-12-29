using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using iTextSharp.text.pdf;
using Ordermanagement_01.Masters;
using System;
using System.IO;
using System.Windows.Forms;

namespace Ordermanagement_01.Test
{
    public partial class Compress_Pdf_File : XtraForm
    {
        public Compress_Pdf_File()
        {
            InitializeComponent();
        }

        private void Compress_Pdf_File_Load(object sender, EventArgs e)
        {

        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Select Pdf File";
                ofd.InitialDirectory = @"C:/Temp/";
                ofd.Filter = "Pdf Files |*.pdf";
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    // DevExpress.Pdf.PdfDocument document = iTextSharp.text.pdf.PdfReader.Open("file1.pdf", PdfDocumentOpenMode.Modify);

                    // Compress content streams of PDF pages.
                    // document.Options.CompressContentStreams = true;

                    string pdfFile = ofd.FileName;
                    Directory.CreateDirectory(@"C:/Temp/");
                    string Filepath = @"C:/Temp/";
                    // string filename = pdfFile + "Compressed" + DateTime.Now.ToString() + ".Pdf";
                    string fileName = pdfFile + "CompressedFile" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".Pdf";

                    iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdfFile);



                    PdfStamper stamper = new PdfStamper(reader, new FileStream(fileName, FileMode.Create), PdfWriter.VERSION_1_5);

                    stamper.FormFlattening = true;

                    int noofpages = reader.NumberOfPages;
                    for (int i = 0; i <= noofpages; i++)
                    {
                        reader.SetPageContent(i, reader.GetPageContent(i));
                    }
                    reader.RemoveAnnotations();
                    stamper.Reader.RemoveUnusedObjects();

                    stamper.Writer.CompressionLevel = iTextSharp.text.pdf.PdfStream.BEST_COMPRESSION;
                    stamper.SetFullCompression();
                    stamper.Writer.SetFullCompression();

                    stamper.Close();
                    lbl_status.Text = "File Compressed SuccesFully";

                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Something Went Wrong ");
                SplashScreenManager.CloseForm(false);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }


        //string pdfFile = ofd.FileName;
        //Directory.CreateDirectory(@"C:/Temp/");
        //            string Filepath = @"C:/Temp/";
        //// string filename = pdfFile + "Compressed" + DateTime.Now.ToString() + ".Pdf";
        //string fileName = pdfFile + "CompressedFile" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".Pdf";

        //iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdfFile);

        //PdfStamper stamper = new PdfStamper(reader, new FileStream(fileName, FileMode.Create), PdfWriter.VERSION_1_5);

        //stamper.FormFlattening = true;
        //            stamper.SetFullCompression();

        //            stamper.Close();
        //            lbl_status.Text = "File Compressed SuccesFully";
    }
}
