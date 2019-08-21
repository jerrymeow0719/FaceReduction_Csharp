using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetect_EmguCV;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using TSFaceDetection;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace FaceReduction
{
    public partial class Form1 : Form
    {
        Emgu.CV.VideoCapture objVideoCapture;
        Emgu.CV.VideoCapture objVideoCapture_Before;
        Emgu.CV.VideoCapture objVideoCapture_After;
        enum PlayState { PlayState_Pause, PlayState_Play };
        double FrameCount = 0;
        int State = (int)PlayState.PlayState_Pause;

        OpenFileDialog OF = new OpenFileDialog();
        SaveFileDialog SF = new SaveFileDialog();
        string ext = "";
        string SaveFileName = "";

        private delegate void ShowFrameDelegate();
        private delegate void ShowProgressBarDelegate(int a);

        //For YOLO
        int[] DetectClass = { 14 };
        string[] Labels = { "aeroplane", "bicycle", "bird", "boat", "bottle", "bus", "car", "cat", "chair", "cow", "diningtable", "dog", "horse", "motorbike", "person", "pottedplant", "sheep", "sofa", "train", "tvmonitor" };
        string YOLO_cfg = Application.StartupPath + @"\yolov2-tiny-voc.cfg";
        string YOLO_weights = Application.StartupPath + @"\yolov2-tiny-voc.weights";

        //int[] DetectClass = { 0 };
        //string[] Labels = { "face" };
        //string YOLO_cfg = Application.StartupPath + @"\yolov2-tiny.cfg";
        //string YOLO_weights = Application.StartupPath + @"\yolov2-tiny_10000.weights";

        // 引用Windows GDI元件，並調用DeleteObject的方法
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public Form1()
        {
            InitializeComponent();
        }

        public void showFrame()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    ShowFrameDelegate SD = new ShowFrameDelegate(showFrame);
                    this.BeginInvoke(SD);
                }
                catch { }
            }
            else
            {
                if (ext == ".jpg")
                {
                    Emgu.CV.Mat matBefore = CvInvoke.Imread(Text_FilePath.Text);
                    Emgu.CV.Mat matAfter = CvInvoke.Imread(SaveFileName);

                    if (matBefore != null && matAfter != null)
                    {
                        // bitmap是GDI+的物件，所以使用一般釋放記憶體的方式(dispose)對於bitmap是無效的
                        // 要能夠成功的釋放GDI+物件所佔用的記憶體，必須要使用Windows GDI元件，才能有效的釋放掉bitmap所使用的記憶體
                        // 將Bitmap物件轉換為平台指標
                        IntPtr gdibitmapBefore = matBefore.Bitmap.GetHbitmap();
                        IntPtr gdibitmapAfter = matAfter.Bitmap.GetHbitmap();
                        // 將bitmap放入至PictureBox的Image屬性
                        File_Before.Image = Image.FromHbitmap(gdibitmapBefore);
                        File_After.Image = Image.FromHbitmap(gdibitmapAfter);
                        // 進行Bitmap資源的釋放
                        DeleteObject(gdibitmapBefore);
                        DeleteObject(gdibitmapAfter);
                    }
                    if (matBefore != null)
                        matBefore.Dispose();
                    if (matAfter != null)
                        matAfter.Dispose();
                    GC.Collect();
                }
                else if (FrameCount == 0 || State == (int)PlayState.PlayState_Play)
                {
                    Emgu.CV.Mat matBefore = objVideoCapture_Before.QueryFrame();
                    Emgu.CV.Mat matAfter = objVideoCapture_After.QueryFrame();
                    
                    if (matBefore != null && matAfter != null)
                    {
                        FrameCount = objVideoCapture_Before.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);

                        // bitmap是GDI+的物件，所以使用一般釋放記憶體的方式(dispose)對於bitmap是無效的
                        // 要能夠成功的釋放GDI+物件所佔用的記憶體，必須要使用Windows GDI元件，才能有效的釋放掉bitmap所使用的記憶體
                        // 將Bitmap物件轉換為平台指標
                        IntPtr gdibitmapBefore = matBefore.Bitmap.GetHbitmap();
                        IntPtr gdibitmapAfter = matAfter.Bitmap.GetHbitmap();
                        // 將bitmap放入至PictureBox的Image屬性
                        File_Before.Image = Image.FromHbitmap(gdibitmapBefore);
                        File_After.Image = Image.FromHbitmap(gdibitmapAfter);
                        // 進行Bitmap資源的釋放
                        DeleteObject(gdibitmapBefore);
                        DeleteObject(gdibitmapAfter);
                    }
                    if (matBefore != null)
                        matBefore.Dispose();
                    if (matAfter != null)
                        matAfter.Dispose();
                    GC.Collect();
                }
            }
        }

        //private void ShowProgressBar(int index)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        try
        //        {
        //            ShowProgressBarDelegate SD = new ShowProgressBarDelegate(ShowProgressBar);
        //            this.BeginInvoke(SD, new object[] { index });
        //        }
        //        catch { }
        //    }
        //    else
        //    {
        //        progressBar1.Value = index;
        //    }
        //}

        private void btm_Load_Click(object sender, EventArgs e)
        {
            OF.Title = "Select file";

            if (OF.ShowDialog() == DialogResult.OK)
            {
                ext = System.IO.Path.GetExtension(OF.FileName);
                ext = ext.ToLower();
                if (ext == ".mov" || ext == ".mp4" || ext == ".jpg")
                {
                    Text_FilePath.Text = OF.FileName;
                    btm_Detect.Enabled = true;
                    btm_Start.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Wrong file format, Please check again.");
                    Text_FilePath.Text = "";
                }
                OF.Dispose();
            }
        }
        private void btm_Start_Click(object sender, EventArgs e)
        {
            SF.Filter = "All files (*.*)|*.*";
            if (SF.ShowDialog() == DialogResult.OK)
            {
                int Index = SF.FileName.IndexOf(".");
                SaveFileName = (Index == -1 ? String.Concat(SF.FileName, ext) : String.Concat(SF.FileName.Substring(0, SF.FileName.IndexOf(".")), ext));
                SF.Dispose();
                if (ext == ".jpg")
                {
                    btm_Play.Enabled = false;
                    btm_Pause.Enabled = false;
                    TSFaceDetection.TSFaceDetection tSFaceDetection = new TSFaceDetection.TSFaceDetection();
                    tSFaceDetection.FaceReduction(Text_FilePath.Text, "D:\\test.txt", SaveFileName);
                    showFrame();
                }
                else
                {
                    btm_Play.Enabled = false;
                    btm_Pause.Enabled = false;
                    objVideoCapture = null;
                    objVideoCapture = new Emgu.CV.VideoCapture(Text_FilePath.Text);
                    TSFaceDetection.TSFaceDetection tSFaceDetection = new TSFaceDetection.TSFaceDetection();
                    tSFaceDetection.FaceReduction(Text_FilePath.Text, "D:\\test.txt", SaveFileName);

                    objVideoCapture_Before = new Emgu.CV.VideoCapture(Text_FilePath.Text);
                    objVideoCapture_After = new Emgu.CV.VideoCapture(SaveFileName);
                    FrameCount = 0;
                    objVideoCapture_Before.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, FrameCount);
                    objVideoCapture_After.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, FrameCount);
                    showFrame();

                    btm_Play.Enabled = true;
                    btm_Pause.Enabled = true;
                }
            }
        }

        private void FaceDetection()
        {
            TSFaceDetection.TSFaceDetection tSFaceDetection = new TSFaceDetection.TSFaceDetection();
            //TSFaceDetection.TSFaceDetection.OpenCVResult openCVResult = new TSFaceDetection.TSFaceDetection.OpenCVResult();
            if (radioButton_Cascade.Checked)
                tSFaceDetection.FaceDetect(Text_FilePath.Text, "D:\\test.txt", "haarcascade_frontalface_default.xml", 10);
            else if (radioButton_YOLO.Checked)
                tSFaceDetection.FaceDetect(Text_FilePath.Text, "D:\\test.txt", YOLO_cfg, YOLO_weights, Labels, DetectClass, 10);
        }

        //private OpenCVResult CaptureFace_YOLO(Emgu.CV.Mat mat)
        //{
        //    List<Rectangle> faces = new List<Rectangle>();
        //    List<Rectangle> eyes = new List<Rectangle>();

        //    if (File.Exists(@"Process.jpg"))
        //        File.Delete(@"Process.jpg");
        //    CvInvoke.Imwrite(@"Process.jpg", mat);
        //    var org = Cv2.ImRead(@"Process.jpg");
        //    var w = org.Width;
        //    var h = org.Height;
        //    //setting blob, parameter are important
        //    var blob = CvDnn.BlobFromImage(org, 1 / 255.0, new OpenCvSharp.Size(416, 416), new OpenCvSharp.Scalar(), true, false);
        //    var net = CvDnn.ReadNetFromDarknet(YOLO_cfg, YOLO_weights);
        //    net.SetInput(blob, "data");
        //    var prob = net.Forward();

        //    /* YOLO2 VOC output
        //     0 1 : center                    2 3 : w/h
        //     4 : confidence                  5 ~24 : class probability */
        //    const int prefix = 5;   //skip 0~4

        //    for (int i = 0; i < prob.Rows; i++)
        //    {
        //        var confidence = prob.At<float>(i, 4);
        //        if (confidence > YOLO_threshold)
        //        {
        //            //get classes probability
        //            Cv2.MinMaxLoc(prob.Row[i].ColRange(prefix, prob.Cols), out _, out OpenCvSharp.Point max);
        //            var classes = max.X;
        //            var probability = prob.At<float>(i, classes + prefix);

        //            if (probability != 0 && DetectClass.Contains(classes)) //more accuracy
        //            {
        //                //get center and width/height
        //                var centerX = prob.At<float>(i, 0) * w;
        //                var centerY = prob.At<float>(i, 1) * h;
        //                var width = prob.At<float>(i, 2) * w;
        //                var height = prob.At<float>(i, 3) * h;
        //                //label formating
        //                //var label = $"{Labels[classes]} {probability * 100:0.00}%";
        //                //Console.WriteLine($"confidence {confidence * 100:0.00}% {label}");
        //                var x1 = (centerX - width / 2) < 0 ? 0 : centerX - width / 2;
        //                var y1 = (centerY - height / 2) < 0 ? 0 : centerY - height / 2;
        //                //avoid left side over edge
        //                //draw result
        //                //org.Rectangle(new OpenCvSharp.Point(x1, centerY - height / 2), new OpenCvSharp.Point(centerX + width / 2, centerY + height / 2), Colors[classes], 2);
        //                //var textSize = Cv2.GetTextSize(label, HersheyFonts.HersheyTriplex, 0.5, 1, out var baseline);
        //                //Cv2.Rectangle(org, new Rect(new OpenCvSharp.Point(x1, centerY - height / 2 - textSize.Height - baseline),
        //                //        new OpenCvSharp.Size(textSize.Width, textSize.Height + baseline)), Colors[classes], Cv2.FILLED);
        //                //Cv2.PutText(org, label, new OpenCvSharp.Point(x1, centerY - height / 2 - baseline), HersheyFonts.HersheyTriplex, 0.5, Scalar.Black);

        //                faces.Add(new Rectangle(
        //                (int)(x1),
        //                (int)(y1),
        //                (int)(width),
        //                (int)(height)
        //                ));
        //            }
        //        }
        //    }
        //    net.Dispose();
        //    blob.Dispose();
        //    prob.Dispose();

        //    //// Using darknet.exe
        //    //if (File.Exists(Application.StartupPath + @"\Process.txt"))
        //    //    File.Delete(Application.StartupPath + @"\Process.txt");

        //    //var w = mat.Width;
        //    //var h = mat.Height;

        //    //CvInvoke.Resize(mat, mat, new System.Drawing.Size(416, 416), 0, 0, Emgu.CV.CvEnum.Inter.Cubic);
        //    //CvInvoke.Imwrite(Application.StartupPath + @"\Process.jpg", mat);

        //    //Process P_YOLO = new Process();
        //    //P_YOLO.StartInfo.FileName = "cmd.exe";
        //    //P_YOLO.StartInfo.UseShellExecute = false;
        //    //P_YOLO.StartInfo.RedirectStandardInput = true;
        //    //P_YOLO.StartInfo.RedirectStandardOutput = true;
        //    //P_YOLO.StartInfo.RedirectStandardError = true;

        //    //P_YOLO.Start();
        //    //P_YOLO.StandardInput.WriteLine("cd " + Application.StartupPath);
        //    //P_YOLO.StandardInput.WriteLine("darknet.exe detector test obj.data yolov2-tiny.cfg yolov2-tiny_10000.weights Process.jpg -save_labels");
        //    //P_YOLO.StandardInput.WriteLine("exit");

        //    ////String str = null;
        //    ////str = p.StandardOutput.ReadToEnd();
        //    //P_YOLO.WaitForExit();
        //    //P_YOLO.Close();

        //    //string line;
        //    //StreamReader SR = new StreamReader(Application.StartupPath + @"\Process.txt");
        //    //while ((line = SR.ReadLine()) != null)
        //    //{
        //    //    double Category_Number = 0.0;
        //    //    double centerX = 0.0;
        //    //    double centerY = 0.0;
        //    //    double width = 0.0;
        //    //    double height = 0.0;
        //    //    double.TryParse(line.Substring(0, 1), out Category_Number);
        //    //    double.TryParse(line.Substring(2, 6), out centerX);
        //    //    centerX = centerX * w;
        //    //    double.TryParse(line.Substring(9, 6), out centerY);
        //    //    centerY = centerY * h;
        //    //    double.TryParse(line.Substring(16, 6), out width);
        //    //    width = width * w;
        //    //    double.TryParse(line.Substring(23, 6), out height);
        //    //    height = height * h;

        //    //    var x1 = (centerX - width / 2) < 0 ? 0 : centerX - width / 2;
        //    //    var y1 = (centerY - height / 2) < 0 ? 0 : centerY - height / 2;

        //    //    faces.Add(new Rectangle(
        //    //    (int)(x1),
        //    //    (int)(y1),
        //    //    (int)(width),
        //    //    (int)(height)
        //    //    ));

        //    //}
        //    //SR.Close();

        //    OpenCVResult result = new OpenCVResult()
        //    {
        //        eyes = eyes,
        //        faces = faces,
        //    };

        //    return result;
        //    //Emgu.CV.Mat mat_return = MatOpenCVSharpToEmgu(org);
        //    //return mat_return;
        //}

        /// <summary>
        /// 透過OpenCV進行人臉是否存在的辨識
        /// </summary>
        /// <param name="objMat"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        //private OpenCVResult CaptureFaceEyes(Emgu.CV.Mat objMat)
        //{
        //    long detectionTime;
        //    List<Rectangle> faces = new List<Rectangle>();
        //    List<Rectangle> eyes = new List<Rectangle>();

        //    DetectFace.Detect(
        //        objMat, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
        //        faces, eyes,
        //        out detectionTime);

        //    // 重新計算比例
        //    decimal diWidth = decimal.Parse(File_Before.Width.ToString()) / decimal.Parse(objMat.Bitmap.Width.ToString());
        //    decimal diHeight = decimal.Parse(File_Before.Height.ToString()) / decimal.Parse(objMat.Bitmap.Height.ToString());

        //    List<Rectangle> objDraw = new List<Rectangle>();

        //    for (int i = 0; i < faces.Count; i++)
        //    {
        //        objDraw.Add(new Rectangle(
        //            (int)(faces[i].X * diWidth),
        //            (int)(faces[i].Y * diHeight),
        //            (int)(faces[i].Width * diWidth),
        //            (int)(faces[i].Height * diHeight)
        //            ));
        //    }

        //    OpenCVResult result = new OpenCVResult()
        //    {
        //        eyes = eyes,
        //        faces = faces,
        //    };

        //    return result;
        //}

        //private OpenCVResult CaptureFace(Emgu.CV.Mat objMat)
        //{
        //    long detectionTime;
        //    List<Rectangle> faces = new List<Rectangle>();

        //    DetectFace.DetectFaceOnly(
        //        objMat, "haarcascade_frontalface_default.xml",
        //        faces,
        //        out detectionTime);

        //    // 重新計算比例
        //    decimal diWidth = decimal.Parse(File_Before.Width.ToString()) / decimal.Parse(objMat.Bitmap.Width.ToString());
        //    decimal diHeight = decimal.Parse(File_Before.Height.ToString()) / decimal.Parse(objMat.Bitmap.Height.ToString());

        //    List<Rectangle> objDraw = new List<Rectangle>();

        //    for (int i = 0; i < faces.Count; i++)
        //    {
        //        objDraw.Add(new Rectangle(
        //            (int)(faces[i].X * diWidth),
        //            (int)(faces[i].Y * diHeight),
        //            (int)(faces[i].Width * diWidth),
        //            (int)(faces[i].Height * diHeight)
        //            ));
        //    }

        //    OpenCVResult result = new OpenCVResult()
        //    {
        //        faces = faces,
        //    };

        //    return result;
        //}

        public class OpenCVResult
        {
            public List<Rectangle> faces { get; set; }
            public List<Rectangle> eyes { get; set; }
        }

        /// <summary>
        /// 馬賽克處理
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="effectWidth"> 影響範圍 每一個格子數 </param>
        /// <returns></returns>
        public Bitmap AdjustTobMosaic(Bitmap bitmap, int Start_Y, int Start_X, int Height, int Width, int effectWidth)
        {
            // 差異最多的就是以照一定範圍取樣完之後直接去下一個範圍
            for (int heightOfffset = Start_Y; heightOfffset < Start_Y + Height; heightOfffset += effectWidth)
            {
                for (int widthOffset = Start_X; widthOffset < Start_X + Width; widthOffset += effectWidth)
                {
                    int avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    for (int x = widthOffset; (x < widthOffset + effectWidth && x < Start_X + Width); x++)
                    {
                        for (int y = heightOfffset; (y < heightOfffset + effectWidth && y < Start_Y + Height); y++)
                        {
                            System.Drawing.Color pixel = bitmap.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    // 計算範圍平均
                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;


                    // 所有範圍內都設定此值
                    for (int x = widthOffset; (x < widthOffset + effectWidth && x < Start_X + Width); x++)
                    {
                        for (int y = heightOfffset; (y < heightOfffset + effectWidth && y < Start_Y + Height); y++)
                        {

                            System.Drawing.Color newColor = System.Drawing.Color.FromArgb(avgR, avgG, avgB);
                            bitmap.SetPixel(x, y, newColor);
                        }
                    }
                }
            }
            return bitmap;
        }

        public Bitmap SetPixelColor(System.Drawing.Bitmap bitmap, int Start_Y, int Start_X, int Height, int Width, System.Drawing.Color color)
        {
            for (int x = Start_X; x < Start_X + Width; x++)
            {
                for (int y = Start_Y; y < Start_Y + Height; y++)
                    bitmap.SetPixel(x, y, color);
            }
            return bitmap;
        }

        private void btm_Play_Click(object sender, EventArgs e)
        {
            State = (int)PlayState.PlayState_Play;
            btm_Play.Enabled = false;
            btm_Pause.Enabled = true;
            timer1.Interval = (int)(1000/objVideoCapture_Before.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
            timer1.Start();
        }

        private void btm_Pause_Click(object sender, EventArgs e)
        {
            State = (int)PlayState.PlayState_Pause;
            btm_Play.Enabled = true;
            btm_Pause.Enabled = false;
            FrameCount = objVideoCapture_Before.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (State == (int)PlayState.PlayState_Play)
            {
                objVideoCapture_Before.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, FrameCount);
                showFrame();
            }
            else
                timer1.Stop();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime time_start = DateTime.Now;
            FaceDetection();
            DateTime time_end = DateTime.Now;
            e.Result = Math.Round(((TimeSpan)(time_end - time_start)).TotalSeconds);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Finish\r\nTotal Time: " + e.Result.ToString() + " Sec");
            btm_Play.Enabled = true;
            btm_Pause.Enabled = true;
            btm_Start.Enabled = true;
        }

        public static Emgu.CV.Mat MatOpenCVSharpToEmgu(OpenCvSharp.Mat opcvsMat)
        {
            #region 正在应用,OpenCvSharp CvPtr指针 Emgu CvArrToMat
            var emptr = Emgu.CV.CvInvoke.CvArrToMat(opcvsMat.CvPtr, true);
            return emptr;
            #endregion
        }

        public static OpenCvSharp.Mat MatEmguToOpenCVSharp(Emgu.CV.Mat emguMat)
        {
            #region 正在应用,Emgu指针,new OpenCvSharp.Mat(IntPtr)
            var ptrMat = new OpenCvSharp.Mat(emguMat.Ptr);
            return ptrMat;
            #endregion
        }

        private void btm_Detect_Click(object sender, EventArgs e)
        {
            btm_Play.Enabled = false;
            btm_Pause.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
