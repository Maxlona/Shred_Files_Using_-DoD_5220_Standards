using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Stronghold.Forms
{
    public partial class FileShredder : Form
    {
        // for folder path,
        private string DirectoryPath = "";


        bool ErrorOccurred = false;

        string fileNameToShred = string.Empty;
        // how many overwrights
        private int runsCount = 3;

        bool canceled = false;
        DateTime startedJob;
        DateTime endedJob;

        //each cycle chunk size  
        int[] chunks = { 512, 1024, 512, 1024, 512, 1024, 512, 1024 };

        //delete folder after all files deleted successfuly?
        bool AllFilesInFolderDeleted = true;
        public FileShredder()
        {
            InitializeComponent();
            toolTip1.RemoveAll();

            FileName.Text = "File Name:";
            RunProgress.Text = "Progress:";
            PrgsBar.Value = 0;
            Stop.Enabled = false;
        }

        private int howManyLoops()
        {
            if (singleCycle.Checked == true) return 1;
            if (threeCycles.Checked == true) return 3;
            if (fiveCycles.Checked == true) return 5;
            if (sevenCycles.Checked == true) return 7;

            return 3; //default
        }


        /// Get random number (sector) seize
        /// Break the file into sectors
        /// Encode each sector, and flush it back to the file
        /// Repeat x number of times
        /// Set file length to zero
        /// Delete th file
        private void shredderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (File.Exists(fileNameToShred))
            {
                File.SetAttributes(fileNameToShred, FileAttributes.Normal);
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                FileStream inputStream = new FileStream(fileNameToShred, FileMode.Open);
                runsCount = howManyLoops();

                try
                {
                    for (int currentPass = 0; currentPass < runsCount; currentPass++)
                    {
                        int packets = chunks[currentPass];
                        byte[] buff = new byte[packets];
                        double sectors = Math.Ceiling((new FileInfo(fileNameToShred).Length / (packets)) / 1.0);

                        if (canceled == true)
                        {
                            inputStream.Close();
                            inputStream.Dispose();
                            return;
                        }

                        inputStream.Position = 0;

                        FileBackGroundWorker.ReportProgress(0);

                        try
                        {
                            for (int sectorsWritten = 0; sectorsWritten < sectors; sectorsWritten++)
                            {
                                if (canceled == true)
                                {
                                    inputStream.Close();
                                    inputStream.Dispose();
                                    break;
                                }

                                int val = (int)Math.Round((sectorsWritten / sectors) * 100);
                                val = val < 100 ? val : val - 1;  // return val to update progress bar
                                FileBackGroundWorker.ReportProgress(val);

                                rng.GetBytes(buff);
                                inputStream.Write(buff, 0, buff.Length);
                                inputStream.Flush();
                                string msg = ($"Processing {currentPass + 1} of {runsCount} cycles, {sectorsWritten} of {sectors} sectors!");
                                updateMsg(RunProgress, msg);
                            }
                        }
                        catch (Exception ex)
                        {
                            //exit forloop
                            break;
                        }

                        //flush
                        System.Threading.Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {

                }

                if (canceled == true)
                {
                    return;
                }

                updateMsg(RunProgress, "Finalizing job.");

                FileBackGroundWorker.ReportProgress(90);
                inputStream.SetLength(0);
                inputStream.Close();
                inputStream.Dispose();
                inputStream = null;
                System.Threading.Thread.Sleep(300);

                DateTime dt = new DateTime(1985, 8, 5, 0, 0, 0);
                File.SetCreationTime(fileNameToShred, dt);
                File.SetLastWriteTime(fileNameToShred, dt);
                File.SetLastAccessTime(fileNameToShred, dt);
                File.Delete(fileNameToShred);
                System.Threading.Thread.Sleep(300);
                FileBackGroundWorker.ReportProgress(100);
            }
        }

        private void updateMsg(Label control, string msg)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() =>
                {
                    control.Text = msg;
                    control.Update();
                    control.Refresh();
                }));
            }
            else
            {
                control.Text = msg;
                control.Update();
                control.Refresh();
            }
        }

        private void browse_Click(object sender, EventArgs e)
        {
            if (FileBackGroundWorker.IsBusy)
            {
                MessageBox.Show("Operation already running, please wait for this job to finish.");
                return;
            }


            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    //Read the contents of the file into a stream
                    long fileLength = new FileInfo(filePath).Length;

                    /// if > 500MB /// skip /// noticed unresponsive UI
                    if (fileLength > 500 * 1024 * 1024) // 500MB
                    {
                        MessageBox.Show("Please select a file smaller than 500MB");
                        return;
                    }
                    DirectoryPath = "";
                    fileNameToShred = filePath;
                    string fmt = FormatSize(fileLength).ToString();
                    FileName.Text = $"{filePath} | {fmt}";
                    toolTip1.SetToolTip(FileName, FileName.Text);
                    PrgsBar.Value = 0;
                    start.Enabled = true;
                }
            }
        }

        string FormatSize(long bytes)
        {
            // Load all suffixes in an array  
            string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1 && counter < 5)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1} {1}", number, suffixes[counter]);
        }

        string FormatTime(double seconds)
        {
            string[] suffixes = { "Seconds", "Minutes", "Hours" };
            int counter = 0;
            double number = seconds;
            while (Math.Round(number / 60) >= 1 && counter < 3)
            {
                number = number / 60;
                counter++;
            }
            return string.Format("{0:n1} {1}", number, suffixes[counter]);
        }

        private void start_Click(object sender, EventArgs e)
        {
            /// process single file...
            if (DirectoryPath == "")
            {
                /// process single file
                if (string.IsNullOrEmpty(fileNameToShred) || !File.Exists(fileNameToShred))
                {
                    MessageBox.Show("Please select a file, then click start");
                    return;
                }
                else
                {
                    try
                    {
                        startedJob = DateTime.Now;
                        canceled = false;
                        Stop.Enabled = true;
                        FileBackGroundWorker.RunWorkerAsync();
                        start.Enabled = false;
                        SelectDirectBtn.Enabled = false;
                        selectFileBtn.Enabled = false;
                        cyclesGroupBox.Enabled = false;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Shredding job ran into a problem, please try a different file!");
                    }
                    finally
                    {
                    }
                }
            }
            else if (DirectoryPath != "")
            {
                // ------------------------------------------------- // process folder...

                if (!Directory.Exists(DirectoryPath) || string.IsNullOrEmpty(DirectoryPath))
                {
                    MessageBox.Show("Please select a directory, then click start");
                    return;
                }
                else
                {
                    try
                    {
                        startedJob = DateTime.Now;
                        DirectoryBackGroundWorker.RunWorkerAsync();
                        cyclesGroupBox.Enabled = false;
                        start.Enabled = false;
                        canceled = false;
                        cyclesGroupBox.Enabled = false;
                        Stop.Enabled = true;
                        SelectDirectBtn.Enabled = false;
                        selectFileBtn.Enabled = false;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Shredding job ran into a problem, please try a different file!");
                    }
                    finally
                    {
                    }

                }

            }

        }

        private void shredderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 100) return;
            PrgsBar.Value = e.ProgressPercentage;
            PrgsBar.Refresh();
            PrgsBar.Update();
        }



        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand,
                                                StringBuilder strReturn,
                                                int iReturnLength,
                                                IntPtr hwndCallback);

        private void shredderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            fileNameToShred = string.Empty;
            endedJob = DateTime.Now;

            /// play sound effect

            double tm = (endedJob - startedJob).TotalSeconds;
            string duration = FormatTime(tm);

            if (canceled == true)
            {
                MessageBox.Show("Shredding job was cancelled");
                updateMsg(RunProgress, $"Status: Cancelled,");
                return;
            }
            else
            {
                MessageBox.Show("Shredding job completed successfully");
                updateMsg(RunProgress, $"Completed Successfuly | Finished in {duration}");
            }


            start.Enabled = false;
            SelectDirectBtn.Enabled = true;
            selectFileBtn.Enabled = true;
            canceled = false;
            Stop.Invoke(new Action(() => { Stop.Enabled = false; }));
            cyclesGroupBox.Invoke(new Action(() =>
            {
                cyclesGroupBox.Enabled = true;
            }));

            // if the form was minimized, the msg box become hidden
            Activate();
            Focus();
            toolTip1.RemoveAll();
        }



        /// ///////// ------------------------------------- ////////////////////////// folder content background worker
        private void SelectDirectBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog diag = new FolderBrowserDialog())
            {
                var res = diag.ShowDialog();
                if (res == DialogResult.OK)
                {
                    if (Directory.Exists(diag.SelectedPath))
                    {
                        DirectoryPath = diag.SelectedPath;
                        start.Enabled = true;

                        FileName.Text = DirectoryPath + "\\*.*";

                        PrgsBar.Value = 0;
                        start.Enabled = true;
                    }
                }
            }
        }

        void DeleteFolderFiles(string DirectPath)
        {
            if (DirectPath != "")
            {
                int curnt = 0;
                string[] files = Directory.GetFiles(DirectPath);
                foreach (string file in files)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            long fileLength = new FileInfo(file).Length;

                            // if file > 500 MB, Skip
                            if (fileLength < (500 * (1024 * 1024)))
                            {
                                string fmt = FormatSize(fileLength).ToString();
                                curnt++;
                                updateMsg(FileName, $"{file} | {fmt}");

                                File.SetAttributes(file, FileAttributes.Normal);
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                FileStream inputStream = new FileStream(file, FileMode.Open);
                                runsCount = howManyLoops();

                                try
                                {
                                    for (int currentPass = 0; currentPass < runsCount; currentPass++)
                                    {
                                        if (canceled == true)
                                        {
                                            inputStream.Close();
                                            inputStream.Dispose();
                                            AllFilesInFolderDeleted = false;
                                            break;
                                        }

                                        int packets = chunks[currentPass];
                                        byte[] buff = new byte[packets];
                                        double sectors = Math.Ceiling((new FileInfo(file).Length / (packets)) / 1.0);

                                        PrgsBar.Invoke(new Action(() => { PrgsBar.Value = 0; }));

                                        inputStream.Position = 0;

                                        try
                                        {
                                            DirectoryBackGroundWorker.ReportProgress(0);
                                            for (int sectorsWritten = 0; sectorsWritten < sectors; sectorsWritten++)
                                            {
                                                if (canceled == true)
                                                {
                                                    inputStream.Close();
                                                    inputStream.Dispose();
                                                    AllFilesInFolderDeleted = false;
                                                    break;
                                                }

                                                int val = (int)Math.Round((sectorsWritten / sectors) * 100);
                                                val = val < 100 ? val : val - 1;  // return val to update progress bar
                                                DirectoryBackGroundWorker.ReportProgress(val);

                                                rng.GetBytes(buff);
                                                inputStream.Write(buff, 0, buff.Length);
                                                inputStream.Flush();
                                                string msg = ($"File {curnt} of {files.Count()} - Processing {currentPass + 1} of {runsCount} cycles, {sectorsWritten} of {sectors} sectors!");
                                                updateMsg(RunProgress, msg);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //exit forloop & skip file..
                                            AllFilesInFolderDeleted = false;
                                            break;
                                        }

                                        //flush
                                        System.Threading.Thread.Sleep(100);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    AllFilesInFolderDeleted = false;
                                }

                                if (canceled == true)
                                {
                                    AllFilesInFolderDeleted = false;
                                    return;
                                }

                                updateMsg(RunProgress, "Finalizing job.");
                                DirectoryBackGroundWorker.ReportProgress(90);
                                inputStream.SetLength(0);
                                inputStream.Close();
                                inputStream = null;
                                System.Threading.Thread.Sleep(300);

                                DateTime dt = new DateTime(1985, 8, 5, 0, 0, 0);
                                File.SetCreationTime(file, dt);
                                File.SetLastWriteTime(file, dt);
                                File.SetLastAccessTime(file, dt);
                                File.Delete(file);
                                System.Threading.Thread.Sleep(300);
                                DirectoryBackGroundWorker.ReportProgress(100);

                            }
                            else
                            {
                                AllFilesInFolderDeleted = false;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        AllFilesInFolderDeleted = false;
                    }
                }


                List<string> directories = Directory.GetDirectories(DirectPath).ToList();
                foreach (var dirct in directories)
                {
                    if (Directory.Exists(dirct))
                    {
                        if (canceled == true) break;
                        DeleteFolderFiles(dirct);
                    }
                }

            }
        }

        private void DirectoryBackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!string.IsNullOrEmpty(DirectoryPath))
                DeleteFolderFiles(DirectoryPath);
        }

        private void DirectoryBackGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 100) return;
            PrgsBar.Value = e.ProgressPercentage;
            PrgsBar.Refresh();
            PrgsBar.Update();
        }

        private void DirectoryBackGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            fileNameToShred = string.Empty;

            endedJob = DateTime.Now;

            /// play sound effect

            double tm = (endedJob - startedJob).TotalSeconds;
            string duration = FormatTime(tm);

            if (canceled == true)
            {
                MessageBox.Show("Shredding job was cancelled");
                updateMsg(RunProgress, $"Status: Cancelled,");
                return;
            }
            else
            {
                MessageBox.Show("Shredding job completed successfully");
                updateMsg(RunProgress, $"Completed Successfuly | Finished in {duration}");
            }

            // all files deleted successfully
            if (AllFilesInFolderDeleted == true)
            {

                try
                {
                    DateTime dt = new DateTime(1985, 8, 5, 0, 0, 0);
                    Directory.SetCreationTime(DirectoryPath, dt);
                    Directory.SetLastWriteTime(DirectoryPath, dt);
                    Directory.SetLastAccessTime(DirectoryPath, dt);

                    DirectoryInfo di = new DirectoryInfo(DirectoryPath);
                    long overallSize = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);

                    Directory.SetCreationTime(DirectoryPath, dt);
                    Directory.SetLastWriteTime(DirectoryPath, dt);
                    Directory.SetLastAccessTime(DirectoryPath, dt);

                    if (overallSize == 0)
                        Directory.Delete(DirectoryPath, true);

                    updateMsg(RunProgress, $"All files deleted successfuly | Finished in {duration}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete the directory, but all nested files has been deleted successfully");
                }
            }
            else
            {
                updateMsg(RunProgress, $"Completed with some errors | Finished in {duration}");
            }

            start.Enabled = false;
            SelectDirectBtn.Enabled = true;
            selectFileBtn.Enabled = true;
            canceled = false;
            Stop.Invoke(new Action(() => { Stop.Enabled = false; }));
            cyclesGroupBox.Invoke(new Action(() =>
            {
                cyclesGroupBox.Enabled = true;
            }));

            // if the form was minimized, the msg box become hidden
            Activate();
            Focus();
            toolTip1.RemoveAll();
        }

        private void FileShredder_Load(object sender, EventArgs e)
        {
            start.Enabled = Focused;
            cyclesGroupBox.Enabled = true;
            selectFileBtn.Enabled = true;
            SelectDirectBtn.Enabled = true;
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            PrgsBar.Value = 0;
            canceled = true;
            Stop.Enabled = false;
        }
    }
}
